/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using RepeaterController.Xml;

namespace RepeaterController.Announcements
{
    /// <summary>
    /// Implements the management of announcments.
    /// </summary>
    public partial class AnnouncementWindow : Form
    {
        /**
         * Fields
         */
        private XmlResource rsrc;
        private AutomaticAnnc autoAnnc;
        private bool shown = false;

        /// <summary>
        /// Flag indicating whether the form was canceled.
        /// </summary>
        public bool Canceled;

        /**
         * Properties
         */
        /// <summary>
        /// Gets or sets the instance of the automatic announcer.
        /// </summary>
        public AutomaticAnnc Announcer
        {
            get { return autoAnnc; }
            set
            {
                if (value != null)
                {
                    this.autoAnnc = value;
                    value.PlayingAnnouncment += Announcer_PlayingAnnouncment;
                }
                else
                    this.autoAnnc = null;
            }
        }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="AnnouncementWindow"/> class.
        /// </summary>
        public AnnouncementWindow()
        {
            InitializeComponent();

            this.autoAnnc = null;

            this.deleteAnnouncmentToolStripMenuItem.Enabled = false;
            this.toolStripDeleteButton.Enabled = false;
            this.executeAnnouncmentToolStripMenuItem.Enabled = false;
            this.toolStripExecuteButton.Enabled = false;

            this.Shown += AnnouncementWindow_Shown;
            this.FormClosing += AnnouncementWindow_FormClosing;

            this.dataGridView.SelectionChanged += DataGridView_SelectionChanged;
            this.dataGridView.MouseDown += DataGridView_MouseDown;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AnnouncementWindow"/> class.
        /// </summary>
        /// <param name="rsrc"></param>
        public AnnouncementWindow(XmlResource rsrc) : this()
        {
            this.rsrc = rsrc;

            LoadXml();
            UpdateModal();
        }

        /// <summary>
        /// Helper to load/generate the user XML configuration data.
        /// </summary>
        public void LoadXml()
        {
            if (rsrc.HasNode("AnnouncementConfig"))
            {
                XmlResource announcementConfig = rsrc.Get<XmlResource>("AnnouncementConfig");

                // get data from XML
                try
                {
                    AutomaticAnnc.TimeAnnouncement = (bool)rsrc.Get<bool>("TimeAnnouncement");
                    AutomaticAnnc.TimeAnnouncementGreeting = (bool)rsrc.Get<bool>("Greeting");
                    AutomaticAnnc.TimeAnnouncementMil = (bool)rsrc.Get<bool>("Military");
                    AutomaticAnnc.TimeAnnouncementInterval = (TimeInterval)rsrc.Get<TimeInterval>("Interval");
                    AutomaticAnnc.UseCourtesyTone = (bool)rsrc.Get<bool>("UseCourtesyTone");
                    AutomaticAnnc.Announcements = (List<annc_ret_t>)rsrc.Get<List<annc_ret_t>>("Announcements");
                }
                catch (ArgumentException ae)
                {
                    // for announcements we'll just reset
                    Messages.TraceException(ae.Message, ae);
                    SetDefaults();
                    SaveXml();
                }
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
            XmlResource announcementConfig = rsrc.CreateNode("AnnouncementConfig");
            announcementConfig.Submit("TimeAnnouncement", AutomaticAnnc.TimeAnnouncement);
            announcementConfig.Submit("Greeting", AutomaticAnnc.TimeAnnouncementGreeting);
            announcementConfig.Submit("Military", AutomaticAnnc.TimeAnnouncementMil);
            announcementConfig.Submit("Interval", AutomaticAnnc.TimeAnnouncementInterval);
            announcementConfig.Submit("UseCourtesyTone", AutomaticAnnc.UseCourtesyTone);
            announcementConfig.Submit("Announcements", AutomaticAnnc.Announcements);

            rsrc.SaveXml(Environment.CurrentDirectory + Path.DirectorySeparatorChar + MainWindow.XML_FILE);
        }

        /// <summary>
        /// Sets default preset values.
        /// </summary>
        public void SetDefaults()
        {
            AutomaticAnnc.TimeAnnouncement = false;
            AutomaticAnnc.TimeAnnouncementGreeting = false;
            AutomaticAnnc.TimeAnnouncementMil = false;
            AutomaticAnnc.UseCourtesyTone = false;
            AutomaticAnnc.TimeAnnouncementInterval = TimeInterval.EveryHour;

            lock (AutomaticAnnc.Announcements)
            {
                AutomaticAnnc.Announcements = new List<annc_ret_t>();
            }
        }

        /// <summary>
        /// Updates the modal to the state of the internal variables.
        /// </summary>
        public void UpdateModal()
        {
            lock (AutomaticAnnc.Announcements)
            {
                if (shown)
                {
                    this.Invoke(new Action(() =>
                    {
                        if (AutomaticAnnc.TimeAnnouncement)
                            automatedTimeAnnouncementToolStripMenuItem.Checked = true;
                        if (AutomaticAnnc.TimeAnnouncementGreeting)
                            greetingToolStripMenuItem.Checked = true;
                        if (AutomaticAnnc.UseCourtesyTone)
                            useCourtesyToneToolStripMenuItem.Checked = true;
                        if (AutomaticAnnc.TimeAnnouncementMil)
                            militaryTimeToolStripMenuItem.Checked = true;

                        switch (AutomaticAnnc.TimeAnnouncementInterval)
                        {
                            case TimeInterval.EveryHour:
                                everyHourToolStripMenuItem.Checked = true;
                                every3HoursToolStripMenuItem.Checked = false;
                                every12HoursToolStripMenuItem.Checked = false;
                                break;
                            case TimeInterval.Every3Hours:
                                everyHourToolStripMenuItem.Checked = false;
                                every3HoursToolStripMenuItem.Checked = true;
                                every12HoursToolStripMenuItem.Checked = false;
                                break;
                            case TimeInterval.Every12Hours:
                                everyHourToolStripMenuItem.Checked = false;
                                every3HoursToolStripMenuItem.Checked = false;
                                every12HoursToolStripMenuItem.Checked = true;
                                break;
                        }

                        dataGridView.Rows.Clear();
                        foreach (annc_ret_t annc in AutomaticAnnc.Announcements)
                            dataGridView.Rows.Add(new object[] { annc.Name, DateTime.FromOADate(annc.Interval).ToString("HH:mm:ss"),
                                DateTime.FromOADate(annc.StartRun).ToString(), DateTime.FromOADate(annc.LastRun).ToString(), DateTime.FromOADate(annc.NextRun).ToString() });
                    }));
                }
            }
        }

        /// <summary>
        /// Occurs when the form is shown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnnouncementWindow_Shown(object sender, EventArgs e)
        {
            this.shown = true;
            UpdateModal();   
        }

        /// <summary>
        /// Occurs when the form is closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnnouncementWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.shown = false;
            if (DialogResult == DialogResult.Cancel)
                this.Canceled = true;
            else
                closeToolStripMenuItem_Click(sender, new EventArgs());
        }

        /// <summary>
        /// Occurs when the "Close" toolstrip menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateModal();
            SaveXml();

            DialogResult = DialogResult.OK;
            Canceled = false;

            this.Hide();
        }

