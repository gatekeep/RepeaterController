/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using NAudio.Wave;

namespace RepeaterController.DSP.MDC1200
{
    /// <summary>
    /// Implements a worker that waits for and decodes samples for a MDC1200 packet(s).
    /// </summary>
    public class MDCWaveIn : IDisposable
    {
        /**
         * Fields
         */
        private WaveIn waveIn;
        private bool disposed = false;
        private Thread captureWorker;

        private WaveFormat format;

        private static AudioWaveProvider sourceBuffer;
        private readonly ISampleSource source;
        private readonly MDCDetector mdcDetector;

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
        /// Gets the sample rate of the MDC1200 detector.
        /// </summary>
        public static int SampleRate { get; } = 48000;

        /// <summary>
        /// Flag indicating whether the live analyzer is capturing samples.
        /// </summary>
        public bool IsRecording { get; private set; }

        /**
         * Events
         */
        /// <summary>
        /// Occurs when a MDC packet is successfully decoded.
        /// </summary>
        public event Action<object, int, MDCPacket, MDCPacket> MDCPacketDetected;

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="MDCWaveIn"/> class.
        /// </summary>
        /// <param name="waveFormat"></param>
        /// <param name="deviceNumber"></param>
        public MDCWaveIn(WaveFormat waveFormat, int deviceNumber)
        {
            this.waveIn = new WaveIn();
            this.waveIn.WaveFormat = waveFormat;
            this.waveIn.DeviceNumber = deviceNumber;

            this.format = waveFormat;

            this.mdcDetector = new MDCDetector(SampleRate);
            this.mdcDetector.DecoderCallback += MdcDecoder_DecoderCallback;
            this.source = new StreamingSampleSource(Buffer(waveIn), SampleRate);
        }

        /// <summary>
        /// Event that occurs when an MDC1200 packet is decoded.
        /// </summary>
        /// <param name="goodFrames"></param>
        /// <param name="first"></param>
        /// <param name="second"></param>
        private void MdcDecoder_DecoderCallback(int goodFrames, MDCPacket first, MDCPacket second)
        {
            MDCPacketDetected?.Invoke(this, goodFrames, first, second);
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
            captureWorker.Name = "MDCWaveIn";
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
        /// Manually process raw sample input byte array for an MDC-1200 packet.
        /// </summary>
        /// <param name="buffer">Buffer source containing samples</param>
        /// <returns>Number of MDC1200 frames processed</returns>
        public void ProcessSamples(byte[] buffer)
        {
            sourceBuffer.AddSamples(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Thread method for detecting MDC1200 packets.
        /// </summary>
        private void Detect()
        {
            while (true)
            {
                if (sourceBuffer.BufferedDuration.Milliseconds < BufferMilliseconds)
                    continue;

                while (source.HasSamples)
                        mdcDetector.ProcessSamples(source.Samples);
                sourceBuffer.ClearBuffer();
            }
        }

        /// <summary>
        /// Helper to generate a <see cref="AudioWaveProvider"/> wave source.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static AudioWaveProvider Buffer(IWaveIn source)
        {
            sourceBuffer = new AudioWaveProvider(source.WaveFormat) { BufferLength = source.WaveFormat.AverageBytesPerSecond * 10, DiscardOnBufferOverflow = false };
            source.DataAvailable += (sender, e) => sourceBuffer.AddSamples(e.Buffer, 0, e.BytesRecorded);

            return sourceBuffer;
        }
    } // public class MDCWaveIn
} // namespace RepeaterController.DSP.MDC1200
