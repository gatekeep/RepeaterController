/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Speech.Synthesis;

using RepeaterController.DSP;
using RepeaterController.DSP.PL;
using RepeaterController.DSP.DPL;
using RepeaterController.Xml;

namespace RepeaterController
{
    /// <summary>
    /// Enumeration of the different RS232 pins.
    /// </summary>
    public enum SerialPortPin
    {
        /// <summary>
        /// Carrier Detect
        /// </summary>
        CD,
        /// <summary>
        /// Clear To Send
        /// </summary>
        CTS,
        /// <summary>
        /// Data Set Ready
        /// </summary>
        DSR,

        /// <summary>
        /// Ready To Send
        /// </summary>
        RTS,
        /// <summary>
        /// Data Terminal Radio
        /// </summary>
        DTR,
        /// <summary>
        /// Both RTS and DTR
        /// </summary>
        BOTH_RTS_AND_DTR,
    } // public enum SerialPortPin

    /// <summary>
    /// Repeater Options Modal
    /// </summary>
    public partial class RepeaterOptions : Form
    {
        /**
         * Fields
         */
        private const int MAX_TRANSMIT_TIME = 1500;
        private const int MAX_TAIL_TIME = 20;

        private XmlResource rsrc;
        private ReadOnlyCollection<InstalledVoice> voices;

        private MainWindow wnd;
        private MultitoneWindow multitoneWindow;

        /// <summary>
        /// Flag indicating whether the form was canceled.
        /// </summary>
        public bool Canceled;

        /// <summary>
        /// The actual repeater options data structure.
        /// </summary>
        public repeater_options_t Options;

        /**
         * Class
         */
        private class ListItem
        {
            /**
             * Fields
             */
            public string Description;
            public int Index;

            /**
             * Methods
             */
            /// <summary>
            /// Initializes a new instance of the <see cref="ListItem"/> class.
            /// </summary>
            public ListItem(int idx, string desc)
            {
                this.Description = desc;
                this.Index = idx;
            }

