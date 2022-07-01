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

namespace RepeaterController.DSP
{
    /// <summary>
    /// Implements a streaming sample source.
    /// </summary>
    public class StreamingSampleSource : ISampleSource
    {
        /**
         * Fields
         */
        private readonly AudioWaveProvider sourceBuffer;
        private readonly ISampleProvider samples;

        /**
         * Properties
         */
        /// <summary>
        /// Flag indicating whether or not this source has samples.
        /// </summary>
        public bool HasSamples { get; } = true;

        /// <summary>
        /// Gets the raw samples.
        /// </summary>
        public IEnumerable<float> Samples
        {
            get
            {
                var buffer = new float[1];

                while (HasSamples)
                {
                    sourceBuffer.WaitForSample();
                    samples.Read(buffer, 0, 1);
                    yield return buffer[0];
                }
            }
        }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamingSampleSource"/> class.
        /// </summary>
        /// <param name="source"></param>
        public StreamingSampleSource(AudioWaveProvider source, int sampleRate)
        {
            sourceBuffer = source;
            samples = source.ToSampleProvider().AsMono().SampleWith(sampleRate);
        }

        /// <summary>
        /// Reads a sample off the source.
        /// </summary>
        /// <returns></returns>
        public float GetSample()
        {
            var buffer = new float[1];

            samples.Read(buffer, 0, 1);
            return buffer[0];
        }
    } // public class StreamingSampleSource : ISampleSource
} // namespace RepeaterController
