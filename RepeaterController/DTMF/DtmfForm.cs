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

namespace RepeaterController.DSP.DTMF
{
    /// <summary>
    /// Editor form for DTMF command data.
    /// </summary>
    public partial class DTMFForm : Form
    {
        /**
         * Fields
         */
        public const int MAX_DTMF_SEQ_LENGTH = 4;

        /// <summary>
        /// Flag indicating whether the form was canceled.
        /// </summary>
        public bool Canceled;

        /// <summary>
        /// Name of the DTMF command.
        /// </summary>
        public string DtmfName;

        /// <summary>
        /// DTMF Sequence
        /// </summary>
        public string DTMFSequence;

        /// <summary>
        /// Instruction to execute.
        /// </summary>
        public DtmfInstruction Instruction;

        /// <summary>
        /// Name of the executable file to execute.
        /// </summary>
        public string Filename;

        /// <summary>
        /// Executable arguments.
        /// </summary>
        public string Arguments;

        /// <summary>
        /// Flag indicating whether we're editing an announcement.
        /// </summary>
        public bool IsEditing;

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
        /// Initializes a new instance of the <see cref="DTMFForm"/> class.
        /// </summary>
        public DTMFForm()
        {
            InitializeComponent();

            this.FormClosing += DTMFForm_FormClosing;

            this.IsEditing = false;

            this.textBoxName.TextChanged += TextBoxName_TextChanged;
            this.textBoxDtmfSequence.TextChanged += TextBoxDtmfSequence_TextChanged;
            this.comboBoxType.SelectedIndexChanged += ComboBoxType_SelectedIndexChanged;
            this.fileTextBox.TextChanged += FileTextBox_TextChanged;
            this.textBoxArguments.TextChanged += TextBoxArguments_TextChanged;

            this.DtmfName = string.Empty;
            this.DTMFSequence = "000";
            this.Instruction = DtmfInstruction.External;
            this.Filename = string.Empty;
            this.Arguments = string.Empty;

            UpdateModal();
        }

        /// <summary>
        /// Updates the modal to the state of the internal variables.
        /// </summary>
        public void UpdateModal()
        {
            this.textBoxName.Text = this.DtmfName;
            if (IsEditing)
                this.textBoxName.Enabled = false;

            this.textBoxDtmfSequence.Text = this.DTMFSequence;

            if (this.Instruction == DtmfInstruction.External)
                this.groupBoxFile.Enabled = true;
            else
                this.groupBoxFile.Enabled = false;

            this.fileTextBox.Text = this.Filename;
            this.textBoxArguments.Text = this.Arguments;

            // this MUST be done in the order of the enum
            this.comboBoxType.Items.Add(new ListItem((int)DtmfInstruction.External, "External"));
            this.comboBoxType.Items.Add(new ListItem((int)DtmfInstruction.RepeaterKnockdown, "Knockdown"));
            this.comboBoxType.Items.Add(new ListItem((int)DtmfInstruction.PlayTimeAnnouncement, "Play Time Announcement"));
            this.comboBoxType.Items.Add(new ListItem((int)DtmfInstruction.PlayAnnouncement, "Play Announcement"));
            this.comboBoxType.SelectedIndex = (int)this.Instruction;
            if (IsEditing)
                this.comboBoxType.Enabled = false;
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

            this.DtmfName = textBoxName.Text;

        }

        /// <summary>
        /// Occurs when the "DTMF" textbox is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxDtmfSequence_TextChanged(object sender, EventArgs e)
        {
            string seq = textBoxDtmfSequence.Text;

            if (seq.Length > MAX_DTMF_SEQ_LENGTH)
            {
                textBoxDtmfSequence.ForeColor = Color.Red;
                return;
            }
            else
                textBoxName.ForeColor = Color.Black;

            this.DTMFSequence = seq;
        }

        /// <summary>
        /// Occurs when the "Filename" textbox is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileTextBox_TextChanged(object sender, EventArgs e)
        {
            this.Filename = fileTextBox.Text;
        }

        /// <summary>
        /// Occurs when the "Arguments" textbox is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxArguments_TextChanged(object sender, EventArgs e)
        {
            this.Arguments = textBoxArguments.Text;
        }

        /// <summary>
        /// Occurs when the "Type" combobox is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Instruction = (DtmfInstruction)comboBoxType.SelectedIndex;
            if (this.Instruction == DtmfInstruction.External)
            {
                this.textBoxName.Enabled = true;
                this.textBoxName.Text = this.DtmfName = string.Empty;
                this.groupBoxFile.Enabled = true;
                return;
            }
            else
            {
                this.groupBoxFile.Enabled = false;
                this.textBoxName.Enabled = false;

                if (!IsEditing || (this.DtmfName == string.Empty))
                {
                    ListItem item = (ListItem)comboBoxType.SelectedItem;
                    this.textBoxName.Text = this.DtmfName = item.Description;
                }
            }

            // handle announcement selection
            if (this.Instruction == DtmfInstruction.PlayAnnouncement)
            {
                if (!IsEditing)
                {
                    this.textBoxName.Enabled = true;
                    this.textBoxName.Text = this.DtmfName = string.Empty;
                }

                this.groupBoxFile.Enabled = true;
                this.labelFilename.Enabled = false;
                this.fileTextBox.Enabled = false;
                this.openExeFile.Enabled = false;
            }
            else
            {
                // make sure these are enabled .. because we might have turned them off
                this.labelFilename.Enabled = true;
                this.fileTextBox.Enabled = true;
                this.openExeFile.Enabled = true;
            }
        }

        /// <summary>
        /// Occurs when the form is closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DTMFForm_FormClosing(object sender, FormClosingEventArgs e)
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

        /// <summary>
        /// Occurs when the "Open File" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openExeFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open Executable File";
            ofd.Filter = "Executable Files (*.exe)|*.exe|Batch Files (*.bat)|*.bat|Batch Files (*.cmd)|*.cmd|VBScript (*.vbs)|*.vbs|All Files (*.*)|*.*";
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
    } // public partial class DTMFForm : Form
} // namespace RepeaterController.DSP.DTMF