            public override string ToString()
            {
                return Description;
            }
        } // private class ListItem

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="RepeaterOptions"/> class.
        /// </summary>
        /// <param name="wnd"></param>
        public RepeaterOptions(MainWindow wnd)
        {
            this.Options = new repeater_options_t();

            InitializeComponent();

            this.wnd = wnd;

            this.multitoneWindow = new MultitoneWindow(wnd, this);

#if DISABLE_PL
            this.checkBoxTxPL.Enabled = false;
            this.checkBoxRxPL.Enabled = false;
#endif

            // populate the rx serial port combobox
            comboBoxRxSerialPort.Items.Add(new ListItem(0, "VOX"));
            comboBoxRxSerialPort.Items.Add(new ListItem(1, "COM1"));
            comboBoxRxSerialPort.Items.Add(new ListItem(2, "COM2"));
            comboBoxRxSerialPort.Items.Add(new ListItem(3, "COM3"));
            comboBoxRxSerialPort.Items.Add(new ListItem(0, "COM4"));

            // populate the tx serial port combobox
            comboBoxTxSerialPort.Items.Add(new ListItem(0, "VOX"));
            comboBoxTxSerialPort.Items.Add(new ListItem(1, "COM1"));
            comboBoxTxSerialPort.Items.Add(new ListItem(2, "COM2"));
            comboBoxTxSerialPort.Items.Add(new ListItem(3, "COM3"));
            comboBoxTxSerialPort.Items.Add(new ListItem(0, "COM4"));

            // populate PL comboboxes
            for (int i = 0; i < PLPureTones.ToneList.Count; i++)
            {
                double pl = PLPureTones.ToneList[i];
                comboBoxRxPL.Items.Add(new ListItem(i, pl.ToString("000.0")));
                comboBoxTxPL.Items.Add(new ListItem(i, pl.ToString("000.0")));
            }

            // populate DPL comboboxes
            for (int i = 0; i < DPLGenerator.ToneList.Count; i++)
            {
                string pl = DPLGenerator.ToneList[i];
                comboBoxRxDPL.Items.Add(new ListItem(i, pl));
                comboBoxTxDPL.Items.Add(new ListItem(i, pl));
            }

            // populate announcment voices
            SpeechSynthesizer synth = new SpeechSynthesizer();
            comboBoxAnncVoice.Items.Add(new ListItem(0, string.Empty));
            try
            {
                this.voices = synth.GetInstalledVoices();
                for (int i = 0; i < voices.Count; i++)
                    comboBoxAnncVoice.Items.Add(new ListItem(i + 1, voices[i].VoiceInfo.Name));
            }
            catch (Exception e)
            {
                Messages.TraceException("failed getting voices", e);
            }

            this.FormClosing += RepeaterOptions_FormClosing;

            SetDefaults();
            UpdateModal();

            callsignTextBox.TextChanged += CallsignTextBox_Changed;
            myIdTextBox.TextChanged += MyIdTextBox_TextChanged;
            numOfPreamblesTextBox.TextChanged += numOfPreamblesTextBox_TextChanged;
            idIntervalTextBox.TextChanged += IdIntervalTextBox_TextChanged;
            checkBoxDisableId.CheckedChanged += CheckBoxDisableId_CheckedChanged;
            checkBoxDisableIdStartup.CheckedChanged += CheckBoxDisableIdStartup_CheckedChanged;
            checkBoxDisablePLForId.CheckedChanged += CheckBoxDisablePLForId_CheckedChanged;

            checkBoxAutoAckEmerg.CheckedChanged += CheckBoxAutoAckEmerg_CheckedChanged;

            racTextBox.TextChanged += RacTextBox_TextChanged;
            checkBoxUseRAC.CheckedChanged += CheckBoxUseRAC_CheckedChanged;
            pttTransmitTextBox.KeyDown += PttTransmitTextBox_KeyDown;

            comboBoxRxSerialPort.SelectedIndexChanged += ComboBoxRxSerialPort_SelectedIndexChanged;
            comboBoxRxPL.SelectedIndexChanged += ComboBoxRxPL_SelectedIndexChanged;
            checkBoxRxPL.CheckedChanged += CheckBoxRxPL_CheckedChanged;
            comboBoxRxDPL.SelectedIndexChanged += ComboBoxRxDPL_SelectedIndexChanged;
            checkBoxUseRxDPL.CheckedChanged += CheckBoxUseRxDPL_CheckedChanged;
            rxControlPinCD.CheckedChanged += RxControlPin_CheckedChanged;
            rxControlPinCTS.CheckedChanged += RxControlPin_CheckedChanged;
            rxControlPinDSR.CheckedChanged += RxControlPin_CheckedChanged;

            comboBoxTxSerialPort.SelectedIndexChanged += ComboBoxTxSerialPort_SelectedIndexChanged;
            comboBoxTxPL.SelectedIndexChanged += ComboBoxTxPL_SelectedIndexChanged;
            checkBoxTxPL.CheckedChanged += CheckBoxTxPL_CheckedChanged;
            comboBoxTxDPL.SelectedIndexChanged += ComboBoxTxDPL_SelectedIndexChanged;
            checkBoxUseTxDPL.CheckedChanged += CheckBoxUseTxDPL_CheckedChanged;
            txControlPinRTS.CheckedChanged += TxControlPin_CheckedChanged;
            txControlPinDTR.CheckedChanged += TxControlPin_CheckedChanged;
            txControlPinBoth.CheckedChanged += TxControlPin_CheckedChanged;
            textBoxTxPLGain.LostFocus += TextBoxTxPLGain_LostFocus;
            watchDogTextBox.TextChanged += WatchDogTextBox_TextChanged;

            maxTransmissionTimeTextBox.TextChanged += MaxTransmissionTimeTextBox_TextChanged;
            tailTimerTextBox.TextChanged += TailTimerTextBox_TextChanged;

            numericUpDownDtmfDigitDelay.ValueChanged += NumericUpDownDtmfDigitDelay_ValueChanged;

            checkBoxNoTone.CheckedChanged += CheckBoxNoTone_CheckedChanged;

            courtesyToneDelayTextBox.TextChanged += CourtesyToneDelayTextBox_TextChanged;
            courtesyTonePitchTextBox.TextChanged += CourtesyTonePitchTextBox_TextChanged;
            courtesyToneDurationTextBox.TextChanged += CourtesyToneDurationTextBox_TextChanged;

            courtesyToneFileTextBox.TextChanged += CourtesyToneFileTextBox_TextChanged;

            courtesyGenerated.CheckedChanged += CourtesyTone_CheckedChanged;
            courtesyPlayback.CheckedChanged += CourtesyTone_CheckedChanged;

            voxVolumeSlider.VolumeChanged += VoxVolumeSlider_VolumeChanged;

            checkBoxDisableAnnc.CheckedChanged += CheckBoxDisableAnnc_CheckedChanged;
            comboBoxAnncVoice.SelectedIndexChanged += ComboBoxAnncVoice_SelectedIndexChanged;
            textBoxAnncAudioGain.LostFocus += TextBoxAnncAudioGain_LostFocus;

            checkBoxMDCOnly.CheckedChanged += CheckBoxMDCOnly_CheckedChanged;
            checkBoxConsoleAnncDTMF.CheckedChanged += CheckBoxConsoleAnncDTMF_CheckedChanged;

            checkBoxAnncSysName.CheckedChanged += CheckBoxAnncSysName_CheckedChanged;
            textBoxSysName.TextChanged += TextBoxSysName_TextChanged;

            checkBoxDbLogging.CheckedChanged += CheckBoxDbLogging_CheckedChanged;
            textBoxDbName.TextChanged += TextBoxDbName_TextChanged;
            textBoxDbServer.TextChanged += TextBoxDbServer_TextChanged;
            textBoxDbUsername.TextChanged += TextBoxDbUsername_TextChanged;
            textBoxDbPassword.TextChanged += TextBoxDbPassword_TextChanged;

#if MDC_CONSOLE
            checkBoxMDCOnly.Enabled = false;

            checkBoxDisableId.Checked = true;
            checkBoxDisableId.Enabled = false;
            checkBoxDisableId.Visible = false;
            checkBoxDisableIdStartup.Checked = true;
            checkBoxDisableIdStartup.Enabled = false;
            checkBoxDisableIdStartup.Visible = false;

            checkBoxDisablePLForId.Visible = false;
            checkBoxDisablePLForId.Enabled = false;

            checkBoxLinkRadio.Visible = false;
            checkBoxLinkRadio.Enabled = false;

            labelRAC.Visible = false;
            racTextBox.Enabled = false;
            racTextBox.Visible = false;
            checkBoxUseRAC.Visible = false;
            checkBoxUseRAC.Enabled = false;

            labelCallsign.Visible = false;
            callsignTextBox.Visible = false;
            callsignTextBox.Enabled = false;

            labelIdInterval.Visible = false;
            idIntervalTextBox.Visible = false;
            idIntervalTextBox.Enabled = false;

            tabControl.TabPages.Remove(tabTimers);
            tabControl.TabPages.Remove(tabAnnc);

            this.Text = "Console Options";
            tabIdOptions.Text = "ID & MDC Settings";
            groupBoxRepeaterId.Text = "My ID";
#endif
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeaterOptions"/> class.
        /// </summary>
        /// <param name="rsrc"></param>
        /// <param name="wnd"></param>
        public RepeaterOptions(XmlResource rsrc, MainWindow wnd) : this(wnd)
        {
            this.Options = new repeater_options_t();

            this.rsrc = rsrc;
            LoadXml();
        }

        /// <summary>
        /// Helper to load/generate the user XML configuration data.
        /// </summary>
        public void LoadXml()
        {
            if (rsrc.HasNode("RepeaterOptions"))
            {
                XmlResource repeaterOptions = rsrc.Get<XmlResource>("RepeaterOptions");
                this.Options = new repeater_options_t();
                repeaterOptions.GetByReflection(this.Options);
            }
            else
            {
                // reset defaults
                SetDefaults();
                SaveXml();
            }
            UpdateModal();
        }

        /// <summary>
        /// Helper to save the XML.
        /// </summary>
        public void SaveXml()
        {
            XmlResource repeaterOptions = rsrc.CreateNode("RepeaterOptions");
            repeaterOptions.SubmitByReflection(this.Options);

            rsrc.SaveXml(Environment.CurrentDirectory + Path.DirectorySeparatorChar + MainWindow.XML_FILE);
        }

        /// <summary>
        /// Sets default preset values.
        /// </summary>
        public void SetDefaults()
        {
            this.Options = new repeater_options_t();
            this.Options.MyID = 0x0001;
            this.Options.Callsign = "ABCD123";
            this.Options.NumberOfPreambles = 6;
            this.Options.IdInterval = 10;
            this.Options.NoId = false;
            this.Options.NoIdStartup = false;
            this.Options.DisablePLForId = false;

            this.Options.AutoAckEmergency = false;
            this.Options.UseRAC = false;
            this.Options.AccessRAC = 0x0000;
            this.Options.PTTKey = (int)Keys.F12;

            this.Options.RxSerialPort = "VOX";
            this.Options.EnableRxPL = false;
            this.Options.RxPL = 0;
            this.Options.RxDPL = 0;
            this.Options.UseRxDPL = false;
            this.Options.RxAssertPin = SerialPortPin.CD;
            this.Options.RxVOX = true;

            this.Options.VOXDetectLevel = 0.006;

            this.Options.TxSerialPort = "VOX";
            this.Options.EnableTxPL = false;
            this.Options.TxPL = 0;
            this.Options.TxPLGain = 0.15;
            this.Options.TxAssertPin = SerialPortPin.BOTH_RTS_AND_DTR;
            this.Options.TxVOX = true;
            this.Options.WatchDogTime = 120;

            this.Options.MaxTransmissionTime = 60;
            this.Options.TailTime = 5;

            this.Options.DTMFDigitTime = 0.5;

            this.Options.NoCourtesyTone = false;

            this.Options.CourtesyDelay = 100;
            this.Options.CourtesyPitch = 500;
            this.Options.CourtesyLength = 100;

            this.Options.CourtesyMultiTone = false;
            this.Options.CourtesyTones = new List<Multitone>(1);

            this.Options.CourtesyUseFile = false;
            this.Options.CourtesyFile = string.Empty;

            this.Options.DisableAnnouncements = false;
            if (voices != null)
            {
                if (voices.Count > 0)
                    this.Options.AnnouncementVoice = voices[0].VoiceInfo.Name;
                else
                    this.Options.AnnouncementVoice = string.Empty;
            }
            else
                this.Options.AnnouncementVoice = string.Empty;
            this.Options.SynthAnncGain = 7.8;

            this.Options.UseSystemName = false;
            this.Options.SystemName = string.Empty;

            this.Options.DatabaseName = "rptcontrol";
            this.Options.DatabaseServer = "127.0.0.1";
            this.Options.DatabaseUsername = "rptcontrol";
            this.Options.DatabasePassword = string.Empty;

            this.Options.MDCConsoleOnly = false;
            this.Options.AllowConsoleAnncDTMF = false;
        }

        /// <summary>
        /// Updates the modal to the state of the internal variables.
        /// </summary>
        public void UpdateModal()
        {
            checkBoxMDCOnly.Checked = this.Options.MDCConsoleOnly;
            if (this.Options.MDCConsoleOnly)
            {
                checkBoxConsoleAnncDTMF.Enabled = true;
                checkBoxConsoleAnncDTMF.Checked = this.Options.AllowConsoleAnncDTMF;

                checkBoxDisableId.Checked = true;
                checkBoxDisableId.Enabled = false;
                checkBoxDisableIdStartup.Checked = true;
                checkBoxDisableIdStartup.Enabled = false;

                checkBoxUseRAC.Checked = false;
                checkBoxUseRAC.Enabled = false;

                ((Control)tabTimers).Enabled = false;

                if (!this.Options.AllowConsoleAnncDTMF)
                    ((Control)tabAnnc).Enabled = false;
                else
                    ((Control)tabAnnc).Enabled = true;
            }
            else
            {
                checkBoxConsoleAnncDTMF.Enabled = false;

                checkBoxDisableId.Enabled = true;
                checkBoxDisableIdStartup.Enabled = true;

                checkBoxUseRAC.Checked = false;
                checkBoxUseRAC.Enabled = true;

                ((Control)tabTimers).Enabled = true;
                ((Control)tabAnnc).Enabled = true;
            }

            callsignTextBox.Text = this.Options.Callsign;
            myIdTextBox.Text = this.Options.MyID.ToString("X4");
            numOfPreamblesTextBox.Text = this.Options.NumberOfPreambles.ToString();
            idIntervalTextBox.Text = this.Options.IdInterval.ToString();
            checkBoxDisableId.Checked = this.Options.NoId;
            if (Options.NoId)
                idIntervalTextBox.Enabled = false;
            else
                idIntervalTextBox.Enabled = true;
            checkBoxDisableIdStartup.Checked = this.Options.NoIdStartup;
            checkBoxDisablePLForId.Checked = this.Options.DisablePLForId;
            if (!this.Options.EnableTxPL)
                checkBoxDisablePLForId.Enabled = false;
            else
                checkBoxDisablePLForId.Enabled = true;

            checkBoxAutoAckEmerg.Checked = this.Options.AutoAckEmergency;

            checkBoxUseRAC.Checked = this.Options.UseRAC;
            if (this.Options.UseRAC)
                racTextBox.Enabled = true;
            else
                racTextBox.Enabled = false;
            racTextBox.Text = this.Options.AccessRAC.ToString("X4");

            pttTransmitTextBox.Text = ((Keys)this.Options.PTTKey).ToString();

            foreach (ListItem item in comboBoxRxSerialPort.Items)
                if (item.Description == this.Options.RxSerialPort)
                    comboBoxRxSerialPort.SelectedIndex = item.Index;
            if (this.Options.EnableRxPL)
            {
                checkBoxRxPL.Checked = true;
                comboBoxRxPL.Enabled = true;
                checkBoxUseRxDPL.Enabled = true;

                if (this.Options.UseRxDPL)
                {
                    checkBoxUseRxDPL.Checked = true;
                    comboBoxRxDPL.Enabled = true;
                    comboBoxRxPL.Enabled = false;
                }
                else
                {
                    comboBoxRxDPL.Enabled = false;
                    comboBoxRxPL.Enabled = true;
                }
            }
            else
            {
                comboBoxRxPL.Enabled = false;
                comboBoxRxDPL.Enabled = false;
                checkBoxUseRxDPL.Enabled = false;
            }

            foreach (ListItem item in comboBoxRxPL.Items)
                if (item.Index == this.Options.RxPL)
                    comboBoxRxPL.SelectedIndex = item.Index;
            foreach (ListItem item in comboBoxRxDPL.Items)
                if (item.Index == this.Options.RxDPL)
                    comboBoxRxDPL.SelectedIndex = item.Index;
            if (this.Options.RxAssertPin == SerialPortPin.CD)
                rxControlPinCD.Checked = true;
            else if (this.Options.RxAssertPin == SerialPortPin.CTS)
                rxControlPinCTS.Checked = true;
            else if (this.Options.RxAssertPin == SerialPortPin.DSR)
                rxControlPinDSR.Checked = true;

            if (this.Options.RxVOX)
            {
                rxControlPinCD.Checked = true;
                groupBoxRxControlPin.Enabled = false;

                this.voxVolumeSlider.Enabled = true;
            }
            else
                this.voxVolumeSlider.Enabled = false;

            this.voxVolumeSlider.Volume = (float)this.Options.VOXDetectLevel;

            foreach (ListItem item in comboBoxTxSerialPort.Items)
                if (item.Description == this.Options.TxSerialPort)
                    comboBoxTxSerialPort.SelectedIndex = item.Index;
            if (this.Options.EnableTxPL)
            {
                checkBoxTxPL.Checked = true;
                comboBoxTxPL.Enabled = true;
                checkBoxUseTxDPL.Enabled = true;
                textBoxTxPLGain.Enabled = true;

                if (this.Options.UseTxDPL)
                {
                    checkBoxUseTxDPL.Checked = true;
                    comboBoxTxDPL.Enabled = true;
                    comboBoxTxPL.Enabled = false;
                }
                else
                {
                    comboBoxTxDPL.Enabled = false;
                    comboBoxTxPL.Enabled = true;
                }
            }
            else
            {
                comboBoxTxPL.Enabled = false;
                comboBoxTxDPL.Enabled = false;
                checkBoxUseTxDPL.Enabled = false;
                textBoxTxPLGain.Enabled = false;
            }

            foreach (ListItem item in comboBoxTxPL.Items)
                if (item.Index == this.Options.TxPL)
                    comboBoxTxPL.SelectedIndex = item.Index;
            foreach (ListItem item in comboBoxTxDPL.Items)
                if (item.Index == this.Options.TxDPL)
                    comboBoxTxDPL.SelectedIndex = item.Index;
            textBoxTxPLGain.Text = this.Options.TxPLGain.ToString();
            if (this.Options.TxAssertPin == SerialPortPin.RTS)
                txControlPinRTS.Checked = true;
            else if (this.Options.TxAssertPin == SerialPortPin.DTR)
                txControlPinDTR.Checked = true;
            else if (this.Options.TxAssertPin == SerialPortPin.BOTH_RTS_AND_DTR)
                txControlPinBoth.Checked = true;

            if (this.Options.TxVOX)
            {
                txControlPinBoth.Checked = true;
                groupBoxTxControlPin.Enabled = false;
            }
            watchDogTextBox.Text = this.Options.WatchDogTime.ToString();

            maxTransmissionTimeTextBox.Text = this.Options.MaxTransmissionTime.ToString();
            tailTimerTextBox.Text = this.Options.TailTime.ToString();

            numericUpDownDtmfDigitDelay.Value = (decimal)this.Options.DTMFDigitTime;

            checkBoxNoTone.Checked = this.Options.NoCourtesyTone;
            if (this.Options.NoCourtesyTone)
                groupBoxCourtesyTone.Enabled = false;
            else
                groupBoxCourtesyTone.Enabled = true;

            courtesyToneDelayTextBox.Text = this.Options.CourtesyDelay.ToString();
            courtesyTonePitchTextBox.Text = this.Options.CourtesyPitch.ToString();
            courtesyToneDurationTextBox.Text = this.Options.CourtesyLength.ToString();

            courtesyToneFileTextBox.Text = this.Options.CourtesyFile;
            if (this.Options.CourtesyUseFile)
                courtesyPlayback.Checked = true;
            else
                courtesyGenerated.Checked = true;

            // change view based on generated radio button
            if (courtesyGenerated.Checked)
            {
                courtesyToneDelayTextBox.Enabled = true;
                courtesyTonePitchTextBox.Enabled = true;
                courtesyToneDurationTextBox.Enabled = true;
                openMultiTone.Enabled = true;
                openMultiTone.ForeColor = Color.Black;

                if (this.Options.CourtesyMultiTone)
                {
                    courtesyToneDelayTextBox.Enabled = false;
                    courtesyTonePitchTextBox.Enabled = false;
                    courtesyToneDurationTextBox.Enabled = false;
                    openMultiTone.ForeColor = Color.Green;
                }

                courtesyToneFileTextBox.Enabled = false;
                openCourtesyToneFile.Enabled = false;
            }

            // change view based on playback radio button
            if (courtesyPlayback.Checked)
            {
                courtesyToneDelayTextBox.Enabled = false;
                courtesyTonePitchTextBox.Enabled = false;
                courtesyToneDurationTextBox.Enabled = false;
                openMultiTone.Enabled = false;

                courtesyToneFileTextBox.Enabled = true;
                openCourtesyToneFile.Enabled = true;
            }

            if (this.Options.DisableAnnouncements)
                checkBoxDisableAnnc.Checked = true;
            else
                checkBoxDisableAnnc.Checked = false;
            if (checkBoxDisableAnnc.Checked)
            {
                comboBoxAnncVoice.Enabled = false;
                textBoxAnncAudioGain.Enabled = false;
                checkBoxAnncSysName.Enabled = false;
                textBoxSysName.Enabled = false;
            }
            else
            {
                comboBoxAnncVoice.Enabled = true;
                textBoxAnncAudioGain.Enabled = true;
                checkBoxAnncSysName.Enabled = true;
                textBoxSysName.Enabled = true;
            }

            if (this.Options.UseSystemName)
                textBoxSysName.Enabled = true;
            else
                textBoxSysName.Enabled = false;

            this.textBoxSysName.Text = this.Options.SystemName;

            if (this.Options.AnnouncementVoice != string.Empty)
            {
                for (int i = 0; i < comboBoxAnncVoice.Items.Count; i++)
                {
                    ListItem item = (ListItem)comboBoxAnncVoice.Items[i];
                    if (item.Description == this.Options.AnnouncementVoice)
                        comboBoxAnncVoice.SelectedIndex = i;
                }
            }
            else
                comboBoxAnncVoice.SelectedIndex = 0;
            textBoxAnncAudioGain.Text = this.Options.SynthAnncGain.ToString();

            if (this.Options.EnableDatabaseLogging)
            {
                this.checkBoxDbLogging.Checked = true;
                this.groupBoxLogging.Enabled = true;
                this.textBoxDbName.Enabled = true;
                this.textBoxDbName.Text = this.Options.DatabaseName;
                this.textBoxDbServer.Enabled = true;
                this.textBoxDbServer.Text = this.Options.DatabaseServer;

                this.textBoxDbUsername.Enabled = true;
                this.textBoxDbUsername.Text = this.Options.DatabaseUsername;
                this.textBoxDbPassword.Enabled = true;
                this.textBoxDbPassword.Text = this.Options.DatabasePassword;
            }
            else
            {
                this.checkBoxDbLogging.Checked = false;
                this.groupBoxLogging.Enabled = false;
                this.textBoxDbName.Enabled = false;
                this.textBoxDbServer.Enabled = false;

                this.textBoxDbUsername.Enabled = false;
                this.textBoxDbPassword.Enabled = false;
                this.Options.DatabasePassword = this.textBoxDbPassword.Text = string.Empty;
            }
        }

        /// <summary>
        /// Helper to get the listing of voices.
        /// </summary>
        /// <returns></returns>
        public List<string> GetVoices()
        {
            List<string> ret = new List<string>();
            if (voices != null)
            {
                for (int i = 0; i < voices.Count; i++)
                    ret.Add(voices[i].VoiceInfo.Name);
            }

            return ret;
        }

        /// <summary>
        /// Helper to set the repeater options to defined external set of options.
        /// </summary>
        /// <param name="options"></param>
        public void SetExternal(repeater_options_t options)
        {
            this.Options = options;
            UpdateModal();
            SaveXml();
        }

        /// <summary>
        /// Occurs when the form is closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepeaterOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.Cancel)
                this.Canceled = true;
            else
                okButton_Click(sender, new EventArgs());
        }

