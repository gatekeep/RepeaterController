/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;

using NAudio.Wave;

namespace RepeaterController.DSP
{
    /// <summary>
    /// Helper class implementing getting audio samples from the SAPI Text-To-Speech API.
    /// </summary>
    public class SAPITTS : IDisposable
    {
        /**
         * Fields
         */
        public static double SynthAudioGain = 3.9;

        private SpeechSynthesizer synth;
        private MemoryStream stream;
        private WaveFormat waveFormat;
        private AudioWaveProvider waveProvider;

        private bool initFailed;

        /**
         * Properties
         */
        /// <summary>
        /// Gets the wave format for the Text-To-Speech.
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return waveFormat; }
        }
        
        /// <summary>
        /// Gets the wave provider for the Text-To-Speech.
        /// </summary>
        public AudioWaveProvider WaveProvider
        {
            get { return waveProvider; }
        }

        /// <summary>
        /// Gets the audio rate.
        /// </summary>
        public int Rate
        {
            get { return synth.Rate; }
            set { synth.Rate = value; }
        }

        /// <summary>
        /// Gets the list of installed voices.
        /// </summary>
        public ReadOnlyCollection<InstalledVoice> Voices
        {
            get { return synth.GetInstalledVoices(); }
        }

        /// <summary>
        /// Flag indicating voice initialization failed.
        /// </summary>
        public bool VoiceInitFailed
        {
            get { return initFailed;  }
        }

        /**
         * Events
         */
        /// <summary>
        /// Event that occurs when speech is finished speaking.
        /// </summary>
        public event EventHandler SpeakCompleted;

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="SAPITTS"/> class.
        /// </summary>
        /// <param name="format"></param>
        public SAPITTS(WaveFormat format)
        {
            this.waveFormat = format;
            this.waveProvider = new AudioWaveProvider(waveFormat);
            this.waveProvider.DiscardOnBufferOverflow = true;
            this.waveProvider.BufferLength = SamplesToMS.MSToSampleBytes(waveFormat, 90000);

            SimpleCompressor compress = new SimpleCompressor(5.0, 10.0, waveFormat.SampleRate);
            compress.Threshold = 16;
            compress.Ratio = 4;
            compress.MakeUpGain = SynthAudioGain;

            this.waveProvider.FilterSampleCallback += (ref double leftSmp, ref double rightSmp) =>
            {
                compress.Process(ref leftSmp, ref rightSmp);
            };

            this.initFailed = false;

            try
            {
                synth = new SpeechSynthesizer();
                synth.SpeakCompleted += synth_SpeakCompleted;
                SelectVoice(null);
            }
            catch (Exception e)
            {
                synth = null;
                initFailed = true;
                Messages.TraceException("failed setting up speech synthesizer", e);
            }
        }

        /// <summary>
        /// Occurs when the speech synthesizer finishes speaking.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void synth_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            MemoryStream audioStream = new MemoryStream();
            stream.WriteTo(audioStream);

            audioStream.Flush();
            audioStream.Seek(0, SeekOrigin.Begin);

            // make sure the stream has some data
            if (audioStream.Length > 0)
            {
                IWaveProvider streamProvider = null;
                WaveFileReader reader = new WaveFileReader(audioStream);
                if (reader.WaveFormat.SampleRate != waveFormat.SampleRate)
                    streamProvider = new WaveFormatConversionStream(waveFormat, reader);
                else
                    streamProvider = reader;

                int ttsLen = SamplesToMS.MSToSampleBytes(waveFormat, (int)reader.TotalTime.TotalMilliseconds);

                if (streamProvider.WaveFormat.BitsPerSample == 32)
                {
                    waveProvider.ClearBuffer();

                    WaveFloatTo16Provider floatTo16 = new WaveFloatTo16Provider(streamProvider);
                    byte[] bSamples = new byte[ttsLen];
                    floatTo16.Read(bSamples, 0, bSamples.Length);

                    waveProvider.AddSamples(bSamples, 0, bSamples.Length);
                }
                else
                {
                    waveProvider.ClearBuffer();

                    byte[] bSamples = new byte[ttsLen];
                    streamProvider.Read(bSamples, 0, bSamples.Length);

                    waveProvider.AddSamples(bSamples, 0, bSamples.Length);
                }

                if (SpeakCompleted != null)
                    SpeakCompleted(sender, e);

                reader.Close();
                audioStream.Close();
            }
        }

        /// <summary>
        /// Helper to select a synthesizer speech voice.
        /// </summary>
        /// <param name="name"></param>
        public void SelectVoice(string name)
        {
            if (synth == null)
                return;

            if (name != null)
                try
                {
                    synth.SelectVoice(name);
                }
                catch (ArgumentException)
                {
                    // select default voice
                    synth.SelectVoice(Voices[0].VoiceInfo.Name);
                }
        }

        /// <summary>
        /// Synthesizes voice from the given textual string.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="volumePercent"></param>
        public void Speak(string text, int volumePercent)
        {
            if (synth == null)
                return;

            synth.SetOutputToNull();

            stream = new MemoryStream();
            synth.SetOutputToWaveStream(stream);

            synth.Volume = volumePercent;
            synth.SpeakAsync(text);
        }

        /// <summary>
        /// Synthesizes voice from the given SSML string.
        /// </summary>
        /// <param name="ssml"></param>
        /// <param name="volumePercent"></param>
        public void SpeakSsml(string ssml, int volumePercent)
        {
            if (synth == null)
                return;

            synth.SetOutputToNull();

            stream = new MemoryStream();
            synth.SetOutputToWaveStream(stream);

            synth.Volume = volumePercent;
            synth.SpeakSsmlAsync(ssml);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (synth != null)
            {
                synth.Dispose();
                synth = null;
            }

            waveFormat = null;
            
            if (waveProvider != null)
            {
                waveProvider.ClearBuffer();
                waveProvider = null;
            }
        }
    } // public class SAPITTS : IDisposable
} // namespace RepeaterController
