/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using RepeaterController.DSP.MDC1200;

namespace RepeaterController
{
    /// <summary>
    /// 'Raw' MDC entry modal.
    /// </summary>
    public partial class RawMDCEncode : Form
    {
        /**
         * Fields
         */
        private MDCGenerator generator;

        private byte firstOp;
        private byte firstArg;
        private ushort firstUnitID;
        private byte secondOp;
        private byte secondArg;
        private ushort secondUnitID;

        /// <summary>
        /// Flag indicating whether the form was canceled.
        /// </summary>
        public bool Canceled;

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="RawMDCEncode"/> class.
        /// </summary>
        /// <param name="generator"></param>
        public RawMDCEncode(MDCGenerator generator)
        {
            this.generator = generator;

            this.firstOp = 0x00;
            this.firstArg = 0x00;
            this.firstUnitID = 0x00;
            this.secondOp = 0x00;
            this.secondArg = 0x00;
            this.secondUnitID = 0x00;

            InitializeComponent();

            this.FormClosing += RawMDCEncode_FormClosing;

            firstOpTextBox.TextChanged += firstOpTextBox_TextChanged;
            firstArgTextBox.TextChanged += firstArgTextBox_TextChanged;
            firstUnitIDTextBox.TextChanged += firstUnitIDTextBox_TextChanged;

            secondOpTextBox.TextChanged += secondOpTextBox_TextChanged;
            secondArgTextBox.TextChanged += secondArgTextBox_TextChanged;
            secondUnitIDTextBox.TextChanged += secondUnitIDTextBox_TextChanged;

            mdcDebugSingle.Click += mdcDebugSingle_Click;
            mdcDebugDouble.Click += mdcDebugDouble_Click;
        }

        /// <summary>
        /// Occurs when the form begins closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RawMDCEncode_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.Cancel)
                this.Canceled = true;
        }

        /// <summary>
        /// Occurs when the text in the first op text box is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void firstOpTextBox_TextChanged(object sender, EventArgs e)
        {
            if (firstOpTextBox.Text.Length < 2)
            {
                firstOpTextBox.ForeColor = Color.Red;
                firstOp = 0x00;
            }
            else
            {
                try
                {
                    firstOpTextBox.ForeColor = Color.Black;
                    firstOp = Convert.ToByte(firstOpTextBox.Text, 16);
                }
                catch (FormatException)
                {
                    firstOpTextBox.ForeColor = Color.Red;
                    firstOp = 0x00;
                }
                catch (OverflowException)
                {
                    firstOpTextBox.ForeColor = Color.Red;
                    firstOp = 0x00;
                }
            }
        }

        /// <summary>
        /// Occurs when the text in the first arg text box is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void firstArgTextBox_TextChanged(object sender, EventArgs e)
        {
            if (firstArgTextBox.Text.Length < 2)
            {
                firstArgTextBox.ForeColor = Color.Red;
                firstArg = 0x00;
            }
            else
            {
                try
                {
                    firstArgTextBox.ForeColor = Color.Black;
                    firstArg = Convert.ToByte(firstArgTextBox.Text, 16);
                }
                catch (FormatException)
                {
                    firstArgTextBox.ForeColor = Color.Red;
                    firstArg = 0x00;
                }
                catch (OverflowException)
                {
                    firstArgTextBox.ForeColor = Color.Red;
                    firstArg = 0x00;
                }
            }
        }

        /// <summary>
        /// Occurs when the text in the first unit text box is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void firstUnitIDTextBox_TextChanged(object sender, EventArgs e)
        {
            if (firstUnitIDTextBox.Text.Length < 4)
            {
                firstUnitIDTextBox.ForeColor = Color.Red;
                firstUnitID = 0x0000;
            }
            else
            {
                try
                {
                    firstUnitIDTextBox.ForeColor = Color.Black;
                    firstUnitID = Convert.ToUInt16(firstUnitIDTextBox.Text, 16);
                }
                catch (FormatException)
                {
                    firstUnitIDTextBox.ForeColor = Color.Red;
                    firstUnitID = 0x0000;
                }
                catch (OverflowException)
                {
                    firstUnitIDTextBox.ForeColor = Color.Red;
                    firstUnitID = 0x0000;
                }
            }
        }

        /// <summary>
        /// Occurs when the text in the second op text box is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void secondOpTextBox_TextChanged(object sender, EventArgs e)
        {
            if (secondOpTextBox.Text.Length < 2)
            {
                secondOpTextBox.ForeColor = Color.Red;
                secondOp = 0x00;
            }
            else
            {
                try
                {
                    secondOpTextBox.ForeColor = Color.Black;
                    secondOp = Convert.ToByte(secondOpTextBox.Text, 16);
                }
                catch (FormatException)
                {
                    secondOpTextBox.ForeColor = Color.Red;
                    secondOp = 0x00;
                }
                catch (OverflowException)
                {
                    secondOpTextBox.ForeColor = Color.Red;
                    secondOp = 0x00;
                }
            }
        }

        /// <summary>
        /// Occurs when the text in the second arg text box is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void secondArgTextBox_TextChanged(object sender, EventArgs e)
        {
            if (secondArgTextBox.Text.Length < 2)
            {
                secondArgTextBox.ForeColor = Color.Red;
                secondArg = 0x00;
            }
            else
            {
                try
                {
                    secondArgTextBox.ForeColor = Color.Black;
                    secondArg = Convert.ToByte(secondArgTextBox.Text, 16);
                }
                catch (FormatException)
                {
                    secondArgTextBox.ForeColor = Color.Red;
                    secondArg = 0x00;
                }
                catch (OverflowException)
                {
                    secondArgTextBox.ForeColor = Color.Red;
                    secondArg = 0x00;
                }
            }
        }

        /// <summary>
        /// Occurs when the text in the second unit text box is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void secondUnitIDTextBox_TextChanged(object sender, EventArgs e)
        {
            if (secondUnitIDTextBox.Text.Length < 4)
            {
                secondUnitIDTextBox.ForeColor = Color.Red;
                secondUnitID = 0x0000;
            }
            else
            {
                try
                {
                    secondUnitIDTextBox.ForeColor = Color.Black;
                    secondUnitID = Convert.ToUInt16(secondUnitIDTextBox.Text, 16);
                }
                catch (FormatException)
                {
                    secondUnitIDTextBox.ForeColor = Color.Red;
                    secondUnitID = 0x0000;
                }
                catch (OverflowException)
                {
                    secondUnitIDTextBox.ForeColor = Color.Red;
                    secondUnitID = 0x0000;
                }
            }
        }

        /// <summary>
        /// Occurs when the "Encode Single Packet" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mdcDebugSingle_Click(object sender, EventArgs e)
        {
            // generate MDC packet
            MDCPacket pckt = new MDCPacket()
            {
                Operation = firstOp,
                Argument = firstArg,
                Target = firstUnitID
            };
            generator.GenerateMDC(pckt);
            DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Occurs when the "Encode Double Packet" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mdcDebugDouble_Click(object sender, EventArgs e)
        {
            // generate MDC packet
            MDCPacket pckt = new MDCPacket()
            {
                Operation = firstOp,
                Argument = firstArg,
                Target = firstUnitID
            };
            MDCPacket pckt2 = new MDCPacket()
            {
                Operation = secondOp,
                Argument = secondArg,
                Target = secondUnitID
            };
            generator.GenerateMDC(pckt, pckt2);
            DialogResult = DialogResult.OK;
            this.Close();
        }
    } // public partial class RawMDCEncode : Form
} // namespace RepeaterController
