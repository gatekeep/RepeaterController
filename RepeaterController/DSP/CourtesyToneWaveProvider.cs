/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
/**
 * Based on code from the NAudio project. (https://github.com/naudio/NAudio)
 * Licensed under the Ms-PL license.
 */
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;

using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudio.Utils;

namespace RepeaterController.DSP
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class Multitone
    {
        /// <summary>
        /// Order index for the tone.
        /// </summary>
        [DataMember]
        public int Index;
        /// <summary>
        /// Frequency of the tone.
        /// </summary>
        [DataMember]
        public double Frequency;
        /// <summary>
        /// Function Frequency of the tone.
        /// </summary>
        public double FrequencyTwo;
        /// <summary>
        /// Length of the tone (in milliseconds).
        /// </summary>
        [DataMember]
        public int Length;
        /// <summary>
        /// Silence length /after/ the tone (in milliseconds).
        /// </summary>
        [DataMember]
        public int SilenceLength;
    } // public class Multitone

    /// <summary>
    /// Provides a buffered store of samples. Read method will return queued samples or fill buffer with zeroes.
    /// </summary>
    public class CourtesyToneWaveProvider : AudioWaveProvider
    {
        /**
         * Fields
         */
        public const int SILENCE_LEN_MS = 500;
        private const float GAIN = 0.95f;

        /**
         * Properties
         */
        /// <summary>
        /// Flag indicating whether we are using multi-tone or not.
        /// </summary>
        public bool UseMultitone { get; set; }
        /// <summary>
        /// List of tines used for a multi-tone courtesy tone.
        /// </summary>
        public List<Multitone> Tones { get; set; }

        /// <summary>
        /// Pitch of the single-tone courtesy tone.
        /// </summary>
        public double CourtesyPitch { get; set; }
        /// <summary>
        /// Length of the single-tone courtesy tone.
        /// </summary>
        public int CourtesyLength { get; set; }
        /// <summary>
        /// Initial silence delay of the single-tone courtesy tone.
        /// </summary>
        public int CourtesyDelay { get; set; }

        /** 
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="AudioWaveProvider"/> class.
        /// </summary>
        /// <param name="waveFormat">WaveFormat</param>
        public CourtesyToneWaveProvider(WaveFormat waveFormat) : base(waveFormat)
        {
            this.UseMultitone = false;
            this.Tones = new List<Multitone>();

            this.CourtesyDelay = 100;
            this.CourtesyPitch = 500;
            this.CourtesyLength = 100;
        }

        /// <summary>
        /// Internal helper to generate the raw samples.
        /// </summary>
        /// <returns></returns>
        private int GenerateSamples()
        {
            int count = 0;

            // generate silence
            int sDuration = SamplesToMS.MSToSampleBytes(WaveFormat, SILENCE_LEN_MS);
            List<byte> silence = new List<byte>();
            for (int i = 0; i < sDuration; i++)
            {
                if (WaveFormat.BitsPerSample == 16)
                {
                    silence.Add(64);
                    silence.Add(0);
                    i++;
                }
                else if (WaveFormat.BitsPerSample == 8)
                    silence.Add(128); // 128 should be audio "zero" in byte format
                else
                    silence.Add(0);
            }

            // inject leader silence
            AddSamples(silence.ToArray(), 0, silence.Count);
            count += silence.Count;

            if (UseMultitone)
            {
                List<byte> samples = new List<byte>();

                // iterate through the defined tones and play
                foreach (Multitone tone in Tones)
                {
                    // generate courtesy tone
                    SignalGenerator gen = new SignalGenerator(waveFormat.SampleRate, waveFormat.Channels);
                    gen.Frequency = tone.Frequency;
                    gen.FrequencyEnd = tone.Frequency;
                    gen.Type = SignalGeneratorType.Sin;
                    gen.Gain = GAIN;

                    if (tone.FrequencyTwo > 0.0d)
                    {
                        gen.Type = SignalGeneratorType.SinFunction;
                        gen.FrequencyTwo = tone.FrequencyTwo;
                    }

                    SampleToWaveProvider16 smpTo16 = new SampleToWaveProvider16(gen);

                    sDuration = SamplesToMS.MSToSampleBytes(waveFormat, tone.Length);

                    byte[] sigBuf = new byte[sDuration];
                    smpTo16.Read(sigBuf, 0, sDuration);

                    for (int i = 0; i < sDuration; i++)
                        samples.Add(sigBuf[i]);

                    // add tail silence
                    sDuration = SamplesToMS.MSToSampleBytes(waveFormat, tone.SilenceLength);
                    for (int i = 0; i < sDuration; i++)
                    {
                        if (WaveFormat.BitsPerSample == 16)
                        {
                            samples.Add(64);
                            samples.Add(0);
                            i++;
                        }
                        else if (WaveFormat.BitsPerSample == 8)
                            samples.Add(128); // 128 should be audio "zero" in byte format
                        else
                            samples.Add(0);
                    }
                }

                // add raw audio samples for tone
                byte[] bSigBuf = samples.ToArray();

                Messages.Trace("generated courtesy tone [multi-tone] WaveFormat = " + waveFormat.ToString() + ", sampleLen = " + bSigBuf.Length);
                AddSamples(bSigBuf, 0, bSigBuf.Length);
                count += bSigBuf.Length;
            }
            else
            {
                // generate courtesy tone
                SignalGenerator gen = new SignalGenerator(waveFormat.SampleRate, waveFormat.Channels);
                gen.Frequency = CourtesyPitch;
                gen.FrequencyEnd = CourtesyPitch;
                gen.Type = SignalGeneratorType.Sin;
                gen.Gain = GAIN;

                SampleToWaveProvider16 smpTo16 = new SampleToWaveProvider16(gen);

                List<byte> samples = new List<byte>();
                sDuration = SamplesToMS.MSToSampleBytes(waveFormat, CourtesyLength);

                byte[] sigBuf = new byte[sDuration];
                smpTo16.Read(sigBuf, 0, sDuration);

                for (int i = 0; i < sDuration; i++)
                    samples.Add(sigBuf[i]);

                // add delay to buffer
                int delayLength = SamplesToMS.MSToSampleBytes(waveFormat, CourtesyDelay);
                byte[] delayBuffer = new byte[delayLength];
                for (int i = 0; i < delayLength; i++)
                {
                    if (WaveFormat.BitsPerSample == 16)
                    {
                        delayBuffer[i] = 64;
                        i++;
                        delayBuffer[i] = 0;
                    }
                    else if (WaveFormat.BitsPerSample == 8)
                        delayBuffer[i] = 128; // 128 should be audio "zero" in byte format
                    else
                        delayBuffer[i] = 0;
                }

                // add raw audio samples for tone
                byte[] bSigBuf = sigBuf.ToArray();
                byte[] bSamples = new byte[bSigBuf.Length + delayBuffer.Length];
                Buffer.BlockCopy(delayBuffer, 0, bSamples, 0, delayBuffer.Length);
                Buffer.BlockCopy(bSigBuf, 0, bSamples, delayBuffer.Length, bSigBuf.Length);

                Messages.Trace("generated courtesy tone [single-tone] WaveFormat = " + waveFormat.ToString() + ", sampleLen = " + bSigBuf.Length);
                AddSamples(bSamples, 0, bSamples.Length);
                count += bSamples.Length;
            }

            // inject trail silence
            AddSamples(silence.ToArray(), 0, silence.Count);
            count += silence.Count;

            return count;
        }

        /// <summary>
        /// Generates the courtesy-tone.
        /// </summary>
        /// <returns></returns>
        public byte[] Generate()
        {
            int count = GenerateSamples();

            // generate and return byte array
            byte[] ret = new byte[count];
            base.Read(ret, 0, count);

            return ret;
        }

        /// <summary>
        /// Reads from this <see cref="IWaveProvider"/>. Will always return count bytes, since we will 
        /// zero-fill the buffer if not enough available.
        /// </summary>
        public override int Read(byte[] buffer, int offset, int count)
        {
            int smpCount = GenerateSamples();
            if (count > smpCount)
                count = smpCount;

            base.Read(buffer, offset, count);
            return count;
        }
    } // public class CourtesyToneWaveProvider : AudioWaveProvider
} // namespace RepeaterController
