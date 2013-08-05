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
    using System;
    using System.Collections.Generic;

    using Ninject;

    using Quartz;

    using STrackerBackgroundWorker.ExternalProviders;

    using STrackerBckUpd.NinjectDependencies;

    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;

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
        private readonly INewestEpisodesRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewEpisodes"/> class.
        /// </summary>
        public NewEpisodes()
        {
            using (IKernel kernel = new StandardKernel(new ModuleForBackgroundUpdater()))
            {
                this.manager = kernel.Get<TvShowsInformationManager>();
                this.repository = kernel.Get<INewestEpisodesRepository>();
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
            
            // TODO

            // Delete old episodes from newest episodes document.
            this.repository.DeleteOldEpisodes();
        }
    }
}