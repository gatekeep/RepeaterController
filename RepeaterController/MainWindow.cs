/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.ServiceModel;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

using MySql.Data;
using MySql.Data.MySqlClient;

using NAudio.Wave;
using NAudio.Wave.SampleProviders;

using RepeaterController.Announcements;
using RepeaterController.DSP;
using RepeaterController.DSP.MDC1200;
using RepeaterController.DSP.DTMF;
using RepeaterController.DSP.DPL;
using RepeaterController.Xml;

namespace RepeaterController
{
    /// <summary>
    /// Main Program Modal
    /// </summary>
    public partial class MainWindow : Form
    {
        /**
         * Constants
         */
        public static int MaxActivityLogCount = 512;

        public const int SAMPLE_RATE = 48000;
        public const int BITS_PER_SECOND = 16;

        public static int FixedAnncVol = 100;
        public static int CWFrequency = 1000;
        public static int CWWPM = 25;

        public static int AudioDropMS = 500;

        public const int MDC_BITS_PER_SAMPLE = 8;
        public const int MAX_PREAMBLES_ALLOWED = 10;

        public const float VOLUME_METER_NOISE_FLOOR = -90; // in dB

        public const string XML_FILE = "user_settings.xml";

        public static bool IsRepeaterTx = false;
        public static bool IsRepeaterRx = false;

        /**
         * P/Invoke
         */
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        /**
         * Fields
         */
        public static MainWindow Instance = null;
        private static IntPtr keybdHookId = IntPtr.Zero;
        private static LowLevelKeyboardProc keybdProc = KeyboardHookCallback;

        private SerialPort rxPort;
        private SerialPort txPort;

        private bool lastDisabledState = false;

        private ushort targetID;                        // Target Radio ID

        private string lastDetectedPL;
        private string lastDetectedDPL;
        public DateTime lastTx;

        private bool enforcedCWInterval = false;

        private ConfigureAudioDevice audioDeviceModal;
        private RepeaterOptions optionsModal;
        private AnnouncementWindow anncModal;
        private DTMFWindow dtmfModal;

        private AutomaticCW automaticCw;
        private AutomaticAnnc automaticAnnc;

        private XmlResource rsrc;

        // wave providers and formats, and input/output wave devices
        private WaveFormat waveFormat;
        private RepeaterWaveIn repeaterProcessor;

        private static bool keyDownEvent = false;

        private MySqlConnection mysqlConn = null;
        private bool mysqlOpen = false;
        private ManualResetEvent mysqlMRE = new ManualResetEvent(true);

        private const string MYSQL_LOG_TABLE = "log";
        private const string MYSQL_MDC_LOG_TABLE = "mdc_log";

        private bool mdcGotTextMessage = false;
        private string mdcTextMessage = string.Empty;

        /**
         * Events
         */
        /// <summary>
        /// Event pass-thru for the DTMF tone processor, where a DTMF tone is detected.
        /// </summary>
        public event Action<object, DtmfToneEnd> DTMFToneDetected;

        /**
         * Properties
         */
        /// <summary>
        /// Gets the instance of the <see cref="AutomaticCW"/> class.
        /// </summary>
        public AutomaticCW AutomaticCW
        {
            get { return automaticCw; }
        }

        /// <summary>
        /// Flag indicating whether the CW interval is controlled externally via RepeaterController.
        /// </summary>
        public bool EnforcedCWInterval
        {
            get { return enforcedCWInterval; }
        }

        /// <summary>
        /// Gets the instance of the <see cref="AutomaticAnnc"/> class.
        /// </summary>
        public AutomaticAnnc AutomaticAnnc
        {
            get { return automaticAnnc; }
        }

        /// <summary>
        /// Gets the instance of the <see cref="AnnouncementWindow"/> modal class.
        /// </summary>
        public AnnouncementWindow AnnouncementWindow
        {
            get { return anncModal; }
        }

        /// <summary>
        /// Gets the instance of the <see cref="DTMFWindow"/> modal class.
        /// </summary>
        public DTMFWindow DTMFWindow
        {
            get { return dtmfModal; }
        }

        /// <summary>
        /// Gets the instance of the <see cref="RepeaterOptions"/> modal class.
        /// </summary>
        public RepeaterOptions RepeaterOptions
        {
            get { return optionsModal; }
        }

        /// <summary>
        /// Gets or sets the flag indicating whether to disable the transmitter.
        /// </summary>
        public bool DisableTransmitter
        {
            get { return repeaterProcessor.DisableTransmitter; }
            set { repeaterProcessor.DisableTransmitter = value; }
        }

        /// <summary>
        /// Gets whether the repeater has a transmit timeout condition.
        /// </summary>
        /// <remarks>Setting this will *ALWAYS* clear a timeout condition.</remarks>
        public bool ClearTransmitterTimeout
        {
            get { return repeaterProcessor.ClearTransmitterTimeout; }
            set { repeaterProcessor.ClearTransmitterTimeout = value; }
        }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Instance = this;

            this.Shown += MainWindow_Shown;
            this.ShowDialog();
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~MainWindow()
        {
            if (keybdHookId != IntPtr.Zero)
                UnhookWindowsHookEx(keybdHookId);
        }

