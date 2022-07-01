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

using RepeaterController.DSP;

namespace RepeaterController
{
    /// <summary>
    /// Editor form for multi-tone tone data.
    /// </summary>
    public partial class MultitoneForm : Form
    {
        /**
         * Fields
         */
        /// <summary>
        /// Flag indicating whether the form was canceled.
        /// </summary>
        public bool Canceled;

        /// <summary>
        /// Tone data.
        /// </summary>
        public Multitone Tone;

        /// <summary>
        /// Flag indicating whether the tone index has changed.
        /// </summary>
        public bool IndexChanged;

        /// <summary>
        /// Flag indicating whether we're editing an announcement.
        /// </summary>
        public bool IsEditing;

        /** 
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="MultitoneForm"/> class.
        /// </summary>
        public MultitoneForm()
        {
            InitializeComponent();

            this.Shown += MultitoneForm_Shown;
            this.FormClosing += MultitoneForm_FormClosing;

            this.IsEditing = false;
            this.IndexChanged = false;

            this.textBoxFrequency.TextChanged += TextBoxFrequency_TextChanged;
            this.textBoxFrequency2.TextChanged += TextBoxFrequency2_TextChanged;
            this.textBoxLength.TextChanged += TextBoxLength_TextChanged;
            this.textBoxSilenceLength.TextChanged += TextBoxSilenceLength_TextChanged;
            this.numericUpDownIndex.ValueChanged += NumericUpDownIndex_ValueChanged;

            this.Tone = new Multitone();
            this.Tone.Index = 0;
            this.Tone.Frequency = 0;
            this.Tone.FrequencyTwo = 0;
            this.Tone.Length = 0;
            this.Tone.SilenceLength = 0;

            UpdateModal();
        }
        
        /// <summary>
        /// Occurs when the dialog is shown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MultitoneForm_Shown(object sender, EventArgs e)
        {
            this.textBoxFrequency.Focus();
        }

        /// <summary>
        /// Updates the modal to the state of the internal variables.
        /// </summary>
        public void UpdateModal()
        {
            this.textBoxFrequency.Text = this.Tone.Frequency.ToString();
            this.textBoxFrequency2.Text = this.Tone.FrequencyTwo.ToString();
            this.textBoxLength.Text = this.Tone.Length.ToString();
            this.textBoxSilenceLength.Text = this.Tone.SilenceLength.ToString();

            this.numericUpDownIndex.Value = this.Tone.Index;
        }

        /// <summary>
        /// Occurs when the "Tone Index" numeric up-down box changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericUpDownIndex_ValueChanged(object sender, EventArgs e)
        {
            this.Tone.Index = Convert.ToInt32(this.numericUpDownIndex.Value);
            this.IndexChanged = true;
        }

        /// <summary>
        /// Occurs when the "Silence Length" textbox text changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxSilenceLength_TextChanged(object sender, EventArgs e)
        {
            int length = 0;
            if (int.TryParse(textBoxSilenceLength.Text, out length))
            {
                this.Tone.SilenceLength = length;
                textBoxSilenceLength.ForeColor = Color.Black;
                UpdateModal();
            }
            else
                textBoxSilenceLength.ForeColor = Color.Red;
        }

        /// <summary>
        /// Occurs when the "Length" textbox text changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxLength_TextChanged(object sender, EventArgs e)
        {
            int length = 0;
            if (int.TryParse(textBoxLength.Text, out length))
            {
                this.Tone.Length = length;
                textBoxLength.ForeColor = Color.Black;
                UpdateModal();
            }
            else
                textBoxLength.ForeColor = Color.Red;
        }

        /// <summary>
        /// Occurs when the "Frequency" textbox text changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxFrequency2_TextChanged(object sender, EventArgs e)
        {
            double frequency = 0;
            if (double.TryParse(textBoxFrequency2.Text, out frequency))
            {
                this.Tone.FrequencyTwo = frequency;
                textBoxFrequency2.ForeColor = Color.Black;
                UpdateModal();
            }
            else
                textBoxFrequency2.ForeColor = Color.Red;
        }

        /// <summary>
        /// Occurs when the "Frequency" textbox text changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxFrequency_TextChanged(object sender, EventArgs e)
        {
            double frequency = 0;
            if (double.TryParse(textBoxFrequency.Text, out frequency))
            {
                this.Tone.Frequency = frequency;
                textBoxFrequency.ForeColor = Color.Black;
                UpdateModal();
            }
            else
                textBoxFrequency.ForeColor = Color.Red;
        }

        /// <summary>
        /// Occurs when the form is closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MultitoneForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.Cancel)
                this.Canceled = true;
        }

        /// <summary>
        /// Occurs when the "OK" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
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
    } // public partial class MultitoneForm : Form
} // namespace RepeaterController