        /// <summary>
        /// Event that occurs when the automatic announcer plays an announcement.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="name"></param>
        private void Announcer_PlayingAnnouncment(object sender, string name)
        {
            int idx = -1;
            foreach (annc_ret_t a in AutomaticAnnc.Announcements)
                if (a.Name == name)
                {
                    idx = AutomaticAnnc.Announcements.IndexOf(a);
                    break;
                }

            if (idx != -1)
                UpdateModal();
        }

        /// <summary>
        /// Event that occurs when the mouse is clicked on the data grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var hti = dataGridView.HitTest(e.X, e.Y);
                dataGridView.ClearSelection();
                if (hti.RowIndex != -1)
                    dataGridView.Rows[hti.RowIndex].Selected = true;
                return;
            }

            if (e.Button == MouseButtons.Right)
            {
                var hti = dataGridView.HitTest(e.X, e.Y);
                dataGridView.ClearSelection();
                if (hti.RowIndex != -1)
                    dataGridView.Rows[hti.RowIndex].Selected = true;
                return;
            }
        }

        /// <summary>
        /// Occurs when items are selected in the data grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                this.deleteAnnouncmentToolStripMenuItem.Enabled = true;
                this.toolStripDeleteButton.Enabled = true;
                this.executeAnnouncmentToolStripMenuItem.Enabled = true;
                this.toolStripExecuteButton.Enabled = true;
            }
            else
            {
                this.deleteAnnouncmentToolStripMenuItem.Enabled = false;
                this.toolStripDeleteButton.Enabled = false;
                this.executeAnnouncmentToolStripMenuItem.Enabled = false;
                this.toolStripExecuteButton.Enabled = false;
            }
        }

        /// <summary>
        /// Occurs when the "Add/Edit" toolstrip button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripAddEditButton_Click(object sender, EventArgs e)
        {
            addEditAnnouncmentToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Occurs when the "Add/Edit Announcement" context menu toolstrip menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addEditAnnouncmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AnnouncementForm form = new AnnouncementForm();
            if (dataGridView.SelectedRows.Count > 0)
            {
                // edit mode
                form.IsEditing = true;
                DataGridViewRow row = dataGridView.SelectedRows[0];
                form.AnncName = (string)row.Cells[0].Value;

                // find the announcement we're updating
                foreach (annc_ret_t annc in AutomaticAnnc.Announcements)
                {
                    if (annc.Name == form.AnncName)
                    {
                        form.AnncName = annc.Name;
                        form.Start = DateTime.FromOADate(annc.StartRun);
                        form.Interval = DateTime.FromOADate(annc.Interval);
                        form.IsWaveFile = annc.IsWaveFile;
                        form.Filename = annc.Filename;
                        form.RawText = annc.RawSyntheizedText;
                        form.IsSuppliedText = annc.IsSuppliedText;
                        break;
                    }
                }

                form.UpdateModal();
            }

            form.ShowDialog();

            // perform add/edit
            if (!form.Canceled)
            {
                if (form.IsEditing)
                {
                    int idx = -1;
                    foreach (annc_ret_t a in AutomaticAnnc.Announcements)
                        if (a.Name == form.AnncName)
                        {
                            idx = AutomaticAnnc.Announcements.IndexOf(a);
                            break;
                        }

                    DateTime formInterval = form.Interval;
                    DateTime formStart = form.Start;
                    annc_ret_t annc = AutomaticAnnc.Announcements[idx];
                    annc.StartRun = formStart.ToOADate();
                    annc.Interval = formInterval.ToOADate();
                    annc.IsWaveFile = form.IsWaveFile;
                    annc.Filename = form.Filename;
                    annc.RawSyntheizedText = form.RawText;
                    annc.IsSuppliedText = form.IsSuppliedText;

                    TimeSpan interval = new TimeSpan(formInterval.Hour, formInterval.Minute, formInterval.Second);
                    if ((annc.LastRun > 0) && (annc.StartRun > annc.LastRun))
                        annc.LastRun = annc.StartRun;

                    annc.NextRun = (formStart + interval).ToOADate();

                    // make sure that if we tried to set the start in the past to make the next run in the future
                    DateTime now = DateTime.Now;
                    if (now.ToOADate() > annc.NextRun)
                        annc.NextRun = (now + interval).ToOADate();

                    lock (AutomaticAnnc.Announcements)
                    {
                        AutomaticAnnc.Announcements[idx] = annc;
                    }

                    UpdateModal();
                    SaveXml();
                }
                else
                {
                    DateTime formInterval = form.Interval;
                    DateTime formStart = form.Start;
                    TimeSpan interval = new TimeSpan(formInterval.Hour, formInterval.Minute, formInterval.Second);

                    // we're adding a new announcement
                    annc_ret_t annc = new annc_ret_t()
                    {
                        Name = form.AnncName,
                        StartRun = formStart.ToOADate(),
                        Interval = formInterval.ToOADate(),
                        IsWaveFile = form.IsWaveFile,
                        Filename = form.Filename,
                        RawSyntheizedText = form.RawText,
                        IsSuppliedText = form.IsSuppliedText,
                        LastRun = formStart.ToOADate(),
                        NextRun = (formStart + interval).ToOADate(),
                    };

                    // make sure that if we tried to set the start in the past to make the next run in the future
                    DateTime now = DateTime.Now;
                    if (now.ToOADate() > annc.NextRun)
                        annc.NextRun = (now + interval).ToOADate();

                    if (annc.Name == string.Empty)
                    {
                        MessageBox.Show("Announcements must have a name! Cannot add an announcement with no name.", AssemblyVersion._NAME, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    lock (AutomaticAnnc.Announcements)
                    {
                        // lets not allow doubles now...
                        foreach (annc_ret_t a in AutomaticAnnc.Announcements)
                            if (a.Name == form.AnncName)
                            {
                                MessageBox.Show("Cannot add announcement named " + form.AnncName + " it already exists!", AssemblyVersion._NAME, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                        AutomaticAnnc.Announcements.Add(annc);
                    }

                    UpdateModal();
                    SaveXml();
                }
            }
        }

        /// <summary>
        /// Occurs when the "Delete" toolstrip button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripDeleteButton_Click(object sender, EventArgs e)
        {
            deleteAnnouncmentToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Occurs when the "Delete Announcement" context menu toolstrip menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteAnnouncmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView.SelectedRows[0];
                string anncName = (string)row.Cells[0].Value;

                int idx = -1;
                foreach (annc_ret_t a in AutomaticAnnc.Announcements)
                    if (a.Name == anncName)
                    {
                        idx = AutomaticAnnc.Announcements.IndexOf(a);
                        break;
                    }

                lock (AutomaticAnnc.Announcements)
                {
                    AutomaticAnnc.Announcements.RemoveAt(idx);
                }

                UpdateModal();
                SaveXml();
            }
        }

        /// <summary>
        /// Occurs when the "Execute" toolstrip button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripExecuteButton_Click(object sender, EventArgs e)
        {
            executeAnnouncmentToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Occurs when the "Execute Announcement" context menu toolstrip menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void executeAnnouncmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView.SelectedRows[0];
                string anncName = (string)row.Cells[0].Value;

                int idx = -1;
                foreach (annc_ret_t a in AutomaticAnnc.Announcements)
                    if (a.Name == anncName)
                    {
                        idx = AutomaticAnnc.Announcements.IndexOf(a);
                        break;
                    }

                lock (AutomaticAnnc.Announcements)
                {
                    if (autoAnnc != null)
                        autoAnnc.PlayRepeaterAnnc(AutomaticAnnc.Announcements[idx], DateTime.Now);
                }

                UpdateModal();
            }
        }

        /// <summary>
        /// Event that occurs when the "Automated Time Announcement" toolstrip menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void automatedTimeAnnouncementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AutomaticAnnc.TimeAnnouncement)
            {
                automatedTimeAnnouncementToolStripMenuItem.Checked = false;
                AutomaticAnnc.TimeAnnouncement = false;
            }
            else
            {
                automatedTimeAnnouncementToolStripMenuItem.Checked = true;
                AutomaticAnnc.TimeAnnouncement = true;
            }

            SaveXml();
        }

        /// <summary>
        /// Event that occurs when the "Play Time Announcement" toolstrip menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playTimeAnnouncmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (autoAnnc != null)
                autoAnnc.PlayTimeAnnc();
        }

        /// <summary>
        /// Event that occurs when the "Every Half Hour" toolstrip menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void everyHalfHourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            everyHalfHourToolStripMenuItem.Checked = true;
            everyHourToolStripMenuItem.Checked = false;
            every3HoursToolStripMenuItem.Checked = false;
            every12HoursToolStripMenuItem.Checked = false;
            AutomaticAnnc.TimeAnnouncementInterval = TimeInterval.EveryHalfHour;
            SaveXml();
        }

        /// <summary>
        /// Event that occurs when the "Every Hour" toolstrip menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void everyHourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            everyHalfHourToolStripMenuItem.Checked = false;
            everyHourToolStripMenuItem.Checked = true;
            every3HoursToolStripMenuItem.Checked = false;
            every12HoursToolStripMenuItem.Checked = false;
            AutomaticAnnc.TimeAnnouncementInterval = TimeInterval.EveryHour;
            SaveXml();
        }

        /// <summary>
        /// Event that occurs when the "Every 3 Hours" toolstrip menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void every3HoursToolStripMenuItem_Click(object sender, EventArgs e)
        {
            everyHalfHourToolStripMenuItem.Checked = false;
            everyHourToolStripMenuItem.Checked = false;
            every3HoursToolStripMenuItem.Checked = true;
            every12HoursToolStripMenuItem.Checked = false;
            AutomaticAnnc.TimeAnnouncementInterval = TimeInterval.Every3Hours;
            SaveXml();
        }

        /// <summary>
        /// Event that occurs when the "Every 12 Hours" toolstrip menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void every12HoursToolStripMenuItem_Click(object sender, EventArgs e)
        {
            everyHalfHourToolStripMenuItem.Checked = false;
            everyHourToolStripMenuItem.Checked = false;
            every3HoursToolStripMenuItem.Checked = false;
            every12HoursToolStripMenuItem.Checked = true;
            AutomaticAnnc.TimeAnnouncementInterval = TimeInterval.Every12Hours;
            SaveXml();
        }

        /// <summary>
        /// Event that occurs when the "Greeting" toolstrip menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void greetingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            greetingToolStripMenuItem.Checked = !greetingToolStripMenuItem.Checked;
            AutomaticAnnc.TimeAnnouncementGreeting = greetingToolStripMenuItem.Checked;
            SaveXml();
        }

        /// <summary>
        /// Event that occurs when the "Use Courtesy Tone" toolstrip menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void useCourtesyToneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            useCourtesyToneToolStripMenuItem.Checked = !useCourtesyToneToolStripMenuItem.Checked;
            AutomaticAnnc.UseCourtesyTone = useCourtesyToneToolStripMenuItem.Checked;
            SaveXml();
        }

        /// <summary>
        /// Event that occurs when the "Military Time" toolstrip menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void militaryTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            militaryTimeToolStripMenuItem.Checked = !militaryTimeToolStripMenuItem.Checked;
            AutomaticAnnc.TimeAnnouncementMil = militaryTimeToolStripMenuItem.Checked;
            SaveXml();
        }
    } // public partial class AnnouncementWindow : Form
} // namespace RepeaterController.Announcements