        /// <summary>
        /// Initialize everything once the form is open.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Shown(object sender, EventArgs e)
        {
#if !DEBUG
            if (keybdHookId == IntPtr.Zero)
                keybdHookId = SetKeybdHook(keybdProc);
#endif
#if DEBUG
            this.testButtonDoNotPressToolStripMenuItem.Visible = true;
            this.testButtonDoNotPressToolStripMenuItem.Enabled = true;
#else
            this.testButtonDoNotPressToolStripMenuItem.Visible = false;
            this.testButtonDoNotPressToolStripMenuItem.Enabled = false;
#endif
            this.notifyIcon.Click += NotifyIcon_Click;
            this.notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            this.emergAck.Enabled = false;

            this.toolStripTransmitLabel.Visible = false;
            this.toolStripStatusLabel.Visible = false;
            this.toolStripRACLabel.Visible = false;
            this.toolStripPLLabel.Visible = false;
            this.toolStripMDCConsoleLabel.Visible = false;
            this.toolStripDisableFilteringLabel.Visible = false;

            this.txVolumeMeter.Amplitude = VOLUME_METER_NOISE_FLOOR;
            this.rxVolumeMeter.Amplitude = VOLUME_METER_NOISE_FLOOR;

            this.Resize += MainWindow_Resize;
            this.FormClosed += new FormClosedEventHandler(MainWindow_FormClosed);

            this.targetMDCID.TextChanged += targetMDCID_TextChanged;

            ResetSettings();
            if (!File.Exists(Environment.CurrentDirectory + Path.DirectorySeparatorChar + XML_FILE))
                SaveXml();
            else
                LoadXml();

            // initialize modals proper
            this.optionsModal = new RepeaterOptions(rsrc, this);
            this.audioDeviceModal = new ConfigureAudioDevice(rsrc, optionsModal);
            this.anncModal = new AnnouncementWindow(rsrc);
            this.dtmfModal = new DTMFWindow(rsrc, this);

            this.lastDetectedPL = "00.0";
            this.lastDetectedDPL = "000";
            this.lastTx = DateTime.Now;

            // setup general audio variables
            this.waveFormat = new WaveFormat(SAMPLE_RATE, BITS_PER_SECOND, 1);

            // initialize the repeater wave in processor
            this.repeaterProcessor = new RepeaterWaveIn();
#if MDC_CONSOLE
            this.optionsModal.Options.MDCConsoleOnly = true;
            this.optionsModal.Options.NoId = true;
            this.optionsModal.Options.NoIdStartup = true;

            this.optionsModal.Options.EnableRxPL = false;
            this.optionsModal.Options.EnableTxPL = false;

            this.optionsModal.UpdateModal();
            this.optionsModal.SaveXml();
#endif
            this.repeaterProcessor.Options = this.optionsModal.Options;
            this.repeaterProcessor.BufferMilliseconds = this.audioDeviceModal.BufferMilliseconds;
            this.repeaterProcessor.NumberOfBuffers = this.audioDeviceModal.BufferCount;
            this.repeaterProcessor.Reset(waveFormat, this.audioDeviceModal.WaveInDevice, this.audioDeviceModal.WaveOutDevice);
#if !MDC_CONSOLE
            repeaterProcessor.RepeaterDisabled += RepeaterProcessor_RepeaterDisabled;
            repeaterProcessor.RepeaterEnabled += RepeaterProcessor_RepeaterEnabled;

            repeaterProcessor.RepeatingAudio += RepeaterProcessor_RepeatingAudio;
            repeaterProcessor.RepeatingAudioFailed += RepeaterProcessor_RepeatingAudioFailed;

            repeaterProcessor.WatchDogInactive += RepeaterProcessor_WatchDogInactive;
            repeaterProcessor.WatchDogTimeout += RepeaterProcessor_WatchDogTimeout;

            repeaterProcessor.ValidRACCode += RepeaterProcessor_ValidRACCode;

            repeaterProcessor.DTMFToneDetected += RepeaterProcessor_DTMFToneDetected;

            repeaterProcessor.PLToneDetected += RepeaterProcessor_PLToneDetected;
            repeaterProcessor.DPLToneDetected += RepeaterProcessor_DPLToneDetected;
#endif
            repeaterProcessor.RepeaterKeyed += RepeaterProcessor_RepeaterKeyed;
            repeaterProcessor.RepeaterDekeyed += RepeaterProcessor_RepeaterDekeyed;
            repeaterProcessor.RepeaterIdle += RepeaterProcessor_RepeaterIdle;

            repeaterProcessor.RepeaterTimeoutCleared += RepeaterProcessor_RepeaterTimeoutCleared;
            repeaterProcessor.RepeaterTimeout += RepeaterProcessor_RepeaterTimeout;
            repeaterProcessor.RepeaterActive += RepeaterProcessor_RepeaterActive;

            repeaterProcessor.MDCPacketDetected += RepeaterProcessor_MDCPacketDetected;

            repeaterProcessor.WaveInMeterSampling += RepeaterProcessor_WaveInMeterSampling;
            repeaterProcessor.WaveOutMeterSampling += RepeaterProcessor_WaveOutMeterSampling;
            repeaterProcessor.MDCSamples += RepeaterProcessor_MDCSamples;

            SetOptionsFromXml();

            // initialize audio
            InitializeAudio();

#if MDC_CONSOLE
            this.toolStripMDCConsoleLabel.Visible = true;
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proc"></param>
        /// <returns></returns>
        private static IntPtr SetKeybdHook(LowLevelKeyboardProc proc)
        {
            const int WH_KEYBOARD_LL = 13;
            using (Process curProcess = Process.GetCurrentProcess())
                using (ProcessModule curModule = curProcess.MainModule)
                    return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private static IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            const int WM_KEYDOWN = 0x0100;
            const int WM_KEYUP = 0x0101;

            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
                keyDownEvent = true;
            else if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP)
            {
                Keys keyData = (Keys)Marshal.ReadInt32(lParam);
                if (keyDownEvent && (Instance != null))
                {
                    if (keyData == (Keys)Instance.optionsModal.Options.PTTKey)
                        Instance.txToolPTT_Click(Instance, new EventArgs());

                    keyDownEvent = false;
                }
            }

            return CallNextHookEx(keybdHookId, nCode, wParam, lParam);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        private void InvokeIfVisible(Delegate method)
        {
            try
            {
                if (this.Visible)
                    this.Invoke(method);
            }
            catch (ObjectDisposedException) { /* ignore */ }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepeaterProcessor_WaveOutMeterSampling(object sender, StreamVolumeEventArgs e)
        {
            txVolumeMeter.Amplitude = e.MaxSampleValues[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepeaterProcessor_WaveInMeterSampling(object sender, StreamVolumeEventArgs e)
        {
            // poll rx volume meter anyway
            rxVolumeMeter.Amplitude = e.MaxSampleValues[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mdcProcessor"></param>
        /// <param name="buffer"></param>
        private void RepeaterProcessor_MDCSamples(object sender, MDCWaveIn mdcProcessor, byte[] buffer)
        {
            if (decodeEncodedPacketsToolStripMenuItem.Checked)
            {
                if (buffer != null)
                    mdcProcessor.ProcessSamples(buffer);
            }
        }

        /// <summary>
        /// Event that is fired when a DTMF tone is detected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="tone"></param>
        private void RepeaterProcessor_DTMFToneDetected(object sender, DtmfToneEnd tone)
        {
            if (DTMFToneDetected != null)
                DTMFToneDetected(this, tone);
        }

        /// <summary>
        /// Event pass-thru for the PL tone processor, where a PL tone is detected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="code"></param>
        private void RepeaterProcessor_DPLToneDetected(object sender, List<int> code)
        {
            this.lastDetectedDPL = code[0].ToString("000");
            this.InvokeIfVisible(new Action(() =>
            {
                this.Invalidate();
            }));
        }

        /// <summary>
        /// Event pass-thru for the PL tone processor, where a PL tone is detected.
        /// </summary>
        /// <param name="sender"></param>
        private void RepeaterProcessor_PLToneDetected(object sender, double tone)
        {
            this.lastDetectedPL = tone.ToString("000.0");
            this.InvokeIfVisible(new Action(() =>
            {
                this.Invalidate();
            }));
        }

        /// <summary>
        /// Event that occurs when we detect the correct and valid MDC RAC code.
        /// </summary>
        /// <param name="sender"></param>
        private void RepeaterProcessor_ValidRACCode(object sender)
        {
            this.InvokeIfVisible(new Action(() =>
            {
                this.toolStripRACLabel.ForeColor = Color.DarkGreen;
            }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="first"></param>
        private void HandleInternalMDC(MDCPacket first)
        {
            if (first.Operation == OpType.RC_TXT_MESSAGE)
            {
                if (!mdcGotTextMessage)
                    mdcTextMessage = string.Empty;
                mdcGotTextMessage = true;

                char[] chars = new char[3];

                chars[0] = (char)first.Argument;
                chars[1] = (char)((first.Target >> 8) & 0xFF);
                chars[2] = (char)(first.Target & 0xFF);

                for (int i = 0; i < 3; i++)
                    mdcTextMessage += chars[i];
            }

            if (first.Operation == OpType.RC_TXT_END)
                mdcGotTextMessage = false;
            if (first.Operation == OpType.RC_TXT_PAD)
            {
                if (!mdcGotTextMessage)
                    mdcTextMessage = string.Empty;
                mdcGotTextMessage = true;
            }

            if ((first.Operation != OpType.RC_TXT_MESSAGE) &&
                (first.Operation != OpType.RC_TXT_PAD) &&
                (first.Operation != OpType.RC_TXT_END))
            {
                mdcGotTextMessage = false;
                mdcTextMessage = string.Empty;
            }
        }

        /// <summary>
        /// Event pass-thru for the MDC packet processor, where a MDC packet is detected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="frameCount"></param>
        /// <param name="first"></param>
        /// <param name="second"></param>
        private void RepeaterProcessor_MDCPacketDetected(object sender, int frameCount, MDCPacket first, MDCPacket second)
        {
            HandleInternalMDC(first);

            if (mdcGotTextMessage == false && mdcTextMessage != string.Empty)
            {
                NewLogMessage("MDC Text Message: " + mdcTextMessage, "MDC");
                mdcTextMessage = string.Empty;
            }
            else
            {
                NewLogMessage(MDCDetector.ToString(first), "MDC");

                if (frameCount == 2)
                    NewLogMessage(MDCDetector.ToString(second), "MDC");

                // is this a PTT ID?
                if (first.Operation == OpType.PTT_ID)
                {
                    this.targetID = first.Target;
                    if (this.Visible)
                    {
                        this.targetMDCID.Invoke(new Action<MDCPacket>((MDCPacket packet) =>
                        {
                            this.targetMDCID.Text = this.targetID.ToString("X4");
                        }), new object[] { first });
                    }
                }

                // is this an emergency packet?
                if (first.Operation == OpType.EMERGENCY)
                {
                    // if we're not automatically acknowledging emergency packets, enable
                    // the console button to acknowledge the packet, otherwise transmit the ack packet
                    if (!this.optionsModal.Options.AutoAckEmergency)
                        this.emergAck.Enabled = true;
                    else
                        emergAck_Click(this, new EventArgs());
                }
            }

            if (this.optionsModal.Options.EnableDatabaseLogging)
            {
                // fire a thread so we can async do DB ops
                new Thread(() =>
                {
                    MDCPacket logFirst = new MDCPacket(first);
                    MDCPacket logSecond = new MDCPacket(second);
                    int logFC = frameCount;

                    mysqlMRE.WaitOne();
                    mysqlMRE.Reset();

                    // log to database if enabled
                    if (this.optionsModal.Options.EnableDatabaseLogging && mysqlOpen)
                    {
                        string dateString = DateTime.Now.ToShortDateString();
                        string timeString = DateTime.Now.ToLongTimeString();

                        MySqlCommand cmd = new MySqlCommand("INSERT INTO " + MYSQL_MDC_LOG_TABLE + " (date, time, op, arg, target) VALUES (" +
                            string.Format("\"{0}\", \"{1}\", {2}, {3}, {4}", dateString, timeString, logFirst.Operation, logFirst.Argument, logFirst.Target) + ")", mysqlConn);
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (MySqlException mse) {
                            Messages.Write(mse.Message, mse); }

                        if (logFC == 2)
                        {
                            cmd = new MySqlCommand("INSERT INTO " + MYSQL_MDC_LOG_TABLE + " (date, time, op, arg, target) VALUES (" +
                                string.Format("\"{0}\", \"{1}\", {2}, {3}, {4}", dateString, timeString, logSecond.Operation, logSecond.Argument, logSecond.Target) + ")", mysqlConn);
                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (MySqlException) { /* ignore */ }
                        }
                    }

                    mysqlMRE.Set();
                }).Start();
            }
        }

        /// <summary>
        /// Event that occurs when all conditions to repeat audio are met.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="time"></param>
        private void RepeaterProcessor_RepeaterActive(object sender, double time)
        {
            // make sure timeout isn't visible, disabled isn't visible, and 
            // set duration to current tx duration
            this.InvokeIfVisible(new Action(() =>
            {
                this.toolStripStatusLabel.Visible = false;
                this.txDuration.Text = "Active for " + (int)(time) + " (s)";
            }));
        }

        /// <summary>
        /// Event that occurs when the repeater transmitter timeout occurs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepeaterProcessor_RepeaterTimeout(object sender, EventArgs e)
        {
            // make sure the timeout label is displayed, the tx vol meter is zeroed,
            // and the current duration is "Timeout"
            this.InvokeIfVisible(new Action(() =>
            {
                // if we're just asserting a timeout, log the entry
                if (!repeaterProcessor.RepeatAudioTimeout)
                    NewLogMessage("Transmitter Timeout (>" + (this.optionsModal.Options.MaxTransmissionTime * 1000) + "s) - During Transmission");

                this.toolStripStatusLabel.Visible = true;
                this.toolStripStatusLabel.Text = "Timeout";
                this.txVolumeMeter.Amplitude = VOLUME_METER_NOISE_FLOOR;
                this.txDuration.Text = "Timeout";
            }));
        }

        /// <summary>
        /// Event that occurs when the repeater transmitter timeout is cleared.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepeaterProcessor_RepeaterTimeoutCleared(object sender, EventArgs e)
        {
            // make sure the timeout label isn't displayed, disable label isn't displayed,
            // and the current duration is "Inactive"
            this.InvokeIfVisible(new Action(() =>
            {
                // if we're just clearing a timeout, log the entry
                if (!repeaterProcessor.RepeatAudioTimeout)
                    NewLogMessage("Transmitter Timeout - Buffered Audio Cleared");
                else
                    NewLogMessage("Transmitter Timeout - Cleared");

                this.toolStripStatusLabel.Visible = false;
                this.txDuration.Text = "Inactive";
            }));
        }

        /// <summary>
        /// Event that occurs when the repeater is idle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepeaterProcessor_RepeaterIdle(object sender, EventArgs e)
        {
            // make sure the timeout label is displayed, tx vol is zereoed, and we set
            // the current duration to "Inactive"
            this.InvokeIfVisible(new Action(() =>
            {
                this.toolStripStatusLabel.Visible = false;
                this.txVolumeMeter.Amplitude = VOLUME_METER_NOISE_FLOOR;
                this.txDuration.Text = "Inactive";

                if (this.optionsModal.Options.MDCConsoleOnly)
                    this.rxVolumeMeter.Amplitude = VOLUME_METER_NOISE_FLOOR;
            }));
        }

        /// <summary>
        /// Event that occurs when the watch dog times out.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepeaterProcessor_WatchDogTimeout(object sender, EventArgs e)
        {
            // make sure the timeout label is displayed, the tx vol meter is zeroed,
            // and the current duration is "Timeout"
            this.InvokeIfVisible(new Action(() =>
            {
                NewLogMessage("Transmitter Stuck!! Watch Dog Timeout");

                this.toolStripStatusLabel.Visible = true;
                this.toolStripStatusLabel.Text = "Timeout";
                this.txVolumeMeter.Amplitude = VOLUME_METER_NOISE_FLOOR;
                this.txDuration.Text = "Timeout";
            }));
        }

        /// <summary>
        /// Event that occurs when the watch dog is inactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepeaterProcessor_WatchDogInactive(object sender, EventArgs e)
        {
            // make sure the timeout label isn't displayed, disable label isn't displayed,
            // and the current duration is "Inactive"
            this.InvokeIfVisible(new Action(() =>
            {
                this.toolStripStatusLabel.Visible = false;
                this.txDuration.Text = "Inactive";
            }));
        }

        /// <summary>
        /// Event that occurs when all conditions to repeat audio are not met.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepeaterProcessor_RepeatingAudioFailed(object sender, EventArgs e)
        {
            // just PL only
            if (this.optionsModal.Options.EnableRxPL)
            {
                this.InvokeIfVisible(new Action(() =>
                {
                    this.toolStripPLLabel.ForeColor = Color.DarkRed;
                }));
            }
        }

        /// <summary>
        /// Event that occurs when all conditions to repeat audio are met.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepeaterProcessor_RepeatingAudio(object sender, EventArgs e)
        {
            // just PL only
            if (this.optionsModal.Options.EnableRxPL)
            {
                this.InvokeIfVisible(new Action(() =>
                {
                    this.toolStripPLLabel.ForeColor = Color.DarkGreen;
                }));
            }
        }

        /// <summary>
        /// Event that occurs when the repeater is dekeyed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepeaterProcessor_RepeaterDekeyed(object sender, EventArgs e)
        {
            // hide the transmitting label
            this.InvokeIfVisible(new Action(() =>
            {
                this.toolStripTransmitLabel.Visible = false;
            }));
            IsRepeaterTx = false;

            this.InvokeIfVisible(new Action(() =>
            {
                this.toolStripRACLabel.ForeColor = Color.DarkRed;
                this.toolStripPLLabel.ForeColor = Color.DarkRed;
            }));
        }

        /// <summary>
        /// Event that occurs when the repeater is keyed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepeaterProcessor_RepeaterKeyed(object sender, EventArgs e)
        {
            if (this.optionsModal.Options.TxVOX)
            {
                // display the transmitting label
                this.InvokeIfVisible(new Action(() =>
                {
                    this.toolStripTransmitLabel.Text = "VOX";
                    this.toolStripTransmitLabel.Visible = true;
                }));
                IsRepeaterTx = true;
            }
            else
            {
                // display the transmitting label
                this.InvokeIfVisible(new Action(() =>
                {
                    this.toolStripTransmitLabel.Text = "Transmitting";
                    this.toolStripTransmitLabel.Visible = true;
                }));
                IsRepeaterTx = true;
            }

            this.lastTx = DateTime.Now;
            this.InvokeIfVisible(new Action(() =>
            {
                this.lastTransmissionLabel.Text = this.lastTx.ToShortDateString() + " " + this.lastTx.ToLongTimeString();
            }));
        }

        /// <summary>
        /// Event that occurs when the repeater transmitter is enabled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepeaterProcessor_RepeaterEnabled(object sender, EventArgs e)
        {
            if (lastDisabledState != repeaterProcessor.DisableTransmitter)
            {
                this.InvokeIfVisible(new Action(() =>
                {
                    disableTransmitterButton.Text = "Disable Transmitter";
                }));
                NewLogMessage("Enabling transmitter from disabled state");
                lastDisabledState = repeaterProcessor.DisableTransmitter;
            }
        }

        /// <summary>
        /// Event that occurs when the repeater transmitter disabled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepeaterProcessor_RepeaterDisabled(object sender, EventArgs e)
        {
            if (lastDisabledState != repeaterProcessor.DisableTransmitter)
            {
                // make sure the timeout label is displayed, disabled label is visible,
                // tx vol is zeroed, and current duration to "Disabled"
                this.InvokeIfVisible(new Action(() =>
                {
                    this.toolStripStatusLabel.Visible = true;
                    this.toolStripStatusLabel.Text = "Disabled";
                    this.txVolumeMeter.Amplitude = VOLUME_METER_NOISE_FLOOR;
                    this.txDuration.Text = "Disabled";

                    disableTransmitterButton.Text = "Enable Transmitter";
                }));
                NewLogMessage("Disabling transmitter from enabled state");
                lastDisabledState = repeaterProcessor.DisableTransmitter;
            }
        }

        /// <summary>
        /// Event that occurs when the tray icon is double-clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            //this.Hide();
            this.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// Event that occurs when the tray icon is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        /// <summary>
        /// Event that occurs when the form is resized.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Resize(object sender, EventArgs e)
        {
            /*
            if (WindowState == FormWindowState.Minimized)
                this.Hide();
            */
        }

        /// <summary>
        /// Occurs when the main program dialog is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            // forward to the exit tool strip event handler...
            exitToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Event that occurs when the window form is painted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.lastTransmissionLabel.Text = this.lastTx.ToShortDateString() + " " + this.lastTx.ToLongTimeString();
            if (this.automaticCw != null)
                this.lastId.Text = this.automaticCw.LastCWID.ToShortDateString() + " " + this.automaticCw.LastCWID.ToLongTimeString();
            else
                this.lastId.Text = string.Empty;
            this.lastPl.Text = lastDetectedPL;
            this.lastDPL.Text = lastDetectedDPL;
        }

        /// <summary>
        /// Helper to clear the repeater activity logs.
        /// </summary>
        public void ClearLogs()
        {
            this.Invoke(new Action(() =>
            {
                activityLog.Items.Clear();
            }));
        }

        /// <summary>
        /// 
        /// </summary>
        public void GenerateCourtesyTone()
        {
            repeaterProcessor.GenerateCourtesyTone();
        }

        /// <summary>
        /// Helper to add a new log message to the activty log.
        /// </summary>
        /// <returns></returns>
        public void NewLogMessage(string newLogMessage, string prefix = "")
        {
            try
            {
                Messages.Trace(prefix + " " + newLogMessage, LogFilter.GENERAL_TRACE, 2);

                // trim activity log
                if (activityLog.Items.Count > MaxActivityLogCount)
                    try
                    {
                        activityLog.Invoke(new Action(() =>
                        {
                            activityLog.Items.RemoveAt(activityLog.Items.Count - 1);
                        }));
                    }
                    catch (InvalidOperationException)
                    {
                        /* ignore */
                    }

                // add new log entry
                string dateString = DateTime.Now.ToShortDateString();
                string timeString = DateTime.Now.ToLongTimeString();
                try
                {
                    activityLog.Invoke(new Action<string>((string str) =>
                    {
                        activityLog.Items.Insert(0, dateString + " " + timeString + "\t" + str);
                    }), new object[] { newLogMessage });
                }
                catch (InvalidOperationException)
                {
                    /* ignore */
                }

                if (this.optionsModal.Options.EnableDatabaseLogging)
                {
                    // fire a thread so we can async do DB ops
                    new Thread(() =>
                    {
                        mysqlMRE.WaitOne();
                        mysqlMRE.Reset();

                        // log to database if enabled
                        if (this.optionsModal.Options.EnableDatabaseLogging && mysqlOpen)
                        {
                            MySqlCommand cmd = new MySqlCommand("INSERT INTO " + MYSQL_LOG_TABLE + " (date, time, data) VALUES (" +
                                string.Format("\"{0}\", \"{1}\", \"{2}\"", dateString, timeString, newLogMessage) + ")", mysqlConn);
                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (MySqlException) { /* ignore */ }
                        }

                        mysqlMRE.Set();
                    }).Start();
                }
            }
            catch (Exception) { /* ignore */ }
        }

        /// <summary>
        /// Resets the application to default settings.
        /// </summary>
        private void ResetSettings()
        {
            this.targetID = 0x0001;

            // reset repeater options modal
            if (optionsModal == null)
                this.optionsModal = new RepeaterOptions(this);
            else
            {
                this.optionsModal.SetDefaults();
                this.optionsModal.UpdateModal();
            }

            // reset audio configuration modal
            if (audioDeviceModal == null)
                this.audioDeviceModal = new ConfigureAudioDevice(optionsModal);
            else
            {
                this.audioDeviceModal.SetDefaults();
                this.audioDeviceModal.UpdateModal();
            }
            
            if (repeaterProcessor != null)
                repeaterProcessor.ResetCourtesyToneFile();
        }

        /// <summary>
        /// Load the user settings to XML.
        /// </summary>
        private void LoadXml()
        {
            try
            {
                rsrc = new XmlResource(Environment.CurrentDirectory + Path.DirectorySeparatorChar + XML_FILE);
                if (rsrc.FailedDigest)
                    rsrc.SaveXml(Environment.CurrentDirectory + Path.DirectorySeparatorChar + XML_FILE);

                // read general settings not controlled by other modals
                try
                {
                    FixedAnncVol = rsrc.Get<int>("FixedAnncVol");
                    CWFrequency = rsrc.Get<int>("CWFrequency");
                    CWWPM = rsrc.Get<int>("CWWPM");
                    MaxActivityLogCount = rsrc.Get<int>("MaxActLogCount");
                    AudioDropMS = rsrc.Get<int>("AudioDropMS");

                    this.decodeEncodedPacketsToolStripMenuItem.Checked = rsrc.Get<bool>("DecodeEncoded");
                }
                catch (ArgumentException ae)
                {
                    // this will occur when a value isn't found
                    Messages.TraceException(ae.Message, ae);
                }
            }
            catch (Exception)
            {
                // reset to defaults
                ResetSettings();
                SaveXml();
            }
        }

        /// <summary>
        /// Save the user settings to XML.
        /// </summary>
        private void SaveXml()
        {
            if (rsrc == null)
                rsrc = new XmlResource();

            // create general settings table
            XmlResource general = rsrc.CreateNode("GeneralSettings");
            general.Submit("FixedAnncVol", FixedAnncVol);
            general.Submit("CWFrequency", CWFrequency);
            general.Submit("CWWPM", CWWPM);
            general.Submit("MaxActLogCount", MaxActivityLogCount);
            general.Submit("AudioDropMS", AudioDropMS);
            general.Submit("DecodeEncoded", this.decodeEncodedPacketsToolStripMenuItem.Checked);

            rsrc.SaveXml(Environment.CurrentDirectory + Path.DirectorySeparatorChar + XML_FILE);
        }

        /// <summary>
        /// Helper to set options from XML
        /// </summary>
        private void SetOptionsFromXml()
        {
            repeaterProcessor.DisableTransmitter = true;

            RepeaterWaveIn.AudioDropMS = AudioDropMS;

            // set the encoder preambles
            repeaterProcessor.MDC.NumberOfPreambles = this.optionsModal.Options.NumberOfPreambles;

            repeaterProcessor.ResetCourtesyToneFile();

            // is this an MDC only console?
            if (this.optionsModal.Options.MDCConsoleOnly)
            {
                toolStripMDCConsoleLabel.Visible = true;
                disableTransmitterButton.Enabled = false;
                clearTimeoutButton.Enabled = false;
#if MDC_CONSOLE
                disableTransmitterButton.Visible = false;
                clearTimeoutButton.Visible = false;

                transmitCallsignCW.Visible = false;

                advancedToolStripMenuItem.Visible = false;
#endif

                toolStripMDCConsoleLabel.Text = "MDC Console";

                // if this is MDC only console -- check if we're allowing announcments and DTMF
                if (!this.optionsModal.Options.AllowConsoleAnncDTMF)
                {
                    toolStripAnnouncementsButton.Enabled = false;
                    toolStripDtmfCommandsButton.Enabled = false;

                    announcementsToolStripMenuItem.Enabled = false;
                    dtmfCommandsToolStripMenuItem.Enabled = false;
                    transmitTimeAnnc.Enabled = false;
                    buttonTestAnnounce.Enabled = false;
                }
                else
                {
                    toolStripAnnouncementsButton.Enabled = true;
                    toolStripDtmfCommandsButton.Enabled = true;

                    announcementsToolStripMenuItem.Enabled = true;
                    dtmfCommandsToolStripMenuItem.Enabled = true;
                    transmitTimeAnnc.Enabled = true;
                    buttonTestAnnounce.Enabled = true;
                }

                // kill any transmissions taking place...
                repeaterProcessor.DekeyTransmitter();
                repeaterProcessor.ReleaseStates();
            }
            else
            {
                toolStripMDCConsoleLabel.Visible = false;
                disableTransmitterButton.Enabled = true;
                transmitTimeAnnc.Enabled = true;
                clearTimeoutButton.Enabled = true;

                groupBoxStatus.Visible = true;
                toolStripAnnouncementsButton.Enabled = true;
                toolStripDtmfCommandsButton.Enabled = true;

                announcementsToolStripMenuItem.Enabled = true;
                dtmfCommandsToolStripMenuItem.Enabled = true;
                buttonTestAnnounce.Enabled = true;

                // are we using RAC?
                if (this.optionsModal.Options.UseRAC)
                {
                    this.toolStripRACLabel.Visible = true;
                    this.toolStripRACLabel.ForeColor = Color.DarkRed;
                }
                else
                    this.toolStripRACLabel.Visible = false;

                // are we using PL?
                if (this.optionsModal.Options.EnableRxPL)
                {
                    this.toolStripPLLabel.Visible = true;
                    this.toolStripPLLabel.ForeColor = Color.DarkRed;
                    this.lastPl.Visible = true;
                    this.labelLastPL.Visible = true;
                    this.lastDPL.Visible = true;
                    this.labelLastDPL.Visible = true;
                }
                else
                {
                    this.toolStripPLLabel.Visible = false;
                    this.lastPl.Visible = false;
                    this.labelLastPL.Visible = false;
                    this.lastDPL.Visible = false;
                    this.labelLastDPL.Visible = false;
                }

                this.targetIDLabel.Visible = true;
                this.targetMDCID.Visible = true;

                this.generateMDCGroupBox.Visible = true;
            }

            if (waveFormat != null)
                InitializeAudio();

            // initialize serial ports
            if (this.optionsModal.Options.RxSerialPort != "VOX")
            {
                if (this.rxPort != null)
                    this.rxPort.Close();

                // baud rate is unimportant
                try
                {
                    this.rxPort = new SerialPort(this.optionsModal.Options.RxSerialPort, 9600);
                    this.rxPort.Open();
                }
                catch (IOException ioe)
                {
                    Messages.Write(ioe.Message, ioe);
                    repeaterOptionsToolStripMenuItem_Click(this, new EventArgs());
                    return;
                }
            }
            else
                this.rxPort = null;

            if (this.optionsModal.Options.TxSerialPort != "VOX")
            {
                if (this.txPort != null)
                {
                    this.txPort.RtsEnable = false;
                    this.txPort.DtrEnable = false;
                    this.txPort.Close();
                }

                // baud rate is unimportant
                try
                {
                    this.txPort = new SerialPort(this.optionsModal.Options.TxSerialPort, 9600);
                    this.txPort.Open();
                }
                catch (IOException ioe)
                {
                    Messages.Write(ioe.Message, ioe);
                    repeaterOptionsToolStripMenuItem_Click(this, new EventArgs());
                    return;
                }
            }
            else
                this.txPort = null;

            repeaterProcessor.SetPorts(rxPort, txPort);

            if (repeaterProcessor.TTS != null && !this.optionsModal.Options.DisableAnnouncements)
            {
                if (this.optionsModal.Options.AnnouncementVoice == string.Empty)
                    repeaterProcessor.TTS.SelectVoice(null);
                else
                    repeaterProcessor.TTS.SelectVoice(this.optionsModal.Options.AnnouncementVoice);
            }
            else
                repeaterProcessor.TTS.SelectVoice(null);

            repeaterProcessor.PL.Gain = this.optionsModal.Options.TxPLGain;
            repeaterProcessor.DPL.Gain = this.optionsModal.Options.TxPLGain;

            repeaterProcessor.DTMFToneSeconds = this.optionsModal.Options.DTMFDigitTime;

            if (this.optionsModal.Options.EnableRxPL)
            {
                this.labelLastPL.Visible = true;
                this.labelLastDPL.Visible = true;
                this.lastPl.Visible = true;
                this.lastDPL.Visible = true;
            }
            else
            {
                this.labelLastPL.Visible = false;
                this.labelLastDPL.Visible = false;
                this.lastPl.Visible = false;
                this.lastDPL.Visible = false;
            }

            SAPITTS.SynthAudioGain = this.optionsModal.Options.SynthAnncGain;

            this.rxVolumeMeter.DetectDb = 20 * (float)Math.Log10(this.optionsModal.Options.VOXDetectLevel);
            this.txVolumeMeter.DetectDb = 20 * (float)Math.Log10(this.txVolumeMeter.MaxDb);
            if (this.optionsModal.Options.Callsign != string.Empty)
                this.sysCallsign.Text = this.optionsModal.Options.Callsign;
            else
                this.sysCallsign.Text = "Set Your Callsign";
            if (this.optionsModal.Options.MDCConsoleOnly)
            {
#if MDC_CONSOLE
                this.Text = "MDC Console";

                this.toolStripRepeaterOptionsButton.ToolTipText = "Console Options";
                this.repeaterOptionsToolStripMenuItem.Text = "Console Options";
                this.toolStripSeparator1.Visible = false;
#endif
            }

            if (this.optionsModal.Options.EnableDatabaseLogging)
            {
                mysqlConn = new MySqlConnection();

                string connString = string.Format("Server={0}; database={1}; UID={2}; password={3}",
                    this.optionsModal.Options.DatabaseServer, this.optionsModal.Options.DatabaseName,
                    this.optionsModal.Options.DatabaseUsername, this.optionsModal.Options.DatabasePassword);

                mysqlConn = new MySqlConnection(connString);
                try
                {
                    mysqlConn.Open();
                    mysqlOpen = true;
                }
                catch (MySqlException mse)
                {
                    mysqlOpen = false;
                    Messages.Write(mse.Message, mse);
                }

                if (mysqlOpen)
                {
                    // test for and create the logging table
                    MySqlCommand cmd = new MySqlCommand("SELECT 1 FROM " + MYSQL_LOG_TABLE + " LIMIT 1", mysqlConn);
                    try
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                            dr.Close();
                    }
                    catch (MySqlException)
                    {
                        cmd = new MySqlCommand("CREATE TABLE " + MYSQL_LOG_TABLE + " (id INT AUTO_INCREMENT KEY, date VARCHAR(45), time VARCHAR(45), data VARCHAR(128), INDEX (id))", mysqlConn);
                        cmd.ExecuteNonQuery();
                    }

                    // test for and create the mdc logging table
                    cmd = new MySqlCommand("SELECT 1 FROM " + MYSQL_MDC_LOG_TABLE + " LIMIT 1", mysqlConn);
                    try
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                            dr.Close();
                    }
                    catch (MySqlException)
                    {
                        cmd = new MySqlCommand("CREATE TABLE " + MYSQL_MDC_LOG_TABLE + " (id INT AUTO_INCREMENT KEY, date VARCHAR(45), time VARCHAR(45), op SMALLINT, arg SMALLINT, target INT, INDEX (id))", mysqlConn);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            repeaterProcessor.DisableTransmitter = false;
        }

        /// <summary>
        /// Initializes the audio wave devices and processors.
        /// </summary>
        private void InitializeAudio()
        {
            try
            {
                // initialize the repeater controller
                repeaterProcessor.Options = this.optionsModal.Options;
                repeaterProcessor.BufferMilliseconds = this.audioDeviceModal.BufferMilliseconds;
                repeaterProcessor.NumberOfBuffers = this.audioDeviceModal.BufferCount;
                repeaterProcessor.AFSKBufferMilliseconds = this.audioDeviceModal.AFSKBufferMilliseconds;
                repeaterProcessor.AFSKNumberOfBuffers = this.audioDeviceModal.AFSKBufferCount;
                repeaterProcessor.DisableFiltering = this.audioDeviceModal.DisableFiltering;
                repeaterProcessor.Reset(waveFormat, this.audioDeviceModal.WaveInDevice, this.audioDeviceModal.WaveOutDevice);
                repeaterProcessor.StartRecording();

                if (this.audioDeviceModal.DisableFiltering)
                    this.toolStripDisableFilteringLabel.Visible = true;
                else
                    this.toolStripDisableFilteringLabel.Visible = false;

                // initialize automatic CW
                if (automaticCw != null)
                    this.automaticCw.Stop();

                this.automaticCw = new AutomaticCW(this, optionsModal);
                repeaterProcessor.AutomaticCW = this.automaticCw;

                this.automaticCw.PlayingCW += AutomaticCw_PlayingCW;
                this.automaticCw.Start();

                // initialize automatic annc
                if (automaticAnnc != null)
                    this.automaticAnnc.Stop();

                this.automaticAnnc = new AutomaticAnnc(repeaterProcessor.TTS, this, optionsModal);
                repeaterProcessor.AutomaticAnnc = this.automaticAnnc;

                this.automaticAnnc.PlayingAnnouncment += AutomaticAnnc_PlayingAnnouncment;
                this.automaticAnnc.Start();

                this.anncModal.Announcer = this.automaticAnnc;

                this.Invoke(new Action(() =>
                {
                    Invalidate();
                }));
            }
            catch (Exception e)
            {
                Messages.Write("Failed to initialize input audio device(s).", e);
            }
        }

        /// <summary>
        /// Event that occurs when the automatic announcer plays an announcement.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="name"></param>
        private void AutomaticAnnc_PlayingAnnouncment(object sender, string name)
        {
            NewLogMessage("Transmitted Announcement - " + name);
        }

        /// <summary>
        /// Event that occurs when the automatic CW plays the CW.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutomaticCw_PlayingCW(object sender, EventArgs e)
        {
            NewLogMessage("Transmitted CW");
        }

        /// <summary>
        /// Occurs when the "About..." menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();
            about.ShowDialog();
        }

        /// <summary>
        /// Occurs when the "Exit" menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.notifyIcon.Visible = false;

            if (this.repeaterProcessor != null)
            {
                repeaterProcessor.DisableTransmitter = true;
                repeaterProcessor.StopRecording();
                repeaterProcessor.Dispose();
                repeaterProcessor = null;
            }

            SaveXml();

            // ignore any exceptions here
            try
            {
                Environment.Exit(0);
            }
            catch (Exception)
            {
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Occurs when the "Configure Audio Button" toolstrip item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripConfigureAudioButton_Click(object sender, EventArgs e)
        {
            configureAudioDeviceToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Event that occurs when the "Configure Audio Device..." toolstrip menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configureAudioDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            repeaterProcessor.DisableTransmitter = true;
            audioDeviceModal.UpdateModal();
            audioDeviceModal.ShowDialog();
            SaveXml();

            InitializeAudio();
            repeaterProcessor.DisableTransmitter = false;
        }

        /// <summary>
        /// Occurs when the "Repeater Options" toolstrip item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripRepeaterOptionsButton_Click(object sender, EventArgs e)
        {
            repeaterOptionsToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Occurs when the "Repeater Options..." menu toolstrip item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repeaterOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            optionsModal.ShowDialog();
            if (optionsModal.Canceled)
                optionsModal.LoadXml();
            else
                SaveXml();
            SetOptionsFromXml();
        }

        /// <summary>
        /// Occurs when the "Announcements..." toolstrip item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripAnnouncementsButton_Click(object sender, EventArgs e)
        {
            announcementsToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Occurs when the "Announcements..." menu toolstrip item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void announcementsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            anncModal.UpdateModal();
            anncModal.ShowDialog();
            if (anncModal.Canceled)
                anncModal.LoadXml();
            else
                anncModal.SaveXml();
        }

        /// <summary>
        /// Occurs when the "DTMF Commands..." toolstrip item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripDtmfCommandsButton_Click(object sender, EventArgs e)
        {
            dtmfCommandsToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Occurs when the "DTMF Commands..." menu toolstrip item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtmfCommandsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dtmfModal.UpdateModal();
            dtmfModal.ShowDialog();
            if (dtmfModal.Canceled)
                dtmfModal.LoadXml();
            else
                dtmfModal.SaveXml();
        }

        /// <summary>
        /// Occurs when the "Clear..." menu toolstrip item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearLogs();
        }

        /** Form Functions */
        /// <summary>
        /// Internal function to invalidate the Target ID input box.
        /// </summary>
        private void InvalidateTargetID()
        {
            targetMDCID.ForeColor = Color.Red;
            targetID = 0x0001;
        }

        /// <summary>
        /// Internal function to validate the Target ID input box.
        /// </summary>
        private void ValidateTargetID()
        {
            try
            {
                targetMDCID.ForeColor = Color.Black;
                targetID = Convert.ToUInt16(targetMDCID.Text, 16);
            }
            catch (FormatException)
            {
                InvalidateTargetID();
            }
            catch (OverflowException)
            {
                InvalidateTargetID();
            }
        }

        /// <summary>
        /// Occurs when the text in the Target ID box changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void targetMDCID_TextChanged(object sender, EventArgs e)
        {
            if (targetMDCID.Text.Length < 4)
                InvalidateTargetID();
            else
                ValidateTargetID();
        }

        /// <summary>
        /// Occurs when the "Transmit Callsign CW" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void transmitCallsignCW_Click(object sender, EventArgs e)
        {
            this.automaticCw.PlayRepeaterCW();
        }

        /// <summary>
        /// Occurs when the "Transmit Time Annc" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void transmitTimeAnnc_Click(object sender, EventArgs e)
        {
            this.automaticAnnc.PlayTimeAnnc();
        }

        /// <summary>
        /// Occurs when the "Disable Transmitter" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void disableTransmitterButton_Click(object sender, EventArgs e)
        {
            if (repeaterProcessor.DisableTransmitter)
                repeaterProcessor.DisableTransmitter = false;
            else
                repeaterProcessor.DisableTransmitter = true;
        }

        /// <summary>
        /// Occurs when the "Clear Timeout" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearTimeoutButton_Click(object sender, EventArgs e)
        {
            repeaterProcessor.ClearTransmitterTimeout = true;
        }

        /// <summary>
        /// Occurs when the "Test Annc" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonTestAnnounce_Click(object sender, EventArgs e)
        {
            if (repeaterProcessor.TTS.VoiceInitFailed)
                MessageBox.Show("No voices installed! Synthesized voice is not available!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                string text = "<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"en-US\">" + "<s>Testing. 1, 2, 3.</s>" + "</speak>";
                annc_ret_t annc = new annc_ret_t()
                {
                    Name = "Test Announcement",
                    Interval = new DateTime(1758, 1, 1, 0, 5, 0, 0).ToOADate(),
                    LastRun = DateTime.Now.ToOADate(),
                    NextRun = DateTime.Now.ToOADate(),
                    Filename = string.Empty,
                    RawSyntheizedText = text,
                    IsWaveFile = false,
                    IsSuppliedText = true,
                };

                automaticAnnc.PlayRepeaterAnnc(annc, DateTime.Now);
            }
        }

        /** CONSOLE BUTTON OPERATIONS */
        /// <summary>
        /// Occurs when the "Tool PTT ID" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txToolPTT_Click(object sender, EventArgs e)
        {
            // generate MDC packet
            MDCPacket pckt = new MDCPacket()
            {
                Operation = OpType.PTT_ID,
                Argument = ArgType.NO_ARG,
                Target = this.optionsModal.Options.MyID
            };
            repeaterProcessor.MDC.GenerateMDC(pckt);
        }

        /// <summary>
        /// Occurs when the "Stun Target" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stunButton_Click(object sender, EventArgs e)
        {
            TargetID tgtId = new TargetID(this.targetID);
            tgtId.ShowDialog();

            if (!tgtId.Canceled)
            {
                // generate MDC packet
                MDCPacket pckt = new MDCPacket()
                {
                    Operation = OpType.RADIO_INHIBIT,
                    Argument = ArgType.NO_ARG,
                    Target = tgtId.TargetRadioID
                };
                repeaterProcessor.MDC.GenerateMDC(pckt);
            }
        }

        /// <summary>
        /// Occurs when the "Revive Target" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reviveButton_Click(object sender, EventArgs e)
        {
            TargetID tgtId = new TargetID(targetID);
            tgtId.ShowDialog();

            if (!tgtId.Canceled)
            {
                // generate MDC packet
                MDCPacket pckt = new MDCPacket()
                {
                    Operation = OpType.RADIO_INHIBIT,
                    Argument = ArgType.CANCEL_INHIBIT,
                    Target = tgtId.TargetRadioID
                };
                repeaterProcessor.MDC.GenerateMDC(pckt);
            }
        }

        /// <summary>
        /// Occurs when the "Radio Check" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioCheckButton_Click(object sender, EventArgs e)
        {
            TargetID tgtId = new TargetID(targetID);
            tgtId.ShowDialog();

            if (!tgtId.Canceled)
            {
                // generate MDC packet
                MDCPacket pckt = new MDCPacket()
                {
                    Operation = OpType.RADIO_CHECK,
                    Argument = ArgType.RADIO_CHECK,
                    Target = tgtId.TargetRadioID
                };
                repeaterProcessor.MDC.GenerateMDC(pckt);
            }
        }

        /// <summary>
        /// Occurs when the "Call Alert/Page" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void callAlertButton_Click(object sender, EventArgs e)
        {
            TargetID tgtId = new TargetID(targetID);
            tgtId.ShowDialog();

            if (!tgtId.Canceled)
            {
                // generate MDC packet
                MDCPacket pckt = new MDCPacket()
                {
                    Operation = OpType.DOUBLE_PACKET_TYPE1,
                    Argument = ArgType.DOUBLE_PACKET_TO,
                    Target = tgtId.TargetRadioID
                };
                MDCPacket pckt2 = new MDCPacket()
                {
                    Operation = OpType.CALL_ALERT_ACK_EXPECTED,
                    Argument = ArgType.CALL_ALERT,
                    Target = this.optionsModal.Options.MyID
                };
                repeaterProcessor.MDC.GenerateMDC(pckt, pckt2);
            }
        }

        /// <summary>
        /// Occurs when the "SelCall" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectiveCallButton_Click(object sender, EventArgs e)
        {
            TargetID tgtId = new TargetID(targetID);
            tgtId.ShowDialog();

            if (!tgtId.Canceled)
            {
                // generate MDC packet
                MDCPacket pckt = new MDCPacket()
                {
                    Operation = OpType.DOUBLE_PACKET_TYPE1,
                    Argument = ArgType.DOUBLE_PACKET_TO,
                    Target = tgtId.TargetRadioID
                };
                MDCPacket pckt2 = new MDCPacket()
                {
                    Operation = OpType.SELECTIVE_CALL_1,
                    Argument = ArgType.CALL_ALERT,
                    Target = this.optionsModal.Options.MyID
                };
                repeaterProcessor.MDC.GenerateMDC(pckt, pckt2);
            }
        }

        /// <summary>
        /// Occurs when the "Status" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void statusButton_Click(object sender, EventArgs e)
        {
            MessageTargetID msgTgtId = new MessageTargetID();
            msgTgtId.ShowDialog();

            if (!msgTgtId.Canceled)
            {
                // generate MDC packet
                MDCPacket pckt = new MDCPacket()
                {
                    Operation = OpType.STATUS_REQUEST,
                    Argument = (byte)msgTgtId.MessageID,
                    Target = msgTgtId.TargetID
                };
                repeaterProcessor.MDC.GenerateMDC(pckt);
            }
        }

        /// <summary>
        /// Occurs when the "Message" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void messageButton_Click(object sender, EventArgs e)
        {
            MessageTargetID msgTgtId = new MessageTargetID();
            msgTgtId.ShowDialog();

            if (!msgTgtId.Canceled)
            {
                // generate MDC packet
                MDCPacket pckt = new MDCPacket()
                {
                    Operation = OpType.MESSAGE,
                    Argument = (byte)msgTgtId.MessageID,
                    Target = msgTgtId.TargetID
                };
                repeaterProcessor.MDC.GenerateMDC(pckt);
            }
        }

        /// <summary>
        /// Occurs when the "Emergency Acknowledge" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void emergAck_Click(object sender, EventArgs e)
        {
            // generate MDC packet
            MDCPacket pckt = new MDCPacket()
            {
                Operation = OpType.EMERGENCY_ACK,
                Argument = ArgType.NO_ARG,
                Target = targetID
            };
            repeaterProcessor.MDC.GenerateMDC(pckt);

            // emergency was acknowledged disable the button now
            emergAck.Enabled = false;
        }

        /// <summary>
        /// Occurs when the "Raw MDC Encode..." menu toolstrip item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rawMDCEncodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RawMDCEncode rawEncode = new RawMDCEncode(repeaterProcessor.MDC);
            rawEncode.ShowDialog();
        }

        /// <summary>
        /// Occurs when the "RAC" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void racButton_Click(object sender, EventArgs e)
        {
            TargetID tgtId = new TargetID(this.targetID);
            tgtId.ShowDialog();

            if (!tgtId.Canceled)
            {
                // generate MDC packet
                MDCPacket pckt = new MDCPacket()
                {
                    Operation = OpType.RAC,
                    Argument = ArgType.RTT,
                    Target = tgtId.TargetRadioID
                };
                repeaterProcessor.MDC.GenerateMDC(pckt);
            }
        }

        /// <summary>
        /// Occurs when the "Save Settings..." menu toolstrip item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveXml();
        }

        /// <summary>
        /// Occurs when the "Set Announcement Every Hour' menu toolstrip item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setAnnouncementEveryHourTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AnncEveryHourSet aehs = new AnncEveryHourSet(automaticAnnc);
            aehs.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void GenerateMDCTextMessage(string message)
        {
            MDCPacket pckt;

            pckt = new MDCPacket()
            {
                Operation = OpType.RC_TXT_PAD,
                Argument = ArgType.NO_ARG,
                Target = 0x0000
            };
            repeaterProcessor.MDC.GenerateMDC(pckt);

            // wait 25ms
            Thread.Sleep(25);

            List<MDCPacket> pckts = new List<MDCPacket>();
            pckts.Add(pckt);

            // parse message into parts to transmit
            int i = 0;
            do
            {
                char[] chars = new char[3];
                for (int j = 0; j < 3; j++)
                {
                    if (i + j >= message.Length)
                        break;
                    chars[j] = message[i + j];
                }

                pckt = new MDCPacket();
                pckt.Operation = OpType.RC_TXT_MESSAGE;
                pckt.Argument = (byte)chars[0];
                pckt.Target = (ushort)(((byte)chars[1] << 8) | (byte)chars[2]);

                pckts.Add(pckt);

                i += 3;
            } while (i < message.Length);

            pckt = new MDCPacket()
            {
                Operation = OpType.RC_TXT_END,
                Argument = ArgType.NO_ARG,
                Target = 0x0000
            };
            pckts.Add(pckt);
            repeaterProcessor.MDC.GenerateMDC(pckts.ToArray());

            // wait 10ms
            Thread.Sleep(10);

            repeaterProcessor.MDC.GenerateMDC(pckt);
        }

        /// <summary>
        /// Occurs when the "Test Button -- Do Not Press" menu toolstrip item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void testButtonDoNotPressToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if DEBUG
            string testString = "8===> Test Message.";
            GenerateMDCTextMessage(testString);
#else
            throw new NotImplementedException();
#endif
        }
    } // public partial class MainWindow : Form
} // namespace RepeaterController
