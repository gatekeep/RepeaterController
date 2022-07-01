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
    /// Helper class to detect DTMF tones based on amplitude.
    /// </summary>
    public class DtmfDetector
    {
        /**
         * Fields
         */
        private const double AMPLITUDE_THRESHOLD = 10.0;

        private readonly DtmfPureTones pureTones = new DtmfPureTones(SampleRate, SampleBlockSize);

        /**
         * Properties
         */
        /// <summary>
        /// Gets the sample rate of the DTMF detector.
        /// </summary>
        public static int SampleRate { get; } = 8000;

        /// <summary>
        /// Gets the sample block size.
        /// </summary>
        /// <remarks>A DTMF Tone has to have a length of at least 40 ms: 8000 Hz * 0.04 s = 320</remarks>
        public static int SampleBlockSize { get; } = SampleRate * 40 / 1000;

        /**
         * Methods
         */
        /// <summary>
        /// Analyze a new set of audio samples for DTMF tones.
        /// </summary>
        /// <param name="samples"></param>
        public DtmfTone Analyze(IEnumerable<float> samples)
        {
            pureTones.ResetAmplitudes();

            // iterate through the samples
            foreach (var sample in samples.Take(SampleBlockSize))
                pureTones.AddSample(sample);

            return GetDtmfToneFromAmplitudes();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DtmfTone GetDtmfToneFromAmplitudes()
        {
            var highTone = pureTones.FindStrongestHighTone();
            var lowTone = pureTones.FindStrongestLowTone();

            if (pureTones[highTone] < AMPLITUDE_THRESHOLD || pureTones[lowTone] < AMPLITUDE_THRESHOLD)
                return DtmfTone.None;

            return DtmfClassification.For(highTone, lowTone);
        }
    } // public class DtmfDetector
} // namespace RepeaterController.DSP.DTMF
