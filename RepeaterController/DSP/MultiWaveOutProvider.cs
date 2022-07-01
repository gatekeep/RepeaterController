/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.Collections.Generic;

using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace RepeaterController.DSP
{
    /// <summary>
    /// 
    /// </summary>
    public class MultiWaveOutProvider :  IDisposable
    {
        /**
         * Fields
         */
        private List<IWaveProvider> inputs;

        private Dictionary<int, WaveOut> waveOutputs;
        private Dictionary<int, AudioWaveProvider> waveOutputProvider;

        private int outputDeviceNumber;
        private bool disposed = false;
        private WaveFormat waveFormat;

        /**
         * Properties
         */
        /// <summary>
        /// <see cref="WaveStream.WaveFormat"/>
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return this.waveFormat; }
        }

        /// <summary>
        /// The number of inputs to this mixer
        /// </summary>
        public int InputCount
        {
            get { return this.inputs.Count; }
        }

        /// <summary>
        /// Flag indicating whether the mixer is playing.
        /// </summary>
        public bool IsPlaying { get; private set; }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiWaveOutProvider"/> class.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="outputDeviceNumber"></param>
        public MultiWaveOutProvider(WaveFormat format, int outputDeviceNumber)
        {
            this.waveFormat = format;
            this.outputDeviceNumber = outputDeviceNumber;

            this.inputs = new List<IWaveProvider>();
            this.waveOutputs = new Dictionary<int, WaveOut>();
            this.waveOutputProvider = new Dictionary<int, AudioWaveProvider>();
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
                foreach (KeyValuePair<int, WaveOut> wo in waveOutputs)
                {
                    if (wo.Value.PlaybackState == PlaybackState.Playing)
                        wo.Value.Stop();
                    wo.Value.Dispose();
                }
                waveOutputs.Clear();

                foreach (KeyValuePair<int, AudioWaveProvider> wp in waveOutputProvider)
                    wp.Value.ClearBuffer();
                waveOutputProvider.Clear();

                inputs.Clear();
            }

            disposed = true;
        }

        /// <summary>
        /// Start playback of all streams.
        /// </summary>
        public void StartPlayback()
        {
            foreach (KeyValuePair<int, WaveOut> output in waveOutputs)
                if (output.Value.PlaybackState != PlaybackState.Playing)
                    output.Value.Play();
        }

        /// <summary>
        /// Stop playback of all streams.
        /// </summary>
        public void StopPlayback()
        {
            foreach (KeyValuePair<int, WaveOut> output in waveOutputs)
                if (output.Value.PlaybackState != PlaybackState.Stopped)
                    output.Value.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cb"></param>
        public void ApplyMeteringCallback(Action<object, StreamVolumeEventArgs> cb)
        {
            foreach (KeyValuePair<int, AudioWaveProvider> kvp in waveOutputProvider)
                kvp.Value.MeterProvider.StreamVolume += (sender, e) => cb(sender, e);
        }

        /// <summary>
        /// Add a new input to the mixer.
        /// </summary>
        /// <param name="waveProvider">The wave input to add</param>
        /// <param name="waveIn"></param>
        /// <param name="handler"></param>
        /// <param name="filterCallback"></param>
        public void AddInputStream(IWaveProvider waveProvider, WaveIn waveIn = null, Action<WaveInEventArgs, AudioWaveProvider> handler = null, WaveProviderStereoFilterSampleCallback filterCallback = null)
        {
            lock (inputs)
            {
                this.inputs.Add(waveProvider);
                int idx = this.inputs.IndexOf(waveProvider);

                WaveOut waveOut = new WaveOut();
                waveOut.DeviceNumber = outputDeviceNumber;
                if (waveIn != null)
                {
                    AudioWaveProvider waveOutProvider = new AudioWaveProvider(waveFormat)
                    {
                        DiscardOnBufferOverflow = true
                    };
                    waveOutProvider.FilterSampleCallback += filterCallback;
                    waveOut.Init(waveOutProvider);
                    this.waveOutputProvider.Add(idx, waveOutProvider);

                    if (handler == null)
                    {
                        waveIn.DataAvailable += (sender, e) =>
                        {
                            waveOutProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
                            waveOutProvider.TriggerMeter();
                        };
                    }
                    else
                        waveIn.DataAvailable += (sender, e) => handler(e, waveOutProvider);
                }
                else
                    waveOut.Init(waveProvider);

                this.waveOutputs.Add(idx, waveOut);
            }
        }

        /// <summary>
        /// Remove an input from the mixer
        /// </summary>
        /// <param name="waveProvider">waveProvider to remove</param>
        public void RemoveInputStream(IWaveProvider waveProvider)
        {
            lock (inputs)
            {
                int idx = this.inputs.IndexOf(waveProvider);
                this.inputs.Remove(waveProvider);

                WaveOut wo = null;
                AudioWaveProvider wp = null;
                if (this.waveOutputs.TryGetValue(idx, out wo))
                {
                    this.waveOutputs.Remove(idx);
                    if (wo != null)
                    {
                        if (wo.PlaybackState == PlaybackState.Playing)
                            wo.Stop();
                        wo.Dispose();
                    }
                }

                if (this.waveOutputProvider.TryGetValue(idx, out wp))
                {
                    this.waveOutputProvider.Remove(idx);
                    if (wp != null)
                        wp.ClearBuffer();
                }
            }
        }

        /// <summary>
        /// Adds samples. Takes a copy of buffer, so that buffer can be reused if necessary.
        /// </summary>
        public void AddSamples(IWaveProvider waveProvider, byte[] buffer, int offset, int count)
        {
            int idx = inputs.IndexOf(waveProvider);
            if (idx <= -1)
                return;

            AudioWaveProvider provider;
            if (waveOutputProvider.TryGetValue(idx, out provider))
                provider.AddSamples(buffer, offset, count);
        }

        /// <summary>
        /// Discards all audio from the buffer.
        /// </summary>
        public void ClearBuffer(IWaveProvider waveProvider)
        {
            int idx = inputs.IndexOf(waveProvider);
            if (idx <= -1)
                return;

            AudioWaveProvider provider;
            if (waveOutputProvider.TryGetValue(idx, out provider))
                provider.ClearBuffer();
        }

        /// <summary>
        /// Discards all audio from the buffer.
        /// </summary>
        public void ClearBuffer()
        {
            foreach (KeyValuePair<int, AudioWaveProvider> buffers in waveOutputProvider)
                buffers.Value.ClearBuffer();
        }
    } // public class MultiOutputProvider : IWaveProvider
} // namespace RepeaterController.DSP
