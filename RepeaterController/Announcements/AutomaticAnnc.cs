/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;

using NAudio.Wave;

using RepeaterController.DSP;

namespace RepeaterController.Announcements
{
    /// <summary>
    /// Enumeration of automated time announcement playback intervals.
    /// </summary>
    public enum TimeInterval
    {
        EveryHalfHour,
        EveryHour,
        Every3Hours,
        Every12Hours
    } // public enum TimeInterval

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public struct annc_ret_t
    {
        /**
         * Fields
         */
        /// <summary>
        /// Name of the announcement.
        /// </summary>
        [DataMember]
        public string Name;
        /// <summary>
        /// Start time the announcement should be run on.
        /// </summary>
        [DataMember]
        public double StartRun;
        /// <summary>
        /// Interval the announcement plays at.
        /// </summary>
        [DataMember]
        public double Interval;
        /// <summary>
        /// Last time the announcement was run.
        /// </summary>
        [DataMember]
        public double LastRun;
        /// <summary>
        /// Next time announcement was run.
        /// </summary>
        [DataMember]
        public double NextRun;
        /// <summary>
        /// Name of the text file to synthesize or wave file to playback.
        /// </summary>
        [DataMember]
        public string Filename;
        /// <summary>
        /// Raw string to synthesize.
        /// </summary>
        [DataMember]
        public string RawSyntheizedText;
        /// <summary>
        /// Flag indicating whether this announcement is a wave file or not.
        /// </summary>
        [DataMember]
        public bool IsWaveFile;
        /// <summary>
        /// Flag indicating whether we're supplying the text directly rather then from a file.
        /// </summary>
        [DataMember]
        public bool IsSuppliedText;
        /// <summary>
        /// Unique Id (used by RepeaterController)
        /// </summary>
        [DataMember]
        public Guid guid;

        /**
         * Methods
         */
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Annoucement Name = " + Name + ", StartRun = " + DateTime.FromOADate(StartRun) + ", Interval = " + DateTime.FromOADate(Interval) + ", LastRun = " + DateTime.FromOADate(LastRun) +
                ", NextRun = " + DateTime.FromOADate(NextRun) + ", Filename = " + Filename + ", IsWaveFile = " + IsWaveFile + ", IsSuppliedText = " + IsSuppliedText;
        }
    } // public struct annc_ret_t

    /* DO NOT REMOVE, BACKWARD COMPAT. */
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public struct Announcement
    {
        /* stub */
    } // public struct Announcement

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    public delegate void AnnouncementPlayback(object sender, string name);

    /// <summary>
    /// Helper class to automatically transmit announcements on an interval.
    /// </summary>
    public class AutomaticAnnc
    {
        /**
         * Fields
         */
        private Thread anncThread;
        private bool threadExecute = false;

        private MainWindow wnd;
        private RepeaterOptions optionsModal;

        private SAPITTS tts;
        private static List<annc_ret_t> announcements;
        internal DateTime everyHour;

        /// <summary>
        /// Gets the wave provider used for this <see cref="AutomaticAnnc"/>.
        /// </summary>
        public AudioWaveProvider WaveProvider;

        /**
         * Properties
         */
        /// <summary>
        /// Gets or sets the list of announcements to play.
        /// </summary>
        public static List<annc_ret_t> Announcements
        {
            get { return announcements; }
            set { announcements = value; }
        }

        /// <summary>
        /// Flag indicating whether the automatic time announcement should be played.
        /// </summary>
        public static bool TimeAnnouncement
        {
            get;
            set;
        }

        /// <summary>
        /// Flag indicating whether the automatic time announcement greeting should be played.
        /// </summary>
        public static bool TimeAnnouncementGreeting
        {
            get;
            set;
        }

        /// <summary>
        /// Flag indicating whether the automatic time announcement should be played as military time.
        /// </summary>
        public static bool TimeAnnouncementMil
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value indicating the interval at which to announce the time.
        /// </summary>
        public static TimeInterval TimeAnnouncementInterval
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the flag indicating if the courtesy tone should be played after an announcement is complete.
        /// </summary>
        public static bool UseCourtesyTone
        {
            get;
            set;
        }

        /// <summary>
        /// Flag indicating if an announcement is playing.
        /// </summary>
        public static bool IsPlayingAnnc
        {
            get;
            set;
        }

        /**
         * Events
         */
        /// <summary>
        /// Event that occurs when an announcment is played back.
        /// </summary>
        public event AnnouncementPlayback PlayingAnnouncment;

        /**
         * Methods
         */
        /// <summary>
        /// Static initializers of the <see cref="AutomaticAnnc"/> class.
        /// </summary>
        static AutomaticAnnc()
        {
            announcements = new List<annc_ret_t>();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AutomaticAnnc"/> class.
        /// </summary>
        /// <param name="tts"></param>
        /// <param name="provider"></param>
        /// <param name="wnd"></param>
        /// <param name="options"></param>
        public AutomaticAnnc(SAPITTS tts, MainWindow wnd, RepeaterOptions options)
        {
            this.tts = tts;

            this.WaveProvider = null;

            this.wnd = wnd;
            this.optionsModal = options;

            this.everyHour = DateTime.Now;
        }

        /// <summary>
        /// Starts the automatic announcement thread.
        /// </summary>
        public void Start()
        {
            if (this.anncThread != null)
                return;

            this.anncThread = new Thread(ThreadStart);
            this.threadExecute = true;
            this.anncThread.Start();
        }

        /// <summary>
        /// Stops the automatic announcement thread.
        /// </summary>
        public void Stop()
        {
            if (this.anncThread != null)
            {
                this.threadExecute = false;
                this.anncThread.Abort();
                this.anncThread.Join();
                this.anncThread = null;
            }
        }

        /// <summary>
        /// Plays the internal automated time announcement.
        /// </summary>
        public void PlayTimeAnnc()
        {
            // no function if MDC console and not in link radio mode
            if (optionsModal.Options.MDCConsoleOnly && !optionsModal.Options.AllowConsoleAnncDTMF)
                return;

            DateTime now = DateTime.Now;
            everyHour = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);

            string systemGreet = string.Empty;
            string timeGreet = string.Empty;

            if (PlayingAnnouncment != null)
                PlayingAnnouncment(this, "Standard Automatic Time");

            // inject the system name
            if (optionsModal.Options.UseSystemName)
                systemGreet = "<s>" + optionsModal.Options.SystemName + "</s>";

            // less then 12 and greater then 6 is 'morning'
            if (TimeAnnouncementGreeting)
            {
                if ((now.Hour < 12) || (now.Hour >= 6))
                    timeGreet = "<s>Good morning.</s>";
                if ((now.Hour > 12) || (now.Hour <= 18))
                    timeGreet = "<s>Good afternoon.</s>";
                if ((now.Hour > 18) || (now.Hour <= 5))
                    timeGreet = "<s>Good evening.</s>";
            }

            string timeString = now.ToString("h:mm tt");
            if (TimeAnnouncementMil)
                timeString = now.ToString("HH:mm");

            if (!tts.VoiceInitFailed)
            {
                string ssml = "<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"en-US\">";
                ssml += systemGreet;
                ssml += timeGreet;
                if (TimeAnnouncementMil)
                    ssml += "<s>The time is, <say-as interpret-as=\"time\" format=\"hms24\">" + timeString + "</say-as></s>";
                else
                    ssml += "<s>The time is, <say-as interpret-as=\"time\" format=\"hms12\">" + timeString + "</say-as></s>";
                ssml += "</speak>";

                tts.SpeakSsml(ssml, MainWindow.FixedAnncVol);
            }
            else
                Messages.Trace("voice init failed, cannot synthesize voice!");

            IsPlayingAnnc = true;
        }

        /// <summary>
        /// Play the announcement.
        /// </summary>
        /// <param name="annc"></param>
        public void PlayRepeaterAnnc(annc_ret_t annc, DateTime now)
        {
            // no function if MDC console and not in link radio mode
            if (optionsModal.Options.MDCConsoleOnly && !optionsModal.Options.AllowConsoleAnncDTMF)
                return;

            if (WaveProvider == null)
                throw new ArgumentNullException();

            DateTime interval = DateTime.FromOADate(annc.Interval);
            annc.LastRun = now.ToOADate();
            annc.NextRun = (now + new TimeSpan(interval.Hour, interval.Minute, interval.Second)).ToOADate();

            int idx = -1;
            foreach (annc_ret_t a in announcements)
                if (a.Name.ToLower() == annc.Name.ToLower())
                {
                    idx = announcements.IndexOf(a);
                    break;
                }

            if (idx != -1)
                announcements[idx] = annc;
            else
            {
                Messages.Trace("couldn't find [" + annc.Name + "] announcement in the list! got idx " + idx);
                Messages.Trace(annc.ToString());
            }

            if (PlayingAnnouncment != null)
                PlayingAnnouncment(this, annc.Name);

            if (annc.IsWaveFile)
            {
                AudioFileReader reader = new AudioFileReader(annc.Filename);
                long bufferLen = reader.Length;
                IWaveProvider provider = reader;
                if (reader.WaveFormat.SampleRate != provider.WaveFormat.SampleRate)
                {
                    provider = new WaveFormatConversionStream(provider.WaveFormat, reader);
                    bufferLen = ((WaveFormatConversionStream)provider).Length;
                }

                if (provider.WaveFormat.BitsPerSample == 32)
                {
                    WaveFloatTo16Provider floatTo16 = new WaveFloatTo16Provider(provider);
                    byte[] bSamples = new byte[bufferLen];
                    floatTo16.Read(bSamples, 0, bSamples.Length);

                    WaveProvider.AddSamples(bSamples, 0, bSamples.Length);
                }
                else
                {
                    byte[] bSamples = new byte[bufferLen];
                    provider.Read(bSamples, 0, bSamples.Length);
                    
                    WaveProvider.ClearBuffer();
                    WaveProvider.AddSamples(bSamples, 0, bSamples.Length);
                }

                reader.Close();
                reader = null;
            }
            else
            {
                if (!tts.VoiceInitFailed)
                {
                    if (annc.IsSuppliedText)
                    {
                        string ssml = annc.RawSyntheizedText;

                        // check if the ssml starts with <speak, if not we assume unformatted text
                        if (!ssml.StartsWith("<speak"))
                        {
                            ssml = ssml.Insert(0, "<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"en-US\">");
                            ssml = ssml.Insert(ssml.Length, "</speak>");
                        }

                        tts.SpeakSsml(ssml, MainWindow.FixedAnncVol);
                    }
                    else
                    {
                        if (File.Exists(annc.Filename))
                        {
                            TextReader reader = File.OpenText(annc.Filename);
                            string ssml = reader.ReadToEnd();

                            // check if the ssml starts with <speak, if not we assume unformatted text
                            if (!ssml.StartsWith("<speak"))
                            {
                                ssml = ssml.Insert(0, "<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"en-US\"><s>");
                                ssml = ssml.Insert(ssml.Length, "</s></speak>");
                            }

                            tts.SpeakSsml(ssml, MainWindow.FixedAnncVol);

                            reader.Close();
                            reader = null;
                        }
                        else
                        {
                            Messages.Trace("text file [" + annc.Filename + "] does not exist!");
                            Messages.Trace(annc.ToString());
                        }
                    }
                }
                else
                    Messages.Trace("voice init failed, cannot synthesize voice!");
            }

            IsPlayingAnnc = true;
        }

        /// <summary>
        /// Internal function containing the thread main entry point.
        /// </summary>
        private void ThreadStart()
        {
            while (this.threadExecute)
            {
                DateTime now = DateTime.Now;

                // are we playing the time announcement?
                if (TimeAnnouncement && !MainWindow.IsRepeaterRx)
                {
                    TimeSpan span = now.Subtract(everyHour);
                    if (TimeAnnouncementInterval == TimeInterval.EveryHalfHour)
                    {
                        if (span.Minutes >= 30)
                            PlayTimeAnnc();
                    }
                    else if (TimeAnnouncementInterval == TimeInterval.EveryHour)
                    {
                        if (span.Hours >= 1)
                            PlayTimeAnnc();
                    }
                    else if (TimeAnnouncementInterval == TimeInterval.Every3Hours)
                    {
                        if (span.Hours >= 2)
                            PlayTimeAnnc();
                    }
                    else if (TimeAnnouncementInterval == TimeInterval.Every12Hours)
                    {
                        if (span.Hours >= 12)
                            PlayTimeAnnc();
                    }
                }

                // are announcements disabled?
                if (!this.optionsModal.Options.DisableAnnouncements && !MainWindow.IsRepeaterRx)
                {
                    for (int i = 0; i < announcements.Count; i++)
                    {
                        lock (announcements)
                        {
                            annc_ret_t annc = announcements[i];
                            TimeSpan span = now.Subtract(DateTime.FromOADate(annc.LastRun));

                            DateTime dtInterval = DateTime.FromOADate(annc.Interval);
                            TimeSpan interval = new TimeSpan(dtInterval.Hour, dtInterval.Minute, dtInterval.Second);

                            // have we reached the interval?
                            if (span.TotalMilliseconds >= interval.TotalMilliseconds)
                            {
                                // play announcement
                                PlayRepeaterAnnc(annc, now);
                            }
                        }

                    }
                }

                // sleep ~1s
                Thread.Sleep(1000);
            }
        }
    } // public class AutomaticAnnc
} // namespace RepeaterController.Announcements
