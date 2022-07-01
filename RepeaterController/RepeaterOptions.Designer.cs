namespace RepeaterController
{
    partial class RepeaterOptions
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepeaterOptions));
            this.okButton = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabIdOptions = new System.Windows.Forms.TabPage();
            this.groupBoxMdcSettings = new System.Windows.Forms.GroupBox();
            this.pttTransmitTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxUseRAC = new System.Windows.Forms.CheckBox();
            this.racTextBox = new System.Windows.Forms.TextBox();
            this.labelRAC = new System.Windows.Forms.Label();
            this.checkBoxAutoAckEmerg = new System.Windows.Forms.CheckBox();
            this.labelMDCPreambles = new System.Windows.Forms.Label();
            this.numOfPreamblesTextBox = new System.Windows.Forms.TextBox();
            this.groupBoxRepeaterId = new System.Windows.Forms.GroupBox();
            this.checkBoxDisablePLForId = new System.Windows.Forms.CheckBox();
            this.checkBoxDisableIdStartup = new System.Windows.Forms.CheckBox();
            this.checkBoxDisableId = new System.Windows.Forms.CheckBox();
            this.idIntervalTextBox = new System.Windows.Forms.TextBox();
            this.labelIdInterval = new System.Windows.Forms.Label();
            this.myIdTextBox = new System.Windows.Forms.TextBox();
            this.labelMyID = new System.Windows.Forms.Label();
            this.callsignTextBox = new System.Windows.Forms.TextBox();
            this.labelCallsign = new System.Windows.Forms.Label();
            this.tabRxOptions = new System.Windows.Forms.TabPage();
            this.rxPlDplGroupBox = new System.Windows.Forms.GroupBox();
            this.labelRxPL = new System.Windows.Forms.Label();
            this.comboBoxRxDPL = new System.Windows.Forms.ComboBox();
            this.comboBoxRxPL = new System.Windows.Forms.ComboBox();
            this.checkBoxUseRxDPL = new System.Windows.Forms.CheckBox();
            this.checkBoxRxPL = new System.Windows.Forms.CheckBox();
            this.labelDPL = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxRxControlPin = new System.Windows.Forms.GroupBox();
            this.rxControlPinDSR = new System.Windows.Forms.RadioButton();
            this.rxControlPinCTS = new System.Windows.Forms.RadioButton();
            this.rxControlPinCD = new System.Windows.Forms.RadioButton();
            this.comboBoxRxSerialPort = new System.Windows.Forms.ComboBox();
            this.labelRxControl = new System.Windows.Forms.Label();
            this.voxVolumeSlider = new RepeaterController.VolumeSlider();
            this.tabTxOptions = new System.Windows.Forms.TabPage();
            this.txPlDplGroupBox = new System.Windows.Forms.GroupBox();
            this.labelTxPL = new System.Windows.Forms.Label();
            this.textBoxTxPLGain = new System.Windows.Forms.TextBox();
            this.comboBoxTxPL = new System.Windows.Forms.ComboBox();
            this.labelTxPLGain = new System.Windows.Forms.Label();
            this.checkBoxTxPL = new System.Windows.Forms.CheckBox();
            this.comboBoxTxDPL = new System.Windows.Forms.ComboBox();
            this.labelTxDPL = new System.Windows.Forms.Label();
            this.checkBoxUseTxDPL = new System.Windows.Forms.CheckBox();
            this.watchDogTextBox = new System.Windows.Forms.TextBox();
            this.labelWatchDog = new System.Windows.Forms.Label();
            this.groupBoxTxControlPin = new System.Windows.Forms.GroupBox();
            this.txControlPinBoth = new System.Windows.Forms.RadioButton();
            this.txControlPinDTR = new System.Windows.Forms.RadioButton();
            this.txControlPinRTS = new System.Windows.Forms.RadioButton();
            this.comboBoxTxSerialPort = new System.Windows.Forms.ComboBox();
            this.labelTransmitterControl = new System.Windows.Forms.Label();
            this.tabTimers = new System.Windows.Forms.TabPage();
            this.numericUpDownDtmfDigitDelay = new System.Windows.Forms.NumericUpDown();
            this.labelDtmfDigitTime = new System.Windows.Forms.Label();
            this.checkBoxNoTone = new System.Windows.Forms.CheckBox();
            this.groupBoxCourtesyTone = new System.Windows.Forms.GroupBox();
            this.buttonTestCourtesy = new System.Windows.Forms.Button();
            this.openMultiTone = new System.Windows.Forms.Button();
            this.openCourtesyToneFile = new System.Windows.Forms.Button();
            this.courtesyToneFileTextBox = new System.Windows.Forms.TextBox();
            this.labelCourtesyFilename = new System.Windows.Forms.Label();
            this.courtesyPlayback = new System.Windows.Forms.RadioButton();
            this.courtesyGenerated = new System.Windows.Forms.RadioButton();
            this.courtesyToneDurationTextBox = new System.Windows.Forms.TextBox();
            this.courtesyTonePitchTextBox = new System.Windows.Forms.TextBox();
            this.courtesyToneDelayTextBox = new System.Windows.Forms.TextBox();
            this.labelCourtesyDuraion = new System.Windows.Forms.Label();
            this.labelCourtesyPitch = new System.Windows.Forms.Label();
            this.labelCourtesyDelay = new System.Windows.Forms.Label();
            this.tailTimerTextBox = new System.Windows.Forms.TextBox();
            this.labelTailTimer = new System.Windows.Forms.Label();
            this.maxTransmissionTimeTextBox = new System.Windows.Forms.TextBox();
            this.labelMaxTransmissionTime = new System.Windows.Forms.Label();
            this.tabAnnc = new System.Windows.Forms.TabPage();
            this.checkBoxAnncSysName = new System.Windows.Forms.CheckBox();
            this.textBoxSysName = new System.Windows.Forms.TextBox();
            this.labelSysName = new System.Windows.Forms.Label();
            this.textBoxAnncAudioGain = new System.Windows.Forms.TextBox();
            this.labelAnncAudioGain = new System.Windows.Forms.Label();
            this.comboBoxAnncVoice = new System.Windows.Forms.ComboBox();
            this.labelAnncVoice = new System.Windows.Forms.Label();
            this.checkBoxDisableAnnc = new System.Windows.Forms.CheckBox();
            this.tabLogging = new System.Windows.Forms.TabPage();
            this.groupBoxLogging = new System.Windows.Forms.GroupBox();
            this.textBoxDbPassword = new System.Windows.Forms.TextBox();
            this.labelDbPassword = new System.Windows.Forms.Label();
            this.textBoxDbUsername = new System.Windows.Forms.TextBox();
            this.labelDbUsername = new System.Windows.Forms.Label();
            this.textBoxDbServer = new System.Windows.Forms.TextBox();
            this.labelDbServer = new System.Windows.Forms.Label();
            this.textBoxDbName = new System.Windows.Forms.TextBox();
            this.labelDbName = new System.Windows.Forms.Label();
            this.checkBoxDbLogging = new System.Windows.Forms.CheckBox();
            this.checkBoxMDCOnly = new System.Windows.Forms.CheckBox();
            this.checkBoxConsoleAnncDTMF = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl.SuspendLayout();
            this.tabIdOptions.SuspendLayout();
            this.groupBoxMdcSettings.SuspendLayout();
            this.groupBoxRepeaterId.SuspendLayout();
            this.tabRxOptions.SuspendLayout();
            this.rxPlDplGroupBox.SuspendLayout();
            this.groupBoxRxControlPin.SuspendLayout();
            this.tabTxOptions.SuspendLayout();
            this.txPlDplGroupBox.SuspendLayout();
            this.groupBoxTxControlPin.SuspendLayout();
            this.tabTimers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDtmfDigitDelay)).BeginInit();
            this.groupBoxCourtesyTone.SuspendLayout();
            this.tabAnnc.SuspendLayout();
            this.tabLogging.SuspendLayout();
            this.groupBoxLogging.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okButton.Location = new System.Drawing.Point(490, 251);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabIdOptions);
            this.tabControl.Controls.Add(this.tabRxOptions);
            this.tabControl.Controls.Add(this.tabTxOptions);
            this.tabControl.Controls.Add(this.tabTimers);
            this.tabControl.Controls.Add(this.tabAnnc);
            this.tabControl.Controls.Add(this.tabLogging);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(557, 233);
            this.tabControl.TabIndex = 1;
            // 
            // tabIdOptions
            // 
            this.tabIdOptions.Controls.Add(this.groupBoxMdcSettings);
            this.tabIdOptions.Controls.Add(this.groupBoxRepeaterId);
            this.tabIdOptions.Location = new System.Drawing.Point(4, 22);
            this.tabIdOptions.Name = "tabIdOptions";
            this.tabIdOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabIdOptions.Size = new System.Drawing.Size(549, 207);
            this.tabIdOptions.TabIndex = 0;
            this.tabIdOptions.Text = "Repeater ID & MDC Settings";
            this.tabIdOptions.UseVisualStyleBackColor = true;
            // 
            // groupBoxMdcSettings
            // 
            this.groupBoxMdcSettings.Controls.Add(this.pttTransmitTextBox);
            this.groupBoxMdcSettings.Controls.Add(this.label2);
            this.groupBoxMdcSettings.Controls.Add(this.checkBoxUseRAC);
            this.groupBoxMdcSettings.Controls.Add(this.racTextBox);
            this.groupBoxMdcSettings.Controls.Add(this.labelRAC);
            this.groupBoxMdcSettings.Controls.Add(this.checkBoxAutoAckEmerg);
            this.groupBoxMdcSettings.Controls.Add(this.labelMDCPreambles);
            this.groupBoxMdcSettings.Controls.Add(this.numOfPreamblesTextBox);
            this.groupBoxMdcSettings.Location = new System.Drawing.Point(6, 90);
            this.groupBoxMdcSettings.Name = "groupBoxMdcSettings";
            this.groupBoxMdcSettings.Size = new System.Drawing.Size(489, 100);
            this.groupBoxMdcSettings.TabIndex = 16;
            this.groupBoxMdcSettings.TabStop = false;
            this.groupBoxMdcSettings.Text = "MDC Settings";
            // 
            // pttTransmitTextBox
            // 
            this.pttTransmitTextBox.Location = new System.Drawing.Point(369, 45);
            this.pttTransmitTextBox.MaxLength = 4;
            this.pttTransmitTextBox.Name = "pttTransmitTextBox";
            this.pttTransmitTextBox.Size = new System.Drawing.Size(52, 20);
            this.pttTransmitTextBox.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(271, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "PTT Transmit Key";
            // 
            // checkBoxUseRAC
            // 
            this.checkBoxUseRAC.AutoSize = true;
            this.checkBoxUseRAC.Location = new System.Drawing.Point(9, 71);
            this.checkBoxUseRAC.Name = "checkBoxUseRAC";
            this.checkBoxUseRAC.Size = new System.Drawing.Size(130, 17);
            this.checkBoxUseRAC.TabIndex = 7;
            this.checkBoxUseRAC.Text = "Use Repeater Access";
            this.checkBoxUseRAC.UseVisualStyleBackColor = true;
            // 
            // racTextBox
            // 
            this.racTextBox.Location = new System.Drawing.Point(129, 45);
            this.racTextBox.Name = "racTextBox";
            this.racTextBox.Size = new System.Drawing.Size(136, 20);
            this.racTextBox.TabIndex = 8;
            // 
            // labelRAC
            // 
            this.labelRAC.AutoSize = true;
            this.labelRAC.Location = new System.Drawing.Point(6, 48);
            this.labelRAC.Name = "labelRAC";
            this.labelRAC.Size = new System.Drawing.Size(117, 13);
            this.labelRAC.TabIndex = 18;
            this.labelRAC.Text = "Repeater Access Code";
            // 
            // checkBoxAutoAckEmerg
            // 
            this.checkBoxAutoAckEmerg.AutoSize = true;
            this.checkBoxAutoAckEmerg.Location = new System.Drawing.Point(192, 19);
            this.checkBoxAutoAckEmerg.Name = "checkBoxAutoAckEmerg";
            this.checkBoxAutoAckEmerg.Size = new System.Drawing.Size(172, 17);
            this.checkBoxAutoAckEmerg.TabIndex = 9;
            this.checkBoxAutoAckEmerg.Text = "Auto Acknowledge Emergency";
            this.checkBoxAutoAckEmerg.UseVisualStyleBackColor = true;
            // 
            // labelMDCPreambles
            // 
            this.labelMDCPreambles.AutoSize = true;
            this.labelMDCPreambles.Location = new System.Drawing.Point(6, 22);
            this.labelMDCPreambles.Name = "labelMDCPreambles";
            this.labelMDCPreambles.Size = new System.Drawing.Size(83, 13);
            this.labelMDCPreambles.TabIndex = 16;
            this.labelMDCPreambles.Text = "MDC Preambles";
            // 
            // numOfPreamblesTextBox
            // 
            this.numOfPreamblesTextBox.Location = new System.Drawing.Point(95, 19);
            this.numOfPreamblesTextBox.MaxLength = 4;
            this.numOfPreamblesTextBox.Name = "numOfPreamblesTextBox";
            this.numOfPreamblesTextBox.Size = new System.Drawing.Size(52, 20);
            this.numOfPreamblesTextBox.TabIndex = 6;
            // 
            // groupBoxRepeaterId
            // 
            this.groupBoxRepeaterId.Controls.Add(this.checkBoxDisablePLForId);
            this.groupBoxRepeaterId.Controls.Add(this.checkBoxDisableIdStartup);
            this.groupBoxRepeaterId.Controls.Add(this.checkBoxDisableId);
            this.groupBoxRepeaterId.Controls.Add(this.idIntervalTextBox);
            this.groupBoxRepeaterId.Controls.Add(this.labelIdInterval);
            this.groupBoxRepeaterId.Controls.Add(this.myIdTextBox);
            this.groupBoxRepeaterId.Controls.Add(this.labelMyID);
            this.groupBoxRepeaterId.Controls.Add(this.callsignTextBox);
            this.groupBoxRepeaterId.Controls.Add(this.labelCallsign);
            this.groupBoxRepeaterId.Location = new System.Drawing.Point(6, 6);
            this.groupBoxRepeaterId.Name = "groupBoxRepeaterId";
            this.groupBoxRepeaterId.Size = new System.Drawing.Size(489, 78);
            this.groupBoxRepeaterId.TabIndex = 15;
            this.groupBoxRepeaterId.TabStop = false;
            this.groupBoxRepeaterId.Text = "Repeater ID";
            // 
            // checkBoxDisablePLForId
            // 
            this.checkBoxDisablePLForId.AutoSize = true;
            this.checkBoxDisablePLForId.Location = new System.Drawing.Point(370, 21);
            this.checkBoxDisablePLForId.Name = "checkBoxDisablePLForId";
            this.checkBoxDisablePLForId.Size = new System.Drawing.Size(116, 17);
            this.checkBoxDisablePLForId.TabIndex = 16;
            this.checkBoxDisablePLForId.Text = "ID without PL/DPL";
            this.checkBoxDisablePLForId.UseVisualStyleBackColor = true;
            // 
            // checkBoxDisableIdStartup
            // 
            this.checkBoxDisableIdStartup.AutoSize = true;
            this.checkBoxDisableIdStartup.Location = new System.Drawing.Point(308, 47);
            this.checkBoxDisableIdStartup.Name = "checkBoxDisableIdStartup";
            this.checkBoxDisableIdStartup.Size = new System.Drawing.Size(103, 17);
            this.checkBoxDisableIdStartup.TabIndex = 15;
            this.checkBoxDisableIdStartup.Text = "No ID at Startup";
            this.checkBoxDisableIdStartup.UseVisualStyleBackColor = true;
            // 
            // checkBoxDisableId
            // 
            this.checkBoxDisableId.AutoSize = true;
            this.checkBoxDisableId.Location = new System.Drawing.Point(202, 47);
            this.checkBoxDisableId.Name = "checkBoxDisableId";
            this.checkBoxDisableId.Size = new System.Drawing.Size(100, 17);
            this.checkBoxDisableId.TabIndex = 5;
            this.checkBoxDisableId.Text = "Disable Auto ID";
            this.checkBoxDisableId.UseVisualStyleBackColor = true;
            // 
            // idIntervalTextBox
            // 
            this.idIntervalTextBox.Location = new System.Drawing.Point(286, 19);
            this.idIntervalTextBox.Name = "idIntervalTextBox";
            this.idIntervalTextBox.Size = new System.Drawing.Size(78, 20);
            this.idIntervalTextBox.TabIndex = 4;
            // 
            // labelIdInterval
            // 
            this.labelIdInterval.AutoSize = true;
            this.labelIdInterval.Location = new System.Drawing.Point(199, 22);
            this.labelIdInterval.Name = "labelIdInterval";
            this.labelIdInterval.Size = new System.Drawing.Size(81, 13);
            this.labelIdInterval.TabIndex = 14;
            this.labelIdInterval.Text = "ID Interval (min)";
            // 
            // myIdTextBox
            // 
            this.myIdTextBox.Location = new System.Drawing.Point(57, 45);
            this.myIdTextBox.Name = "myIdTextBox";
            this.myIdTextBox.Size = new System.Drawing.Size(136, 20);
            this.myIdTextBox.TabIndex = 3;
            // 
            // labelMyID
            // 
            this.labelMyID.AutoSize = true;
            this.labelMyID.Location = new System.Drawing.Point(16, 48);
            this.labelMyID.Name = "labelMyID";
            this.labelMyID.Size = new System.Drawing.Size(35, 13);
            this.labelMyID.TabIndex = 12;
            this.labelMyID.Text = "My ID";
            // 
            // callsignTextBox
            // 
            this.callsignTextBox.Location = new System.Drawing.Point(57, 19);
            this.callsignTextBox.Name = "callsignTextBox";
            this.callsignTextBox.Size = new System.Drawing.Size(136, 20);
            this.callsignTextBox.TabIndex = 2;
            // 
            // labelCallsign
            // 
            this.labelCallsign.AutoSize = true;
            this.labelCallsign.Location = new System.Drawing.Point(8, 22);
            this.labelCallsign.Name = "labelCallsign";
            this.labelCallsign.Size = new System.Drawing.Size(43, 13);
            this.labelCallsign.TabIndex = 10;
            this.labelCallsign.Text = "Callsign";
            // 
            // tabRxOptions
            // 
            this.tabRxOptions.Controls.Add(this.rxPlDplGroupBox);
            this.tabRxOptions.Controls.Add(this.label1);
            this.tabRxOptions.Controls.Add(this.groupBoxRxControlPin);
            this.tabRxOptions.Controls.Add(this.comboBoxRxSerialPort);
            this.tabRxOptions.Controls.Add(this.labelRxControl);
            this.tabRxOptions.Controls.Add(this.voxVolumeSlider);
            this.tabRxOptions.Location = new System.Drawing.Point(4, 22);
            this.tabRxOptions.Name = "tabRxOptions";
            this.tabRxOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabRxOptions.Size = new System.Drawing.Size(549, 207);
            this.tabRxOptions.TabIndex = 1;
            this.tabRxOptions.Text = "Receiver";
            this.tabRxOptions.UseVisualStyleBackColor = true;
            // 
            // rxPlDplGroupBox
            // 
            this.rxPlDplGroupBox.Controls.Add(this.labelRxPL);
            this.rxPlDplGroupBox.Controls.Add(this.comboBoxRxDPL);
            this.rxPlDplGroupBox.Controls.Add(this.comboBoxRxPL);
            this.rxPlDplGroupBox.Controls.Add(this.checkBoxUseRxDPL);
            this.rxPlDplGroupBox.Controls.Add(this.checkBoxRxPL);
            this.rxPlDplGroupBox.Controls.Add(this.labelDPL);
            this.rxPlDplGroupBox.Location = new System.Drawing.Point(237, 6);
            this.rxPlDplGroupBox.Name = "rxPlDplGroupBox";
            this.rxPlDplGroupBox.Size = new System.Drawing.Size(253, 63);
            this.rxPlDplGroupBox.TabIndex = 14;
            this.rxPlDplGroupBox.TabStop = false;
            this.rxPlDplGroupBox.Text = "PL/DPL Settings";
            // 
            // labelRxPL
            // 
            this.labelRxPL.AutoSize = true;
            this.labelRxPL.Location = new System.Drawing.Point(6, 16);
            this.labelRxPL.Name = "labelRxPL";
            this.labelRxPL.Size = new System.Drawing.Size(23, 13);
            this.labelRxPL.TabIndex = 7;
            this.labelRxPL.Text = "PL:";
            // 
            // comboBoxRxDPL
            // 
            this.comboBoxRxDPL.FormattingEnabled = true;
            this.comboBoxRxDPL.Location = new System.Drawing.Point(179, 13);
            this.comboBoxRxDPL.Name = "comboBoxRxDPL";
            this.comboBoxRxDPL.Size = new System.Drawing.Size(65, 21);
            this.comboBoxRxDPL.TabIndex = 13;
            // 
            // comboBoxRxPL
            // 
            this.comboBoxRxPL.FormattingEnabled = true;
            this.comboBoxRxPL.Location = new System.Drawing.Point(35, 13);
            this.comboBoxRxPL.Name = "comboBoxRxPL";
            this.comboBoxRxPL.Size = new System.Drawing.Size(65, 21);
            this.comboBoxRxPL.TabIndex = 8;
            // 
            // checkBoxUseRxDPL
            // 
            this.checkBoxUseRxDPL.AutoSize = true;
            this.checkBoxUseRxDPL.Location = new System.Drawing.Point(145, 40);
            this.checkBoxUseRxDPL.Name = "checkBoxUseRxDPL";
            this.checkBoxUseRxDPL.Size = new System.Drawing.Size(85, 17);
            this.checkBoxUseRxDPL.TabIndex = 12;
            this.checkBoxUseRxDPL.Text = "Use Rx DPL";
            this.checkBoxUseRxDPL.UseVisualStyleBackColor = true;
            // 
            // checkBoxRxPL
            // 
            this.checkBoxRxPL.AutoSize = true;
            this.checkBoxRxPL.Location = new System.Drawing.Point(9, 40);
            this.checkBoxRxPL.Name = "checkBoxRxPL";
            this.checkBoxRxPL.Size = new System.Drawing.Size(121, 17);
            this.checkBoxRxPL.TabIndex = 9;
            this.checkBoxRxPL.Text = "Enable Receiver PL";
            this.checkBoxRxPL.UseVisualStyleBackColor = true;
            // 
            // labelDPL
            // 
            this.labelDPL.AutoSize = true;
            this.labelDPL.Location = new System.Drawing.Point(142, 16);
            this.labelDPL.Name = "labelDPL";
            this.labelDPL.Size = new System.Drawing.Size(31, 13);
            this.labelDPL.TabIndex = 10;
            this.labelDPL.Text = "DPL:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 133);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "VOX Level:";
            // 
            // groupBoxRxControlPin
            // 
            this.groupBoxRxControlPin.Controls.Add(this.rxControlPinDSR);
            this.groupBoxRxControlPin.Controls.Add(this.rxControlPinCTS);
            this.groupBoxRxControlPin.Controls.Add(this.rxControlPinCD);
            this.groupBoxRxControlPin.Location = new System.Drawing.Point(9, 33);
            this.groupBoxRxControlPin.Name = "groupBoxRxControlPin";
            this.groupBoxRxControlPin.Size = new System.Drawing.Size(200, 89);
            this.groupBoxRxControlPin.TabIndex = 5;
            this.groupBoxRxControlPin.TabStop = false;
            this.groupBoxRxControlPin.Text = "Control Pin";
            // 
            // rxControlPinDSR
            // 
            this.rxControlPinDSR.AutoSize = true;
            this.rxControlPinDSR.Location = new System.Drawing.Point(6, 65);
            this.rxControlPinDSR.Name = "rxControlPinDSR";
            this.rxControlPinDSR.Size = new System.Drawing.Size(133, 17);
            this.rxControlPinDSR.TabIndex = 5;
            this.rxControlPinDSR.TabStop = true;
            this.rxControlPinDSR.Text = "DSR (Data Set Ready)";
            this.rxControlPinDSR.UseVisualStyleBackColor = true;
            // 
            // rxControlPinCTS
            // 
            this.rxControlPinCTS.AutoSize = true;
            this.rxControlPinCTS.Location = new System.Drawing.Point(6, 42);
            this.rxControlPinCTS.Name = "rxControlPinCTS";
            this.rxControlPinCTS.Size = new System.Drawing.Size(123, 17);
            this.rxControlPinCTS.TabIndex = 4;
            this.rxControlPinCTS.TabStop = true;
            this.rxControlPinCTS.Text = "CTS (Clear To Send)";
            this.rxControlPinCTS.UseVisualStyleBackColor = true;
            // 
            // rxControlPinCD
            // 
            this.rxControlPinCD.AutoSize = true;
            this.rxControlPinCD.Location = new System.Drawing.Point(6, 19);
            this.rxControlPinCD.Name = "rxControlPinCD";
            this.rxControlPinCD.Size = new System.Drawing.Size(114, 17);
            this.rxControlPinCD.TabIndex = 3;
            this.rxControlPinCD.TabStop = true;
            this.rxControlPinCD.Text = "CD (Carrier Detect)";
            this.rxControlPinCD.UseVisualStyleBackColor = true;
            // 
            // comboBoxRxSerialPort
            // 
            this.comboBoxRxSerialPort.FormattingEnabled = true;
            this.comboBoxRxSerialPort.Location = new System.Drawing.Point(101, 6);
            this.comboBoxRxSerialPort.Name = "comboBoxRxSerialPort";
            this.comboBoxRxSerialPort.Size = new System.Drawing.Size(121, 21);
            this.comboBoxRxSerialPort.TabIndex = 2;
            // 
            // labelRxControl
            // 
            this.labelRxControl.AutoSize = true;
            this.labelRxControl.Location = new System.Drawing.Point(6, 9);
            this.labelRxControl.Name = "labelRxControl";
            this.labelRxControl.Size = new System.Drawing.Size(89, 13);
            this.labelRxControl.TabIndex = 0;
            this.labelRxControl.Text = "Receiver Control:";
            // 
            // voxVolumeSlider
            // 
            this.voxVolumeSlider.Location = new System.Drawing.Point(73, 128);
            this.voxVolumeSlider.Name = "voxVolumeSlider";
            this.voxVolumeSlider.Size = new System.Drawing.Size(279, 25);
            this.voxVolumeSlider.TabIndex = 6;
            // 
            // tabTxOptions
            // 
            this.tabTxOptions.Controls.Add(this.txPlDplGroupBox);
            this.tabTxOptions.Controls.Add(this.watchDogTextBox);
            this.tabTxOptions.Controls.Add(this.labelWatchDog);
            this.tabTxOptions.Controls.Add(this.groupBoxTxControlPin);
            this.tabTxOptions.Controls.Add(this.comboBoxTxSerialPort);
            this.tabTxOptions.Controls.Add(this.labelTransmitterControl);
            this.tabTxOptions.Location = new System.Drawing.Point(4, 22);
            this.tabTxOptions.Name = "tabTxOptions";
            this.tabTxOptions.Size = new System.Drawing.Size(549, 207);
            this.tabTxOptions.TabIndex = 2;
            this.tabTxOptions.Text = "Transmitter";
            this.tabTxOptions.UseVisualStyleBackColor = true;
            // 
            // txPlDplGroupBox
            // 
            this.txPlDplGroupBox.Controls.Add(this.labelTxPL);
            this.txPlDplGroupBox.Controls.Add(this.textBoxTxPLGain);
            this.txPlDplGroupBox.Controls.Add(this.comboBoxTxPL);
            this.txPlDplGroupBox.Controls.Add(this.labelTxPLGain);
            this.txPlDplGroupBox.Controls.Add(this.checkBoxTxPL);
            this.txPlDplGroupBox.Controls.Add(this.comboBoxTxDPL);
            this.txPlDplGroupBox.Controls.Add(this.labelTxDPL);
            this.txPlDplGroupBox.Controls.Add(this.checkBoxUseTxDPL);
            this.txPlDplGroupBox.Location = new System.Drawing.Point(237, 6);
            this.txPlDplGroupBox.Name = "txPlDplGroupBox";
            this.txPlDplGroupBox.Size = new System.Drawing.Size(251, 95);
            this.txPlDplGroupBox.TabIndex = 26;
            this.txPlDplGroupBox.TabStop = false;
            this.txPlDplGroupBox.Text = "PL/DPL Settings";
            // 
            // labelTxPL
            // 
            this.labelTxPL.AutoSize = true;
            this.labelTxPL.Location = new System.Drawing.Point(6, 16);
            this.labelTxPL.Name = "labelTxPL";
            this.labelTxPL.Size = new System.Drawing.Size(23, 13);
            this.labelTxPL.TabIndex = 17;
            this.labelTxPL.Text = "PL:";
            // 
            // textBoxTxPLGain
            // 
            this.textBoxTxPLGain.Location = new System.Drawing.Point(60, 63);
            this.textBoxTxPLGain.Name = "textBoxTxPLGain";
            this.textBoxTxPLGain.Size = new System.Drawing.Size(57, 20);
            this.textBoxTxPLGain.TabIndex = 25;
            // 
            // comboBoxTxPL
            // 
            this.comboBoxTxPL.FormattingEnabled = true;
            this.comboBoxTxPL.Location = new System.Drawing.Point(35, 13);
            this.comboBoxTxPL.Name = "comboBoxTxPL";
            this.comboBoxTxPL.Size = new System.Drawing.Size(65, 21);
            this.comboBoxTxPL.TabIndex = 18;
            // 
            // labelTxPLGain
            // 
            this.labelTxPLGain.AutoSize = true;
            this.labelTxPLGain.Location = new System.Drawing.Point(6, 66);
            this.labelTxPLGain.Name = "labelTxPLGain";
            this.labelTxPLGain.Size = new System.Drawing.Size(48, 13);
            this.labelTxPLGain.TabIndex = 24;
            this.labelTxPLGain.Text = "PL Gain:";
            // 
            // checkBoxTxPL
            // 
            this.checkBoxTxPL.AutoSize = true;
            this.checkBoxTxPL.Location = new System.Drawing.Point(9, 40);
            this.checkBoxTxPL.Name = "checkBoxTxPL";
            this.checkBoxTxPL.Size = new System.Drawing.Size(130, 17);
            this.checkBoxTxPL.TabIndex = 19;
            this.checkBoxTxPL.Text = "Enable Transmitter PL";
            this.checkBoxTxPL.UseVisualStyleBackColor = true;
            // 
            // comboBoxTxDPL
            // 
            this.comboBoxTxDPL.FormattingEnabled = true;
            this.comboBoxTxDPL.Location = new System.Drawing.Point(179, 13);
            this.comboBoxTxDPL.Name = "comboBoxTxDPL";
            this.comboBoxTxDPL.Size = new System.Drawing.Size(65, 21);
            this.comboBoxTxDPL.TabIndex = 23;
            // 
            // labelTxDPL
            // 
            this.labelTxDPL.AutoSize = true;
            this.labelTxDPL.Location = new System.Drawing.Point(142, 16);
            this.labelTxDPL.Name = "labelTxDPL";
            this.labelTxDPL.Size = new System.Drawing.Size(31, 13);
            this.labelTxDPL.TabIndex = 20;
            this.labelTxDPL.Text = "DPL:";
            // 
            // checkBoxUseTxDPL
            // 
            this.checkBoxUseTxDPL.AutoSize = true;
            this.checkBoxUseTxDPL.Location = new System.Drawing.Point(145, 40);
            this.checkBoxUseTxDPL.Name = "checkBoxUseTxDPL";
            this.checkBoxUseTxDPL.Size = new System.Drawing.Size(84, 17);
            this.checkBoxUseTxDPL.TabIndex = 22;
            this.checkBoxUseTxDPL.Text = "Use Tx DPL";
            this.checkBoxUseTxDPL.UseVisualStyleBackColor = true;
            // 
            // watchDogTextBox
            // 
            this.watchDogTextBox.Location = new System.Drawing.Point(129, 128);
            this.watchDogTextBox.Name = "watchDogTextBox";
            this.watchDogTextBox.Size = new System.Drawing.Size(78, 20);
            this.watchDogTextBox.TabIndex = 15;
            // 
            // labelWatchDog
            // 
            this.labelWatchDog.AutoSize = true;
            this.labelWatchDog.Location = new System.Drawing.Point(6, 131);
            this.labelWatchDog.Name = "labelWatchDog";
            this.labelWatchDog.Size = new System.Drawing.Size(120, 13);
            this.labelWatchDog.TabIndex = 16;
            this.labelWatchDog.Text = "Watch Dog Timer (sec):";
            // 
            // groupBoxTxControlPin
            // 
            this.groupBoxTxControlPin.Controls.Add(this.txControlPinBoth);
            this.groupBoxTxControlPin.Controls.Add(this.txControlPinDTR);
            this.groupBoxTxControlPin.Controls.Add(this.txControlPinRTS);
            this.groupBoxTxControlPin.Location = new System.Drawing.Point(9, 33);
            this.groupBoxTxControlPin.Name = "groupBoxTxControlPin";
            this.groupBoxTxControlPin.Size = new System.Drawing.Size(200, 89);
            this.groupBoxTxControlPin.TabIndex = 4;
            this.groupBoxTxControlPin.TabStop = false;
            this.groupBoxTxControlPin.Text = "Control Pin";
            // 
            // txControlPinBoth
            // 
            this.txControlPinBoth.AutoSize = true;
            this.txControlPinBoth.Location = new System.Drawing.Point(6, 65);
            this.txControlPinBoth.Name = "txControlPinBoth";
            this.txControlPinBoth.Size = new System.Drawing.Size(47, 17);
            this.txControlPinBoth.TabIndex = 5;
            this.txControlPinBoth.TabStop = true;
            this.txControlPinBoth.Text = "Both";
            this.txControlPinBoth.UseVisualStyleBackColor = true;
            // 
            // txControlPinDTR
            // 
            this.txControlPinDTR.AutoSize = true;
            this.txControlPinDTR.Location = new System.Drawing.Point(6, 42);
            this.txControlPinDTR.Name = "txControlPinDTR";
            this.txControlPinDTR.Size = new System.Drawing.Size(157, 17);
            this.txControlPinDTR.TabIndex = 4;
            this.txControlPinDTR.TabStop = true;
            this.txControlPinDTR.Text = "DTR (Data Terminal Ready)";
            this.txControlPinDTR.UseVisualStyleBackColor = true;
            // 
            // txControlPinRTS
            // 
            this.txControlPinRTS.AutoSize = true;
            this.txControlPinRTS.Location = new System.Drawing.Point(6, 19);
            this.txControlPinRTS.Name = "txControlPinRTS";
            this.txControlPinRTS.Size = new System.Drawing.Size(140, 17);
            this.txControlPinRTS.TabIndex = 3;
            this.txControlPinRTS.TabStop = true;
            this.txControlPinRTS.Text = "RTS (Request To Send)";
            this.txControlPinRTS.UseVisualStyleBackColor = true;
            // 
            // comboBoxTxSerialPort
            // 
            this.comboBoxTxSerialPort.FormattingEnabled = true;
            this.comboBoxTxSerialPort.Location = new System.Drawing.Point(110, 6);
            this.comboBoxTxSerialPort.Name = "comboBoxTxSerialPort";
            this.comboBoxTxSerialPort.Size = new System.Drawing.Size(121, 21);
            this.comboBoxTxSerialPort.TabIndex = 2;
            // 
            // labelTransmitterControl
            // 
            this.labelTransmitterControl.AutoSize = true;
            this.labelTransmitterControl.Location = new System.Drawing.Point(6, 9);
            this.labelTransmitterControl.Name = "labelTransmitterControl";
            this.labelTransmitterControl.Size = new System.Drawing.Size(98, 13);
            this.labelTransmitterControl.TabIndex = 2;
            this.labelTransmitterControl.Text = "Transmitter Control:";
            // 
            // tabTimers
            // 
            this.tabTimers.Controls.Add(this.numericUpDownDtmfDigitDelay);
            this.tabTimers.Controls.Add(this.labelDtmfDigitTime);
            this.tabTimers.Controls.Add(this.checkBoxNoTone);
            this.tabTimers.Controls.Add(this.groupBoxCourtesyTone);
            this.tabTimers.Controls.Add(this.tailTimerTextBox);
            this.tabTimers.Controls.Add(this.labelTailTimer);
            this.tabTimers.Controls.Add(this.maxTransmissionTimeTextBox);
            this.tabTimers.Controls.Add(this.labelMaxTransmissionTime);
            this.tabTimers.Location = new System.Drawing.Point(4, 22);
            this.tabTimers.Name = "tabTimers";
            this.tabTimers.Size = new System.Drawing.Size(549, 207);
            this.tabTimers.TabIndex = 3;
            this.tabTimers.Text = "Timers & Courtesy Tone";
            this.tabTimers.UseVisualStyleBackColor = true;
            // 
            // numericUpDownDtmfDigitDelay
            // 
            this.numericUpDownDtmfDigitDelay.DecimalPlaces = 3;
            this.numericUpDownDtmfDigitDelay.Increment = new decimal(new int[] {
            50,
            0,
            0,
            196608});
            this.numericUpDownDtmfDigitDelay.Location = new System.Drawing.Point(414, 7);
            this.numericUpDownDtmfDigitDelay.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownDtmfDigitDelay.Name = "numericUpDownDtmfDigitDelay";
            this.numericUpDownDtmfDigitDelay.Size = new System.Drawing.Size(70, 20);
            this.numericUpDownDtmfDigitDelay.TabIndex = 6;
            // 
            // labelDtmfDigitTime
            // 
            this.labelDtmfDigitTime.AutoSize = true;
            this.labelDtmfDigitTime.Location = new System.Drawing.Point(292, 9);
            this.labelDtmfDigitTime.Name = "labelDtmfDigitTime";
            this.labelDtmfDigitTime.Size = new System.Drawing.Size(116, 13);
            this.labelDtmfDigitTime.TabIndex = 5;
            this.labelDtmfDigitTime.Text = "DTMF Inter-digit Delay:";
            // 
            // checkBoxNoTone
            // 
            this.checkBoxNoTone.AutoSize = true;
            this.checkBoxNoTone.Location = new System.Drawing.Point(372, 35);
            this.checkBoxNoTone.Name = "checkBoxNoTone";
            this.checkBoxNoTone.Size = new System.Drawing.Size(112, 17);
            this.checkBoxNoTone.TabIndex = 4;
            this.checkBoxNoTone.Text = "No Courtesy Tone";
            this.checkBoxNoTone.UseVisualStyleBackColor = true;
            // 
            // groupBoxCourtesyTone
            // 
            this.groupBoxCourtesyTone.Controls.Add(this.buttonTestCourtesy);
            this.groupBoxCourtesyTone.Controls.Add(this.openMultiTone);
            this.groupBoxCourtesyTone.Controls.Add(this.openCourtesyToneFile);
            this.groupBoxCourtesyTone.Controls.Add(this.courtesyToneFileTextBox);
            this.groupBoxCourtesyTone.Controls.Add(this.labelCourtesyFilename);
            this.groupBoxCourtesyTone.Controls.Add(this.courtesyPlayback);
            this.groupBoxCourtesyTone.Controls.Add(this.courtesyGenerated);
            this.groupBoxCourtesyTone.Controls.Add(this.courtesyToneDurationTextBox);
            this.groupBoxCourtesyTone.Controls.Add(this.courtesyTonePitchTextBox);
            this.groupBoxCourtesyTone.Controls.Add(this.courtesyToneDelayTextBox);
            this.groupBoxCourtesyTone.Controls.Add(this.labelCourtesyDuraion);
            this.groupBoxCourtesyTone.Controls.Add(this.labelCourtesyPitch);
            this.groupBoxCourtesyTone.Controls.Add(this.labelCourtesyDelay);
            this.groupBoxCourtesyTone.Location = new System.Drawing.Point(9, 58);
            this.groupBoxCourtesyTone.Name = "groupBoxCourtesyTone";
            this.groupBoxCourtesyTone.Size = new System.Drawing.Size(475, 138);
            this.groupBoxCourtesyTone.TabIndex = 4;
            this.groupBoxCourtesyTone.TabStop = false;
            this.groupBoxCourtesyTone.Text = "Courtesy Tone";
            // 
            // buttonTestCourtesy
            // 
            this.buttonTestCourtesy.Location = new System.Drawing.Point(137, 43);
            this.buttonTestCourtesy.Name = "buttonTestCourtesy";
            this.buttonTestCourtesy.Size = new System.Drawing.Size(75, 23);
            this.buttonTestCourtesy.TabIndex = 13;
            this.buttonTestCourtesy.Text = "Test...";
            this.buttonTestCourtesy.UseVisualStyleBackColor = true;
            this.buttonTestCourtesy.Click += new System.EventHandler(this.buttonTestCourtesy_Click);
            // 
            // openMultiTone
            // 
            this.openMultiTone.Location = new System.Drawing.Point(137, 17);
            this.openMultiTone.Name = "openMultiTone";
            this.openMultiTone.Size = new System.Drawing.Size(75, 23);
            this.openMultiTone.TabIndex = 12;
            this.openMultiTone.Text = "Multitone...";
            this.openMultiTone.UseVisualStyleBackColor = true;
            this.openMultiTone.Click += new System.EventHandler(this.openMultiTone_Click);
            // 
            // openCourtesyToneFile
            // 
            this.openCourtesyToneFile.Location = new System.Drawing.Point(319, 43);
            this.openCourtesyToneFile.Name = "openCourtesyToneFile";
            this.openCourtesyToneFile.Size = new System.Drawing.Size(75, 23);
            this.openCourtesyToneFile.TabIndex = 11;
            this.openCourtesyToneFile.Text = "Open File...";
            this.openCourtesyToneFile.UseVisualStyleBackColor = true;
            this.openCourtesyToneFile.Click += new System.EventHandler(this.openCourtesyToneFile_Click);
            // 
            // courtesyToneFileTextBox
            // 
            this.courtesyToneFileTextBox.Location = new System.Drawing.Point(319, 17);
            this.courtesyToneFileTextBox.Name = "courtesyToneFileTextBox";
            this.courtesyToneFileTextBox.Size = new System.Drawing.Size(150, 20);
            this.courtesyToneFileTextBox.TabIndex = 10;
            // 
            // labelCourtesyFilename
            // 
            this.labelCourtesyFilename.AutoSize = true;
            this.labelCourtesyFilename.Location = new System.Drawing.Point(261, 20);
            this.labelCourtesyFilename.Name = "labelCourtesyFilename";
            this.labelCourtesyFilename.Size = new System.Drawing.Size(52, 13);
            this.labelCourtesyFilename.TabIndex = 10;
            this.labelCourtesyFilename.Text = "Filename:";
            // 
            // courtesyPlayback
            // 
            this.courtesyPlayback.AutoSize = true;
            this.courtesyPlayback.Location = new System.Drawing.Point(319, 115);
            this.courtesyPlayback.Name = "courtesyPlayback";
            this.courtesyPlayback.Size = new System.Drawing.Size(88, 17);
            this.courtesyPlayback.TabIndex = 6;
            this.courtesyPlayback.TabStop = true;
            this.courtesyPlayback.Text = "Playback File";
            this.courtesyPlayback.UseVisualStyleBackColor = true;
            // 
            // courtesyGenerated
            // 
            this.courtesyGenerated.AutoSize = true;
            this.courtesyGenerated.Location = new System.Drawing.Point(84, 115);
            this.courtesyGenerated.Name = "courtesyGenerated";
            this.courtesyGenerated.Size = new System.Drawing.Size(103, 17);
            this.courtesyGenerated.TabIndex = 5;
            this.courtesyGenerated.TabStop = true;
            this.courtesyGenerated.Text = "Generated Tone";
            this.courtesyGenerated.UseVisualStyleBackColor = true;
            // 
            // courtesyToneDurationTextBox
            // 
            this.courtesyToneDurationTextBox.Location = new System.Drawing.Point(84, 69);
            this.courtesyToneDurationTextBox.Name = "courtesyToneDurationTextBox";
            this.courtesyToneDurationTextBox.Size = new System.Drawing.Size(47, 20);
            this.courtesyToneDurationTextBox.TabIndex = 9;
            // 
            // courtesyTonePitchTextBox
            // 
            this.courtesyTonePitchTextBox.Location = new System.Drawing.Point(84, 43);
            this.courtesyTonePitchTextBox.Name = "courtesyTonePitchTextBox";
            this.courtesyTonePitchTextBox.Size = new System.Drawing.Size(47, 20);
            this.courtesyTonePitchTextBox.TabIndex = 8;
            // 
            // courtesyToneDelayTextBox
            // 
            this.courtesyToneDelayTextBox.Location = new System.Drawing.Point(84, 17);
            this.courtesyToneDelayTextBox.Name = "courtesyToneDelayTextBox";
            this.courtesyToneDelayTextBox.Size = new System.Drawing.Size(47, 20);
            this.courtesyToneDelayTextBox.TabIndex = 7;
            // 
            // labelCourtesyDuraion
            // 
            this.labelCourtesyDuraion.AutoSize = true;
            this.labelCourtesyDuraion.Location = new System.Drawing.Point(6, 72);
            this.labelCourtesyDuraion.Name = "labelCourtesyDuraion";
            this.labelCourtesyDuraion.Size = new System.Drawing.Size(72, 13);
            this.labelCourtesyDuraion.TabIndex = 2;
            this.labelCourtesyDuraion.Text = "Duration (ms):";
            // 
            // labelCourtesyPitch
            // 
            this.labelCourtesyPitch.AutoSize = true;
            this.labelCourtesyPitch.Location = new System.Drawing.Point(6, 46);
            this.labelCourtesyPitch.Name = "labelCourtesyPitch";
            this.labelCourtesyPitch.Size = new System.Drawing.Size(56, 13);
            this.labelCourtesyPitch.TabIndex = 1;
            this.labelCourtesyPitch.Text = "Pitch (Hz):";
            // 
            // labelCourtesyDelay
            // 
            this.labelCourtesyDelay.AutoSize = true;
            this.labelCourtesyDelay.Location = new System.Drawing.Point(6, 20);
            this.labelCourtesyDelay.Name = "labelCourtesyDelay";
            this.labelCourtesyDelay.Size = new System.Drawing.Size(59, 13);
            this.labelCourtesyDelay.TabIndex = 0;
            this.labelCourtesyDelay.Text = "Delay (ms):";
            // 
            // tailTimerTextBox
            // 
            this.tailTimerTextBox.Location = new System.Drawing.Point(68, 32);
            this.tailTimerTextBox.Name = "tailTimerTextBox";
            this.tailTimerTextBox.Size = new System.Drawing.Size(47, 20);
            this.tailTimerTextBox.TabIndex = 3;
            // 
            // labelTailTimer
            // 
            this.labelTailTimer.AutoSize = true;
            this.labelTailTimer.Location = new System.Drawing.Point(6, 35);
            this.labelTailTimer.Name = "labelTailTimer";
            this.labelTailTimer.Size = new System.Drawing.Size(56, 13);
            this.labelTailTimer.TabIndex = 2;
            this.labelTailTimer.Text = "Tail Timer:";
            // 
            // maxTransmissionTimeTextBox
            // 
            this.maxTransmissionTimeTextBox.Location = new System.Drawing.Point(135, 6);
            this.maxTransmissionTimeTextBox.Name = "maxTransmissionTimeTextBox";
            this.maxTransmissionTimeTextBox.Size = new System.Drawing.Size(57, 20);
            this.maxTransmissionTimeTextBox.TabIndex = 2;
            // 
            // labelMaxTransmissionTime
            // 
            this.labelMaxTransmissionTime.AutoSize = true;
            this.labelMaxTransmissionTime.Location = new System.Drawing.Point(6, 9);
            this.labelMaxTransmissionTime.Name = "labelMaxTransmissionTime";
            this.labelMaxTransmissionTime.Size = new System.Drawing.Size(123, 13);
            this.labelMaxTransmissionTime.TabIndex = 0;
            this.labelMaxTransmissionTime.Text = "Max. Transmission Time:";
            // 
            // tabAnnc
            // 
            this.tabAnnc.Controls.Add(this.checkBoxAnncSysName);
            this.tabAnnc.Controls.Add(this.textBoxSysName);
            this.tabAnnc.Controls.Add(this.labelSysName);
            this.tabAnnc.Controls.Add(this.textBoxAnncAudioGain);
            this.tabAnnc.Controls.Add(this.labelAnncAudioGain);
            this.tabAnnc.Controls.Add(this.comboBoxAnncVoice);
            this.tabAnnc.Controls.Add(this.labelAnncVoice);
            this.tabAnnc.Controls.Add(this.checkBoxDisableAnnc);
            this.tabAnnc.Location = new System.Drawing.Point(4, 22);
            this.tabAnnc.Name = "tabAnnc";
            this.tabAnnc.Size = new System.Drawing.Size(549, 207);
            this.tabAnnc.TabIndex = 4;
            this.tabAnnc.Text = "Announcements";
            this.tabAnnc.UseVisualStyleBackColor = true;
            // 
            // checkBoxAnncSysName
            // 
            this.checkBoxAnncSysName.AutoSize = true;
            this.checkBoxAnncSysName.Location = new System.Drawing.Point(11, 115);
            this.checkBoxAnncSysName.Name = "checkBoxAnncSysName";
            this.checkBoxAnncSysName.Size = new System.Drawing.Size(143, 17);
            this.checkBoxAnncSysName.TabIndex = 12;
            this.checkBoxAnncSysName.Text = "Announce System Name";
            this.toolTip.SetToolTip(this.checkBoxAnncSysName, "Should the System Name be used during time announcements.");
            this.checkBoxAnncSysName.UseVisualStyleBackColor = true;
            // 
            // textBoxSysName
            // 
            this.textBoxSysName.Location = new System.Drawing.Point(89, 89);
            this.textBoxSysName.Name = "textBoxSysName";
            this.textBoxSysName.Size = new System.Drawing.Size(139, 20);
            this.textBoxSysName.TabIndex = 11;
            // 
            // labelSysName
            // 
            this.labelSysName.AutoSize = true;
            this.labelSysName.Location = new System.Drawing.Point(8, 92);
            this.labelSysName.Name = "labelSysName";
            this.labelSysName.Size = new System.Drawing.Size(75, 13);
            this.labelSysName.TabIndex = 10;
            this.labelSysName.Text = "System Name:";
            // 
            // textBoxAnncAudioGain
            // 
            this.textBoxAnncAudioGain.Location = new System.Drawing.Point(76, 63);
            this.textBoxAnncAudioGain.Name = "textBoxAnncAudioGain";
            this.textBoxAnncAudioGain.Size = new System.Drawing.Size(57, 20);
            this.textBoxAnncAudioGain.TabIndex = 9;
            // 
            // labelAnncAudioGain
            // 
            this.labelAnncAudioGain.AutoSize = true;
            this.labelAnncAudioGain.Location = new System.Drawing.Point(8, 66);
            this.labelAnncAudioGain.Name = "labelAnncAudioGain";
            this.labelAnncAudioGain.Size = new System.Drawing.Size(62, 13);
            this.labelAnncAudioGain.TabIndex = 8;
            this.labelAnncAudioGain.Text = "Audio Gain:";
            // 
            // comboBoxAnncVoice
            // 
            this.comboBoxAnncVoice.FormattingEnabled = true;
            this.comboBoxAnncVoice.Location = new System.Drawing.Point(107, 36);
            this.comboBoxAnncVoice.Name = "comboBoxAnncVoice";
            this.comboBoxAnncVoice.Size = new System.Drawing.Size(121, 21);
            this.comboBoxAnncVoice.TabIndex = 7;
            // 
            // labelAnncVoice
            // 
            this.labelAnncVoice.AutoSize = true;
            this.labelAnncVoice.Location = new System.Drawing.Point(9, 39);
            this.labelAnncVoice.Name = "labelAnncVoice";
            this.labelAnncVoice.Size = new System.Drawing.Size(92, 13);
            this.labelAnncVoice.TabIndex = 6;
            this.labelAnncVoice.Text = "Announcer Voice:";
            // 
            // checkBoxDisableAnnc
            // 
            this.checkBoxDisableAnnc.AutoSize = true;
            this.checkBoxDisableAnnc.Location = new System.Drawing.Point(12, 13);
            this.checkBoxDisableAnnc.Name = "checkBoxDisableAnnc";
            this.checkBoxDisableAnnc.Size = new System.Drawing.Size(141, 17);
            this.checkBoxDisableAnnc.TabIndex = 5;
            this.checkBoxDisableAnnc.Text = "Disable Announcements";
            this.checkBoxDisableAnnc.UseVisualStyleBackColor = true;
            // 
            // tabLogging
            // 
            this.tabLogging.Controls.Add(this.groupBoxLogging);
            this.tabLogging.Controls.Add(this.checkBoxDbLogging);
            this.tabLogging.Location = new System.Drawing.Point(4, 22);
            this.tabLogging.Name = "tabLogging";
            this.tabLogging.Padding = new System.Windows.Forms.Padding(3);
            this.tabLogging.Size = new System.Drawing.Size(549, 207);
            this.tabLogging.TabIndex = 5;
            this.tabLogging.Text = "Logging";
            this.tabLogging.UseVisualStyleBackColor = true;
            // 
            // groupBoxLogging
            // 
            this.groupBoxLogging.Controls.Add(this.textBoxDbPassword);
            this.groupBoxLogging.Controls.Add(this.labelDbPassword);
            this.groupBoxLogging.Controls.Add(this.textBoxDbUsername);
            this.groupBoxLogging.Controls.Add(this.labelDbUsername);
            this.groupBoxLogging.Controls.Add(this.textBoxDbServer);
            this.groupBoxLogging.Controls.Add(this.labelDbServer);
            this.groupBoxLogging.Controls.Add(this.textBoxDbName);
            this.groupBoxLogging.Controls.Add(this.labelDbName);
            this.groupBoxLogging.Location = new System.Drawing.Point(6, 29);
            this.groupBoxLogging.Name = "groupBoxLogging";
            this.groupBoxLogging.Size = new System.Drawing.Size(287, 131);
            this.groupBoxLogging.TabIndex = 1;
            this.groupBoxLogging.TabStop = false;
            this.groupBoxLogging.Text = "MySQL Settings";
            // 
            // textBoxDbPassword
            // 
            this.textBoxDbPassword.Location = new System.Drawing.Point(99, 103);
            this.textBoxDbPassword.Name = "textBoxDbPassword";
            this.textBoxDbPassword.PasswordChar = '*';
            this.textBoxDbPassword.Size = new System.Drawing.Size(182, 20);
            this.textBoxDbPassword.TabIndex = 7;
            // 
            // labelDbPassword
            // 
            this.labelDbPassword.AutoSize = true;
            this.labelDbPassword.Location = new System.Drawing.Point(6, 106);
            this.labelDbPassword.Name = "labelDbPassword";
            this.labelDbPassword.Size = new System.Drawing.Size(56, 13);
            this.labelDbPassword.TabIndex = 6;
            this.labelDbPassword.Text = "Password:";
            // 
            // textBoxDbUsername
            // 
            this.textBoxDbUsername.Location = new System.Drawing.Point(99, 77);
            this.textBoxDbUsername.Name = "textBoxDbUsername";
            this.textBoxDbUsername.Size = new System.Drawing.Size(182, 20);
            this.textBoxDbUsername.TabIndex = 5;
            // 
            // labelDbUsername
            // 
            this.labelDbUsername.AutoSize = true;
            this.labelDbUsername.Location = new System.Drawing.Point(6, 80);
            this.labelDbUsername.Name = "labelDbUsername";
            this.labelDbUsername.Size = new System.Drawing.Size(58, 13);
            this.labelDbUsername.TabIndex = 4;
            this.labelDbUsername.Text = "Username:";
            // 
            // textBoxDbServer
            // 
            this.textBoxDbServer.Location = new System.Drawing.Point(99, 39);
            this.textBoxDbServer.Name = "textBoxDbServer";
            this.textBoxDbServer.Size = new System.Drawing.Size(182, 20);
            this.textBoxDbServer.TabIndex = 3;
            // 
            // labelDbServer
            // 
            this.labelDbServer.AutoSize = true;
            this.labelDbServer.Location = new System.Drawing.Point(6, 42);
            this.labelDbServer.Name = "labelDbServer";
            this.labelDbServer.Size = new System.Drawing.Size(90, 13);
            this.labelDbServer.TabIndex = 2;
            this.labelDbServer.Text = "Database Server:";
            // 
            // textBoxDbName
            // 
            this.textBoxDbName.Location = new System.Drawing.Point(99, 13);
            this.textBoxDbName.Name = "textBoxDbName";
            this.textBoxDbName.Size = new System.Drawing.Size(182, 20);
            this.textBoxDbName.TabIndex = 1;
            // 
            // labelDbName
            // 
            this.labelDbName.AutoSize = true;
            this.labelDbName.Location = new System.Drawing.Point(6, 16);
            this.labelDbName.Name = "labelDbName";
            this.labelDbName.Size = new System.Drawing.Size(87, 13);
            this.labelDbName.TabIndex = 0;
            this.labelDbName.Text = "Database Name:";
            // 
            // checkBoxDbLogging
            // 
            this.checkBoxDbLogging.AutoSize = true;
            this.checkBoxDbLogging.Location = new System.Drawing.Point(6, 6);
            this.checkBoxDbLogging.Name = "checkBoxDbLogging";
            this.checkBoxDbLogging.Size = new System.Drawing.Size(149, 17);
            this.checkBoxDbLogging.TabIndex = 0;
            this.checkBoxDbLogging.Text = "Enable Database Logging";
            this.checkBoxDbLogging.UseVisualStyleBackColor = true;
            // 
            // checkBoxMDCOnly
            // 
            this.checkBoxMDCOnly.AutoSize = true;
            this.checkBoxMDCOnly.Location = new System.Drawing.Point(12, 255);
            this.checkBoxMDCOnly.Name = "checkBoxMDCOnly";
            this.checkBoxMDCOnly.Size = new System.Drawing.Size(115, 17);
            this.checkBoxMDCOnly.TabIndex = 2;
            this.checkBoxMDCOnly.Text = "MDC Console Only";
            this.toolTip.SetToolTip(this.checkBoxMDCOnly, "Puts RepeaterController into a special mode that allows it to function as an MDC " +
        "dispatch console.");
            this.checkBoxMDCOnly.UseVisualStyleBackColor = true;
            // 
            // checkBoxConsoleAnncDTMF
            // 
            this.checkBoxConsoleAnncDTMF.AutoSize = true;
            this.checkBoxConsoleAnncDTMF.Location = new System.Drawing.Point(133, 255);
            this.checkBoxConsoleAnncDTMF.Name = "checkBoxConsoleAnncDTMF";
            this.checkBoxConsoleAnncDTMF.Size = new System.Drawing.Size(166, 17);
            this.checkBoxConsoleAnncDTMF.TabIndex = 4;
            this.checkBoxConsoleAnncDTMF.Text = "Allow Console Announcments";
            this.toolTip.SetToolTip(this.checkBoxConsoleAnncDTMF, "Puts RepeaterController into \"Link Radio Mode\" where it will act as an MDC Consol" +
        "e with Announcement and DTMF Command capabilities.");
            this.checkBoxConsoleAnncDTMF.UseVisualStyleBackColor = true;
            // 
            // RepeaterOptions
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.okButton;
            this.ClientSize = new System.Drawing.Size(581, 285);
            this.Controls.Add(this.checkBoxConsoleAnncDTMF);
            this.Controls.Add(this.checkBoxMDCOnly);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.okButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RepeaterOptions";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Repeater Options";
            this.tabControl.ResumeLayout(false);
            this.tabIdOptions.ResumeLayout(false);
            this.groupBoxMdcSettings.ResumeLayout(false);
            this.groupBoxMdcSettings.PerformLayout();
            this.groupBoxRepeaterId.ResumeLayout(false);
            this.groupBoxRepeaterId.PerformLayout();
            this.tabRxOptions.ResumeLayout(false);
            this.tabRxOptions.PerformLayout();
            this.rxPlDplGroupBox.ResumeLayout(false);
            this.rxPlDplGroupBox.PerformLayout();
            this.groupBoxRxControlPin.ResumeLayout(false);
            this.groupBoxRxControlPin.PerformLayout();
            this.tabTxOptions.ResumeLayout(false);
            this.tabTxOptions.PerformLayout();
            this.txPlDplGroupBox.ResumeLayout(false);
            this.txPlDplGroupBox.PerformLayout();
            this.groupBoxTxControlPin.ResumeLayout(false);
            this.groupBoxTxControlPin.PerformLayout();
            this.tabTimers.ResumeLayout(false);
            this.tabTimers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDtmfDigitDelay)).EndInit();
            this.groupBoxCourtesyTone.ResumeLayout(false);
            this.groupBoxCourtesyTone.PerformLayout();
            this.tabAnnc.ResumeLayout(false);
            this.tabAnnc.PerformLayout();
            this.tabLogging.ResumeLayout(false);
            this.tabLogging.PerformLayout();
            this.groupBoxLogging.ResumeLayout(false);
            this.groupBoxLogging.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabIdOptions;
        private System.Windows.Forms.TabPage tabRxOptions;
        private System.Windows.Forms.GroupBox groupBoxMdcSettings;
        private System.Windows.Forms.Label labelMDCPreambles;
        private System.Windows.Forms.TextBox numOfPreamblesTextBox;
        private System.Windows.Forms.GroupBox groupBoxRepeaterId;
        private System.Windows.Forms.TextBox callsignTextBox;
        private System.Windows.Forms.Label labelCallsign;
        private System.Windows.Forms.TextBox idIntervalTextBox;
        private System.Windows.Forms.Label labelIdInterval;
        private System.Windows.Forms.TabPage tabTxOptions;
        private System.Windows.Forms.GroupBox groupBoxRxControlPin;
        private System.Windows.Forms.RadioButton rxControlPinDSR;
        private System.Windows.Forms.RadioButton rxControlPinCTS;
        private System.Windows.Forms.RadioButton rxControlPinCD;
        private System.Windows.Forms.ComboBox comboBoxRxSerialPort;
        private VolumeSlider voxVolumeSlider;
        private System.Windows.Forms.Label labelRxControl;
        private System.Windows.Forms.GroupBox groupBoxTxControlPin;
        private System.Windows.Forms.RadioButton txControlPinBoth;
        private System.Windows.Forms.RadioButton txControlPinDTR;
        private System.Windows.Forms.RadioButton txControlPinRTS;
        private System.Windows.Forms.ComboBox comboBoxTxSerialPort;
        private System.Windows.Forms.Label labelTransmitterControl;
        private System.Windows.Forms.TabPage tabTimers;
        private System.Windows.Forms.GroupBox groupBoxCourtesyTone;
        private System.Windows.Forms.TextBox courtesyTonePitchTextBox;
        private System.Windows.Forms.TextBox courtesyToneDelayTextBox;
        private System.Windows.Forms.Label labelCourtesyDuraion;
        private System.Windows.Forms.Label labelCourtesyPitch;
        private System.Windows.Forms.Label labelCourtesyDelay;
        private System.Windows.Forms.TextBox tailTimerTextBox;
        private System.Windows.Forms.Label labelTailTimer;
        private System.Windows.Forms.TextBox maxTransmissionTimeTextBox;
        private System.Windows.Forms.Label labelMaxTransmissionTime;
        private System.Windows.Forms.Button openCourtesyToneFile;
        private System.Windows.Forms.TextBox courtesyToneFileTextBox;
        private System.Windows.Forms.Label labelCourtesyFilename;
        private System.Windows.Forms.RadioButton courtesyPlayback;
        private System.Windows.Forms.RadioButton courtesyGenerated;
        private System.Windows.Forms.TextBox courtesyToneDurationTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxNoTone;
        private System.Windows.Forms.CheckBox checkBoxAutoAckEmerg;
        private System.Windows.Forms.CheckBox checkBoxUseRAC;
        private System.Windows.Forms.TextBox racTextBox;
        private System.Windows.Forms.Label labelRAC;
        private System.Windows.Forms.TextBox myIdTextBox;
        private System.Windows.Forms.Label labelMyID;
        private System.Windows.Forms.CheckBox checkBoxMDCOnly;
        private System.Windows.Forms.CheckBox checkBoxDisableId;
        private System.Windows.Forms.TextBox watchDogTextBox;
        private System.Windows.Forms.Label labelWatchDog;
        private System.Windows.Forms.TabPage tabAnnc;
        private System.Windows.Forms.ComboBox comboBoxAnncVoice;
        private System.Windows.Forms.Label labelAnncVoice;
        private System.Windows.Forms.CheckBox checkBoxDisableAnnc;
        private System.Windows.Forms.CheckBox checkBoxDisableIdStartup;
        private System.Windows.Forms.CheckBox checkBoxConsoleAnncDTMF;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TextBox textBoxAnncAudioGain;
        private System.Windows.Forms.Label labelAnncAudioGain;
        private System.Windows.Forms.Button openMultiTone;
        private System.Windows.Forms.ComboBox comboBoxRxPL;
        private System.Windows.Forms.Label labelRxPL;
        private System.Windows.Forms.ComboBox comboBoxTxPL;
        private System.Windows.Forms.Label labelTxPL;
        private System.Windows.Forms.CheckBox checkBoxRxPL;
        private System.Windows.Forms.CheckBox checkBoxTxPL;
        private System.Windows.Forms.Button buttonTestCourtesy;
        private System.Windows.Forms.CheckBox checkBoxAnncSysName;
        private System.Windows.Forms.TextBox textBoxSysName;
        private System.Windows.Forms.Label labelSysName;
        private System.Windows.Forms.CheckBox checkBoxUseRxDPL;
        private System.Windows.Forms.Label labelDPL;
        private System.Windows.Forms.CheckBox checkBoxUseTxDPL;
        private System.Windows.Forms.Label labelTxDPL;
        private System.Windows.Forms.ComboBox comboBoxRxDPL;
        private System.Windows.Forms.ComboBox comboBoxTxDPL;
        private System.Windows.Forms.CheckBox checkBoxDisablePLForId;
        private System.Windows.Forms.TextBox textBoxTxPLGain;
        private System.Windows.Forms.Label labelTxPLGain;
        private System.Windows.Forms.GroupBox rxPlDplGroupBox;
        private System.Windows.Forms.GroupBox txPlDplGroupBox;
        private System.Windows.Forms.TextBox pttTransmitTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabLogging;
        private System.Windows.Forms.GroupBox groupBoxLogging;
        private System.Windows.Forms.TextBox textBoxDbPassword;
        private System.Windows.Forms.Label labelDbPassword;
        private System.Windows.Forms.TextBox textBoxDbUsername;
        private System.Windows.Forms.Label labelDbUsername;
        private System.Windows.Forms.TextBox textBoxDbServer;
        private System.Windows.Forms.Label labelDbServer;
        private System.Windows.Forms.TextBox textBoxDbName;
        private System.Windows.Forms.Label labelDbName;
        private System.Windows.Forms.CheckBox checkBoxDbLogging;
        private System.Windows.Forms.Label labelDtmfDigitTime;
        private System.Windows.Forms.NumericUpDown numericUpDownDtmfDigitDelay;
    }
}