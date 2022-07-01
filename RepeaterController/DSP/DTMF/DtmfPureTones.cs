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

namespace RepeaterController.DSP.DTMF
{
    /// <summary>
    /// Implements the pure 2-tone analyzer for DTMF.
    /// </summary>
    public class DtmfPureTones
    {
        /**
         * Fields
         */
        private static readonly IEnumerable<int> LowPureTones = new[] { 697, 770, 852, 941 };
        private static readonly IEnumerable<int> HighPureTones = new[] { 1209, 1336, 1477, 1633 };
        private readonly Dictionary<int, AmplitudeEstimator> estimators;

        /** 
         * Properties
         */
        /// <summary>
        /// 
        /// </summary>
        public double this[int tone] => estimators[tone].Amplitude;

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="DtmfPureTones"/> class.
        /// </summary>
        /// <param name="sampleRate"></param>
        /// <param name="sampleBlockSize"></param>
        public DtmfPureTones(int sampleRate, int sampleBlockSize)
        {
            estimators = LowPureTones.Concat(HighPureTones).ToDictionary(tone => tone, tone => new AmplitudeEstimator(tone, sampleRate, sampleBlockSize));
        }

        /// <summary>
        /// Helper to reset the amplitude estimator.
        /// </summary>
        public void ResetAmplitudes()
        {
            foreach (AmplitudeEstimator estimator in estimators.Values)
                estimator.Reset();
        }

        /// <summary>
        /// Helper to add a audio sample to the amplitude estimator.
        /// </summary>
        /// <param name="sample"></param>
        public void AddSample(float sample)
        {
            foreach (AmplitudeEstimator estimator in estimators.Values)
                estimator.Add(sample);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int FindStrongestHighTone() => StrongestOf(HighPureTones);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int FindStrongestLowTone() => StrongestOf(LowPureTones);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pureTones"></param>
        /// <returns></returns>
        private int StrongestOf(IEnumerable<int> pureTones)
        {
            return pureTones.Select(tone => new { Tone = tone, estimators[tone].Amplitude }).OrderBy(result => result.Amplitude).Select(result => result.Tone).Last();
        }
    } // public class PureTones
} // namespace RepeaterController.DSP.DTMF
