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

namespace RepeaterController.DSP.PL
{
    /// <summary>
    /// Helper class to detect a PL tone based on amplitude.
    /// </summary>
    public class PLDetector
    {
        /**
         * Fields
         */
        private const double AMPLITUDE_THRESHOLD = 2.5;

        private readonly PLPureTones pureTones = new PLPureTones(SampleRate, SampleBlockSize);

        /**
         * Properties
         */
        /// <summary>
        /// Gets the sample rate of the PL detector.
        /// </summary>
        public static int SampleRate { get; } = 8000;

        /// <summary>
        /// Gets the sample block size.
        /// </summary>
        public static int SampleBlockSize { get; } = SampleRate * 50 / 1000;

        /**
         * Methods
         */
        /// <summary>
        /// Analyze a new set of audio samples for PL tones.
        /// </summary>
        /// <param name="samples"></param>
        public double Analyze(IEnumerable<float> samples)
        {
            pureTones.ResetAmplitudes();

            // iterate through the samples
            foreach (var sample in samples.Take(SampleBlockSize))
                pureTones.AddSample(sample);

            return GetToneFromAmplitudes();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private double GetToneFromAmplitudes()
        {
            double tone = pureTones.FindStrongestTone();

            if (pureTones[tone] < AMPLITUDE_THRESHOLD)
                return 0.0f;

            return tone;
        }
    } // public class PLDetector
} // namespace RepeaterController.DSP.PL
