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

namespace RepeaterController.DSP.DPL
{
    /// <summary>
    /// Helper class to detect a DPL signal.
    /// </summary>
    public class DPLDetector
    {
        /**
         * Fields
         */
        private const int BITS_PER_WORD = 23;
        private const double BAUD_RATE = 134.3;

        private float average;
        private int bitCounter;
        private float center;

        private float max;
        private float min;
        private float previousAverage;
        private byte[] receivedCode = new byte[BITS_PER_WORD];
        private int sampleCount;
        private float samplesPerBit = (float)(SampleRate / BAUD_RATE);

        private List<float> samples = new List<float>();

        /**
         * Properties
         */
        /// <summary>
        /// Gets the sample rate of the DPL detector.
        /// </summary>
        public static int SampleRate { get; } = 8000;

        /// <summary>
        /// Gets the sample block size.
        /// </summary>
        public static int SampleBlockSize { get; } = SampleRate * 50 / 1000;

        /// <summary>
        /// Gets if a DPL code is available.
        /// </summary>
        public bool CodeAvailable { get; private set; } = false;

        /// <summary>
        /// Gets the recieved DPL code.
        /// </summary>
        public byte[] ReceivedCode
        {
            get { return receivedCode; }
        }

        /**
         * Methods
         */
        /// <summary>
        /// Analyze a new set of audio samples for PL tones.
        /// </summary>
        /// <param name="samples"></param>
        public void Analyze(IEnumerable<float> samples)
        {
            this.samples = new List<float>();

            // iterate through the samples
            foreach (var sample in samples.Take(SampleBlockSize))
                this.samples.Add((float)sample);

            AnalyzeBuffer(this.samples.ToArray());
        }

        /// <summary>
        /// Analyze a new set of audio samples for PL tones.
        /// </summary>
        /// <param name="samples"></param>
        public void AnalyzeBuffer(float[] buffer)
        {
            if (buffer == null)
                return;

            if (buffer.Length > 0)
            {
                int length = buffer.Length;

                this.max = float.MinValue;
                this.min = float.MaxValue;

                for (int i = 0; i < length; i++)
                {
                    float a = buffer[i];
                    this.max = Math.Max(max, a);
                    this.min = Math.Min(min, a);
                }

                center = (max + min) * 0.5f;

                for (int j = 0; j < length; j++)
                    this.SampleAdd(buffer[j]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sample"></param>
        private void SampleAdd(float sample)
        {
            this.average = (average + sample) * 0.5f;
            this.sampleCount++;

            //Messages.Trace("avg = " + average + ", count = " + sampleCount + ", center = " + center);

            if ((previousAverage > center) && (average <= center))
                sampleCount = ((int)samplesPerBit) / 2;

            previousAverage = average;
            if (sampleCount >= samplesPerBit)
            {
                sampleCount = 0;
                if (average < center)
                    this.AddBit(true);
                else
                    this.AddBit(false);

                average = center;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bit"></param>
        private void AddBit(bool bit)
        {
            if (bitCounter >= BITS_PER_WORD)
            {
                bitCounter = 0;
                CodeAvailable = true;
            }

            if (bit)
                this.receivedCode[bitCounter] = 1;
            else
                this.receivedCode[bitCounter] = 0;

            bitCounter++;
        }
    } // public class DPLDetector
} // namespace RepeaterController.DSP.DPL
