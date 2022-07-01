/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.Threading;

namespace RepeaterController.DSP
{
    /// <summary>
    /// Helper class to automatically transmit the CW ID on an interval.
    /// </summary>
    public class AutomaticCW
    {
        /**
         * Fields
         */
        private Thread cwThread;
        private bool threadExecute = false;

        private MainWindow wnd;
        private RepeaterOptions optionsModal;

        /// <summary>
        /// Gets the wave provider used for this <see cref="AutomaticCW"/>.
        /// </summary>
        public AudioWaveProvider WaveProvider;

        /// <summary>
        /// Gets the date and time for the last transmitted CW.
        /// </summary>
        public DateTime LastCWID;

        /**
         * Events
         */
        /// <summary>
        /// Event that occurs when the CW is played back.
        /// </summary>
        public event EventHandler PlayingCW;

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="AutomaticCW"/> class.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="wnd"></param>
        /// <param name="options"></param>
        public AutomaticCW(MainWindow wnd, RepeaterOptions options)
        {
            this.WaveProvider = null;

            this.wnd = wnd;
            this.optionsModal = options;

            this.LastCWID = DateTime.Now;
        }

        /// <summary>
        /// Starts the automatic CW thread.
        /// </summary>
        public void Start()
        {
            if (this.cwThread != null)
                return;

            this.cwThread = new Thread(ThreadStart);
            this.threadExecute = true;
            this.cwThread.Start();
        }

        /// <summary>
        /// Stops the automatic CW thread.
        /// </summary>
        public void Stop()
        {
            if (this.cwThread != null)
            {
                this.threadExecute = false;
                this.cwThread.Abort();
                this.cwThread.Join();
                this.cwThread = null;
            }
        }

        /// <summary>
        /// Play the repeater callsign.
        /// </summary>
        public void PlayRepeaterCW()
        {
            if (PlayingCW != null)
                PlayingCW(this, new EventArgs());

            if (WaveProvider != null)
            {
                MorseGenerator gen = new MorseGenerator(WaveProvider, MainWindow.CWFrequency, MainWindow.CWWPM);
                gen.GenerateString(this.optionsModal.Options.Callsign);
            }
            else
                throw new ArgumentNullException();

            this.LastCWID = DateTime.Now;
        }

        /// <summary>
        /// Internal function containing the thread main entry point.
        /// </summary>
        private void ThreadStart()
        {
            while (this.threadExecute)
            {
                // no function if MDC console and not in link radio mode
                if (optionsModal.Options.MDCConsoleOnly && !optionsModal.Options.AllowConsoleAnncDTMF)
                    return;

                DateTime now = DateTime.Now;
                TimeSpan span = now.Subtract(LastCWID);

                // is auto ID disabled?
                if (!this.optionsModal.Options.NoId && !this.wnd.EnforcedCWInterval && !MainWindow.IsRepeaterRx)
                {
                    // have we reached our interval?
                    if (span.TotalMinutes >= this.optionsModal.Options.IdInterval)
                        PlayRepeaterCW();
                }

                // sleep ~1s
                Thread.Sleep(1000);
            }
        }
    } // public class AutomaticCW
} // namespace RepeaterController
