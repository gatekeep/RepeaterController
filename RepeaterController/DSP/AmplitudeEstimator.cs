/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;

namespace RepeaterController.DSP
{
    /// <summary>
    /// Helper class to analyze an audio sample and estimate the amplitude.
    /// </summary>
    public class AmplitudeEstimator
    {
        /**
         * Fields
         */
        private readonly double c;
        private double s1;
        private double s2;
        private Lazy<double> amplitude;

        /// <summary>
        /// 
        /// </summary>
        public double Amplitude => amplitude.Value;

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="AmplitudeEstimator"/> class.
        /// </summary>
        /// <param name="targetFrequency"></param>
        /// <param name="sampleRate"></param>
        /// <param name="numberOfSamples"></param>
        public AmplitudeEstimator(double targetFrequency, double sampleRate, int numberOfSamples)
        {
            Reset();

            double k = targetFrequency / sampleRate * numberOfSamples;
            c = 2.0 * Math.Cos(2.0 * Math.PI * k / numberOfSamples);
        }

        /// <summary>
        /// Adds a new sample for amplitude estimation.
        /// </summary>
        /// <param name="sample"></param>
        public void Add(float sample)
        {
            double s0 = sample + c * s1 - s2;
            s2 = s1;
            s1 = s0;
        }

        /// <summary>
        /// Resets the estimator.
        /// </summary>
        public void Reset()
        {
            s1 = s2 = .0;
            amplitude = new Lazy<double>(() => Math.Sqrt(s1 * s1 + s2 * s2 - s1 * s2 * c));
        }
    } // public class AmplitudeEstimator
} // namespace RepeaterController
