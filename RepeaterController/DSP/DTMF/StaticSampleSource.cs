/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.Collections.Generic;
using System.Linq;

using NAudio.Wave;

namespace RepeaterController.DSP.DTMF
{
    /// <summary>
    /// 
    /// </summary>
    public class StaticSampleSource : ISampleSource
    {
        /**
         * Fields
         */
        private readonly SampleBlockProvider samples;
        private int numSamplesRead;

        /** 
         * Properties
         */
        /// <summary>
        /// 
        /// </summary>
        public bool HasSamples => numSamplesRead >= samples.BlockSize;

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<float> Samples
        {
            get
            {
                if (!HasSamples)
                    throw new InvalidOperationException("No more data available");

                numSamplesRead = samples.ReadNextBlock();
                return samples.CurrentBlock.Take(numSamplesRead);
            }
        }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="StaticSampleSource"/> class.
        /// </summary>
        /// <param name="source"></param>
        public StaticSampleSource(WaveStream source)
        {
            samples = source.ToSampleProvider().AsMono().SampleWith(DtmfDetector.SampleRate).Blockwise(DtmfDetector.SampleBlockSize);
            // optimistically assume that we are going to read at least BlockSize bytes.
            numSamplesRead = samples.BlockSize;
        }
    } // public class StaticSampleSource : ISampleSource 
} // namespace RepeaterController.DSP.DTMF
