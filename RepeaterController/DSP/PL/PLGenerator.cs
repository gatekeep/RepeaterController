/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;

using NAudio.Wave;

namespace RepeaterController.DSP.PL
{
    /// <summary>
    /// Implements a sample provider that synthesizes a sinusoidal PL tone.
    /// </summary>
    public class PLGenerator : ISampleProvider
    {
        /**
         * Fields
         */
        private const double LOW_PASS_FREQ = 280;
        private const double TWO_PI = 2 * Math.PI;
        private const double GAIN = 0.10;

        private ulong nSample = 0;

        private readonly WaveFormat waveFormat;
        private readonly WaveFormat waveFormat16;

        private bool generateReverseBurst = false;
        private bool plGenerated;
        private double plTone = 67.0;

        /**
         * Properties
         */
        /// <summary>
        /// The waveformat of this WaveProvider (same as the source)
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return waveFormat; }
        }

        /// <summary>
        /// Flag indicating whether samples have been generated.
        /// </summary>
        public bool Generated
        {
            get { return plGenerated; }
        }

        /// <summary>
        /// Gets or sets the gain of the generated signal.
        /// </summary>
        public double Gain
        {
            get;
            set;
        }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="PLGenerator"/> class.
        /// </summary>
        /// <param name="sampleRate"></param>
        public PLGenerator(int sampleRate)
        {
            waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, 1);
            waveFormat16 = new WaveFormat(sampleRate, 16, 1);
            this.Gain = GAIN;
        }

        /// <summary>
        /// Sets the given PL tone.
        /// </summary>
        /// <param name="pl"></param>
        /// <param name="ignoreChecking"></param>
        public void SetTone(double pl, bool ignoreChecking = false)
        {
            bool validTone = false;
            if (pl > 254.1)
                throw new ArgumentOutOfRangeException();

            if (!ignoreChecking)
            {
                // make sure tone is valid
                foreach (double tone in PLPureTones.ToneList)
                    if (pl == tone)
                        validTone = true;

                if (!validTone)
                    throw new ArgumentException();
            }

            nSample = 0;
            plTone = pl;
            plGenerated = true;
            return;
        }

        /// <summary>
        /// Gets a byte array of samples with the selected PL tone for the given duration (in MS).
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        public byte[] GetSamples(int duration)
        {
            if (!plGenerated)
                return null;

            generateReverseBurst = false;

            SampleToAudioProvider16 smpTo16 = new SampleToAudioProvider16(this);
            BiQuad lowPass = new LowpassFilter(waveFormat.SampleRate, LOW_PASS_FREQ);
            smpTo16.FilterSampleCallback += (ref double smp) =>
            {
                smp = lowPass.Process((float)smp);
            };

            int sDuration = SamplesToMS.MSToSampleBytes(waveFormat16, duration);
            byte[] sigBuf = new byte[sDuration];
            smpTo16.Read(sigBuf, 0, sDuration);

            return sigBuf;
        }

        /// <summary>
        /// Gets a byte array of samples with the reverse burst for the selected PL tone for the given duration (in MS).
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        public byte[] GetRevSamples(int duration)
        {
            if (!plGenerated)
                return null;

            generateReverseBurst = true;

            SampleToAudioProvider16 smpTo16 = new SampleToAudioProvider16(this);
            BiQuad lowPass = new LowpassFilter(waveFormat.SampleRate, LOW_PASS_FREQ);
            smpTo16.FilterSampleCallback += (ref double smp) =>
            {
                smp = lowPass.Process((float)smp);
            };

            int sDuration = SamplesToMS.MSToSampleBytes(waveFormat16, duration);
            byte[] sigBuf = new byte[sDuration];
            smpTo16.Read(sigBuf, 0, sDuration);

            return sigBuf;
        }

        /// <summary>
        /// Reads from this provider.
        /// </summary>
        public int Read(float[] buffer, int offset, int count)
        {
            if (!plGenerated)
                return 0;

            int outIndex = offset;
            double phase = 0.0;
            if (generateReverseBurst)
                phase += 2.09;

            // generator current value
            double multiple, sampleValue;

            // complete buffer
            for (int sampleCount = 0; sampleCount < count / waveFormat.Channels; sampleCount++)
            {
                // sinus generator
                multiple = TWO_PI * plTone / waveFormat.SampleRate;
                sampleValue = Gain * Math.Sin(nSample * multiple + phase);

                if ((nSample + 1) < ulong.MaxValue)
                    nSample++;
                else
                    nSample = 0;

                buffer[outIndex++] = (float)sampleValue;
            }
            return count;
        }
    } // public class PLGenerator : ISampleProvider
} // namespace RepeaterController.DSP.PL