        /// <summary>
        /// Event that occurs when the buffer textbox changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CallsignTextBox_Changed(object sender, EventArgs e)
        {
            this.Options.Callsign = callsignTextBox.Text;
        }

        /// <summary>
        /// Internal function to invalidate the PTT ID input box.
        /// </summary>
        private void InvalidateMyID()
        {
            myIdTextBox.ForeColor = Color.Red;
            this.Options.MyID = 0x0001;
        }

        /// <summary>
        /// Internal function to validate the PTT ID input box.
        /// </summary>
        private void ValidateMyID()
        {
            try
            {
                myIdTextBox.ForeColor = Color.Black;
                this.Options.MyID = Convert.ToUInt16(myIdTextBox.Text, 16);
            }
            catch (FormatException)
            {
                InvalidateMyID();
            }
            catch (OverflowException)
            {
                InvalidateMyID();
            }
        }

        /// <summary>
        /// Occurs when the text in the PTT ID box changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyIdTextBox_TextChanged(object sender, EventArgs e)
        {
            if (myIdTextBox.Text.Length < 4)
                InvalidateMyID();
            else
            {
                char[] checkValues = myIdTextBox.Text.ToCharArray();

                // no value in the ID field should contain 'F'
                if ((checkValues[0] == 'F') || (checkValues[1] == 'F') || (checkValues[2] == 'F') || (checkValues[3] == 'F'))
                    InvalidateMyID();
                else
                    ValidateMyID();
            }
        }

