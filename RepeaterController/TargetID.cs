/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RepeaterController
{
    /// <summary>
    /// Helper MDC Target ID Modal
    /// </summary>
    public partial class TargetID : Form
    {
        /**
         * Fields
         */
        /// <summary>
        /// Target ID to use.
        /// </summary>
        public ushort TargetRadioID;

        /// <summary>
        /// Flag indicating whether the form was canceled.
        /// </summary>
        public bool Canceled;

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="TargetID"/> class.
        /// </summary>
        /// <param name="targetId"></param>
        public TargetID(ushort targetId)
        {
            InitializeComponent();

            this.Shown += TargetID_Shown;
            this.FormClosing += TargetID_FormClosing;

            Canceled = false;
            TargetRadioID = targetId;
            targetMDCID.Text = targetId.ToString("X4");

            targetMDCID.TextChanged += targetMDCID_TextChanged;
        }

        /// <summary>
        /// Occurs when the form is shown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TargetID_Shown(object sender, EventArgs e)
        {
            this.targetMDCID.Focus();
        }

        /// <summary>
        /// Occurs when the form begins closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TargetID_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.Cancel)
                this.Canceled = true;
        }

        /// <summary>
        /// Internal function to invalidate the Target ID input box.
        /// </summary>
        private void InvalidateTargetID()
        {
            targetMDCID.ForeColor = Color.Red;
            TargetRadioID = 0x0001;
        }

        /// <summary>
        /// Internal function to validate the Target ID input box.
        /// </summary>
        private void ValidateTargetID()
        {
            try
            {
                targetMDCID.ForeColor = Color.Black;
                TargetRadioID = Convert.ToUInt16(targetMDCID.Text, 16);
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
        private void targetMDCID_TextChanged(object sender, EventArgs e)
        {
            if (targetMDCID.Text.Length < 1)
                InvalidateTargetID();
            else
                ValidateTargetID();
        }

        /// <summary>
        /// Occurs when the "OK" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }
    } // public partial class TargetID : Form
} // namespace RepeaterController
