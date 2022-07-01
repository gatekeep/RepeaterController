/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.Collections.Generic;

using NAudio.Wave;

using RepeaterController.DSP.PL;

namespace RepeaterController.DSP.DPL
{
    /// <summary>
    /// Implements a sample provider that synthesizes a DPL signal.
    /// </summary>
    public class DPLGenerator : ISampleProvider
    {
        /**
         * Fields
         */
        private const int BITS_PER_WORD = 23;
        private const double BAUD_RATE = 134.3;
        private const double LOW_PASS_FREQ = 280;
        private const double GAIN = 0.10;

        private ulong nSample = 0;

        public static readonly List<string> ToneList = new List<string>() {
            "023", "025", "026",
            "031", "032", "036",
            "043", "047",
            "051", "053", "054",
            "065",
            "071", "072", "073", "074",
            "114", "115", "116",
            "122", "125",
            "131", "132", "134",
            "143", "145",
            "152", "155", "156",
            "162", "165",
            "172", "174",
            "205",
            "212",
            "223", "225", "226",
            "243", "244", "245", "246",
            "251", "252", "255",
            "261", "263", "265", "266",
            "271", "274",
            "306", "311", "315",
            "325",
            "331", "332",
            "343", "346",
            "351", "356",
            "364", "365",
            "371",
            "411", "412", "413",
            "423",
            "431", "432",
            "445", "446",
            "452", "454", "455",
            "462", "464", "465", "466",
            "503", "506",
            "516",
            "523", "526",
            "532",
            "546",
            "565",
            "606",
            "612",
            "624", "627",
            "631", "632",
            "654",
            "662", "664",
            "703",
            "712",
            "723",
            "731", "732", "734",
            "743",
            "754"
        };

        private DPLEncoder encoder;
        private readonly WaveFormat waveFormat;
        private readonly WaveFormat waveFormat16;
        private float samplesPerBit;

        private PLGenerator plGenerator;

        private double gain;
        private bool dplGenerated;
        private byte[] bitPattern;

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
            get { return dplGenerated; }
        }

        /// <summary>
        /// Gets or sets the gain of the generated signal.
        /// </summary>
        public double Gain
        {
            get { return gain; }
            set { this.plGenerator.Gain = gain = value; }
        }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="DPLGenerator"/> class.
        /// </summary>
        /// <param name="sampleRate"></param>
        public DPLGenerator(int sampleRate)
        {
            waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, 1);
            waveFormat16 = new WaveFormat(sampleRate, 16, 1);
            samplesPerBit = (float)(waveFormat.SampleRate / BAUD_RATE);
            this.encoder = new DPLEncoder();
            this.plGenerator = new PLGenerator(sampleRate);
            this.plGenerator.SetTone(BAUD_RATE, true);
            this.Gain = GAIN;
        }

        /// <summary>
        /// Generates the DPL bit array to synthesize when this sample provider is read.
        /// </summary>
        /// <param name="dpl"></param>
        public void GenerateDPL(int dpl)
        {
            if (dpl > 777)
                throw new ArgumentOutOfRangeException();

            // generate actual digits
            string digits = Convert.ToString(dpl);
            if (digits.Length <= 1)
                digits = digits.Insert(0, "00");
            if (digits.Length <= 2)
                digits = digits.Insert(0, "0");

            GenerateDPL(digits);
        }

        /// <summary>
        /// Generates the DPL bit array to synthesize when this sample provider is read.
        /// </summary>
        /// <param name="dpl"></param>
        public void GenerateDPL(string dpl)
        {
            // split the string into parts
            byte d1 = Convert.ToByte(dpl[0].ToString());
            byte d2 = Convert.ToByte(dpl[1].ToString());
            byte d3 = Convert.ToByte(dpl[2].ToString());

            bitPattern = encoder.GenerateDPLBits(d1, d2, d3);
            dplGenerated = true;
            return;
        }

        /// <summary>
        /// Gets a byte array of samples with the selected PL tone for the given duration (in MS).
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        public byte[] GetSamples(int duration)
        {
            if (!dplGenerated)
                return null;

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
            if (!plGenerator.Generated)
                return null;

            return plGenerator.GetRevSamples(duration);
        }

        /// <summary>
        /// Reads from this provider.
        /// </summary>
        public int Read(float[] buffer, int offset, int count)
        {
            if (!dplGenerated)
                return 0;
            if (bitPattern == null)
                throw new ArgumentNullException();

            int outIndex = offset;
            int sampleBit = 0;

            double sampleValue;

            // complete buffer
            for (int sampleCount = 0; sampleCount < count / waveFormat.Channels; sampleCount++)
            {
                if (nSample > samplesPerBit)
                {
                    nSample = 0;
                    sampleBit++;

                    if (sampleBit > bitPattern.Length - 1)
                        sampleBit = 0;
                }

                double entropy = Math.Sin(2 * Math.PI * BAUD_RATE * nSample / waveFormat.SampleRate) + Math.Cos(2 * Math.PI * BAUD_RATE * nSample / waveFormat.SampleRate);
                if (bitPattern[sampleBit] == 1)
                    sampleValue = Gain * (1.0 + (entropy / (8 * Math.PI)));
                else
                    sampleValue = Gain * (-1.0 + (entropy / (8 * Math.PI)));

                if ((nSample + 1) < ulong.MaxValue)
                    nSample++;
                else
                    nSample = 0;

                buffer[outIndex++] = (float)sampleValue;
            }
            return count;
        }
    } // public class DPLGenerator : ISampleProvider
} // namespace RepeaterController.DSP.DPL