        /// <summary>
        /// Occurs when the text in the preambles text box is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numOfPreamblesTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                numOfPreamblesTextBox.ForeColor = Color.Black;
                int numOfPreambles = Convert.ToInt32(numOfPreamblesTextBox.Text);

                // limit the maximum number of preambles
                if (numOfPreambles > MainWindow.MAX_PREAMBLES_ALLOWED)
                {
                    numOfPreamblesTextBox.Text = string.Empty + MainWindow.MAX_PREAMBLES_ALLOWED;
                    this.Options.NumberOfPreambles = MainWindow.MAX_PREAMBLES_ALLOWED;
                }
                else
                    this.Options.NumberOfPreambles = numOfPreambles;
            }
            catch (FormatException)
            {
                numOfPreamblesTextBox.ForeColor = Color.Red;
                this.Options.NumberOfPreambles = 3;
            }
            catch (OverflowException)
            {
                numOfPreamblesTextBox.ForeColor = Color.Red;
                this.Options.NumberOfPreambles = 3;
            }
        }

        /// <summary>
        /// Occurs when the text in the ID interval is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IdIntervalTextBox_TextChanged(object sender, EventArgs e)
        {
            int idInterval = 0;
            if (int.TryParse(idIntervalTextBox.Text, out idInterval))
            {
                this.Options.IdInterval = idInterval;
                UpdateModal();
                this.idIntervalTextBox.ForeColor = Color.Black;
            }
            else
                this.idIntervalTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// Occurs when the Disable Auto ID checkbox is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxDisableId_CheckedChanged(object sender, EventArgs e)
        {
            this.Options.NoId = checkBoxDisableId.Checked;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the No ID at Startup checkbox is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxDisableIdStartup_CheckedChanged(object sender, EventArgs e)
        {
            this.Options.NoIdStartup = checkBoxDisableIdStartup.Checked;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the Disable PL/DPL for ID checkbox is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxDisablePLForId_CheckedChanged(object sender, EventArgs e)
        {
            this.Options.DisablePLForId = checkBoxDisablePLForId.Checked;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the Auto Acknowledge Emergency checkbox is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxAutoAckEmerg_CheckedChanged(object sender, EventArgs e)
        {
            this.Options.AutoAckEmergency = checkBoxAutoAckEmerg.Checked;
            UpdateModal();
        }

        /// <summary>
        /// Internal function to invalidate the RAC input box.
        /// </summary>
        private void InvalidateRAC()
        {
            racTextBox.ForeColor = Color.Red;
            this.Options.AccessRAC = 0;
        }

        /// <summary>
        /// Internal function to validate the RAC input box.
        /// </summary>
        private void ValidateRAC()
        {
            try
            {
                racTextBox.ForeColor = Color.Black;
                this.Options.AccessRAC = Convert.ToUInt16(racTextBox.Text, 16);
            }
            catch (FormatException)
            {
                InvalidateRAC();
            }
            catch (OverflowException)
            {
                InvalidateRAC();
            }
        }

        /// <summary>
        /// Occurs when the text in the Repeater Access Code text box changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RacTextBox_TextChanged(object sender, EventArgs e)
        {
            if (racTextBox.Text.Length < 4)
                InvalidateRAC();
            else
            {
                char[] checkValues = racTextBox.Text.ToCharArray();

                // no value in the ID field should contain 'F'
                if ((checkValues[0] == 'F') || (checkValues[1] == 'F') || (checkValues[2] == 'F') || (checkValues[3] == 'F'))
                    InvalidateRAC();
                else
                    ValidateRAC();
            }
        }

        /// <summary>
        /// Occurs when a key is pressed in the PTT transmit textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PttTransmitTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            this.Options.PTTKey = (int)e.KeyCode;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the Use Repeater Access checkbox is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxUseRAC_CheckedChanged(object sender, EventArgs e)
        {
            this.Options.UseRAC = checkBoxUseRAC.Checked;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the courtesy tone type changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CourtesyTone_CheckedChanged(object sender, EventArgs e)
        {
            if (courtesyGenerated.Checked)
            {
                courtesyGenerated.Checked = true;
                courtesyPlayback.Checked = false;

                this.Options.CourtesyFile = string.Empty;
                this.Options.CourtesyUseFile = false;

                UpdateModal();
            }
            else if (courtesyPlayback.Checked)
            {
                courtesyGenerated.Checked = false;
                courtesyPlayback.Checked = true;

                this.Options.CourtesyUseFile = true;

                UpdateModal();
            }

        }

        /// <summary>
        /// Occurs when the courtesy tone file text box is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CourtesyToneFileTextBox_TextChanged(object sender, EventArgs e)
        {
            string file = courtesyToneFileTextBox.Text;
            if (!File.Exists(file))
            {
                MessageBox.Show("Courtesy Tone wave file does not seem to be accessible or exist!", "Repeater Options", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Options.CourtesyFile = string.Empty;
                UpdateModal();
                this.courtesyToneFileTextBox.ForeColor = Color.Red;
            }
            else
            {
                this.Options.CourtesyFile = file;
                UpdateModal();
                this.courtesyToneFileTextBox.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// Occurs when the text in the courtesy tone duration text box is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CourtesyToneDurationTextBox_TextChanged(object sender, EventArgs e)
        {
            int toneDuration = 0;
            if (int.TryParse(courtesyToneDurationTextBox.Text, out toneDuration))
            {
                this.Options.CourtesyLength = toneDuration;
                UpdateModal();
                this.courtesyToneDurationTextBox.ForeColor = Color.Black;
            }
            else
                this.courtesyToneDurationTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// Occurs when the text in the courtesy tone pitch text box is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CourtesyTonePitchTextBox_TextChanged(object sender, EventArgs e)
        {
            int tonePitch = 0;
            if (int.TryParse(courtesyTonePitchTextBox.Text, out tonePitch))
            {
                this.Options.CourtesyPitch = tonePitch;
                UpdateModal();
                this.courtesyTonePitchTextBox.ForeColor = Color.Black;
            }
            else
                this.courtesyTonePitchTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// Occurs when the text in the courtesy tone delay text box is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CourtesyToneDelayTextBox_TextChanged(object sender, EventArgs e)
        {
            int toneDelay = 0;
            if (int.TryParse(courtesyToneDelayTextBox.Text, out toneDelay))
            {
                this.Options.CourtesyDelay = toneDelay;
                UpdateModal();
                this.courtesyToneDelayTextBox.ForeColor = Color.Black;
            }
            else
                this.courtesyToneDelayTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// Occurs when the No Courtesy Tone checkbox is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxNoTone_CheckedChanged(object sender, EventArgs e)
        {
            this.Options.NoCourtesyTone = checkBoxNoTone.Checked;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the DTMF Inter-digit Time text box is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericUpDownDtmfDigitDelay_ValueChanged(object sender, EventArgs e)
        {
            this.Options.DTMFDigitTime = (double)numericUpDownDtmfDigitDelay.Value;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the text in the tail timer text box is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TailTimerTextBox_TextChanged(object sender, EventArgs e)
        {
            int tailTimer = 0;
            if (int.TryParse(tailTimerTextBox.Text, out tailTimer))
            {
                if (this.Options.TailTime > MAX_TAIL_TIME)
                    this.tailTimerTextBox.ForeColor = Color.Red;
                else
                {
                    this.Options.TailTime = tailTimer;
                    UpdateModal();
                    this.tailTimerTextBox.ForeColor = Color.Black;
                }
            }
            else
                this.tailTimerTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// Occurs when the text in the max transmission time text box is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaxTransmissionTimeTextBox_TextChanged(object sender, EventArgs e)
        {
            int maxTransmissionTime = 0;
            if (int.TryParse(maxTransmissionTimeTextBox.Text, out maxTransmissionTime))
            {
                if ((maxTransmissionTime > this.Options.WatchDogTime) || (maxTransmissionTime > MAX_TRANSMIT_TIME))
                    this.maxTransmissionTimeTextBox.ForeColor = Color.Red;
                else
                {
                    this.Options.MaxTransmissionTime = maxTransmissionTime;
                    UpdateModal();
                    this.maxTransmissionTimeTextBox.ForeColor = Color.Black;
                }
            }
            else
                this.maxTransmissionTimeTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// Occurs when the text in the watch dog time text box is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WatchDogTextBox_TextChanged(object sender, EventArgs e)
        {
            int watchDogTime = 0;
            if (int.TryParse(watchDogTextBox.Text, out watchDogTime))
            {
                if ((watchDogTime < this.Options.MaxTransmissionTime) || (watchDogTime > MAX_TRANSMIT_TIME))
                    this.watchDogTextBox.ForeColor = Color.Red;
                else
                {
                    this.Options.WatchDogTime = watchDogTime;
                    UpdateModal();
                    this.watchDogTextBox.ForeColor = Color.Black;
                }
            }
            else
                this.watchDogTextBox.ForeColor = Color.Red;
        }

        /// <summary>
        /// Occurs when the text in the transmitter control pin type changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxControlPin_CheckedChanged(object sender, EventArgs e)
        {
            if (txControlPinRTS.Checked)
            {
                txControlPinRTS.Checked = true;
                txControlPinDTR.Checked = false;
                txControlPinBoth.Checked = false;

                this.Options.TxAssertPin = SerialPortPin.RTS;
            }
            else if (txControlPinDTR.Checked)
            {
                txControlPinRTS.Checked = false;
                txControlPinDTR.Checked = true;
                txControlPinBoth.Checked = false;

                this.Options.TxAssertPin = SerialPortPin.DTR;
            }
            else if (txControlPinBoth.Checked)
            {
                txControlPinRTS.Checked = false;
                txControlPinDTR.Checked = false;
                txControlPinBoth.Checked = true;

                this.Options.TxAssertPin = SerialPortPin.BOTH_RTS_AND_DTR;
            }

            UpdateModal();
        }

        /// <summary>
        /// Occurs when the "PL Gain" textbox is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxTxPLGain_LostFocus(object sender, EventArgs e)
        {
            double gain = 0.0;
            if (double.TryParse(textBoxTxPLGain.Text, out gain))
            {
                this.textBoxTxPLGain.ForeColor = Color.Black;
                this.Options.TxPLGain = gain;
                UpdateModal();
            }
            else
                this.textBoxTxPLGain.ForeColor = Color.Red;
        }

        /// <summary>
        /// Occurs when the "Use Tx DPL" checkbox is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxUseTxDPL_CheckedChanged(object sender, EventArgs e)
        {
            this.Options.UseTxDPL = checkBoxUseTxDPL.Checked;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the text in the transmitter DPL selection is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxTxDPL_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem selectedItem = (ListItem)comboBoxTxDPL.SelectedItem;
            this.Options.TxDPL = selectedItem.Index;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the "Enable Transmitter PL" checkbox is checked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxTxPL_CheckedChanged(object sender, EventArgs e)
        {
            this.Options.EnableTxPL = checkBoxTxPL.Checked;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the text in the transmitter PL selection is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxTxPL_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem selectedItem = (ListItem)comboBoxTxPL.SelectedItem;
            this.Options.TxPL = selectedItem.Index;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the text in the transmitter serial port selection is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxTxSerialPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem selectedItem = (ListItem)comboBoxTxSerialPort.SelectedItem;
            this.Options.TxSerialPort = selectedItem.Description;
            if (this.Options.TxSerialPort == "VOX")
            {
                this.Options.TxVOX = true;
                this.groupBoxTxControlPin.Enabled = false;
            }
            else
            {
                this.Options.TxVOX = false;
                this.groupBoxTxControlPin.Enabled = true;
            }

            UpdateModal();
        }

        /// <summary>
        /// Occurs when the text in the receiver control pin type changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RxControlPin_CheckedChanged(object sender, EventArgs e)
        {
            if (rxControlPinCD.Checked)
            {
                rxControlPinCD.Checked = true;
                rxControlPinCTS.Checked = false;
                rxControlPinDSR.Checked = false;

                this.Options.RxAssertPin = SerialPortPin.CD;
            }
            else if (rxControlPinCTS.Checked)
            {
                rxControlPinCD.Checked = false;
                rxControlPinCTS.Checked = true;
                rxControlPinDSR.Checked = false;

                this.Options.RxAssertPin = SerialPortPin.CTS;
            }
            else if (rxControlPinDSR.Checked)
            {
                rxControlPinCD.Checked = false;
                rxControlPinCTS.Checked = false;
                rxControlPinDSR.Checked = true;

                this.Options.RxAssertPin = SerialPortPin.DSR;
            }

            UpdateModal();
        }

        /// <summary>
        /// Occurs when the "Use Rx DPL" checkbox is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxUseRxDPL_CheckedChanged(object sender, EventArgs e)
        {
            this.Options.UseRxDPL = checkBoxUseRxDPL.Checked;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the text in the receiver DPL selection is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxRxDPL_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem selectedItem = (ListItem)comboBoxRxDPL.SelectedItem;
            this.Options.RxDPL = selectedItem.Index;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the "Enable Receiver PL" checkbox is checked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxRxPL_CheckedChanged(object sender, EventArgs e)
        {
            this.Options.EnableRxPL = checkBoxRxPL.Checked;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the text in the receiver PL selection is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxRxPL_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem selectedItem = (ListItem)comboBoxRxPL.SelectedItem;
            this.Options.RxPL = selectedItem.Index;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the text in the receiver serial port selection is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxRxSerialPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem selectedItem = (ListItem)comboBoxRxSerialPort.SelectedItem;
            this.Options.RxSerialPort = selectedItem.Description;
            if (this.Options.RxSerialPort == "VOX")
            {
                this.Options.RxVOX = true;
                this.groupBoxRxControlPin.Enabled = false;
            }
            else
            {
                this.Options.RxVOX = false;
                this.groupBoxRxControlPin.Enabled = true;
            }

            UpdateModal();
        }

        /// <summary>
        /// Occurs when the volume setting in the VOX volume slider is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VoxVolumeSlider_VolumeChanged(object sender, EventArgs e)
        {
            this.Options.VOXDetectLevel = voxVolumeSlider.Volume;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the "MDC Only Console" checkbox is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxMDCOnly_CheckedChanged(object sender, EventArgs e)
        {
            this.Options.MDCConsoleOnly = checkBoxMDCOnly.Checked;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the "Link Radio" checkbox is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxConsoleAnncDTMF_CheckedChanged(object sender, EventArgs e)
        {
            this.Options.AllowConsoleAnncDTMF = checkBoxConsoleAnncDTMF.Checked;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the "Announcment Voice" combo box is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxAnncVoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem item = (ListItem)comboBoxAnncVoice.SelectedItem;
            this.Options.AnnouncementVoice = item.Description;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the "Disable Announcments" checkbox is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxDisableAnnc_CheckedChanged(object sender, EventArgs e)
        {
            this.Options.DisableAnnouncements = checkBoxDisableAnnc.Checked;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the "Audio Gain" textbox is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxAnncAudioGain_LostFocus(object sender, EventArgs e)
        {
            double gain = 0.0;
            if (double.TryParse(textBoxAnncAudioGain.Text, out gain))
            {
                this.textBoxAnncAudioGain.ForeColor = Color.Black;
                this.Options.SynthAnncGain = gain;
                UpdateModal();
            }
            else
                this.textBoxAnncAudioGain.ForeColor = Color.Red;
        }

        /// <summary>
        /// Occurs when the "System Name" textbox text is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxSysName_TextChanged(object sender, EventArgs e)
        {
            this.Options.SystemName = textBoxSysName.Text;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the "Use System Name" checkbox is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxAnncSysName_CheckedChanged(object sender, EventArgs e)
        {
            this.Options.UseSystemName = checkBoxAnncSysName.Checked;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the "Database Password" textbox is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxDbPassword_TextChanged(object sender, EventArgs e)
        {
            this.Options.DatabasePassword = textBoxDbPassword.Text;
            UpdateModal();
        }
        
        /// <summary>
        /// Occurs when the "Database Username" textbox is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxDbUsername_TextChanged(object sender, EventArgs e)
        {
            this.Options.DatabaseUsername = textBoxDbUsername.Text;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the "Database Server" textbox is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxDbServer_TextChanged(object sender, EventArgs e)
        {
            this.Options.DatabaseServer = textBoxDbServer.Text;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the "Database Name" textbox is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxDbName_TextChanged(object sender, EventArgs e)
        {
            this.Options.DatabaseName = textBoxDbName.Text;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the "Enable Logging" checkbox is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxDbLogging_CheckedChanged(object sender, EventArgs e)
        {
            this.Options.EnableDatabaseLogging = checkBoxDbLogging.Checked;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the "Ok" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            UpdateModal();
            SaveXml();

            DialogResult = DialogResult.OK;
            Canceled = false;

            this.Hide();
        }

        /// <summary>
        /// Occurs when the "Open File" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openCourtesyToneFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open Courtesy Tone Wave File";
            ofd.Filter = "Wave Files (*.wav)|*.wav|All Files (*.*)|*.*";
            DialogResult dr = ofd.ShowDialog();

            if (dr == DialogResult.Cancel)
            {
                this.Options.CourtesyFile = string.Empty;
                UpdateModal();
                return;
            }

            this.Options.CourtesyFile = ofd.FileName;
            UpdateModal();
        }

        /// <summary>
        /// Helper to update the multi-tone options.
        /// </summary>
        public void UpdateMultiTone()
        {
            if (this.multitoneWindow.Tones.Count > 0)
                this.Options.CourtesyMultiTone = true;
            else
                this.Options.CourtesyMultiTone = false;
            this.Options.CourtesyTones = this.multitoneWindow.Tones;
        }
        
        /// <summary>
        /// Occurs when the "Multitone" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openMultiTone_Click(object sender, EventArgs e)
        {
            multitoneWindow.Tones = this.Options.CourtesyTones;
            multitoneWindow.UpdateModal();

            multitoneWindow.ShowDialog();
        }

        /// <summary>
        /// Occurs when the "Test" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonTestCourtesy_Click(object sender, EventArgs e)
        {
            this.wnd.GenerateCourtesyTone();
        }
    } // public partial class RepeaterOptions : Form
} // namespace RepeaterController
