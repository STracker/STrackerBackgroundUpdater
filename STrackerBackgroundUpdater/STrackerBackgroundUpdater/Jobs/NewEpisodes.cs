// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewEpisodes.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Background worker for get new episodes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerBackgroundUpdater.Jobs
{
    using System.Collections.Generic;

    using Ninject;

    using Quartz;

    using STrackerBackgroundWorker.ExternalProviders;

    using STrackerBckUpd.NinjectDependencies;

    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.Core.SeasonsRepositories;
    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.Logger.Core;

    /// <summary>
    /// The do work.
    /// </summary>
    public class NewEpisodes : IJob
    {
        /// <summary>
        /// The manager.
        /// </summary>
        private readonly TvShowsInformationManager manager;

        /// <summary>
        /// The repository.
        /// </summary>
        private readonly ITvShowNewEpisodesRepository repository;

        /// <summary>
        /// The television shows repository.
        /// </summary>
        private readonly ITvShowsRepository tvshowsRepository;

        /// <summary>
        /// The seasons repository.
        /// </summary>
        private readonly ISeasonsRepository seasonsRepository;

        /// <summary>
        /// The episodes repository.
        /// </summary>
        private readonly IEpisodesRepository episodesRepository;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewEpisodes"/> class.
        /// </summary>
        public NewEpisodes()
        {
            using (IKernel kernel = new StandardKernel(new ModuleForBackgroundUpdater()))
            {
                this.manager = kernel.Get<TvShowsInformationManager>();
                this.repository = kernel.Get<ITvShowNewEpisodesRepository>();
                this.seasonsRepository = kernel.Get<ISeasonsRepository>();
                this.episodesRepository = kernel.Get<IEpisodesRepository>();
                this.tvshowsRepository = kernel.Get<ITvShowsRepository>();
                this.logger = kernel.Get<ILogger>();
            }
        }

        /// <summary>
        /// Called by the <see cref="T:Quartz.IScheduler"/> when a <see cref="T:Quartz.ITrigger"/>
        ///             fires that is associated with the <see cref="T:Quartz.IJob"/>.
        /// </summary>
        /// <remarks>
        /// The implementation may wish to set a  result object on the 
        ///             JobExecutionContext before this method exits.  The result itself
        ///             is meaningless to Quartz, but may be informative to 
        ///             <see cref="T:Quartz.IJobListener"/>s or 
        ///             <see cref="T:Quartz.ITriggerListener"/>s that are watching the job's 
        ///             execution.
        /// </remarks>
        /// <param name="context">The execution context.</param>
        public void Execute(IJobExecutionContext context)
        {
            var provider = this.manager.GetDefaultProvider();
          
            List<Episode> episodes;
            provider.GetNewEpisodes(out episodes);

            foreach (var episode in episodes)
            {
                this.CreateEpisode(episode);
            }

            // Delete old episodes from newest episodes document.
            this.repository.DeleteOldEpisodes();
        }

        /// <summary>
        /// Creates the episode or updates the old one.
        /// </summary>
        /// <param name="episode">
        /// The episode.
        /// </param>
        private void CreateEpisode(Episode episode)
        {
            var tvshow = this.tvshowsRepository.Read(episode.Id.TvShowId);

            if (tvshow == null)
            {
                return;
            }

            var seasonId = new Season.SeasonId { TvShowId = episode.Id.TvShowId, SeasonNumber = episode.Id.SeasonNumber };

            var season = this.seasonsRepository.Read(seasonId);

            if (season == null)
            {
                this.seasonsRepository.Create(new Season(seasonId));
            }

            var oldEpisode = this.episodesRepository.Read(episode.Id);

            if (oldEpisode == null)
            {
                this.episodesRepository.Create(episode);
            }
            else
            {
                this.episodesRepository.Update(episode);
            }

            this.logger.Debug("Updater:CreateEpisode", "None", string.Format("TvShow:{0}, Season:{1}, Episode:{2}", episode.Id.TvShowId, episode.Id.SeasonNumber, episode.Id.EpisodeNumber));
        }
    }
}