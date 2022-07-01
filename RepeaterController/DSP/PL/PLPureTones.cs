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
    /// Implements the pure tone analyzer for PL.
    /// </summary>
    public class PLPureTones
    {
        /**
         * Fields
         */
        public static readonly List<double> ToneList = new List<double>() {
            67.0, 69.3,
            71.9, 74.4, 77.0, 79.7,
            82.5, 85.4, 88.5,
            91.5, 94.8, 97.4,
            100.0, 103.5, 107.2,
            110.9, 114.8, 118.8,
            123.0, 127.3,
            131.8, 136.5,
            141.3, 146.2,
            151.4, 156.7,
            162.2, 167.9,
            173.8, 179.9,
            186.2,
            192.8,
            203.5, 206.5,
            210.7, 218.1,
            225.7, 229.1,
            233.6,
            241.8,
            250.3, 254.1,
        };
        private readonly Dictionary<double, AmplitudeEstimator> estimators;

        /** 
         * Properties
         */
        /// <summary>
        /// 
        /// </summary>
        public double this[double tone] => estimators[tone].Amplitude;

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="PLPureTones"/> class.
        /// </summary>
        /// <param name="sampleRate"></param>
        /// <param name="sampleBlockSize"></param>
        public PLPureTones(int sampleRate, int sampleBlockSize)
        {
            estimators = ToneList.ToDictionary(tone => tone, tone => new AmplitudeEstimator(tone, sampleRate, sampleBlockSize));
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
        public double FindStrongestTone() => StrongestOf(ToneList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pureTones"></param>
        /// <returns></returns>
        private double StrongestOf(IEnumerable<double> pureTones)
        {
            return pureTones.Select(tone => new { Tone = tone, estimators[tone].Amplitude }).OrderBy(result => result.Amplitude).Select(result => result.Tone).Last();
        }
    } // public class PureTones
} // namespace RepeaterController.DSP.PL
