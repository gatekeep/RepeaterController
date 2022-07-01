/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.Threading;

using NAudio.Wave;

namespace RepeaterController.DSP.PL
{
    /// <summary>
    /// Implements a worker that waits for and decodes samples for a PL tone.
    /// </summary>
    public class PLWaveIn : IDisposable
    {
        /**
         * Fields
         */
        private const double LOW_PASS_FREQ = 280;

        private WaveIn waveIn;
        private bool disposed = false;
        private Thread captureWorker;

        private readonly ISampleSource source;
        private readonly PLDetector plDetector = new PLDetector();

        /**
         * Properties
         */
        /// <summary>
        /// Milliseconds for the buffer. Recommended value is 100ms
        /// </summary>
        public int BufferMilliseconds
        {
            get { return waveIn.BufferMilliseconds; }
            set { waveIn.BufferMilliseconds = value; }
        }

        /// <summary>
        /// Number of Buffers to use (usually 2 or 3)
        /// </summary>
        public int NumberOfBuffers
        {
            get { return waveIn.NumberOfBuffers; }
            set { waveIn.NumberOfBuffers = value; }
        }

        /// <summary>
        /// Flag indicating whether the live analyzer is capturing samples.
        /// </summary>
        public bool IsRecording { get; private set; }

        /// <summary>
        /// Gets the last detected DTMF tone.
        /// </summary>
        public double LastPLTone { get; private set; }

        /**
         * Events
         */
        /// <summary>
        /// 
        /// </summary>
        public event Action<object, double> PLToneDetected;

        /// <summary>
        /// 
        /// </summary>
        public event Action<object> PLToneLost;

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="PLWaveIn"/> class.
        /// </summary>
        /// <param name="waveFormat"></param>
        /// <param name="deviceNumber"></param>
        public PLWaveIn(WaveFormat waveFormat, int deviceNumber)
        {
            this.waveIn = new WaveIn();
            this.waveIn.WaveFormat = waveFormat;
            this.waveIn.DeviceNumber = deviceNumber;

            this.LastPLTone = 67.0;
            this.source = new StreamingSampleSource(Buffer(waveIn), PLDetector.SampleRate);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (IsRecording)
                    this.StopRecording();

                if (waveIn != null)
                    waveIn.Dispose();

                if (captureWorker != null)
                {
                    if (captureWorker.IsAlive)
                    {
                        captureWorker.Abort();
                        captureWorker.Join();
                    }

                    captureWorker = null;
                }
            }

            disposed = true;
        }

        /// <summary>
        /// Starts capturing samples for analysis.
        /// </summary>
        public void StartRecording()
        {
            if (IsRecording)
                return;

            waveIn.StartRecording();

            IsRecording = true;
            captureWorker = new Thread(Detect);
            captureWorker.Name = "PLWaveIn";
            captureWorker.Start();
        }

        /// <summary>
        /// Stops capturing samples for analysis.
        /// </summary>
        public void StopRecording()
        {
            if (!IsRecording)
                return;

            IsRecording = false;
            captureWorker.Abort();
            captureWorker.Join();

            waveIn.StopRecording();
        }

        /// <summary>
        /// Helper to wait for a new PL tone.
        /// </summary>
        /// <returns></returns>
        public double WaitForPLTone()
        {
            while (source.HasSamples)
            {
                LastPLTone = plDetector.Analyze(source.Samples);
                if (LastPLTone != 0.0)
                    return LastPLTone;
            }

            return 0.0;
        }

        /// <summary>
        /// Helper to wait for the end of the last detected PL tone.
        /// </summary>
        public void WaitForEndOfLastPLTone()
        {
            while (source.HasSamples)
            {
                double nextPLTone = plDetector.Analyze(source.Samples);
                if (nextPLTone != LastPLTone)
                    return;
            }
        }

        /// <summary>
        /// Thread method for detecting PL tones.
        /// </summary>
        private void Detect()
        {
            while (true)
            {
                double tone = WaitForPLTone();
                PLToneDetected?.Invoke(this, tone);
                Messages.Trace("PL Detect: " + tone, LogFilter.PL_DPL_TRACE);

                WaitForEndOfLastPLTone();
                PLToneLost?.Invoke(this);
            }
        }

        /// <summary>
        /// Helper to generate a <see cref="AudioWaveProvider"/> wave source.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static AudioWaveProvider Buffer(IWaveIn source)
        {
            BiQuad filter = new LowpassFilter(source.WaveFormat.SampleRate, LOW_PASS_FREQ);
            AudioWaveProvider sourceBuffer = new AudioWaveProvider(source.WaveFormat) { DiscardOnBufferOverflow = true };
            source.DataAvailable += (sender, e) => sourceBuffer.AddSamples(e.Buffer, 0, e.BytesRecorded);
            sourceBuffer.FilterSampleCallback += (ref double leftSmp, ref double rightSmp) =>
            {
                leftSmp = filter.Process((float)leftSmp);
                rightSmp = filter.Process((float)rightSmp);
            };

            return sourceBuffer;
        }
    } // public class PLWaveIn
} // namespace RepeaterController.DSP.PL
