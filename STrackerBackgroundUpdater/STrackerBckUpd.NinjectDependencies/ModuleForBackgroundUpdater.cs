// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleForBackgroundUpdater.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Ninject module for STrackerBackgroundUpdater dependencies.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerBckUpd.NinjectDependencies
{
    using System.Configuration;

    using MongoDB.Driver;

    using Ninject.Modules;

    using STrackerBackgroundWorker.ExternalProviders;
    using STrackerBackgroundWorker.ExternalProviders.Core;
    using STrackerBackgroundWorker.ExternalProviders.Providers;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.Core.SeasonsRepositories;
    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.Core.UsersRepositories;
    using STrackerServer.Repository.MongoDB.Core;
    using STrackerServer.Repository.MongoDB.Core.EpisodesRepositories;
    using STrackerServer.Repository.MongoDB.Core.SeasonsRepositories;
    using STrackerServer.Repository.MongoDB.Core.TvShowsRepositories;
    using STrackerServer.Repository.MongoDB.Core.UsersRepositories;

    /// <summary>
    /// The module for background updater.
    /// </summary>
    public class ModuleForBackgroundUpdater : NinjectModule 
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            // MongoDB stuff dependencies...
            this.Bind<MongoUrl>().ToSelf().InSingletonScope().WithConstructorArgument("url", ConfigurationManager.AppSettings["MongoDBURL"]);

            // MongoClient class is thread safe.
            this.Bind<MongoClient>().ToSelf().InSingletonScope();

            // Television shows stuff dependencies...
            this.Bind<ITvShowsRepository>().To<TvShowsRepository>();
            this.Bind<IGenresRepository>().To<GenresRepository>();
            this.Bind<ITvShowCommentsRepository>().To<TvShowCommentsRepository>();
            this.Bind<ITvShowRatingsRepository>().To<TvShowRatingsRepository>();

            // Seasons stuff dependencies...
            this.Bind<ISeasonsRepository>().To<SeasonsRepository>();

            // Episodes stuff dependencies...
            this.Bind<IEpisodesRepository>().To<EpisodesRepository>();
            this.Bind<IEpisodeCommentsRepository>().To<EpisodeCommentsRepository>();
            this.Bind<IEpisodeRatingsRepository>().To<EpisodeRatingsRepository>();
            this.Bind<INewestEpisodesRepository>().To<NewestEpisodesRepository>();

            // Users stuff dependencies...
            this.Bind<IUsersRepository>().To<UsersRepository>();

            // Providers dependencies...
            this.Bind<ITvShowsInformationProvider>().To<TheTvDbProvider>();
            this.Bind<TvShowsInformationManager>().ToSelf().InSingletonScope();
        }
    }
}