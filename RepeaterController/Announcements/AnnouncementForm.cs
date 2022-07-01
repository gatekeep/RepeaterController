/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RepeaterController.Announcements
{
    /// <summary>
    /// Editor form for announcement data.
    /// </summary>
    public partial class AnnouncementForm : Form
    {
        /**
         * Fields
         */
        /// <summary>
        /// Flag indicating whether the form was canceled.
        /// </summary>
        public bool Canceled;

        /// <summary>
        /// Name of the announcement.
        /// </summary>
        public string AnncName;

        /// <summary>
        /// Time the announcement should start at.
        /// </summary>
        public DateTime Start;

        /// <summary>
        /// Interval the announcement plays at.
        /// </summary>
        public DateTime Interval;

        /// <summary>
        /// Name of the text file to synthesize or wave file to playback.
        /// </summary>
        public string Filename;
        /// <summary>
        /// Flag indicating whether this announcement is a wave file or not.
        /// </summary>
        public bool IsWaveFile;

        /// <summary>
        /// Flag indicating whether we're supplying the text directly rather then from a file.
        /// </summary>
        public bool IsSuppliedText;
        /// <summary>
        /// Text to synthesize or wave file to playback.
        /// </summary>
        public string RawText;

        /// <summary>
        /// Flag indicating whether we're editing an announcement.
        /// </summary>
        public bool IsEditing;

        /** 
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="AnnouncementForm"/> class.
        /// </summary>
        public AnnouncementForm()
        {
            InitializeComponent();

            this.FormClosing += AnnouncementForm_FormClosing;

            this.IsEditing = false;

            this.textBoxName.TextChanged += TextBoxName_TextChanged;
            this.speechText.TextChanged += SpeechText_TextChanged;
            this.dateTimePickerStart.ValueChanged += DateTimePickerStart_ValueChanged;
            this.dateTimePickerInterval.ValueChanged += DateTimePickerInterval_ValueChanged;
            this.anncFileTextBox.TextChanged += AnncFileTextBox_TextChanged;

            this.checkBoxWave.CheckedChanged += CheckBoxWave_CheckedChanged;

            this.AnncName = string.Empty;
            this.Start = DateTime.Now;
            this.Interval = new DateTime();
            this.Filename = string.Empty;
            this.RawText = string.Empty;
            this.IsWaveFile = false;
            this.IsSuppliedText = false;

            UpdateModal();
        }

        /// <summary>
        /// Updates the modal to the state of the internal variables.
        /// </summary>
        public void UpdateModal()
        {
            this.textBoxName.Text = this.AnncName;
            if (IsEditing)
                this.textBoxName.Enabled = false;
            
            if (IsSuppliedText)
            {
                this.radioButtonUseFile.Checked = false;
                this.radioButtonUseText.Checked = true;

                this.anncFileTextBox.Enabled = false;
                this.openAnncFile.Enabled = false;
                this.checkBoxWave.Enabled = false;
                this.speechText.Enabled = true;
            }
            else
            {
                this.radioButtonUseFile.Checked = true;
                this.radioButtonUseText.Checked = false;

                this.anncFileTextBox.Enabled = true;
                this.openAnncFile.Enabled = true;
                this.checkBoxWave.Enabled = true;
                this.speechText.Enabled = false;
            }

            // we 'invent' the date because that part is not really useful for our needs
            DateTime time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Interval.Hour, Interval.Minute, Interval.Second, Interval.Millisecond);
            this.dateTimePickerInterval.Value = time;
            this.dateTimePickerStart.Value = DateTime.Now;

            this.checkBoxWave.Checked = this.IsWaveFile;

            this.anncFileTextBox.Text = this.Filename;
            this.speechText.Text = this.RawText;
        }

        /// <summary>
        /// Occurs when the "Prerecorded Announcement" checkbox check is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxWave_CheckedChanged(object sender, EventArgs e)
        {
            this.IsWaveFile = checkBoxWave.Checked;
            UpdateModal();
        }

        /// <summary>
        /// Occurs then the "Name" textbox text is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxName_TextChanged(object sender, EventArgs e)
        {
            string name = textBoxName.Text;

            if (name == string.Empty)
            {
                textBoxName.ForeColor = Color.Red;
                return;
            }
            else
                textBoxName.ForeColor = Color.Black;

            this.AnncName = textBoxName.Text;
        }

        /// <summary>
        /// Occurs then the "Filename" textbox text is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnncFileTextBox_TextChanged(object sender, EventArgs e)
        {
            this.Filename = anncFileTextBox.Text;
        }

        /// <summary>
        /// Helper function to validate the set interval.
        /// </summary>
        private void ValidateInterval()
        {
            DateTime time = this.dateTimePickerInterval.Value;
            if (time.Minute <= 0)
                time = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute + 5, time.Second, time.Millisecond);
            this.Interval = time;
        }

        /// <summary>
        /// Occurs when the "Interval" date/time picker is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateTimePickerInterval_ValueChanged(object sender, EventArgs e)
        {
            ValidateInterval();
        }

        /// <summary>
        /// Occurs when the "Start" date/time picker is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateTimePickerStart_ValueChanged(object sender, EventArgs e)
        {
            this.Start = this.dateTimePickerStart.Value;
        }

        /// <summary>
        /// Occurs when the form is closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnnouncementForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.Cancel)
                this.Canceled = true;
        }

        /// <summary>
        /// Occurs then the "Speech" textbox text is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpeechText_TextChanged(object sender, EventArgs e)
        {
            this.RawText = speechText.Text;
        }

        /// <summary>
        /// Occurs when the "OK" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            ValidateInterval();
            DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Occurs when the "Cancel" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Occurs when the "Open File" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openAnncFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open Announcement File";
            if (IsWaveFile)
                ofd.Filter = "Wave Files (*.wav)|*.wav|All Files (*.*)|*.*";
            else
                ofd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            DialogResult dr = ofd.ShowDialog();

            if (dr == DialogResult.Cancel)
            {
                this.Filename = string.Empty;
                UpdateModal();
                return;
            }

            this.Filename = ofd.FileName;
            UpdateModal();
        }

        /// <summary>
        /// Occurs when the "Use File" radio is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonUseFile_CheckedChanged(object sender, EventArgs e)
        {
            this.checkBoxWave.Checked = false;

            this.anncFileTextBox.Enabled = true;
            this.openAnncFile.Enabled = true;
            this.checkBoxWave.Enabled = true;
            this.speechText.Enabled = false;

            this.IsSuppliedText = false;
        }

        /// <summary>
        /// Occurs when the "Text" radio is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonUseText_CheckedChanged(object sender, EventArgs e)
        {
            this.checkBoxWave.Checked = false;

            this.anncFileTextBox.Enabled = false;
            this.openAnncFile.Enabled = false;
            this.checkBoxWave.Enabled = false;
            this.speechText.Enabled = true;

            this.IsSuppliedText = true;
        }
    } // public partial class AnnouncementForm : Form
} // namespace RepeaterController.Announcements
