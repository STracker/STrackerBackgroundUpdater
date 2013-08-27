// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Entry point of the background worker.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerBackgroundUpdater
{
    using System.Configuration;

    using Quartz;
    using Quartz.Impl;

    using STrackerBackgroundUpdater.Jobs;

    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        public static void Main(string[] args)
        {
            var timeout = int.Parse(ConfigurationManager.AppSettings["Timeout"]);
            var scheduler = new StdSchedulerFactory().GetScheduler();
            scheduler.Start();
            var job = JobBuilder.Create<NewEpisodes>().Build();
            var trigger = TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInHours(timeout).RepeatForever()).Build();
            scheduler.ScheduleJob(job, trigger);
        }
    }
}