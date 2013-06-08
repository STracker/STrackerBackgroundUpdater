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
            var work = new DoWork(timeout);
            work.PerformWork();
        }
    }
}