// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoWork.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Background worker.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerBackgroundUpdater
{
    using System;
    using System.Threading;

    /// <summary>
    /// The do work.
    /// </summary>
    public class DoWork
    {
        /// <summary>
        /// The timeout.
        /// </summary>
        private readonly int timeout;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoWork"/> class.
        /// </summary>
        /// <param name="timeout">
        /// The timeout.
        /// </param>
        public DoWork(int timeout)
        {
            this.timeout = timeout;
        }

        /// <summary>
        /// The perform work.
        /// </summary>
        /// Using Monitor for wait passively.
        public void PerformWork()
        {
            do
            {
                lock (this)
                {
                    try
                    {
                        Monitor.Wait(this, this.timeout);
                    }
                    catch (Exception)
                    {
                        // Catch exceptions for the background not stop.
                    } 
                }
                Console.WriteLine("timeout!");

                // Update Episodes.
                Updater.UpdateEpisodes();
            }
            while (true);
// ReSharper disable FunctionNeverReturns
        }
// ReSharper restore FunctionNeverReturns
    }
}
