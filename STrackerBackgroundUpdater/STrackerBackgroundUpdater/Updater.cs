// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Updater.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Background worker.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerBackgroundUpdater
{
    using System.Collections.Generic;

    using STrackerBackgroundWorker.ExternalProviders.Core;
    using STrackerBackgroundWorker.ExternalProviders.Providers;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The updater.
    /// </summary>
    public class Updater
    {
        /// <summary>
        /// The manager.
        /// </summary>
        private static readonly ITvShowsInformationProvider provider = new TheTvDbProvider();
      
        /// <summary>
        /// The update episodes.
        /// </summary>
        public static void UpdateEpisodes()
        {
            List<Episode> episodes;
            provider.UpdateEpisodes(out episodes);
            if (episodes == null)
            {
                return;
            }

            // Update
        }
    }
}
