/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 * 
 * $created guid: f39f7ca1-2851-404f-83f1-4107feba6cc5 2012/1/11$
 */
namespace RepeaterController
{
    /// <summary>
    /// This class serves as the main interface for the RepeaterController application.
    /// </summary>
    public partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.advancedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rawMDCEncodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.setAnnouncementEveryHourTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testButtonDoNotPressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureAudioDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.repeaterOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.announcementsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dtmfCommandsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.decodeEncodedPacketsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripDisableFilteringLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripMDCConsoleLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripPLLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripRACLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripTransmitLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.transmitTimeAnnc = new System.Windows.Forms.Button();
            this.txDuration = new System.Windows.Forms.Label();
            this.txDurationLabel = new System.Windows.Forms.Label();
            this.buttonTestAnnounce = new System.Windows.Forms.Button();
            this.clearTimeoutButton = new System.Windows.Forms.Button();
            this.groupBoxStatus = new System.Windows.Forms.GroupBox();
            this.lastDPL = new System.Windows.Forms.Label();
            this.labelLastDPL = new System.Windows.Forms.Label();
            this.lastPl = new System.Windows.Forms.Label();
            this.labelLastPL = new System.Windows.Forms.Label();
            this.lastId = new System.Windows.Forms.Label();
            this.lastTransmissionLabel = new System.Windows.Forms.Label();
            this.labelLastId = new System.Windows.Forms.Label();
            this.labelLastTransmission = new System.Windows.Forms.Label();
            this.generateMDCGroupBox = new System.Windows.Forms.GroupBox();
            this.racButton = new System.Windows.Forms.Button();
            this.emergAck = new System.Windows.Forms.Button();
            this.messageButton = new System.Windows.Forms.Button();
            this.statusButton = new System.Windows.Forms.Button();
            this.selectiveCallButton = new System.Windows.Forms.Button();
            this.callAlertButton = new System.Windows.Forms.Button();
            this.radioCheckButton = new System.Windows.Forms.Button();
            this.reviveButton = new System.Windows.Forms.Button();
            this.stunButton = new System.Windows.Forms.Button();
            this.txToolPTT = new System.Windows.Forms.Button();
            this.disableTransmitterButton = new System.Windows.Forms.Button();
            this.activityLog = new System.Windows.Forms.ListBox();
            this.labelTxAudioLevel = new System.Windows.Forms.Label();
            this.captureLogLabel = new System.Windows.Forms.Label();
            this.labelRxAudioLevel = new System.Windows.Forms.Label();
            this.targetMDCID = new System.Windows.Forms.TextBox();
            this.txVolumeMeter = new RepeaterController.VolumeMeter();
            this.targetIDLabel = new System.Windows.Forms.Label();
            this.rxVolumeMeter = new RepeaterController.VolumeMeter();
            this.transmitCallsignCW = new System.Windows.Forms.Button();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripRepeaterOptionsButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripConfigureAudioButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripAnnouncementsButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripDtmfCommandsButton = new System.Windows.Forms.ToolStripButton();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.sysCallsign = new System.Windows.Forms.Label();
            this.labelCallsign = new System.Windows.Forms.Label();
            this.mainMenuStrip.SuspendLayout();
            this.logContextMenu.SuspendLayout();
            this.toolStripContainer.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.groupBoxStatus.SuspendLayout();
            this.generateMDCGroupBox.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.advancedToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(671, 24);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveSettingsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveSettingsToolStripMenuItem
            // 
            this.saveSettingsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveSettingsToolStripMenuItem.Image")));
            this.saveSettingsToolStripMenuItem.Name = "saveSettingsToolStripMenuItem";
            this.saveSettingsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveSettingsToolStripMenuItem.Text = "Save Settings...";
            this.saveSettingsToolStripMenuItem.Click += new System.EventHandler(this.saveSettingsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exitToolStripMenuItem.Image")));
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // advancedToolStripMenuItem
            // 
            this.advancedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rawMDCEncodeToolStripMenuItem,
            this.toolStripMenuItem1,
            this.setAnnouncementEveryHourTimeToolStripMenuItem,
            this.testButtonDoNotPressToolStripMenuItem});
            this.advancedToolStripMenuItem.Name = "advancedToolStripMenuItem";
            this.advancedToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.advancedToolStripMenuItem.Text = "Advanced";
            // 
            // rawMDCEncodeToolStripMenuItem
            // 
            this.rawMDCEncodeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("rawMDCEncodeToolStripMenuItem.Image")));
            this.rawMDCEncodeToolStripMenuItem.Name = "rawMDCEncodeToolStripMenuItem";
            this.rawMDCEncodeToolStripMenuItem.Size = new System.Drawing.Size(282, 22);
            this.rawMDCEncodeToolStripMenuItem.Text = "Raw MDC Encode...";
            this.rawMDCEncodeToolStripMenuItem.Click += new System.EventHandler(this.rawMDCEncodeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(279, 6);
            // 
            // setAnnouncementEveryHourTimeToolStripMenuItem
            // 
            this.setAnnouncementEveryHourTimeToolStripMenuItem.Name = "setAnnouncementEveryHourTimeToolStripMenuItem";
            this.setAnnouncementEveryHourTimeToolStripMenuItem.Size = new System.Drawing.Size(282, 22);
            this.setAnnouncementEveryHourTimeToolStripMenuItem.Text = "Set Announcement \'Every Hour\' Time...";
            this.setAnnouncementEveryHourTimeToolStripMenuItem.Click += new System.EventHandler(this.setAnnouncementEveryHourTimeToolStripMenuItem_Click);
            // 
            // testButtonDoNotPressToolStripMenuItem
            // 
            this.testButtonDoNotPressToolStripMenuItem.Name = "testButtonDoNotPressToolStripMenuItem";
            this.testButtonDoNotPressToolStripMenuItem.Size = new System.Drawing.Size(282, 22);
            this.testButtonDoNotPressToolStripMenuItem.Text = "Test Button -- Do Not Press...";
            this.testButtonDoNotPressToolStripMenuItem.Click += new System.EventHandler(this.testButtonDoNotPressToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configureAudioDeviceToolStripMenuItem,
            this.repeaterOptionsToolStripMenuItem,
            this.toolStripSeparator1,
            this.announcementsToolStripMenuItem,
            this.dtmfCommandsToolStripMenuItem,
            this.toolStripSeparator2,
            this.decodeEncodedPacketsToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // configureAudioDeviceToolStripMenuItem
            // 
            this.configureAudioDeviceToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("configureAudioDeviceToolStripMenuItem.Image")));
            this.configureAudioDeviceToolStripMenuItem.Name = "configureAudioDeviceToolStripMenuItem";
            this.configureAudioDeviceToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.configureAudioDeviceToolStripMenuItem.Text = "Configure Audio Device...";
            this.configureAudioDeviceToolStripMenuItem.Click += new System.EventHandler(this.configureAudioDeviceToolStripMenuItem_Click);
            // 
            // repeaterOptionsToolStripMenuItem
            // 
            this.repeaterOptionsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("repeaterOptionsToolStripMenuItem.Image")));
            this.repeaterOptionsToolStripMenuItem.Name = "repeaterOptionsToolStripMenuItem";
            this.repeaterOptionsToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.repeaterOptionsToolStripMenuItem.Text = "Repeater Options...";
            this.repeaterOptionsToolStripMenuItem.Click += new System.EventHandler(this.repeaterOptionsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(206, 6);
            // 
            // announcementsToolStripMenuItem
            // 
            this.announcementsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("announcementsToolStripMenuItem.Image")));
            this.announcementsToolStripMenuItem.Name = "announcementsToolStripMenuItem";
            this.announcementsToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.announcementsToolStripMenuItem.Text = "Announcements...";
            this.announcementsToolStripMenuItem.Click += new System.EventHandler(this.announcementsToolStripMenuItem_Click);
            // 
            // dtmfCommandsToolStripMenuItem
            // 
            this.dtmfCommandsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("dtmfCommandsToolStripMenuItem.Image")));
            this.dtmfCommandsToolStripMenuItem.Name = "dtmfCommandsToolStripMenuItem";
            this.dtmfCommandsToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.dtmfCommandsToolStripMenuItem.Text = "DTMF Commands...";
            this.dtmfCommandsToolStripMenuItem.Click += new System.EventHandler(this.dtmfCommandsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(206, 6);
            // 
            // decodeEncodedPacketsToolStripMenuItem
            // 
            this.decodeEncodedPacketsToolStripMenuItem.Checked = true;
            this.decodeEncodedPacketsToolStripMenuItem.CheckOnClick = true;
            this.decodeEncodedPacketsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.decodeEncodedPacketsToolStripMenuItem.Name = "decodeEncodedPacketsToolStripMenuItem";
            this.decodeEncodedPacketsToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.decodeEncodedPacketsToolStripMenuItem.Text = "Decode Encoded Packets";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // logContextMenu
            // 
            this.logContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem});
            this.logContextMenu.Name = "rxPacketsContextMenu";
            this.logContextMenu.Size = new System.Drawing.Size(139, 26);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("clearToolStripMenuItem.Image")));
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.clearToolStripMenuItem.Text = "Clear Logs...";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.BottomToolStripPanel
            // 
            this.toolStripContainer.BottomToolStripPanel.Controls.Add(this.statusStrip);
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.Controls.Add(this.transmitTimeAnnc);
            this.toolStripContainer.ContentPanel.Controls.Add(this.txDuration);
            this.toolStripContainer.ContentPanel.Controls.Add(this.txDurationLabel);
            this.toolStripContainer.ContentPanel.Controls.Add(this.buttonTestAnnounce);
            this.toolStripContainer.ContentPanel.Controls.Add(this.clearTimeoutButton);
            this.toolStripContainer.ContentPanel.Controls.Add(this.groupBoxStatus);
            this.toolStripContainer.ContentPanel.Controls.Add(this.generateMDCGroupBox);
            this.toolStripContainer.ContentPanel.Controls.Add(this.disableTransmitterButton);
            this.toolStripContainer.ContentPanel.Controls.Add(this.activityLog);
            this.toolStripContainer.ContentPanel.Controls.Add(this.labelTxAudioLevel);
            this.toolStripContainer.ContentPanel.Controls.Add(this.captureLogLabel);
            this.toolStripContainer.ContentPanel.Controls.Add(this.labelRxAudioLevel);
            this.toolStripContainer.ContentPanel.Controls.Add(this.targetMDCID);
            this.toolStripContainer.ContentPanel.Controls.Add(this.txVolumeMeter);
            this.toolStripContainer.ContentPanel.Controls.Add(this.targetIDLabel);
            this.toolStripContainer.ContentPanel.Controls.Add(this.rxVolumeMeter);
            this.toolStripContainer.ContentPanel.Controls.Add(this.transmitCallsignCW);
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(671, 416);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.Location = new System.Drawing.Point(0, 24);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.Size = new System.Drawing.Size(671, 465);
            this.toolStripContainer.TabIndex = 46;
            this.toolStripContainer.Text = "toolStripContainer1";
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolStrip);
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // statusStrip
            // 
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDisableFilteringLabel,
            this.toolStripMDCConsoleLabel,
            this.toolStripPLLabel,
            this.toolStripRACLabel,
            this.toolStripStatusLabel,
            this.toolStripTransmitLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(671, 24);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 0;
            // 
            // toolStripDisableFilteringLabel
            // 
            this.toolStripDisableFilteringLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripDisableFilteringLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripDisableFilteringLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripDisableFilteringLabel.ForeColor = System.Drawing.Color.DarkBlue;
            this.toolStripDisableFilteringLabel.Name = "toolStripDisableFilteringLabel";
            this.toolStripDisableFilteringLabel.Size = new System.Drawing.Size(111, 19);
            this.toolStripDisableFilteringLabel.Text = "No Audio Filtering";
            // 
            // toolStripMDCConsoleLabel
            // 
            this.toolStripMDCConsoleLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripMDCConsoleLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripMDCConsoleLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMDCConsoleLabel.ForeColor = System.Drawing.Color.DarkBlue;
            this.toolStripMDCConsoleLabel.Name = "toolStripMDCConsoleLabel";
            this.toolStripMDCConsoleLabel.Size = new System.Drawing.Size(84, 19);
            this.toolStripMDCConsoleLabel.Text = "MDC Console";
            // 
            // toolStripPLLabel
            // 
            this.toolStripPLLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripPLLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripPLLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripPLLabel.ForeColor = System.Drawing.Color.DarkRed;
            this.toolStripPLLabel.Name = "toolStripPLLabel";
            this.toolStripPLLabel.Size = new System.Drawing.Size(24, 19);
            this.toolStripPLLabel.Text = "PL";
            // 
            // toolStripRACLabel
            // 
            this.toolStripRACLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripRACLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripRACLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripRACLabel.ForeColor = System.Drawing.Color.DarkRed;
            this.toolStripRACLabel.Name = "toolStripRACLabel";
            this.toolStripRACLabel.Size = new System.Drawing.Size(34, 19);
            this.toolStripRACLabel.Text = "RAC";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripStatusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel.ForeColor = System.Drawing.Color.DarkRed;
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(58, 19);
            this.toolStripStatusLabel.Text = "Disabled";
            // 
            // toolStripTransmitLabel
            // 
            this.toolStripTransmitLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripTransmitLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripTransmitLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripTransmitLabel.Name = "toolStripTransmitLabel";
            this.toolStripTransmitLabel.Size = new System.Drawing.Size(81, 19);
            this.toolStripTransmitLabel.Text = "Transmitting";
            // 
            // transmitTimeAnnc
            // 
            this.transmitTimeAnnc.Image = ((System.Drawing.Image)(resources.GetObject("transmitTimeAnnc.Image")));
            this.transmitTimeAnnc.Location = new System.Drawing.Point(487, 332);
            this.transmitTimeAnnc.Name = "transmitTimeAnnc";
            this.transmitTimeAnnc.Size = new System.Drawing.Size(168, 23);
            this.transmitTimeAnnc.TabIndex = 79;
            this.transmitTimeAnnc.Text = "Transmit Time Annc";
            this.transmitTimeAnnc.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.transmitTimeAnnc.UseVisualStyleBackColor = true;
            this.transmitTimeAnnc.Click += new System.EventHandler(this.transmitTimeAnnc_Click);
            // 
            // txDuration
            // 
            this.txDuration.AutoSize = true;
            this.txDuration.Location = new System.Drawing.Point(104, 329);
            this.txDuration.Name = "txDuration";
            this.txDuration.Size = new System.Drawing.Size(18, 13);
            this.txDuration.TabIndex = 77;
            this.txDuration.Text = "0s";
            // 
            // txDurationLabel
            // 
            this.txDurationLabel.AutoSize = true;
            this.txDurationLabel.Location = new System.Drawing.Point(19, 329);
            this.txDurationLabel.Name = "txDurationLabel";
            this.txDurationLabel.Size = new System.Drawing.Size(65, 13);
            this.txDurationLabel.TabIndex = 76;
            this.txDurationLabel.Text = "Tx Duration:";
            // 
            // buttonTestAnnounce
            // 
            this.buttonTestAnnounce.Location = new System.Drawing.Point(487, 361);
            this.buttonTestAnnounce.Name = "buttonTestAnnounce";
            this.buttonTestAnnounce.Size = new System.Drawing.Size(81, 23);
            this.buttonTestAnnounce.TabIndex = 75;
            this.buttonTestAnnounce.Text = "Test Annc";
            this.buttonTestAnnounce.UseVisualStyleBackColor = true;
            this.buttonTestAnnounce.Click += new System.EventHandler(this.buttonTestAnnounce_Click);
            // 
            // clearTimeoutButton
            // 
            this.clearTimeoutButton.Location = new System.Drawing.Point(570, 361);
            this.clearTimeoutButton.Name = "clearTimeoutButton";
            this.clearTimeoutButton.Size = new System.Drawing.Size(85, 23);
            this.clearTimeoutButton.TabIndex = 74;
            this.clearTimeoutButton.Text = "Clear Timeout";
            this.clearTimeoutButton.UseVisualStyleBackColor = true;
            this.clearTimeoutButton.Click += new System.EventHandler(this.clearTimeoutButton_Click);
            // 
            // groupBoxStatus
            // 
            this.groupBoxStatus.Controls.Add(this.sysCallsign);
            this.groupBoxStatus.Controls.Add(this.labelCallsign);
            this.groupBoxStatus.Controls.Add(this.lastDPL);
            this.groupBoxStatus.Controls.Add(this.labelLastDPL);
            this.groupBoxStatus.Controls.Add(this.lastPl);
            this.groupBoxStatus.Controls.Add(this.labelLastPL);
            this.groupBoxStatus.Controls.Add(this.lastId);
            this.groupBoxStatus.Controls.Add(this.lastTransmissionLabel);
            this.groupBoxStatus.Controls.Add(this.labelLastId);
            this.groupBoxStatus.Controls.Add(this.labelLastTransmission);
            this.groupBoxStatus.Location = new System.Drawing.Point(228, 281);
            this.groupBoxStatus.Name = "groupBoxStatus";
            this.groupBoxStatus.Size = new System.Drawing.Size(247, 132);
            this.groupBoxStatus.TabIndex = 73;
            this.groupBoxStatus.TabStop = false;
            this.groupBoxStatus.Text = "Status";
            // 
            // lastDPL
            // 
            this.lastDPL.AutoSize = true;
            this.lastDPL.Location = new System.Drawing.Point(106, 82);
            this.lastDPL.Name = "lastDPL";
            this.lastDPL.Size = new System.Drawing.Size(44, 13);
            this.lastDPL.TabIndex = 45;
            this.lastDPL.Text = "lastDPL";
            // 
            // labelLastDPL
            // 
            this.labelLastDPL.AutoSize = true;
            this.labelLastDPL.Location = new System.Drawing.Point(7, 81);
            this.labelLastDPL.Name = "labelLastDPL";
            this.labelLastDPL.Size = new System.Drawing.Size(54, 13);
            this.labelLastDPL.TabIndex = 44;
            this.labelLastDPL.Text = "Last DPL:";
            // 
            // lastPl
            // 
            this.lastPl.AutoSize = true;
            this.lastPl.Location = new System.Drawing.Point(106, 65);
            this.lastPl.Name = "lastPl";
            this.lastPl.Size = new System.Drawing.Size(32, 13);
            this.lastPl.TabIndex = 43;
            this.lastPl.Text = "lastPl";
            // 
            // labelLastPL
            // 
            this.labelLastPL.AutoSize = true;
            this.labelLastPL.Location = new System.Drawing.Point(7, 64);
            this.labelLastPL.Name = "labelLastPL";
            this.labelLastPL.Size = new System.Drawing.Size(46, 13);
            this.labelLastPL.TabIndex = 42;
            this.labelLastPL.Text = "Last PL:";
            // 
            // lastId
            // 
            this.lastId.AutoSize = true;
            this.lastId.Location = new System.Drawing.Point(106, 49);
            this.lastId.Name = "lastId";
            this.lastId.Size = new System.Drawing.Size(32, 13);
            this.lastId.TabIndex = 41;
            this.lastId.Text = "lastId";
            // 
            // lastTransmissionLabel
            // 
            this.lastTransmissionLabel.AutoSize = true;
            this.lastTransmissionLabel.Location = new System.Drawing.Point(106, 31);
            this.lastTransmissionLabel.Name = "lastTransmissionLabel";
            this.lastTransmissionLabel.Size = new System.Drawing.Size(84, 13);
            this.lastTransmissionLabel.TabIndex = 40;
            this.lastTransmissionLabel.Text = "lastTransmission";
            // 
            // labelLastId
            // 
            this.labelLastId.AutoSize = true;
            this.labelLastId.Location = new System.Drawing.Point(7, 48);
            this.labelLastId.Name = "labelLastId";
            this.labelLastId.Size = new System.Drawing.Size(44, 13);
            this.labelLastId.TabIndex = 39;
            this.labelLastId.Text = "Last ID:";
            // 
            // labelLastTransmission
            // 
            this.labelLastTransmission.AutoSize = true;
            this.labelLastTransmission.Location = new System.Drawing.Point(6, 31);
            this.labelLastTransmission.Name = "labelLastTransmission";
            this.labelLastTransmission.Size = new System.Drawing.Size(94, 13);
            this.labelLastTransmission.TabIndex = 38;
            this.labelLastTransmission.Text = "Last Transmission:";
            // 
            // generateMDCGroupBox
            // 
            this.generateMDCGroupBox.Controls.Add(this.racButton);
            this.generateMDCGroupBox.Controls.Add(this.emergAck);
            this.generateMDCGroupBox.Controls.Add(this.messageButton);
            this.generateMDCGroupBox.Controls.Add(this.statusButton);
            this.generateMDCGroupBox.Controls.Add(this.selectiveCallButton);
            this.generateMDCGroupBox.Controls.Add(this.callAlertButton);
            this.generateMDCGroupBox.Controls.Add(this.radioCheckButton);
            this.generateMDCGroupBox.Controls.Add(this.reviveButton);
            this.generateMDCGroupBox.Controls.Add(this.stunButton);
            this.generateMDCGroupBox.Controls.Add(this.txToolPTT);
            this.generateMDCGroupBox.Location = new System.Drawing.Point(481, 21);
            this.generateMDCGroupBox.Name = "generateMDCGroupBox";
            this.generateMDCGroupBox.Size = new System.Drawing.Size(184, 225);
            this.generateMDCGroupBox.TabIndex = 64;
            this.generateMDCGroupBox.TabStop = false;
            this.generateMDCGroupBox.Text = "MDC1200 Encoder";
            // 
            // racButton
            // 
            this.racButton.Location = new System.Drawing.Point(6, 165);
            this.racButton.Name = "racButton";
            this.racButton.Size = new System.Drawing.Size(95, 23);
            this.racButton.TabIndex = 13;
            this.racButton.Text = "RAC";
            this.racButton.UseVisualStyleBackColor = true;
            this.racButton.Click += new System.EventHandler(this.racButton_Click);
            // 
            // emergAck
            // 
            this.emergAck.ForeColor = System.Drawing.Color.Teal;
            this.emergAck.Location = new System.Drawing.Point(6, 136);
            this.emergAck.Name = "emergAck";
            this.emergAck.Size = new System.Drawing.Size(168, 23);
            this.emergAck.TabIndex = 10;
            this.emergAck.Text = "Emergency Acknowledge";
            this.emergAck.UseVisualStyleBackColor = true;
            this.emergAck.Click += new System.EventHandler(this.emergAck_Click);
            // 
            // messageButton
            // 
            this.messageButton.Location = new System.Drawing.Point(107, 107);
            this.messageButton.Name = "messageButton";
            this.messageButton.Size = new System.Drawing.Size(67, 23);
            this.messageButton.TabIndex = 9;
            this.messageButton.Text = "Message";
            this.messageButton.UseVisualStyleBackColor = true;
            this.messageButton.Click += new System.EventHandler(this.messageButton_Click);
            // 
            // statusButton
            // 
            this.statusButton.Location = new System.Drawing.Point(6, 107);
            this.statusButton.Name = "statusButton";
            this.statusButton.Size = new System.Drawing.Size(95, 23);
            this.statusButton.TabIndex = 8;
            this.statusButton.Text = "Status";
            this.statusButton.UseVisualStyleBackColor = true;
            this.statusButton.Click += new System.EventHandler(this.statusButton_Click);
            // 
            // selectiveCallButton
            // 
            this.selectiveCallButton.Location = new System.Drawing.Point(107, 78);
            this.selectiveCallButton.Name = "selectiveCallButton";
            this.selectiveCallButton.Size = new System.Drawing.Size(67, 23);
            this.selectiveCallButton.TabIndex = 7;
            this.selectiveCallButton.Text = "SelCall";
            this.selectiveCallButton.UseVisualStyleBackColor = true;
            this.selectiveCallButton.Click += new System.EventHandler(this.selectiveCallButton_Click);
            // 
            // callAlertButton
            // 
            this.callAlertButton.Location = new System.Drawing.Point(6, 78);
            this.callAlertButton.Name = "callAlertButton";
            this.callAlertButton.Size = new System.Drawing.Size(95, 23);
            this.callAlertButton.TabIndex = 6;
            this.callAlertButton.Text = "Call Alert/Page";
            this.callAlertButton.UseVisualStyleBackColor = true;
            this.callAlertButton.Click += new System.EventHandler(this.callAlertButton_Click);
            // 
            // radioCheckButton
            // 
            this.radioCheckButton.Location = new System.Drawing.Point(6, 49);
            this.radioCheckButton.Name = "radioCheckButton";
            this.radioCheckButton.Size = new System.Drawing.Size(168, 23);
            this.radioCheckButton.TabIndex = 5;
            this.radioCheckButton.Text = "Radio Check";
            this.radioCheckButton.UseVisualStyleBackColor = true;
            this.radioCheckButton.Click += new System.EventHandler(this.radioCheckButton_Click);
            // 
            // reviveButton
            // 
            this.reviveButton.ForeColor = System.Drawing.Color.ForestGreen;
            this.reviveButton.Location = new System.Drawing.Point(89, 196);
            this.reviveButton.Name = "reviveButton";
            this.reviveButton.Size = new System.Drawing.Size(85, 23);
            this.reviveButton.TabIndex = 12;
            this.reviveButton.Text = "Revive Target";
            this.reviveButton.UseVisualStyleBackColor = true;
            this.reviveButton.Click += new System.EventHandler(this.reviveButton_Click);
            // 
            // stunButton
            // 
            this.stunButton.ForeColor = System.Drawing.Color.Firebrick;
            this.stunButton.Location = new System.Drawing.Point(6, 196);
            this.stunButton.Name = "stunButton";
            this.stunButton.Size = new System.Drawing.Size(81, 23);
            this.stunButton.TabIndex = 11;
            this.stunButton.Text = "Stun Target";
            this.stunButton.UseVisualStyleBackColor = true;
            this.stunButton.Click += new System.EventHandler(this.stunButton_Click);
            // 
            // txToolPTT
            // 
            this.txToolPTT.Location = new System.Drawing.Point(29, 20);
            this.txToolPTT.Name = "txToolPTT";
            this.txToolPTT.Size = new System.Drawing.Size(123, 23);
            this.txToolPTT.TabIndex = 4;
            this.txToolPTT.Text = "Console PTT ID";
            this.txToolPTT.UseVisualStyleBackColor = true;
            this.txToolPTT.Click += new System.EventHandler(this.txToolPTT_Click);
            // 
            // disableTransmitterButton
            // 
            this.disableTransmitterButton.Image = ((System.Drawing.Image)(resources.GetObject("disableTransmitterButton.Image")));
            this.disableTransmitterButton.Location = new System.Drawing.Point(487, 387);
            this.disableTransmitterButton.Name = "disableTransmitterButton";
            this.disableTransmitterButton.Size = new System.Drawing.Size(168, 23);
            this.disableTransmitterButton.TabIndex = 71;
            this.disableTransmitterButton.Text = "Disable Transmitter";
            this.disableTransmitterButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.disableTransmitterButton.UseVisualStyleBackColor = true;
            this.disableTransmitterButton.Click += new System.EventHandler(this.disableTransmitterButton_Click);
            // 
            // activityLog
            // 
            this.activityLog.ContextMenuStrip = this.logContextMenu;
            this.activityLog.FormattingEnabled = true;
            this.activityLog.Location = new System.Drawing.Point(6, 21);
            this.activityLog.Name = "activityLog";
            this.activityLog.Size = new System.Drawing.Size(469, 251);
            this.activityLog.TabIndex = 65;
            // 
            // labelTxAudioLevel
            // 
            this.labelTxAudioLevel.AutoSize = true;
            this.labelTxAudioLevel.Location = new System.Drawing.Point(4, 308);
            this.labelTxAudioLevel.Name = "labelTxAudioLevel";
            this.labelTxAudioLevel.Size = new System.Drawing.Size(81, 13);
            this.labelTxAudioLevel.TabIndex = 70;
            this.labelTxAudioLevel.Text = "Tx Audio Level:";
            // 
            // captureLogLabel
            // 
            this.captureLogLabel.AutoSize = true;
            this.captureLogLabel.Location = new System.Drawing.Point(3, 2);
            this.captureLogLabel.Name = "captureLogLabel";
            this.captureLogLabel.Size = new System.Drawing.Size(65, 13);
            this.captureLogLabel.TabIndex = 62;
            this.captureLogLabel.Text = "Activity Log;";
            // 
            // labelRxAudioLevel
            // 
            this.labelRxAudioLevel.AutoSize = true;
            this.labelRxAudioLevel.Location = new System.Drawing.Point(2, 283);
            this.labelRxAudioLevel.Name = "labelRxAudioLevel";
            this.labelRxAudioLevel.Size = new System.Drawing.Size(82, 13);
            this.labelRxAudioLevel.TabIndex = 69;
            this.labelRxAudioLevel.Text = "Rx Audio Level:";
            // 
            // targetMDCID
            // 
            this.targetMDCID.Location = new System.Drawing.Point(103, 350);
            this.targetMDCID.MaxLength = 4;
            this.targetMDCID.Name = "targetMDCID";
            this.targetMDCID.Size = new System.Drawing.Size(106, 20);
            this.targetMDCID.TabIndex = 61;
            this.targetMDCID.Text = "0001";
            // 
            // txVolumeMeter
            // 
            this.txVolumeMeter.Amplitude = 0.05F;
            this.txVolumeMeter.DetectDb = 4F;
            this.txVolumeMeter.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.txVolumeMeter.Location = new System.Drawing.Point(103, 306);
            this.txVolumeMeter.MaxDb = 10F;
            this.txVolumeMeter.MinDb = -90F;
            this.txVolumeMeter.Name = "txVolumeMeter";
            this.txVolumeMeter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.txVolumeMeter.Size = new System.Drawing.Size(119, 15);
            this.txVolumeMeter.TabIndex = 67;
            // 
            // targetIDLabel
            // 
            this.targetIDLabel.AutoSize = true;
            this.targetIDLabel.Location = new System.Drawing.Point(13, 352);
            this.targetIDLabel.Name = "targetIDLabel";
            this.targetIDLabel.Size = new System.Drawing.Size(71, 13);
            this.targetIDLabel.TabIndex = 63;
            this.targetIDLabel.Text = "Last MDC ID:";
            // 
            // rxVolumeMeter
            // 
            this.rxVolumeMeter.Amplitude = 0.05F;
            this.rxVolumeMeter.DetectDb = 4F;
            this.rxVolumeMeter.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.rxVolumeMeter.Location = new System.Drawing.Point(103, 281);
            this.rxVolumeMeter.MaxDb = 10F;
            this.rxVolumeMeter.MinDb = -90F;
            this.rxVolumeMeter.Name = "rxVolumeMeter";
            this.rxVolumeMeter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.rxVolumeMeter.Size = new System.Drawing.Size(119, 15);
            this.rxVolumeMeter.TabIndex = 68;
            // 
            // transmitCallsignCW
            // 
            this.transmitCallsignCW.Image = ((System.Drawing.Image)(resources.GetObject("transmitCallsignCW.Image")));
            this.transmitCallsignCW.Location = new System.Drawing.Point(487, 303);
            this.transmitCallsignCW.Name = "transmitCallsignCW";
            this.transmitCallsignCW.Size = new System.Drawing.Size(168, 23);
            this.transmitCallsignCW.TabIndex = 66;
            this.transmitCallsignCW.Text = "Transmit Callsign CW";
            this.transmitCallsignCW.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.transmitCallsignCW.UseVisualStyleBackColor = true;
            this.transmitCallsignCW.Click += new System.EventHandler(this.transmitCallsignCW_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripRepeaterOptionsButton,
            this.toolStripConfigureAudioButton});
            this.toolStrip.Location = new System.Drawing.Point(3, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(58, 25);
            this.toolStrip.TabIndex = 0;
            // 
            // toolStripRepeaterOptionsButton
            // 
            this.toolStripRepeaterOptionsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripRepeaterOptionsButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripRepeaterOptionsButton.Image")));
            this.toolStripRepeaterOptionsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripRepeaterOptionsButton.Name = "toolStripRepeaterOptionsButton";
            this.toolStripRepeaterOptionsButton.Size = new System.Drawing.Size(23, 22);
            this.toolStripRepeaterOptionsButton.Text = "Repeater Options";
            this.toolStripRepeaterOptionsButton.Click += new System.EventHandler(this.toolStripRepeaterOptionsButton_Click);
            // 
            // toolStripConfigureAudioButton
            // 
            this.toolStripConfigureAudioButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripConfigureAudioButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripConfigureAudioButton.Image")));
            this.toolStripConfigureAudioButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripConfigureAudioButton.Name = "toolStripConfigureAudioButton";
            this.toolStripConfigureAudioButton.Size = new System.Drawing.Size(23, 22);
            this.toolStripConfigureAudioButton.Text = "Configure Audio Device";
            this.toolStripConfigureAudioButton.Click += new System.EventHandler(this.toolStripConfigureAudioButton_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripAnnouncementsButton,
            this.toolStripDtmfCommandsButton});
            this.toolStrip1.Location = new System.Drawing.Point(61, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(268, 25);
            this.toolStrip1.TabIndex = 1;
            // 
            // toolStripAnnouncementsButton
            // 
            this.toolStripAnnouncementsButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripAnnouncementsButton.Image")));
            this.toolStripAnnouncementsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAnnouncementsButton.Name = "toolStripAnnouncementsButton";
            this.toolStripAnnouncementsButton.Size = new System.Drawing.Size(124, 22);
            this.toolStripAnnouncementsButton.Text = "Announcements...";
            this.toolStripAnnouncementsButton.Click += new System.EventHandler(this.toolStripAnnouncementsButton_Click);
            // 
            // toolStripDtmfCommandsButton
            // 
            this.toolStripDtmfCommandsButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDtmfCommandsButton.Image")));
            this.toolStripDtmfCommandsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDtmfCommandsButton.Name = "toolStripDtmfCommandsButton";
            this.toolStripDtmfCommandsButton.Size = new System.Drawing.Size(132, 22);
            this.toolStripDtmfCommandsButton.Text = "DTMF Commands...";
            this.toolStripDtmfCommandsButton.Click += new System.EventHandler(this.toolStripDtmfCommandsButton_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "RepeaterController";
            this.notifyIcon.Visible = true;
            // 
            // sysCallsign
            // 
            this.sysCallsign.AutoSize = true;
            this.sysCallsign.Location = new System.Drawing.Point(106, 14);
            this.sysCallsign.Name = "sysCallsign";
            this.sysCallsign.Size = new System.Drawing.Size(58, 13);
            this.sysCallsign.TabIndex = 47;
            this.sysCallsign.Text = "sysCallsign";
            // 
            // labelCallsign
            // 
            this.labelCallsign.AutoSize = true;
            this.labelCallsign.Location = new System.Drawing.Point(6, 14);
            this.labelCallsign.Name = "labelCallsign";
            this.labelCallsign.Size = new System.Drawing.Size(46, 13);
            this.labelCallsign.TabIndex = 46;
            this.labelCallsign.Text = "Callsign:";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 489);
            this.Controls.Add(this.toolStripContainer);
            this.Controls.Add(this.mainMenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenuStrip;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(687, 528);
            this.MinimumSize = new System.Drawing.Size(687, 528);
            this.Name = "MainWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RepeaterController Console";
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.logContextMenu.ResumeLayout(false);
            this.toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.ContentPanel.PerformLayout();
            this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.groupBoxStatus.ResumeLayout(false);
            this.groupBoxStatus.PerformLayout();
            this.generateMDCGroupBox.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip logContextMenu;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configureAudioDeviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decodeEncodedPacketsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem advancedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rawMDCEncodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem repeaterOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private System.Windows.Forms.Button buttonTestAnnounce;
        private System.Windows.Forms.Button clearTimeoutButton;
        private System.Windows.Forms.GroupBox groupBoxStatus;
        private System.Windows.Forms.Label lastId;
        private System.Windows.Forms.Label lastTransmissionLabel;
        private System.Windows.Forms.Label labelLastId;
        private System.Windows.Forms.Label labelLastTransmission;
        private System.Windows.Forms.GroupBox generateMDCGroupBox;
        private System.Windows.Forms.Button racButton;
        private System.Windows.Forms.Button emergAck;
        private System.Windows.Forms.Button messageButton;
        private System.Windows.Forms.Button statusButton;
        private System.Windows.Forms.Button selectiveCallButton;
        private System.Windows.Forms.Button callAlertButton;
        private System.Windows.Forms.Button radioCheckButton;
        private System.Windows.Forms.Button reviveButton;
        private System.Windows.Forms.Button stunButton;
        private System.Windows.Forms.Button txToolPTT;
        private System.Windows.Forms.Button disableTransmitterButton;
        private System.Windows.Forms.ListBox activityLog;
        private System.Windows.Forms.Label labelTxAudioLevel;
        private System.Windows.Forms.Label captureLogLabel;
        private System.Windows.Forms.Label labelRxAudioLevel;
        private System.Windows.Forms.TextBox targetMDCID;
        private VolumeMeter txVolumeMeter;
        private System.Windows.Forms.Label targetIDLabel;
        private VolumeMeter rxVolumeMeter;
        private System.Windows.Forms.Button transmitCallsignCW;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripTransmitLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripRACLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripMDCConsoleLabel;
        private System.Windows.Forms.Label txDuration;
        private System.Windows.Forms.Label txDurationLabel;
        private System.Windows.Forms.ToolStripButton toolStripRepeaterOptionsButton;
        private System.Windows.Forms.ToolStripButton toolStripConfigureAudioButton;
        private System.Windows.Forms.ToolStripMenuItem announcementsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripAnnouncementsButton;
        private System.Windows.Forms.ToolStripMenuItem saveSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dtmfCommandsToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripDtmfCommandsButton;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ToolStripStatusLabel toolStripPLLabel;
        private System.Windows.Forms.Button transmitTimeAnnc;
        private System.Windows.Forms.ToolStripMenuItem setAnnouncementEveryHourTimeToolStripMenuItem;
        private System.Windows.Forms.Label lastPl;
        private System.Windows.Forms.Label labelLastPL;
        private System.Windows.Forms.ToolStripMenuItem testButtonDoNotPressToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripDisableFilteringLabel;
        private System.Windows.Forms.Label lastDPL;
        private System.Windows.Forms.Label labelLastDPL;
        private System.Windows.Forms.Label sysCallsign;
        private System.Windows.Forms.Label labelCallsign;
    } // public partial class MainWindow
} // namespace RepeaterController
