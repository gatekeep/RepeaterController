/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;

using NAudio.Wave;

namespace RepeaterController.DSP
{
    /// <summary>
    /// 
    /// </summary>
    public class SampleBlockProvider
    {
        /**
         * Fields
         */
        private readonly ISampleProvider source;

        /**
         * Properties
         */
        /// <summary>
        /// 
        /// </summary>
        public int BlockSize { get; }

        /// <summary>
        /// 
        /// </summary>
        public float[] CurrentBlock { get; }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleBlockProvider"/> class.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="blockSize"></param>
        public SampleBlockProvider(ISampleProvider source, int blockSize)
        {
            this.source = source;
            BlockSize = blockSize;
            CurrentBlock = new float[blockSize];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ReadNextBlock()
        {
            return source.Read(CurrentBlock, 0, BlockSize);
        }
    } // public class SampleBlockProvider
} // namespace RepeaterController
