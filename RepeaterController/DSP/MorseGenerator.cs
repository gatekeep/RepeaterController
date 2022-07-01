/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.Collections.Generic;

using NAudio.Wave.SampleProviders;

namespace RepeaterController.DSP
{
    /// <summary>
    /// Helper class to generate 'morse'/CW audio samples.
    /// </summary>
    public class MorseGenerator
    {
        /**
         * Fields
         */
        private const float GAIN = 0.95f;

        public int Frequency;
        public int WordPerMinute;

        public int Length;

        private AudioWaveProvider waveProvider;

        private char[] letters = new char[] {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.', ',', ':', '?', '\\', '-' , '/', '(', ')', '"', '@', '=', ' '};

        private string[] morsecode = new string[] {".-","-...","-.-.","-..",".","..-.","--.","....","..",".--        -",
            "-.-",".-..", "--","-.","---",".--.","--.-",".-.","...", "-","..-","...-",".--","-..-","-.--        ","--..",".----",
            "..---", "...--","....-",".....","-....","--...","---..","----.","-----",".-.-.-","--..--",
            "---...","..--..",".----.","-....-","-..-.","-.--.-", "-.--.-",".-..-.",".--.-        .", "-...-" ,"/" };

        private const int DITS_PER_WORD = 50;  // based on "PARIS "

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="MorseGenerator"/> class.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="freq"></param>
        /// <param name="wpm"></param>
        public MorseGenerator(AudioWaveProvider provider, int freq, int wpm)
        {
            this.Frequency = freq;
            this.WordPerMinute = wpm;

            this.waveProvider = provider;
        }

        /// <summary>
        /// Converts the given textual string to a morse code string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string ConvertTextToMorse(string text)
        {
            text = text.ToUpper();
            string result = "";
            int index = -1;
            for (int i = 0; i <= text.Length - 1; i++)
            {
                index = Array.IndexOf(letters, text[i]);
                if (index != -1)
                    result += morsecode[index] + " ";
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="morse"></param>
        /// <param name="wpm"></param>
        /// <param name="farnsworth"></param>
        /// <returns></returns>
        private int[] FarnsworthMorseTimings(string morse, int wpm, int farnsworth)
        {
            int dit = 60000 / (DITS_PER_WORD * wpm);
            int r = wpm / farnsworth;  // slow down the spaces by this ratio
            return GeneralMorseTimings(morse, dit, 3 * dit, dit, 3 * dit * r, 7 * dit * r);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="morse"></param>
        /// <param name="dit"></param>
        /// <param name="dah"></param>
        /// <param name="ditSpace"></param>
        /// <param name="charSpace"></param>
        /// <param name="wordSpace"></param>
        /// <returns></returns>
        private int[] GeneralMorseTimings(string morse, int dit, int dah, int ditSpace, int charSpace, int wordSpace)
        {
            List<int> times = new List<int>();
            char c;
            for (var i = 0; i < morse.Length; i++)
            {
                c = morse[i];
                if (c == '.' || c == '-')
                {
                    if (c == '.')
                        times.Add(dit);
                    else
                        times.Add(dah);
                    times.Add(ditSpace);
                }
                else if (c == ' ')
                {
                    times.Reverse();
                    times.RemoveAt(0);
                    times.Reverse();
                    times.Add(charSpace);
                }
                else if (c == '/')
                {
                    times.Reverse();
                    times.RemoveAt(0);
                    times.Reverse();
                    times.Add(wordSpace);
                }
            }
            times.RemoveAt(times.Count - 1);  // take off the last ditSpace           
            return times.ToArray();
        }

        /// <summary>
        /// Internal helper to generate the raw audio samples for the morse identifier.
        /// </summary>
        /// <param name="timings"></param>
        /// <param name="freq"></param>
        /// <returns></returns>
        private byte[] GenerateMorseSamples(int[] timings, int freq)
        {
            List<byte> buf = new List<byte>();
            SignalGenerator gen = new SignalGenerator(waveProvider.WaveFormat.SampleRate, waveProvider.WaveFormat.Channels);
            gen.Type = SignalGeneratorType.Sin;
            gen.Frequency = freq;

            SampleToWaveProvider16 smpTo16 = new SampleToWaveProvider16(gen);

            double counterIncrementAmount = Math.PI * 2 * freq / waveProvider.WaveFormat.SampleRate;
            double on = GAIN;

            for (int t = 0; t < timings.Length; t += 1)
            {
                int duration = SamplesToMS.MSToSampleBytes(waveProvider.WaveFormat, timings[t]);
                gen.Gain = on;

                byte[] sigBuf = new byte[duration];
                smpTo16.Read(sigBuf, 0, duration);

                for (int i = 0; i < duration; i++)
                    buf.Add(sigBuf[i]);

                on = GAIN - on;
            }

            return buf.ToArray();
        }

        /// <summary>
        /// Converts the given string to more code and adds the raw audio samples to the wave provider.
        /// </summary>
        /// <param name="str"></param>
        public void GenerateString(string str)
        {
            string morse = ConvertTextToMorse(str);
            int[] timings = FarnsworthMorseTimings(morse, WordPerMinute, WordPerMinute);
            if (timings.Length > 0)
            {
                byte[] samples = GenerateMorseSamples(timings, Frequency);
                waveProvider.AddSamples(samples, 0, samples.Length);
            }
        }
    } // public class MorseGenerator
} // namespace RepeaterController
