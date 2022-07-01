/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;

using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace RepeaterController.DSP
{
    /// <summary>
    /// Class extension for <see cref="ISampleProvider"/>.
    /// </summary>
    public static class SampleProviderExtensions
    {
        /**
         * Methods
         */
        /// <summary>
        /// Extension function to resample this provider into another sample rate.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="sampleRate"></param>
        public static ISampleProvider SampleWith(this ISampleProvider source, int sampleRate)
        {
            return source.WaveFormat.SampleRate != sampleRate ? new WdlResamplingSampleProvider(source, sampleRate) : source;
        }

        /// <summary>
        /// Extension function to convert this sample provider to mono.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static ISampleProvider AsMono(this ISampleProvider source)
        {
            return source.WaveFormat.Channels != 1 ? new MultiplexingSampleProvider(new[] { source }, 1) : source;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="blockSize"></param>
        /// <returns></returns>
        public static SampleBlockProvider Blockwise(this ISampleProvider source, int blockSize)
        {
            return new SampleBlockProvider(source, blockSize);
        }
    } // public static class SampleProviderExtensions
} // namespace RepeaterController
