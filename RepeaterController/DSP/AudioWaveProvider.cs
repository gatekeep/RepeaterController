/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
/**
 * Based on code from the NAudio project. (https://github.com/naudio/NAudio)
 * Licensed under the Ms-PL license.
 */
using System;
using System.Threading;

using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace RepeaterController.DSP
{
    public delegate void WaveProviderStereoFilterSampleCallback(ref double leftSmp, ref double rightSmp);

    /// <summary>
    /// Provides a buffered store of samples. Read method will return queued samples or fill buffer with zeroes.
    /// </summary>
    public class AudioWaveProvider : IWaveProvider
    {
        /**
         * Fields
         */
        protected CircularBuffer circularBuffer;
        protected readonly WaveFormat waveFormat;

        private BufferedWaveProvider meterInternalBuffer;
        private SampleChannel sampleChannel;
        private MeteringSampleProvider meterProvider;

        protected TimeSpan bufferedDuration;
        protected TimeSpan sampleDuration;

        private int bytesPerSample;

        /**
         * Events
         */
        /// <summary>
        /// 
        /// </summary>
        public event WaveProviderStereoFilterSampleCallback FilterSampleCallback;

        /**
         * Properties
         */
        /// <summary>
        /// If true, always read the amount of data requested, padding with zeroes if necessary.
        /// By default is set to true.
        /// </summary>
        public bool ReadFully { get; set; }

        /// <summary>
        /// Buffer length in bytes.
        /// </summary>
        public int BufferLength { get; set; }

        /// <summary>
        /// Buffer duration.
        /// </summary>
        public TimeSpan BufferDuration
        {
            get { return TimeSpan.FromSeconds((double)BufferLength / WaveFormat.AverageBytesPerSecond); }
            set { BufferLength = (int)(value.TotalSeconds * WaveFormat.AverageBytesPerSecond); }
        }

        /// <summary>
        /// If true, when the buffer is full, start throwing away data.
        /// if false, AddSamples will throw an exception when buffer is full.
        /// </summary>
        public bool DiscardOnBufferOverflow { get; set; }

        /// <summary>
        /// The number of buffered bytes.
        /// </summary>
        public int BufferedBytes
        {
            get { return circularBuffer == null ? 0 : circularBuffer.Count; }
        }

        /// <summary>
        /// Buffered Duration.
        /// </summary>
        public TimeSpan BufferedDuration
        {
            get { return bufferedDuration; }
        }

        /// <summary>
        /// Sample rate duration.
        /// </summary>
        public TimeSpan SampleDuration
        {
            get { return sampleDuration; }
        }

        /// <summary>
        /// Gets the <see cref="WaveFormat"/>.
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return waveFormat; }
        }

        /// <summary>
        /// Gets the internal <see cref="MeteringSampleProvider"/> for this <see cref="AudioWaveProvider"/>.
        /// </summary>
        public MeteringSampleProvider MeterProvider
        {
            get { return meterProvider; }
        }

        /// <summary>
        /// Gets or sets the flag indicating whether we ignore filtering.
        /// </summary>
        public bool IgnoreFilter
        {
            get; set;
        }

        /** 
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="AudioWaveProvider"/> class.
        /// </summary>
        /// <param name="waveFormat">WaveFormat</param>
        public AudioWaveProvider(WaveFormat waveFormat)
        {
            this.waveFormat = waveFormat;
            this.BufferLength = waveFormat.AverageBytesPerSecond * 5;
            this.ReadFully = true;
            this.DiscardOnBufferOverflow = true;
            this.bytesPerSample = waveFormat.BitsPerSample / 8;
            this.IgnoreFilter = false;

            this.meterInternalBuffer = new BufferedWaveProvider(waveFormat);
            this.meterInternalBuffer.DiscardOnBufferOverflow = true;
            this.sampleChannel = new SampleChannel(meterInternalBuffer);
            this.meterProvider = new MeteringSampleProvider(sampleChannel);

            this.bufferedDuration = new TimeSpan();
            this.sampleDuration = new TimeSpan();
        }

        /// <summary>
        /// Internal helper to extract bytes from the given buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="start"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        protected void ReadSamples(byte[] buffer, int start, out double left, out double right)
        {
            if (bytesPerSample == 4)
            {
                left = BitConverter.ToSingle(buffer, start);
                if (waveFormat.Channels > 1)
                    right = BitConverter.ToSingle(buffer, start + bytesPerSample);
                else
                    right = left;
            }
            else if (bytesPerSample == 2)
            {
                left = BitConverter.ToInt16(buffer, start) / 32768.0;
                if (waveFormat.Channels > 1)
                    right = BitConverter.ToInt16(buffer, start + bytesPerSample) / 32768.0;
                else
                    right = left;
            }
            else if (bytesPerSample == 1)
            {
                left = buffer[start] / 127.0;
                if (waveFormat.Channels > 1)
                    right = buffer[start + bytesPerSample] / 127.0;
                else
                    right = left;
            }
            else
                throw new InvalidOperationException(String.Format("Unsupported bytes per sample: {0}", bytesPerSample));
        }

        /// <summary>
        /// Internal helper to inject bytes into the given buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="start"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        protected void WriteSamples(byte[] buffer, int start, double left, double right)
        {
            if (bytesPerSample == 4)
            {
                Array.Copy(BitConverter.GetBytes((float)left), 0, buffer, start, bytesPerSample);
                if (waveFormat.Channels > 1)
                    Array.Copy(BitConverter.GetBytes((float)right), 0, buffer, start + bytesPerSample, bytesPerSample);
            }
            else if (bytesPerSample == 2)
            {
                Array.Copy(BitConverter.GetBytes((short)(left * 32768.0)), 0, buffer, start, bytesPerSample);
                if (waveFormat.Channels > 1)
                    Array.Copy(BitConverter.GetBytes((short)(right * 32768.0)), 0, buffer, start + bytesPerSample, bytesPerSample);
            }
            else if (bytesPerSample == 1)
            {
                Array.Copy(BitConverter.GetBytes((byte)(left * 127.0)), 0, buffer, start, bytesPerSample);
                if (waveFormat.Channels > 1)
                    Array.Copy(BitConverter.GetBytes((byte)(right * 127.0)), 0, buffer, start + bytesPerSample, bytesPerSample);
            }
            else
                throw new InvalidOperationException(String.Format("Unsupported bytes per sample: {0}", bytesPerSample));
        }

        /// <summary>
        /// Adds samples. Takes a copy of buffer, so that buffer can be reused if necessary.
        /// </summary>
        public virtual void AddSamples(byte[] buffer, int offset, int count)
        {
            byte[] temp = buffer;

            // create buffer here to allow user to customise buffer length
            if (circularBuffer == null)
                circularBuffer = new CircularBuffer(BufferLength);

            // apply individual sample filter
            if (FilterSampleCallback != null && !IgnoreFilter)
            {
                temp = new byte[buffer.Length];
                int sampleCount = buffer.Length / (bytesPerSample * waveFormat.Channels);
                for (int sample = 0; sample < sampleCount; sample++)
                {
                    int start = sample * bytesPerSample * waveFormat.Channels;
                    double leftSmp = 0.0, rightSmp = 0.0;
                    ReadSamples(buffer, start, out leftSmp, out rightSmp);
                    FilterSampleCallback(ref leftSmp, ref rightSmp);
                    WriteSamples(temp, offset + start, leftSmp, rightSmp);
                }
            }

            var written = circularBuffer.Write(temp, offset, count);
            if (written < count && !DiscardOnBufferOverflow)
                throw new InvalidOperationException("Buffer full");

            // calculate durations
            bufferedDuration += TimeSpan.FromSeconds((double)count / WaveFormat.AverageBytesPerSecond); // buffer
            sampleDuration += TimeSpan.FromSeconds((double)count / WaveFormat.SampleRate); // sample

            // add samples to the meter buffer
            this.meterInternalBuffer.AddSamples(temp, offset, count);
        }

        /// <summary>
        /// Reads from this <see cref="IWaveProvider"/>. Will always return count bytes, since we will 
        /// zero-fill the buffer if not enough available.
        /// </summary>
        public virtual int Read(byte[] buffer, int offset, int count)
        {
            // meter read as well
            float[] temp = new float[count];
            this.meterProvider.Read(temp, offset, count);

            int read = 0;
            if (circularBuffer != null) // not yet created
                read = circularBuffer.Read(buffer, offset, count);

            TimeSpan bufferedDiff = TimeSpan.FromSeconds((double)read / WaveFormat.AverageBytesPerSecond); // buffer
            bufferedDuration -= bufferedDiff;
            TimeSpan sampleDiff = TimeSpan.FromSeconds((double)read / WaveFormat.SampleRate); // sample
            sampleDuration -= sampleDiff;

            if (ReadFully && read < count)
            {
                // zero the end of the buffer
                Array.Clear(buffer, offset + read, count - read);
                read = count;
            }

            return read;
        }

        /// <summary>
        /// 
        /// </summary>
        public void TriggerMeter()
        {
            float[] temp = new float[meterInternalBuffer.BufferedBytes];
            this.meterProvider.Read(temp, 0, temp.Length);
        }

        /// <summary>
        /// Discards all audio from the buffer.
        /// </summary>
        public void ClearBuffer()
        {
            if (circularBuffer != null)
                circularBuffer.Reset();

            this.bufferedDuration = new TimeSpan();
            this.sampleDuration = new TimeSpan();
        }

        /// <summary>
        /// Helper to block a thread while waiting for samples in this buffer.
        /// </summary>
        public void WaitForSamples()
        {
            while (HasSamples())
                Thread.Sleep(1);
        }

        /// <summary>
        /// Helper to determine if this buffer has samples in it.
        /// </summary>
        /// <returns></returns>
        public bool HasSamples()
        {
            int bytesPerSample = WaveFormat.BitsPerSample / 8 * WaveFormat.Channels;
            return (BufferedBytes < bytesPerSample);
        }

        /// <summary>
        /// Helper to wait for new samples on the provider.
        /// </summary>
        /// <param name="source"></param>
        public void WaitForSample()
        {
            int bytesPerSample = WaveFormat.BitsPerSample / 8 * WaveFormat.Channels;

            while (BufferedBytes < bytesPerSample)
                Thread.Sleep(1);
        }
    } // public class AudioWaveProvider : IWaveProvider
} // namespace RepeaterController
