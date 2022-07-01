/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.Collections.Generic;

namespace RepeaterController.DSP
{
    /// <summary>
    /// Interface defines an NAudio sample source.
    /// </summary>
    public interface ISampleSource
    {
        /**
         * Fields
         */
        /// <summary>
        /// Flag indicating whether the sample source has samples.
        /// </summary>
        bool HasSamples { get; }

        /// <summary>
        /// Gets the raw samples.
        /// </summary>
        IEnumerable<float> Samples { get; }
    } // public interface ISampleSource
} // namespace RepeaterController
