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
    /// Helper MDC Target ID and Message ID Modal
    /// </summary>
    public partial class MessageTargetID : Form
    {
        /**
         * Fields
         */
        /// <summary>
        /// Target ID to use.
        /// </summary>
        public ushort TargetID;
        /// <summary>
        /// Message ID for packet.
        /// </summary>
        public ushort MessageID;

        /// <summary>
        /// Flag indicating whether the form was canceled.
        /// </summary>
        public bool Canceled;

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTargetID"/> class.
        /// </summary>
        public MessageTargetID()
        {
            InitializeComponent();

            this.Shown += MessageTargetID_Shown;
            this.FormClosing += MessageTargetID_FormClosing;

            TargetID = 0x0001;
            MessageID = 0x0001;

            targetMDCID.TextChanged += targetMDCID_TextChanged;
            messageMDCID.TextChanged += messageMDCID_TextChanged;
        }

        /// <summary>
        /// Occurs when the form is shown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageTargetID_Shown(object sender, EventArgs e)
        {
            this.targetMDCID.Focus();
        }

        /// <summary>
        /// Occurs when the form begins closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageTargetID_FormClosing(object sender, FormClosingEventArgs e)
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
            TargetID = 0x0001;
        }

        /// <summary>
        /// Internal function to validate the Target ID input box.
        /// </summary>
        private void ValidateTargetID()
        {
            try
            {
                targetMDCID.ForeColor = Color.Black;
                TargetID = Convert.ToUInt16(targetMDCID.Text, 16);
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
            if (targetMDCID.Text.Length < 4)
                InvalidateTargetID();
            else
                ValidateTargetID();
        }

        /// <summary>
        /// Internal function to invalidate the Target ID input box.
        /// </summary>
        private void InvalidateMessageID()
        {
            messageMDCID.ForeColor = Color.Red;
            MessageID = 0x0001;
        }

        /// <summary>
        /// Internal function to validate the Target ID input box.
        /// </summary>
        private void ValidateMessageID()
        {
            try
            {
                messageMDCID.ForeColor = Color.Black;
                MessageID = Convert.ToUInt16(messageMDCID.Text, 16);
            }
            catch (FormatException)
            {
                InvalidateMessageID();
            }
            catch (OverflowException)
            {
                InvalidateMessageID();
            }
        }

        /// <summary>
        /// Occurs when the text in the Target ID box changes.
        /// </summary>
        private void messageMDCID_TextChanged(object sender, EventArgs e)
        {
            if (messageMDCID.Text.Length < 1)
                InvalidateMessageID();
            else
                ValidateMessageID();
        }

        /// <summary>
        /// Occurs when the "Transmit" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void transmitButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }
    } // public partial class MessageTargetID : Form
} // namespace RepeaterController
