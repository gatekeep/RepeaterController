/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using RepeaterController.Announcements;
using RepeaterController.DSP.MDC1200;
using RepeaterController.DSP.DTMF;
using RepeaterController.DSP.DPL;
using RepeaterController.DSP.PL;

using NAudio;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace RepeaterController.DSP
{
    /// <summary>
    /// This implements the main logic and audio processing class for the repeater.
    /// </summary>
    public class RepeaterWaveIn : IDisposable
    {
        /**
         * Fields
         */
        private bool hasDoneInitialCW = false;
        private bool hasHadTxDeKey = false;

        private const double LOW_PASS_FREQ = 3000;
        private const double HIGH_PASS_FREQ = 300;

        private const int REV_SQS_SAMPLE_LEN_MS = 180;
        private const int SQS_SAMPLE_LEN_MS = 360;
        private const int SQS_END_LEN_MS = 25;

        private const int DEKEY_TIMER = 250;
        public const int SILENCE_LEN_MS = 500;

        public static int AudioDropMS = 500;

        private WaveIn waveIn;
        private WaveIn linkWaveIn;

        private MultiWaveOutProvider multiWaveOut;

        private Stopwatch dropAudio;
        private Stopwatch linkDropAudio;
        private Stopwatch voxDekeyTimer;

        private bool disposed = false;
        private Thread captureWorker;
        private bool stopCaptureThread = false;
        private Thread txWatchDog;
        private bool stopWatchDogThread = false;
        private Thread mdcWorker;

        private SerialPort rxPort;
        private SerialPort txPort;

        private bool disableTransmitter = false;

        private bool haveLinkAudio = false;
        private bool repeatAudioTimeout = false;
        private bool repeatAudio = false;
        private bool audioDetect = false;
        private bool gotRACCode = false;
        private bool gotPLTone = false;

        private bool allowPL = true;

        private bool useLinkAudio = false;

        private double lastSQSAdd = 0.0;

        private WaveFormat waveFormat;
        private WaveFormat afskWaveFormat;
        private AudioWaveProvider waveProvider;
        private AudioWaveProvider linkWaveProvider;
        private AudioWaveProvider externalProvider;
        private AudioWaveProvider sqsProvider;
        private ISampleSource source;
        private ISampleSource linkSource;
        private ISampleSource externalSource;
        private AudioFileReader courtesyTone;

        private AutomaticCW cwInstance;
        private AutomaticAnnc anncInstance;

        private MDCGenerator mdcGenerator;
        private MDCWaveIn mdcProcessor;

        private PLGenerator plGenerator;
        private PLWaveIn plProcessor;

        private DPLGenerator dplGenerator;
        private DPLWaveIn dplProcessor;

        private DtmfWaveIn dtmfProcessor;

        private SAPITTS tts;

        /**
         * Properties
         */
        /// <summary>
        /// Milliseconds for the buffer. Recommended value is 100ms
        /// </summary>
        public int BufferMilliseconds { get; set; }

        /// <summary>
        /// Number of Buffers to use (usually 2 or 3)
        /// </summary>
        public int NumberOfBuffers { get; set; }

        /// <summary>
        /// Milliseconds for the buffer. Recommended value is 100ms
        /// </summary>
        public int AFSKBufferMilliseconds { get; set; }

        /// <summary>
        /// Number of Buffers to use (usually 2 or 3)
        /// </summary>
        public int AFSKNumberOfBuffers { get; set; }

        /// <summary>
        /// Flag indicating whether main input audio filtering is disabled or not.
        /// </summary>
        public bool DisableFiltering { get; set; }

        /// <summary>
        /// Gets the length of time a DTMF tone should be detected before signalling a DTMF detect.
        /// </summary>
        public double DTMFToneSeconds { get; set; }

        /// <summary>
        /// Flag indicating whether or not repeated audio caused a timeout.
        /// </summary>
        public bool RepeatAudioTimeout
        {
            get { return repeatAudioTimeout; }
        }

        /// <summary>
        /// Flag indicating whether the live analyzer is capturing samples.
        /// </summary>
        public bool IsRecording { get; private set; }

        /// <summary>
        /// Gets or sets the repeater options.
        /// </summary>
        public repeater_options_t Options { get; set; }

        /// <summary>
        /// Gets the last transmission time.
        /// </summary>
        public DateTime LastTx { get; private set; }

        /// <summary>
        /// Gets the instance of the <see cref="AutomaticCW"/> class.
        /// </summary>
        public AutomaticCW AutomaticCW
        {
            get { return cwInstance; }
            set
            {
                cwInstance = value;
                cwInstance.WaveProvider = externalProvider;
                cwInstance.PlayingCW += (sender, e) =>
                {
                    if (this.Options.EnableTxPL)
                    {
                        if (this.Options.DisablePLForId)
                            allowPL = false;    // this will get automatically reset when the external provider is done
                    }
                };
            }
        }

        /// <summary>
        /// Gets the instance of the <see cref="AutomaticAnnc"/> class.
        /// </summary>
        public AutomaticAnnc AutomaticAnnc
        {
            get { return anncInstance; }
            set
            {
                anncInstance = value;
                anncInstance.WaveProvider = externalProvider;
            }
        }

        /// <summary>
        /// Gets the external wave provider for supplying external audio to the <see cref="RepeaterWaveIn"/>.
        /// </summary>
        public AudioWaveProvider ExternalProvider
        {
            get { return externalProvider; }
        }

        /// <summary>
        /// Gets or sets the flag indicating whether to disable the transmitter.
        /// </summary>
        public bool DisableTransmitter
        {
            get { return disableTransmitter; }
            set
            {
                disableTransmitter = value;
                if (disableTransmitter)
                {
                    repeatAudio = false;
                    audioDetect = false;
                    DekeyTransmitter();

                    // fire disabled event
                    if (RepeaterDisabled != null)
                        RepeaterDisabled(this, new EventArgs());
                }
                else
                {
                    // fire enabled event
                    if (RepeaterEnabled != null)
                        RepeaterEnabled(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Gets whether the repeater has a transmit timeout condition.
        /// </summary>
        /// <remarks>Setting this will *ALWAYS* clear a timeout condition.</remarks>
        public bool ClearTransmitterTimeout
        {
            get { return repeatAudioTimeout; }
            set
            {
                DekeyTransmitter();
                ReleaseStates();
                this.repeatAudioTimeout = false;
            }
        }

        /// <summary>
        /// Returns the instance of the MDC1200 packet synthesizer.
        /// </summary>
        public MDCGenerator MDC
        {
            get { return mdcGenerator; }
        }

        /// <summary>
        /// Returns the instance of the PL synthesizer.
        /// </summary>
        public PLGenerator PL
        {
            get { return plGenerator; }
        }

        /// <summary>
        /// Returns the instnace of the DPL synthesizer.
        /// </summary>
        public DPLGenerator DPL
        {
            get { return dplGenerator; }
        }

        /// <summary>
        /// Returns the instnace of the SAPI TTS synthesizer.
        /// </summary>
        public SAPITTS TTS
        {
            get { return tts; }
        }

        /**
         * Events
         */
        /// <summary>
        /// Event that occurs when the repeater is idle.
        /// </summary>
        public event Action<object, EventArgs> RepeaterIdle;
        /// <summary>
        /// Event that occurs when the repeater is active.
        /// </summary>
        public event Action<object, double> RepeaterActive;

        /// <summary>
        /// Event that occurs when the repeater is keyed.
        /// </summary>
        public event Action<object, EventArgs> RepeaterKeyed;
        /// <summary>
        /// Event that occurs when the repeater is dekeyed.
        /// </summary>
        public event Action<object, EventArgs> RepeaterDekeyed;
        
        /// <summary>
        /// Event that occurs when all conditions to repeat audio are met.
        /// </summary>
        public event Action<object, EventArgs> RepeatingAudio;
        /// <summary>
        /// Event that occurs when all conditions to repeat audio are not met.
        /// </summary>
        public event Action<object, EventArgs> RepeatingAudioFailed;
        
        /// <summary>
        /// Event that occurs when the repeater transmitter is enabled.
        /// </summary>
        public event Action<object, EventArgs> RepeaterEnabled;
        /// <summary>
        /// Event that occurs when the repeater transmitter disabled.
        /// </summary>
        public event Action<object, EventArgs> RepeaterDisabled;

        /// <summary>
        /// Event that occurs when the repeater transmitter timeout occurs.
        /// </summary>
        public event Action<object, EventArgs> RepeaterTimeout;
        /// <summary>
        /// Event that occurs when the repeater transmitter timeout is cleared.
        /// </summary>
        public event Action<object, EventArgs> RepeaterTimeoutCleared;
        
        /// <summary>
        /// Event that occurs when the watch dog is inactive.
        /// </summary>
        public event Action<object, EventArgs> WatchDogInactive;
        /// <summary>
        /// Event that occurs when the watch dog times out.
        /// </summary>
        public event Action<object, EventArgs> WatchDogTimeout;

        /// <summary>
        /// Event that occurs when we detect the correct and valid MDC RAC code.
        /// </summary>
        public event Action<object> ValidRACCode;

        /// <summary>
        /// Event pass-thru for the MDC packet processor, where a MDC packet is detected.
        /// </summary>
        public event Action<object, int, MDCPacket, MDCPacket> MDCPacketDetected;
        /// <summary>
        /// Event pass-thru for the DTMF tone processor, where a DTMF tone is detected.
        /// </summary>
        public event Action<object, DtmfToneEnd> DTMFToneDetected;
        /// <summary>
        /// Event pass-thru for the PL tone processor, where a PL tone is detected.
        /// </summary>
        public event Action<object, double> PLToneDetected;
        /// <summary>
        /// Event pass-thru for the PL/DPL tone processors, where the squelch tone is lost.
        /// </summary>
        public event Action<object> PLToneLost;
        /// <summary>
        /// Event pass-thru for the PL tone processor, where a PL tone is detected.
        /// </summary>
        public event Action<object, List<int>> DPLToneDetected;

        /// <summary>
        /// Event that passes a defined log string.
        /// </summary>
        public event Action<object, string> LogEvent;

        /// <summary>
        /// 
        /// </summary>
        public event Action<object, StreamVolumeEventArgs> WaveInMeterSampling;
        /// <summary>
        /// 
        /// </summary>
        public event Action<object, StreamVolumeEventArgs> WaveOutMeterSampling;

        /// <summary>
        /// 
        /// </summary>
        public event Action<object, MDCWaveIn, byte[]> MDCSamples;

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="RepeaterWaveIn"/> class.
        /// </summary>
        public RepeaterWaveIn()
        {
            this.BufferMilliseconds = 35;
            this.NumberOfBuffers = 3;

            this.AFSKBufferMilliseconds = 500;
            this.AFSKNumberOfBuffers = 6;

            this.rxPort = null;
            this.txPort = null;

            this.repeatAudio = false;
            this.audioDetect = false;
            this.disableTransmitter = true;

            this.courtesyTone = null;

            this.multiWaveOut = null;
            
            this.waveIn = null;
            this.linkWaveIn = null;

            this.waveFormat = null;
            this.waveProvider = null;
            this.externalProvider = null;

            this.mdcGenerator = null;
            this.mdcProcessor = null;

            this.plGenerator = null;
            this.plProcessor = null;

            this.dplGenerator = null;
            this.dplProcessor = null;

            this.DTMFToneSeconds = 1;
            this.dtmfProcessor = null;

            this.tts = null;

            this.LastTx = DateTime.Now;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                InternalDisposeResources();
            }

            disposed = true;
        }

        /// <summary>
        /// Helper to internally dispose resources.
        /// </summary>
        private void InternalDisposeResources()
        {
            DekeyTransmitter();
            ReleaseStates();

            if (IsRecording)
                this.StopRecording();

            if (mdcWorker != null)
            {
                if (mdcWorker.IsAlive)
                {
                    mdcWorker.Abort();
                    mdcWorker.Join();
                }

                mdcWorker = null;
            }

            if (captureWorker != null)
            {
                if (captureWorker.IsAlive)
                {
                    captureWorker.Abort();
                    captureWorker.Join();
                }

                captureWorker = null;
            }

            if (txWatchDog != null)
            {
                if (txWatchDog.IsAlive)
                {
                    txWatchDog.Abort();
                    txWatchDog.Join();
                }

                txWatchDog = null;
            }

            if (this.mdcProcessor != null)
            {
                mdcProcessor.StopRecording();
                mdcProcessor.Dispose();
                mdcProcessor = null;
            }

            if (this.dtmfProcessor != null)
            {
                dtmfProcessor.StopRecording();
                dtmfProcessor.Dispose();
                dtmfProcessor = null;
            }

            if (this.plProcessor != null)
            {
                plProcessor.StopRecording();
                plProcessor.Dispose();
                plProcessor = null;
            }

            if (this.dplProcessor != null)
            {
                dplProcessor.StopRecording();
                dplProcessor.Dispose();
                dplProcessor = null;
            }

            if (this.tts != null)
            {
                this.tts.Dispose();
                this.tts = null;
            }

            if (useLinkAudio)
                if (linkWaveIn != null)
                    linkWaveIn.Dispose();

            if (waveIn != null)
                waveIn.Dispose();

            if (this.multiWaveOut != null)
            {
                multiWaveOut.Dispose();
                multiWaveOut = null;
            }

            if (this.courtesyTone != null)
            {
                courtesyTone.Dispose();
                courtesyTone = null;
            }
        }

        /// <summary>
        /// Helper to reset the courtesy tone file.
        /// </summary>
        public void ResetCourtesyToneFile()
        {
            if (this.Options.CourtesyUseFile)
            {
                if (this.courtesyTone != null)
                {
                    this.courtesyTone.Dispose();
                    this.courtesyTone = null;
                }

                if (this.Options.CourtesyFile != string.Empty)
                    this.courtesyTone = new AudioFileReader(this.Options.CourtesyFile);
            }
        }

        /// <summary>
        /// Helper to reset the state of this <see cref="RepeaterWaveIn"/>.
        /// </summary>
        /// <param name="waveFormat"></param>
        /// <param name="inputDeviceNumber"></param>
        /// <param name="outputDeviceNumber"></param>
        /// <param name="useLinkInput"></param>
        /// <param name="linkInputDeviceNumber"></param>
        public void Reset(WaveFormat waveFormat, int inputDeviceNumber, int outputDeviceNumber, bool useLinkInput = false, int linkInputDeviceNumber = 0)
        {
            BiQuad lo = new LowpassFilter(waveFormat.SampleRate, LOW_PASS_FREQ);
            BiQuad high = new HighpassFilter(waveFormat.SampleRate, HIGH_PASS_FREQ);
            BiQuad shelf = new HighShelfFilter(waveFormat.SampleRate, LOW_PASS_FREQ, -60.0);

            InternalDisposeResources();

            this.useLinkAudio = useLinkInput;

            this.dropAudio = new Stopwatch();
            this.linkDropAudio = new Stopwatch();
            this.voxDekeyTimer = new Stopwatch();

            this.waveFormat = waveFormat;
            this.afskWaveFormat = new WaveFormat(waveFormat.SampleRate, 16, 1);

            // initialize the multi waveout provider
            this.multiWaveOut = new MultiWaveOutProvider(waveFormat, outputDeviceNumber);

            // initialize the primary input audio provider
            this.waveIn = new WaveIn();
            this.waveIn.WaveFormat = waveFormat;
            this.waveIn.DeviceNumber = inputDeviceNumber;
            this.waveIn.BufferMilliseconds = this.BufferMilliseconds;
            this.waveIn.NumberOfBuffers = this.NumberOfBuffers;
            this.waveProvider = new AudioWaveProvider(waveFormat) { DiscardOnBufferOverflow = true };
            this.waveProvider.MeterProvider.StreamVolume += WaveProviderMeterProvider_StreamVolume;
            this.multiWaveOut.AddInputStream(waveProvider, waveIn, (e, provider) =>
            {
                provider.IgnoreFilter = false;
                if (repeatAudio && !repeatAudioTimeout)
                {
                    provider.AddSamples(e.Buffer, 0, e.BytesRecorded);
                    provider.TriggerMeter();
                }
            }, (ref double leftSmp, ref double rightSmp) =>
            {
                leftSmp = high.Process((float)leftSmp);
                rightSmp = high.Process((float)rightSmp);

                leftSmp = lo.Process((float)leftSmp);
                rightSmp = lo.Process((float)rightSmp);
            });
            AudioWaveProvider continuousWave = new AudioWaveProvider(waveFormat) { DiscardOnBufferOverflow = true };
            continuousWave.MeterProvider.StreamVolume += WaveProviderMeterProvider_StreamVolume;
            this.waveIn.DataAvailable += (sender, e) =>
            {
                continuousWave.AddSamples(e.Buffer, 0, e.BytesRecorded);
                continuousWave.TriggerMeter();
            };
            this.source = new StreamingSampleSource(continuousWave, waveFormat.SampleRate);

            // initialize the input link audio provider
            if (useLinkInput)
            {
                this.linkWaveIn = new WaveIn();
                this.linkWaveIn.WaveFormat = waveFormat;
                this.linkWaveIn.DeviceNumber = linkInputDeviceNumber;
                this.linkWaveIn.BufferMilliseconds = this.BufferMilliseconds;
                this.linkWaveIn.NumberOfBuffers = this.NumberOfBuffers;
                this.linkWaveProvider = new AudioWaveProvider(waveFormat) { DiscardOnBufferOverflow = true };
                if (!DisableFiltering)
                {
                    linkWaveProvider.FilterSampleCallback += (ref double leftSmp, ref double rightSmp) =>
                    {
                        leftSmp = high.Process((float)leftSmp);
                        rightSmp = high.Process((float)rightSmp);
                        leftSmp = lo.Process((float)leftSmp);
                        rightSmp = lo.Process((float)rightSmp);
                    };
                }
                this.linkWaveProvider.MeterProvider.StreamVolume += LinkWaveProviderMeterProvider_StreamVolume;
                this.multiWaveOut.AddInputStream(linkWaveProvider, linkWaveIn, (e, provider) =>
                {
                    if (haveLinkAudio && repeatAudio && !repeatAudioTimeout)
                    {
                        provider.AddSamples(e.Buffer, 0, e.BytesRecorded);
                        provider.TriggerMeter();
                    }
                });
                this.linkSource = new StreamingSampleSource(linkWaveProvider, waveFormat.SampleRate);
            }

            // initialize the internal audio provider
            this.externalProvider = new AudioWaveProvider(waveFormat);
            this.externalProvider.DiscardOnBufferOverflow = true;
            this.multiWaveOut.AddInputStream(externalProvider, null, null);
            this.externalSource = new StreamingSampleSource(externalProvider, waveFormat.SampleRate);

            // initialize the squelch system (PL and DPL) audio provider
            this.sqsProvider = new AudioWaveProvider(waveFormat);
            this.multiWaveOut.AddInputStream(sqsProvider, null, null, (ref double leftSmp, ref double rightSmp) =>
            {
                // apply low pass for SQS
                leftSmp = shelf.Process((float)leftSmp);
                rightSmp = shelf.Process((float)rightSmp);
            });

            // initialize MDC1200 audio input processor
            if (this.mdcProcessor != null)
            {
                mdcProcessor.StopRecording();
                mdcProcessor.Dispose();
                mdcProcessor = null;
            }

            mdcProcessor = new MDCWaveIn(afskWaveFormat, inputDeviceNumber);
            mdcProcessor.BufferMilliseconds = AFSKBufferMilliseconds;
            mdcProcessor.NumberOfBuffers = AFSKNumberOfBuffers;
            mdcProcessor.MDCPacketDetected += MdcProcessor_MDCPacketDetected;
            mdcProcessor.StartRecording();

            mdcGenerator = new MDCGenerator(afskWaveFormat.SampleRate);
            mdcGenerator.NumberOfPreambles = this.Options.NumberOfPreambles;
            mdcGenerator.InjectSilenceLeader = true;
            mdcGenerator.InjectSilenceTail = true;

            // initialize DTMF audio input processor
            this.DTMFToneSeconds = 0.5;
            if (this.dtmfProcessor != null)
            {
                dtmfProcessor.StopRecording();
                dtmfProcessor.Dispose();
                dtmfProcessor = null;
            }

            dtmfProcessor = new DtmfWaveIn(waveFormat, inputDeviceNumber);
            dtmfProcessor.BufferMilliseconds = AFSKBufferMilliseconds;
            dtmfProcessor.NumberOfBuffers = AFSKNumberOfBuffers;
            dtmfProcessor.DtmfToneStopped += DtmfProcessor_DtmfToneStopped;
            dtmfProcessor.StartRecording();

            // initialize PL audio input processor
            if (this.plProcessor != null)
            {
                plProcessor.StopRecording();
                plProcessor.Dispose();
                plProcessor = null;
            }

            plProcessor = new PLWaveIn(afskWaveFormat, inputDeviceNumber);
            plProcessor.BufferMilliseconds = AFSKBufferMilliseconds;
            plProcessor.NumberOfBuffers = AFSKNumberOfBuffers;
            plProcessor.PLToneDetected += PlProcessor_PLToneDetected;
            plProcessor.PLToneLost += PlProcessor_PLToneLost;
            plProcessor.StartRecording();

            plGenerator = new PLGenerator(afskWaveFormat.SampleRate);

            // initialize DPL audio input processor
            if (this.dplProcessor != null)
            {
                dplProcessor.StopRecording();
                dplProcessor.Dispose();
                dplProcessor = null;
            }

            dplProcessor = new DPLWaveIn(afskWaveFormat, inputDeviceNumber);
            dplProcessor.BufferMilliseconds = AFSKBufferMilliseconds;
            dplProcessor.NumberOfBuffers = AFSKNumberOfBuffers;
            dplProcessor.DPLTonesDetected += DplProcessor_DPLTonesDetected;
            dplProcessor.DPLToneLost += DplProcessor_DPLToneLost;
            dplProcessor.StartRecording();

            dplGenerator = new DPLGenerator(afskWaveFormat.SampleRate);

            // initialize speech provider
            this.tts = new SAPITTS(waveFormat);
            this.tts.SpeakCompleted += Tts_SpeakCompleted;

            this.multiWaveOut.ApplyMeteringCallback(WaveOutProviderMeterProvider_StreamVolume);

            ResetCourtesyToneFile();
        }

        /// <summary>
        /// Event that occurs when the oupt[ut meter sample provider detects samples.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WaveOutProviderMeterProvider_StreamVolume(object sender, StreamVolumeEventArgs e)
        {
            if (WaveOutMeterSampling != null)
                WaveOutMeterSampling(sender, e);
        }

        /// <summary>
        /// Event that occurs when the input meter sample provider detects samples.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinkWaveProviderMeterProvider_StreamVolume(object sender, StreamVolumeEventArgs e)
        {
            if (WaveInMeterSampling != null)
                WaveInMeterSampling(sender, e);

            if (this.Options.RxVOX)
            {
                if (hasHadTxDeKey)
                {
                    if (voxDekeyTimer == null)
                        voxDekeyTimer = new Stopwatch();
                    if (!voxDekeyTimer.IsRunning)
                        voxDekeyTimer.Start();

                    if (voxDekeyTimer.IsRunning && (voxDekeyTimer.ElapsedMilliseconds > (AudioDropMS / 2)))
                    {
                        voxDekeyTimer.Reset();
                        hasHadTxDeKey = false;
                    }
                }

                if (!hasHadTxDeKey)
                {
                    // handle Rx triggered by internal VOX
                    if (e.MaxSampleValues[0] > this.Options.VOXDetectLevel)
                    {
                        // only do VOX detection if we're not an MDC console
                        if (!this.Options.MDCConsoleOnly)
                            haveLinkAudio = true;
                        audioDetect = true;

                        // reset the drop audio stopwatch
                        linkDropAudio.Reset();
                    }
                    else
                    {
                        // if we've exceeded the audio drop timeout, then really drop the audio
                        if (linkDropAudio.IsRunning && (linkDropAudio.ElapsedMilliseconds > AudioDropMS))
                        {
                            haveLinkAudio = false;
                            linkDropAudio.Reset();
                        }

                        if (!linkDropAudio.IsRunning)
                            linkDropAudio.Start();
                    }
                }
            }
            else
                hasHadTxDeKey = false;
        }

        /// <summary>
        /// Event that occurs when the input meter sample provider detects samples.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WaveProviderMeterProvider_StreamVolume(object sender, StreamVolumeEventArgs e)
        {
            if (WaveInMeterSampling != null)
                WaveInMeterSampling(sender, e);

            if (this.Options.RxVOX)
            {
                if (hasHadTxDeKey)
                {
                    if (voxDekeyTimer == null)
                        voxDekeyTimer = new Stopwatch();
                    if (!voxDekeyTimer.IsRunning)
                        voxDekeyTimer.Start();

                    if (voxDekeyTimer.IsRunning && (voxDekeyTimer.ElapsedMilliseconds > (AudioDropMS / 2)))
                    {
                        voxDekeyTimer.Reset();
                        hasHadTxDeKey = false;
                    }
                }

                if (!hasHadTxDeKey)
                {
                    // handle Rx triggered by internal VOX
                    if (e.MaxSampleValues[0] > this.Options.VOXDetectLevel)
                    {
                        // only do VOX for this if we're not an MDC Console
                        if (!this.Options.MDCConsoleOnly)
                            RepeatAudio();
                        audioDetect = true;

                        // reset the drop audio stopwatch
                        dropAudio.Reset();
                    }
                    else
                    {
                        // if we've exceeded the audio drop timeout, then really drop the audio
                        if (dropAudio.IsRunning && (dropAudio.ElapsedMilliseconds > AudioDropMS))
                        {
                            repeatAudio = false;
                            audioDetect = false;
                            ReleaseStates();

                            dropAudio.Reset();
                        }

                        if (!dropAudio.IsRunning)
                            dropAudio.Start();
                    }
                }
            }
            else
                hasHadTxDeKey = false;
        }

        /// <summary>
        /// Occurs when the MDC-1200 packet processor, decodes a packet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="frameCount"></param>
        /// <param name="first"></param>
        /// <param name="second"></param>
        private void MdcProcessor_MDCPacketDetected(object sender, int frameCount, MDCPacket first, MDCPacket second)
        {
            // fire pass-thru event
            if (MDCPacketDetected != null)
                MDCPacketDetected(this, frameCount, first, second);

            // is this a repeater access packet?
            if (first.Operation == OpType.RAC)
            {
                if (this.Options.UseRAC)
                {
                    // make sure the RAC code is correct, if so grant permission to key the transmitter
                    if (first.Target == this.Options.AccessRAC)
                    {
                        gotRACCode = true;
                        if (ValidRACCode != null)
                            ValidRACCode(this);
                    }
                    else
                        gotRACCode = false;
                }
                else
                    gotRACCode = false;
            }
        }

        /// <summary>
        /// Occurs when the DTMF wave in decoder detects the end of a DTMF tone.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="tone"></param>
        private void DtmfProcessor_DtmfToneStopped(object sender, DtmfToneEnd tone)
        {
            if (tone.Duration.TotalSeconds >= this.DTMFToneSeconds)
            {
                Messages.Trace("DTMF Tone Detect: " + tone.DtmfTone.KeyString, LogFilter.DTMF_TRACE);

                // fire pass-thru event
                if (DTMFToneDetected != null)
                    DTMFToneDetected(this, tone);
            }
        }

        /// <summary>
        /// Occurs when a PL tone is detected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="tone"></param>
        private void PlProcessor_PLToneDetected(object sender, double tone)
        {
            // fire pass-thru event
            if (PLToneDetected != null)
                PLToneDetected(this, tone);

            if (this.Options.UseRxDPL)
                return;

            if (this.Options.EnableRxPL)
            {
                if (tone == PLPureTones.ToneList[this.Options.RxPL])
                    gotPLTone = true;
                else
                {
                    DekeyTransmitter();
                    ReleaseStates();

                    gotPLTone = false;
                }
            }
            else
                gotPLTone = false;
        }

        /// <summary>
        /// Occurs when a PL tone is lost.
        /// </summary>
        /// <param name="sender"></param>
        private void PlProcessor_PLToneLost(object sender)
        {
            // fire pass-thru event
            if (PLToneLost != null)
                PLToneLost(this);

            if ((this.Options.EnableRxPL && !this.Options.UseRxDPL) && gotPLTone)
            {
                Messages.Trace("PL tone lost -- dropping states", LogFilter.VERBOSE_TRACE);

                ReleaseStates();
                gotPLTone = false;
            }
        }

        /// <summary>
        /// Occurs when a DPL tone is detected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="code"></param>
        private void DplProcessor_DPLTonesDetected(object sender, List<int> code)
        {
            // fire pass-thru event
            if (DPLToneDetected != null)
                DPLToneDetected(this, code);

            if ((this.Options.EnableRxPL) && (this.Options.UseRxDPL))
            {
                int setCode = Convert.ToInt32(DPLGenerator.ToneList[this.Options.RxDPL]);
                if (code.Contains(setCode))
                    gotPLTone = true;
                else
                {
                    DekeyTransmitter();
                    ReleaseStates();

                    gotPLTone = false;
                }
            }
        }

        /// <summary>
        /// Occurs when a DPL tone is lost.
        /// </summary>
        /// <param name="sender"></param>
        private void DplProcessor_DPLToneLost(object sender)
        {
            // fire pass-thru event
            if (PLToneLost != null)
                PLToneLost(this);

            if ((this.Options.EnableRxPL && this.Options.UseRxDPL) && gotPLTone)
            {
                Messages.Trace("DPL tone lost -- dropping states", LogFilter.VERBOSE_TRACE);

                ReleaseStates();
                gotPLTone = false;
            }
        }

        /// <summary>
        /// Event that occurs when the TTS is finished speaking.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tts_SpeakCompleted(object sender, EventArgs e)
        {
            // generate silence
            int sDuration = SamplesToMS.MSToSampleBytes(externalProvider.WaveFormat, SILENCE_LEN_MS * 2);
            List<byte> silence = new List<byte>();
            for (int i = 0; i < sDuration; i++)
                silence.Add(128); // 128 should be audio "zero" in byte format

            // inject leader silence
            externalProvider.AddSamples(silence.ToArray(), 0, silence.Count);

            if (tts.WaveFormat.BitsPerSample == 32)
            {
                WaveFloatTo16Provider floatTo16 = new WaveFloatTo16Provider(tts.WaveProvider);
                byte[] bSamples = new byte[tts.WaveProvider.BufferedBytes];
                int read = floatTo16.Read(bSamples, 0, bSamples.Length);

                externalProvider.AddSamples(bSamples, 0, bSamples.Length);
            }
            else
            {
                byte[] bSamples = new byte[tts.WaveProvider.BufferedBytes];
                int read = tts.WaveProvider.Read(bSamples, 0, bSamples.Length);

                externalProvider.AddSamples(bSamples, 0, bSamples.Length);
            }

            // i'm not sure why this is necessary -- but clear the TTS waveprovider
            tts.WaveProvider.ClearBuffer();
        }

        /// <summary>
        /// Sets the serial ports.
        /// </summary>
        /// <param name="rx"></param>
        /// <param name="tx"></param>
        public void SetPorts(SerialPort rx, SerialPort tx)
        {
            this.rxPort = rx;
            this.txPort = tx;
        }

        /// <summary>
        /// Starts capturing samples for analysis.
        /// </summary>
        public void StartRecording()
        {
            if (IsRecording)
                return;

            multiWaveOut.StartPlayback();

            waveIn.StartRecording();
            if (useLinkAudio)
                linkWaveIn.StartRecording();

            IsRecording = true;

            // start transmitter watchdog
            stopWatchDogThread = false;
            txWatchDog = new Thread(TransmitWatchDog);
            txWatchDog.Name = "TxWatchDog";
            txWatchDog.Start();

            // start main capture worker
            stopCaptureThread = false;
            captureWorker = new Thread(WaitForSamples);
            captureWorker.Name = "RepeaterWaveIn";
            captureWorker.Start();

            // start MDC1200  worker
            mdcWorker = new Thread(WaitForMDCSamples);
            mdcWorker.Name = "MDCWorker";
            mdcWorker.Start();
        }

        /// <summary>
        /// Stops capturing samples for analysis.
        /// </summary>
        public void StopRecording()
        {
            if (!IsRecording)
                return;

            DekeyTransmitter();
            ReleaseStates();

            IsRecording = false;
            if (mdcWorker != null)
            {
                mdcWorker.Abort();
                mdcWorker.Join();
            }

            if (captureWorker != null)
            {
                stopCaptureThread = true;
                captureWorker.Abort();
                captureWorker.Join();
            }

            if (txWatchDog != null)
            {
                stopWatchDogThread = true;
                txWatchDog.Abort();
                txWatchDog.Join();
            }

            waveIn.StopRecording();
            if (useLinkAudio)
                linkWaveIn.StopRecording();

            multiWaveOut.StopPlayback();
        }

        /// <summary>
        /// Helper function to determine if the transmitter is keyed.
        /// </summary>
        /// <returns></returns>
        public bool IsTransmitterKeyed()
        {
            // if transmitter is in VOX mode we do nothing
            if (this.Options.TxVOX)
                return false;
            else
            {
                // handle Tx triggered by serial port
                if (txPort != null)
                {
                    SerialPortPin pin = this.Options.TxAssertPin;
                    switch (pin)
                    {
                        case SerialPortPin.RTS:
                            if (txPort.RtsEnable)
                                return true;
                            return false;
                        case SerialPortPin.DTR:
                            if (txPort.DtrEnable)
                                return true;
                            return false;
                        case SerialPortPin.BOTH_RTS_AND_DTR:
                            if (txPort.RtsEnable || txPort.DtrEnable)
                                return true;
                            return false;
                    }
                }
                else
                    return false;
            }

            return false;
        }

        /// <summary>
        /// Helper function to assert serial port lines and cause the transmitter to key.
        /// </summary>
        public void KeyTransmitter()
        {
            // if transmitter is in VOX mode we do nothing
            if (this.Options.TxVOX)
            {
                // fire repeater keyed event
                if (RepeaterKeyed != null)
                    RepeaterKeyed(this, new EventArgs());
                return;
            }
            else
            {
                // handle Tx triggered by serial port
                if (txPort != null)
                {
                    SerialPortPin pin = this.Options.TxAssertPin;
                    switch (pin)
                    {
                        case SerialPortPin.RTS:
                            txPort.RtsEnable = true;
                            break;
                        case SerialPortPin.DTR:
                            txPort.DtrEnable = true;
                            break;
                        case SerialPortPin.BOTH_RTS_AND_DTR:
                            txPort.RtsEnable = true;
                            txPort.DtrEnable = true;
                            break;
                    }

                    // fire repeater keyed event
                    if (RepeaterKeyed != null)
                        RepeaterKeyed(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Helper functions to deassert serial port lines and cause the transmitter to unkey.
        /// </summary>
        public void DekeyTransmitter()
        {
            // if transmitter is in VOX mode we do nothing
            if (this.Options.TxVOX)
            {
                //Messages.Trace("transmitter key being released!", LogFilter.VERBOSE_TRACE, 2);

                // fire repeater dekeyed event
                if (RepeaterDekeyed != null)
                    RepeaterDekeyed(this, new EventArgs());
                return;
            }
            else
            {
                // handle Tx triggered by serial port
                if (txPort != null)
                {
                    SerialPortPin pin = this.Options.TxAssertPin;
                    switch (pin)
                    {
                        case SerialPortPin.RTS:
                            txPort.RtsEnable = false;
                            break;
                        case SerialPortPin.DTR:
                            txPort.DtrEnable = false;
                            break;
                        case SerialPortPin.BOTH_RTS_AND_DTR:
                            txPort.RtsEnable = false;
                            txPort.DtrEnable = false;
                            break;
                    }

                    // fire repeater dekeyed event
                    if (RepeaterDekeyed != null)
                        RepeaterDekeyed(this, new EventArgs());
                }
            }

            hasHadTxDeKey = true;
            if (lastSQSAdd > 0.0)
                lastSQSAdd = 0.0;
        }

        /// <summary>
        /// Internal helper to perform RAC validation and start audio repeat via audio flag.
        /// </summary>
        private void EnableAudioViaRAC()
        {
            // just RAC only
            if (this.Options.UseRAC)
            {
                if (gotRACCode)
                    repeatAudio = true;
                else
                    repeatAudio = false;
            }
            else
                repeatAudio = true;
        }
       
        /// <summary>
        /// Internal helper to process setting the repeat audio flag.
        /// </summary>
        public void RepeatAudio()
        {
            if (!repeatAudio)
            {
                // just PL only
                if (this.Options.EnableRxPL)
                {
                    if (gotPLTone)
                    {
                        EnableAudioViaRAC();

                        // fire repeat audio event
                        if (RepeatingAudio != null)
                            RepeatingAudio(this, new EventArgs());
                    }
                    else
                    {
                        repeatAudio = false;

                        // fire repeat audio event
                        if (RepeatingAudioFailed != null)
                            RepeatingAudioFailed(this, new EventArgs());
                    }
                }
                else
                    EnableAudioViaRAC();
            }
        }

        /// <summary>
        /// Helper to release any states.
        /// </summary>
        public void ReleaseStates()
        {
            gotPLTone = false;
            gotRACCode = false;

            // additionally if we're using Rx PL release the repeatAudio flag
            if (this.Options.EnableRxPL)
                repeatAudio = false;

            // additionally if we're using RAC release the repeatAudio flag
            if (this.Options.UseRAC)
                repeatAudio = false;

            audioDetect = false;
        }

        /// <summary>
        /// Helper to internally generate a courtesy tone.
        /// </summary>
        public void GenerateCourtesyTone()
        {
            CourtesyToneWaveProvider ctWave = new CourtesyToneWaveProvider(externalProvider.WaveFormat);
            ctWave.CourtesyDelay = this.Options.CourtesyDelay;
            ctWave.CourtesyLength = this.Options.CourtesyLength;
            ctWave.CourtesyPitch = this.Options.CourtesyPitch;
            ctWave.UseMultitone = this.Options.CourtesyMultiTone;
            ctWave.Tones = this.Options.CourtesyTones;

            byte[] ct = ctWave.Generate();
            GenerateSquelchSamples(0.0, true);
            Messages.Trace("playing generated based courtesy tone WaveFormat = " + waveFormat.ToString() + ", sampleLen = " + ct.Length);
            externalProvider.AddSamples(ct, 0, ct.Length);
        }

        /// <summary>
        /// Helper to play the courtesy tone wave file.
        /// </summary>
        public void PlayCourtesyTone()
        {
            if (this.courtesyTone == null)
                GenerateCourtesyTone();

            // generate silence
            int sDuration = SamplesToMS.MSToSampleBytes(externalProvider.WaveFormat, SILENCE_LEN_MS);
            List<byte> silence = new List<byte>();
            for (int i = 0; i < sDuration; i++)
                silence.Add(128); // 128 should be audio "zero" in byte format

            // inject leader silence
            externalProvider.AddSamples(silence.ToArray(), 0, silence.Count);

            IWaveProvider provider = courtesyTone;
            long bufferLen = courtesyTone.Length;
            GenerateSquelchSamples(courtesyTone.TotalTime.TotalMilliseconds, true);
            if (courtesyTone.WaveFormat.SampleRate != waveFormat.SampleRate)
            {
                provider = new WaveFormatConversionStream(waveFormat, courtesyTone);
                bufferLen = ((WaveFormatConversionStream)provider).Length;
            }

            if (provider.WaveFormat.BitsPerSample == 32)
            {
                WaveFloatTo16Provider floatTo16 = new WaveFloatTo16Provider(provider);
                byte[] bSamples = new byte[bufferLen];
                floatTo16.Read(bSamples, 0, bSamples.Length);

                Messages.Trace("playing wave file based courtesy tone WaveFormat = " + waveFormat.ToString() + ", sampleLen = " + bSamples.Length);

                externalProvider.AddSamples(bSamples, 0, bSamples.Length);
                courtesyTone.Seek(0, SeekOrigin.Begin);
            }
            else
            {
                byte[] bSamples = new byte[bufferLen];
                provider.Read(bSamples, 0, bSamples.Length);

                Messages.Trace("playing wave file based courtesy tone WaveFormat = " + waveFormat.ToString() + ", sampleLen = " + bSamples.Length);

                externalProvider.AddSamples(bSamples, 0, bSamples.Length);
                courtesyTone.Seek(0, SeekOrigin.Begin);
            }

            // inject trailer silence
            externalProvider.AddSamples(silence.ToArray(), 0, silence.Count);
        }

        /// <summary>
        /// Helper to generate and play our squelch system signal.
        /// </summary>
        /// <param name="totalMilli"></param>
        /// <param name="initialAdd"></param>
        public void GenerateSquelchSamples(double totalMilli, bool initialAdd = false)
        {
            byte[] samples = null;
            if (this.Options.EnableTxPL && allowPL)
            {
                if (totalMilli > 0.0 && initialAdd)
                    initialAdd = false;

                Messages.Trace("called sqs gen (totalMilli = " + totalMilli + ", lastSQSAdd = " + lastSQSAdd + ", initialAdd = " + initialAdd + ")", LogFilter.PL_DPL_TRACE, 2);

                // do we need to add more samples for the squelch?
                if (((totalMilli - lastSQSAdd) >= (SQS_SAMPLE_LEN_MS - SQS_END_LEN_MS)) || initialAdd)
                {
                    Messages.Trace("generateing sqs", LogFilter.PL_DPL_TRACE);

                    if (this.Options.UseTxDPL)
                    {
                        // do we need to generate our signal?
                        if (!dplGenerator.Generated)
                            dplGenerator.GenerateDPL(DPLGenerator.ToneList[this.Options.TxDPL]);

                        if (dplGenerator.Generated)
                        {
                            samples = dplGenerator.GetSamples(SQS_SAMPLE_LEN_MS);
                            sqsProvider.AddSamples(samples, 0, samples.Length);
                        }
                    }
                    else
                    {
                        // do we need to generate our signal?
                        if (!plGenerator.Generated)
                            plGenerator.SetTone(PLPureTones.ToneList[this.Options.TxPL]);

                        if (plGenerator.Generated)
                        {
                            samples = plGenerator.GetSamples(SQS_SAMPLE_LEN_MS);
                            sqsProvider.AddSamples(samples, 0, samples.Length);
                        }
                    }

                    lastSQSAdd = totalMilli - 25;
                }
            }
        }

        /// <summary>
        /// Helper to generate and play our squelch system signal.
        /// </summary>
        /// <param name="totalMilli"></param>
        /// <param name="initialAdd"></param>
        public void GenerateReverseSquelchSamples()
        {
            byte[] samples = null;
            if (this.Options.EnableTxPL && allowPL)
            {
                if ((multiWaveOut != null && sqsProvider != null))
                {
                    multiWaveOut.ClearBuffer(sqsProvider);

                    if (this.Options.UseTxDPL)
                    {
                        samples = dplGenerator.GetRevSamples(REV_SQS_SAMPLE_LEN_MS);
                        if (samples != null)
                            sqsProvider.AddSamples(samples, 0, samples.Length);
                    }
                    else
                    {
                        samples = plGenerator.GetRevSamples(REV_SQS_SAMPLE_LEN_MS);
                        if (samples != null)
                            sqsProvider.AddSamples(samples, 0, samples.Length);
                    }
                }
            }
        }

        /// <summary>
        /// Helper to cancel transmit if transmit is asserted for too long.
        /// </summary>
        private void TransmitWatchDog()
        {
            Stopwatch txStopwatch = null;

            // initially wait 1s before starting actual processing
            Thread.Sleep(1000);

            try
            {
                while (!stopWatchDogThread)
                {
                    if (IsTransmitterKeyed() && txStopwatch == null)
                    {
                        txStopwatch = new Stopwatch();
                        txStopwatch.Start();
                    }

                    if (txStopwatch != null)
                    {
                        if (!IsTransmitterKeyed())
                        {
                            if (txStopwatch != null)
                            {
                                txStopwatch.Stop();
                                txStopwatch = null;
                            }

                            if (repeatAudioTimeout)
                            {
                                // fire watch dog inactive event
                                if (WatchDogInactive != null)
                                    WatchDogInactive(this, new EventArgs());

                                // clear timeout
                                ReleaseStates();
                                repeatAudioTimeout = false;
                            }
                        }
                        else
                        {
                            if (txStopwatch.ElapsedMilliseconds > (this.Options.WatchDogTime * 1000))
                            {
                                // keep the transmitter suppressed
                                DekeyTransmitter();

                                // forcibly drop the audio buffer
                                multiWaveOut.ClearBuffer();

                                // fire watch dog timeout event
                                if (WatchDogTimeout != null)
                                    WatchDogTimeout(this, new EventArgs());


                                // tx timeout occurred...
                                ReleaseStates();
                                repeatAudioTimeout = true;
                            }
                        }
                    }

                    Thread.Sleep(1);
                }
            }
            catch (ThreadAbortException)
            {
                /* ignore */
            }
            catch (Exception e)
            {
                Messages.Write("Unhandled error occurred in the watch dog thread", e);
                throw e;
            }
        }

        /// <summary>
        /// Helper to wait for and process incoming audio samples.
        /// </summary>
        /// <returns></returns>
        public void WaitForSamples()
        {
            Stopwatch dekeyTimer = new Stopwatch();
            Stopwatch txStopwatch = new Stopwatch();

            Stopwatch txConsoleTimer = new Stopwatch();

            Stopwatch txTailTimer = new Stopwatch();
            Stopwatch txRevBurstTimer = new Stopwatch();

            double audioTotalMS = 0.0;
            double revSquelchMS = 0.0;

            bool hadTx = false;

            // initially wait 1s before starting actual processing
            Thread.Sleep(1000);

            DisableTransmitter = false;

            try
            {
                // trigger CW on startup (unless we're an MDC console, then we won't) and if we're set to ID at startup
                if ((!this.Options.MDCConsoleOnly || this.Options.NoId) && !this.Options.NoIdStartup && !this.hasDoneInitialCW)
                {
                    if (AutomaticCW != null)
                        AutomaticCW.PlayRepeaterCW();
                    hasDoneInitialCW = true;
                }

                while (!stopCaptureThread)
                {
                    if (!this.Options.RxVOX && !this.Options.MDCConsoleOnly)
                    {
                        // handle Rx triggered by serial port
                        if (rxPort != null)
                        {
                            SerialPortPin pin = this.Options.RxAssertPin;
                            switch (pin)
                            {
                                case SerialPortPin.CD:
                                    if (rxPort.CDHolding)
                                    {
                                        RepeatAudio();
                                        audioDetect = true;
                                        MainWindow.IsRepeaterRx = true; // nope nope nope
                                    }
                                    else
                                    {
                                        repeatAudio = false;
                                        audioDetect = false;
                                        MainWindow.IsRepeaterRx = false; // nope nope nope
                                    }
                                    break;
                                case SerialPortPin.CTS:
                                    if (rxPort.CtsHolding)
                                    {
                                        RepeatAudio();
                                        audioDetect = true;
                                        MainWindow.IsRepeaterRx = true; // nope nope nope
                                    }
                                    else
                                    {
                                        repeatAudio = false;
                                        audioDetect = false;
                                        MainWindow.IsRepeaterRx = false; // nope nope nope
                                    }
                                    break;
                                case SerialPortPin.DSR:
                                    if (rxPort.DsrHolding)
                                    {
                                        RepeatAudio();
                                        audioDetect = true;
                                        MainWindow.IsRepeaterRx = true; // nope nope nope
                                    }
                                    else
                                    {
                                        repeatAudio = false;
                                        audioDetect = false;
                                        MainWindow.IsRepeaterRx = false; // nope nope nope
                                    }
                                    break;
                            }
                        }
                    }

                    // make sure audio repeat is always false for MDC console...
                    if (this.Options.MDCConsoleOnly)
                    {
                        repeatAudio = false;

                        if (!txStopwatch.IsRunning)
                        {
                            if (audioDetect)
                            {
                                if (!txConsoleTimer.IsRunning)
                                {
                                    txConsoleTimer = new Stopwatch();
                                    txConsoleTimer.Start();
                                }

                                // fire repeater active event
                                if (RepeaterActive != null)
                                    RepeaterActive(this, txConsoleTimer.Elapsed.TotalSeconds);
                            }
                            else
                            {
                                // reset the transmit timer
                                if (txConsoleTimer.IsRunning)
                                    txConsoleTimer.Reset();

                                // fire repeater idle event
                                if (RepeaterIdle != null)
                                    RepeaterIdle(this, new EventArgs());
                            }
                        }
                    }

                    // don't allow us to procede if the transmitter is disabled...
                    if (DisableTransmitter)
                    {
                        ReleaseStates();
                        repeatAudioTimeout = false;
                        hadTx = false;

                        // reset the transmit timer
                        if (txStopwatch.IsRunning)
                            txStopwatch.Reset();

                        // reset tail timer
                        if (txTailTimer.IsRunning)
                            txTailTimer.Reset();

                        // reset dekey timer
                        if (dekeyTimer.IsRunning)
                            dekeyTimer.Reset();

                        // keep the transmitter suppressed
                        DekeyTransmitter();

                        // forcibly drop the audio buffer
                        multiWaveOut.ClearBuffer();

                        // fire repeater disabled event
                        if (RepeaterDisabled != null)
                            RepeaterDisabled(this, new EventArgs());

                        Thread.Sleep(1);
                        continue;
                    }
                    else
                    {
                        // are we repeating audio or have internal audio playing?
                        if (!hadTx && (!repeatAudio) && !((externalProvider.SampleDuration.TotalMilliseconds > 0) || (audioTotalMS > 0)) && !txTailTimer.IsRunning)
                        {
                            // this is a saftey -- we'll just KILL internal audio playback if for some reason we've lost the tx stopwatch
                            DekeyTransmitter();

                            // clear buffers
                            multiWaveOut.ClearBuffer(sqsProvider);
                            multiWaveOut.ClearBuffer();

                            audioTotalMS = 0.0;

                            txStopwatch.Reset();

                            if (!this.Options.MDCConsoleOnly)
                            {
                                // fire repeater idle event
                                if (RepeaterIdle != null)
                                    RepeaterIdle(this, new EventArgs());
                            }
                        }
                    }

                    // check how long we've been transmitting, if we've exceeded the maximum time; kill any remaining
                    // audio and stop transmitting
                    if (txStopwatch.IsRunning && !txTailTimer.IsRunning)
                    {
                        if (txStopwatch.ElapsedMilliseconds >= (this.Options.MaxTransmissionTime * 1000))
                        {
                            if (this.Options.EnableTxPL)
                            {
                                if (revSquelchMS == 0.0)
                                {
                                    revSquelchMS = txStopwatch.ElapsedMilliseconds + REV_SQS_SAMPLE_LEN_MS;
                                    GenerateReverseSquelchSamples();
                                }
                                else if (txStopwatch.ElapsedMilliseconds < revSquelchMS)
                                    continue;

                                revSquelchMS = 0.0;
                            }

                            // keep the transmitter suppressed
                            DekeyTransmitter();

                            // forcibly drop the audio buffer(s)
                            multiWaveOut.ClearBuffer(sqsProvider);
                            multiWaveOut.ClearBuffer();

                            // are we currently trying to repeat audio? or do we have buffered internal audio?
                            if (!repeatAudio)
                            {
                                // fire timeout cleared event
                                if (RepeaterTimeoutCleared != null)
                                    RepeaterTimeoutCleared(this, new EventArgs());

                                ReleaseStates();
                                repeatAudioTimeout = false;

                                // reset the transmit timer
                                if (txStopwatch.IsRunning)
                                    txStopwatch.Reset();
                                if (txTailTimer.IsRunning)
                                    txTailTimer.Reset();
                                if (txRevBurstTimer.IsRunning)
                                    txRevBurstTimer.Reset();
                            }
                            else
                            {
                                // fire timeout event
                                if (RepeaterTimeout != null)
                                    RepeaterTimeout(this, new EventArgs());

                                // tx timeout occurred...
                                ReleaseStates();
                                repeatAudioTimeout = true;
                            }
                        }
                        else
                        {
                            // are we repeating audio or have a transmit timer?
                            if (repeatAudio || (txStopwatch.IsRunning))
                            {
                                // fire repeater active event
                                if (RepeaterActive != null)
                                    RepeaterActive(this, txStopwatch.Elapsed.TotalSeconds);
                            }
                            else
                            {
                                // this is a saftey -- we'll just KILL internal audio playback if for some reason we've lost the tx stopwatch
                                DekeyTransmitter();

                                // clear buffers
                                multiWaveOut.ClearBuffer(sqsProvider);
                                multiWaveOut.ClearBuffer();

                                audioTotalMS = 0.0;

                                txStopwatch.Reset();
                                if (txTailTimer.IsRunning)
                                    txTailTimer.Reset();
                                if (txRevBurstTimer.IsRunning)
                                    txRevBurstTimer.Reset();

                                // fire repeater idle event
                                if (RepeaterIdle != null)
                                    RepeaterIdle(this, new EventArgs());
                            }
                        }
                    }

                    try
                    {
                        // has the repeater transmit timed out?
                        if (!repeatAudioTimeout)
                        {
                            // are we repeating audio?
                            if (repeatAudio)
                            {
                                // if we're actually repeating audio make sure to reset the PL flags
                                if (this.Options.EnableTxPL && !allowPL)
                                    allowPL = true;

                                // reset tail timer and start all over
                                if (txTailTimer.IsRunning && hadTx)
                                    txTailTimer.Reset();

                                // if we're not repeating audio -- lets start doing that
                                if (source.HasSamples)
                                {
                                    // is the transmitter keyed yet?
                                    if (!IsTransmitterKeyed())
                                    {
                                        if (!txStopwatch.IsRunning)
                                        {
                                            txStopwatch.Reset();
                                            txStopwatch.Start();

                                            GenerateSquelchSamples(0.0, true);
                                        }

                                        KeyTransmitter();
                                    }

                                    LastTx = DateTime.Now;
                                    waveProvider.TriggerMeter();

                                    if (this.Options.EnableTxPL)
                                        GenerateSquelchSamples(txStopwatch.Elapsed.TotalMilliseconds);
                                    hadTx = true;
                                }
                            }
                            else
                            {
                                if (txRevBurstTimer.IsRunning)
                                {
                                    if (txRevBurstTimer.ElapsedMilliseconds >= REV_SQS_SAMPLE_LEN_MS)
                                    {
                                        txRevBurstTimer.Reset();
                                        if (txTailTimer.IsRunning)
                                            txTailTimer.Reset();

                                        ReleaseStates();
                                        hadTx = false;
                                    }
                                }
                                else
                                {
                                    if (hadTx && (!txTailTimer.IsRunning))
                                    {
                                        if (!((externalProvider.SampleDuration.TotalMilliseconds > 0) || (audioTotalMS > 0)))
                                        {
                                            txTailTimer.Reset();
                                            txTailTimer.Start();
                                        }
                                    }

                                    // do we have a running tx tail timer?
                                    if (txTailTimer.IsRunning)
                                    {
                                        GenerateSquelchSamples(txStopwatch.Elapsed.TotalMilliseconds, true);

                                        // is the elapsed time for the tail timer greater then the configured time?
                                        if (txTailTimer.ElapsedMilliseconds >= (this.Options.TailTime * 1000))
                                        {
                                            // stop and clear the tx tail timer
                                            txTailTimer.Reset();

                                            // forcibly drop the audio buffer(s)
                                            multiWaveOut.ClearBuffer(sqsProvider);
                                            multiWaveOut.ClearBuffer();

                                            // do we have running tx timer? -- if so stop the running tx timer
                                            if (txStopwatch.IsRunning)
                                            {
                                                Messages.Trace("transmitted repeater audio for - " + txStopwatch.ElapsedMilliseconds + " ms");
                                                txStopwatch.Reset();
                                            }

                                            // are we using a courtesy tone?
                                            if (!this.Options.NoCourtesyTone)
                                            {
                                                // play courtesy tone
                                                if (this.Options.CourtesyUseFile)
                                                    PlayCourtesyTone();
                                                else
                                                    GenerateCourtesyTone();
                                            }

                                            // fire log event
                                            if (LogEvent != null)
                                                LogEvent(this, "Audio Transmission");

                                            if (this.Options.EnableTxPL)
                                            {
                                                GenerateReverseSquelchSamples();

                                                txRevBurstTimer.Reset();
                                                txRevBurstTimer.Start();
                                            }
                                            else
                                            {
                                                txRevBurstTimer.Reset();
                                                ReleaseStates();
                                                hadTx = false;
                                            }
                                        }
                                    }
                                }
                            }

                            // are we playing some internal audio?
                            if ((externalProvider.SampleDuration.TotalMilliseconds > 0) || (audioTotalMS > 0))
                            {
                                // only being writing samples if the stopwatch hasn't started
                                if (!txStopwatch.IsRunning && externalSource.HasSamples)
                                {
                                    // is the transmitter keyed yet?
                                    if (!IsTransmitterKeyed())
                                    {
                                        txStopwatch = Stopwatch.StartNew();
                                        GenerateSquelchSamples(0.0, true);

                                        KeyTransmitter();

                                        audioTotalMS = externalProvider.SampleDuration.TotalMilliseconds;
                                        if (audioTotalMS < 500)
                                            audioTotalMS += 500;

                                        LastTx = DateTime.Now;
                                    }
                                }

                                // do we have a running tx timer?
                                if (txStopwatch.IsRunning)
                                {
                                    GenerateSquelchSamples(txStopwatch.Elapsed.TotalMilliseconds, true);

                                    if (txStopwatch.ElapsedMilliseconds > audioTotalMS)
                                    {
                                        // stop and clear
                                        externalProvider.ClearBuffer();

                                        // reset PL flags .. the CW can disable the PL
                                        if (this.Options.EnableTxPL && !allowPL)
                                            allowPL = true;

                                        if (txStopwatch.IsRunning)
                                            Messages.Trace("transmitted CW/announcment audio for - " + txStopwatch.ElapsedMilliseconds + " ms");

                                        // if we have an announcement play back .. and we have repeat  audio -- clear the playing annc flag
                                        if (AutomaticAnnc.IsPlayingAnnc && hadTx)
                                            AutomaticAnnc.IsPlayingAnnc = false;

                                        // special stupid logic for announcement courtesy beep
                                        if (AutomaticAnnc.IsPlayingAnnc && !hadTx)
                                        {
                                            // zero playback time
                                            audioTotalMS = 0.0;

                                            // do we have running tx timer and no repeat audio? -- if so stop the running tx timer
                                            if (txStopwatch.IsRunning)
                                                txStopwatch.Reset();

                                            // play the repeater courtesy tone
                                            if (AutomaticAnnc.UseCourtesyTone && !this.Options.NoCourtesyTone)
                                            {
                                                if (this.Options.CourtesyUseFile)
                                                    PlayCourtesyTone();
                                                else
                                                    GenerateCourtesyTone();
                                            }

                                            AutomaticAnnc.IsPlayingAnnc = false;
                                            continue;
                                        }

                                        // do we have running tx timer and no repeat audio? -- if so stop the running tx timer
                                        if ((txStopwatch.IsRunning) && !hadTx)
                                            txStopwatch.Reset();

                                        audioTotalMS = 0.0;

                                        // don't ask questions
                                        externalProvider.ClearBuffer();
                                        waveProvider.ClearBuffer();

                                        if (this.Options.EnableTxPL)
                                        {
                                            GenerateReverseSquelchSamples();

                                            txRevBurstTimer.Reset();
                                            txRevBurstTimer.Start();
                                        }
                                    }
                                }
                                else
                                    audioTotalMS = 0.0; // shouldn't happen
                            }

                            // saftey check -- if we're not repeating audio and no audio is buffered -- ensure deadkey can't happen
                            if ((!repeatAudio && !hadTx) && !((externalProvider.SampleDuration.TotalMilliseconds > 0) || (audioTotalMS > 0)))
                            {
                                if (txRevBurstTimer.IsRunning && this.Options.EnableTxPL)
                                    continue;

                                multiWaveOut.ClearBuffer(sqsProvider);

                                if (dekeyTimer.IsRunning && (dekeyTimer.ElapsedMilliseconds > DEKEY_TIMER))
                                {
                                    // this is a saftey -- we'll just KILL internal audio playback if for some reason we've lost the tx stopwatch
                                    DekeyTransmitter();

                                    // clear buffers
                                    multiWaveOut.ClearBuffer(sqsProvider);
                                    multiWaveOut.ClearBuffer();

                                    audioTotalMS = 0.0;

                                    txStopwatch.Reset();
                                    txRevBurstTimer.Reset();
                                    dekeyTimer.Reset();
                                }

                                if (!dekeyTimer.IsRunning)
                                    dekeyTimer.Start();
                            }
                            else
                                dekeyTimer.Reset();
                        }
                    }
                    catch (MmException e)
                    {
                        Messages.TraceException("error handling buffer -- " + e.Message, e);
                        hadTx = false;
                    }

                    // let the CPU rest (unless transmitting)
                    if (!hadTx)
                        Thread.Sleep(1);
                }
            }
            catch (ThreadAbortException) { /* ignore */ }
            catch (Exception e)
            {
                Messages.Write("Unhandled error occurred in the transmit audio thread", e);
                throw e;
            }
        }

        /// <summary>
        /// Helper to wait for and process incoming audio samples.
        /// </summary>
        /// <returns></returns>
        public void WaitForMDCSamples()
        {
            while (true)
            {
                // wait for samples
                while (!mdcGenerator.HasSamples)
                    Thread.Sleep(1);

                // process samples
                byte[] buffer = mdcGenerator.GetSamples();
                if (MDCSamples != null)
                    MDCSamples(this, mdcProcessor, buffer);

                externalProvider.AddSamples(buffer, 0, buffer.Length);
            }
        }
    } // public class RepeaterWaveIn
} // namespace RepeaterController.DSP
