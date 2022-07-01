/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.Windows.Forms;

using RepeaterController.Announcements;

namespace RepeaterController
{
    /// <summary>
    /// Helper Announcment "Every Hour" Set Modal
    /// </summary>
    public partial class AnncEveryHourSet : Form
    {
        /**
         * Fields
         */
        private AutomaticAnnc annc;

        /// <summary>
        /// Flag indicating whether the form was canceled.
        /// </summary>
        public bool Canceled;

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="AnncEveryHourSet"/> class.
        /// </summary>
        /// <param name="annc"></param>
        public AnncEveryHourSet(AutomaticAnnc annc)
        {
            InitializeComponent();

            this.annc = annc;
            this.FormClosing += AnncEveryHourSet_FormClosing;
            if (AutomaticAnnc.TimeAnnouncementInterval == TimeInterval.EveryHalfHour)
            {
                TimeSpan interval = new TimeSpan(0, 30, 0);
                DateTime next = annc.everyHour + interval;
                this.labelNextHour.Text = next.ToLongTimeString();
            }
            else if (AutomaticAnnc.TimeAnnouncementInterval == TimeInterval.EveryHour)
            {
                TimeSpan interval = new TimeSpan(1, 0, 0);
                DateTime next = annc.everyHour + interval;
                this.labelNextHour.Text = next.ToLongTimeString();
            }
            else if (AutomaticAnnc.TimeAnnouncementInterval == TimeInterval.Every3Hours)
            {
                TimeSpan interval = new TimeSpan(3, 0, 0);
                DateTime next = annc.everyHour + interval;
                this.labelNextHour.Text = next.ToLongTimeString();
            }
            else if (AutomaticAnnc.TimeAnnouncementInterval == TimeInterval.Every12Hours)
            {
                TimeSpan interval = new TimeSpan(12, 0, 0);
                DateTime next = annc.everyHour + interval;
                this.labelNextHour.Text = next.ToLongTimeString();
            }


            this.dateTimePicker1.Value = annc.everyHour;

            Canceled = false;
        }

        /// <summary>
        /// Occurs when the form begins closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnncEveryHourSet_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.Cancel)
                this.Canceled = true;
        }

        /// <summary>
        /// Occurs when the "Set" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setButton_Click(object sender, EventArgs e)
        {
            annc.everyHour = dateTimePicker1.Value;

            DialogResult = DialogResult.OK;
            this.Close();
        }
    } // public partial class AnncEveryHourSet : Form
} // namespace RepeaterController
