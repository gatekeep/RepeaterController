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

namespace RepeaterController.DSP.DTMF
{
    /// <summary>
    /// Implements a worker that waits for and decodes samples for DTMF tone(s).
    /// </summary>
    public class DtmfWaveIn : IDisposable
    {
        /**
         * Fields
         */
        private const double LOW_PASS_FREQ = 1800;
        private const double HIGH_PASS_FREQ = 500;

        private WaveIn waveIn;
        private bool disposed = false;
        private Thread captureWorker;

        private readonly ISampleSource source;
        private readonly DtmfDetector dtmfDetector = new DtmfDetector();

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
        public DtmfTone LastDtmfTone { get; private set; }

        /**
         * Events
         */
        /// <summary>
        /// 
        /// </summary>
        public event Action<object, DtmfToneStart> DtmfToneStarting;

        /// <summary>
        /// 
        /// </summary>
        public event Action<object, DtmfToneEnd> DtmfToneStopped;

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="DtmfWaveIn"/> class.
        /// </summary>
        /// <param name="waveFormat"></param>
        /// <param name="deviceNumber"></param>
        public DtmfWaveIn(WaveFormat waveFormat, int deviceNumber)
        {
            this.waveIn = new WaveIn();
            this.waveIn.WaveFormat = waveFormat;
            this.waveIn.DeviceNumber = deviceNumber;

            this.LastDtmfTone = DtmfTone.None;
            this.source = new StreamingSampleSource(Buffer(waveIn), DtmfDetector.SampleRate);
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
            captureWorker.Name = "DtmfWaveIn";
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
        /// Helper to wait for a new DTMF tone.
        /// </summary>
        /// <returns></returns>
        public DtmfTone WaitForDtmfTone()
        {
            while (source.HasSamples)
            {
                LastDtmfTone = dtmfDetector.Analyze(source.Samples);
                if (LastDtmfTone != DtmfTone.None)
                    return LastDtmfTone;
            }

            return DtmfTone.None;
        }

        /// <summary>
        /// Helper to wait for the end of the last detected DTMF tone.
        /// </summary>
        public void WaitForEndOfLastDtmfTone()
        {
            while (source.HasSamples)
            {
                var nextDtmfTone = dtmfDetector.Analyze(source.Samples);
                if (nextDtmfTone != LastDtmfTone)
                    return;
            }
        }

        /// <summary>
        /// Thread method for detecting DTMF tones.
        /// </summary>
        private void Detect()
        {
            while (true)
            {
                DtmfTone dtmfTone = WaitForDtmfTone();

                DateTime start = DateTime.Now;
                DtmfToneStarting?.Invoke(this, new DtmfToneStart(dtmfTone, start));

                WaitForEndOfLastDtmfTone();

                TimeSpan duration = DateTime.Now - start;
                DtmfToneStopped?.Invoke(this, new DtmfToneEnd(dtmfTone, duration));
                Messages.Trace("DTMF Tone: " + dtmfTone.KeyString, LogFilter.DTMF_TRACE);
            }
        }

        /// <summary>
        /// Helper to generate a <see cref="AudioWaveProvider"/> wave source.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static AudioWaveProvider Buffer(IWaveIn source)
        {
            BiQuad lo = new LowpassFilter(source.WaveFormat.SampleRate, LOW_PASS_FREQ);
            BiQuad high = new HighpassFilter(source.WaveFormat.SampleRate, HIGH_PASS_FREQ);
            AudioWaveProvider sourceBuffer = new AudioWaveProvider(source.WaveFormat) { DiscardOnBufferOverflow = true };
            source.DataAvailable += (sender, e) => sourceBuffer.AddSamples(e.Buffer, 0, e.BytesRecorded);
            sourceBuffer.FilterSampleCallback += (ref double leftSmp, ref double rightSmp) =>
            {
                leftSmp = high.Process((float)leftSmp);
                rightSmp = high.Process((float)rightSmp);
                leftSmp = lo.Process((float)leftSmp);
                rightSmp = lo.Process((float)rightSmp);
            };

            return sourceBuffer;
        }
    } // public class DtmfWaveIn
} // namespace RepeaterController.DSP.DTMF
