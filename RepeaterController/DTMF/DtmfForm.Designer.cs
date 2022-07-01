namespace RepeaterController.DSP.DTMF
{
    partial class DTMFForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DTMFForm));
            this.groupBoxDetails = new System.Windows.Forms.GroupBox();
            this.labelType = new System.Windows.Forms.Label();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.textBoxDtmfSequence = new System.Windows.Forms.TextBox();
            this.labelDtmf = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.groupBoxFile = new System.Windows.Forms.GroupBox();
            this.labelArguments = new System.Windows.Forms.Label();
            this.textBoxArguments = new System.Windows.Forms.TextBox();
            this.openExeFile = new System.Windows.Forms.Button();
            this.fileTextBox = new System.Windows.Forms.TextBox();
            this.labelFilename = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBoxDetails.SuspendLayout();
            this.groupBoxFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxDetails
            // 
            this.groupBoxDetails.Controls.Add(this.labelType);
            this.groupBoxDetails.Controls.Add(this.comboBoxType);
            this.groupBoxDetails.Controls.Add(this.textBoxDtmfSequence);
            this.groupBoxDetails.Controls.Add(this.labelDtmf);
            this.groupBoxDetails.Controls.Add(this.textBoxName);
            this.groupBoxDetails.Controls.Add(this.labelName);
            this.groupBoxDetails.Location = new System.Drawing.Point(12, 12);
            this.groupBoxDetails.Name = "groupBoxDetails";
            this.groupBoxDetails.Size = new System.Drawing.Size(226, 100);
            this.groupBoxDetails.TabIndex = 15;
            this.groupBoxDetails.TabStop = false;
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Location = new System.Drawing.Point(6, 74);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(34, 13);
            this.labelType.TabIndex = 22;
            this.labelType.Text = "Type:";
            // 
            // comboBoxType
            // 
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Location = new System.Drawing.Point(64, 71);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(150, 21);
            this.comboBoxType.TabIndex = 21;
            // 
            // textBoxDtmfSequence
            // 
            this.textBoxDtmfSequence.Location = new System.Drawing.Point(64, 45);
            this.textBoxDtmfSequence.Name = "textBoxDtmfSequence";
            this.textBoxDtmfSequence.Size = new System.Drawing.Size(150, 20);
            this.textBoxDtmfSequence.TabIndex = 20;
            // 
            // labelDtmf
            // 
            this.labelDtmf.AutoSize = true;
            this.labelDtmf.Location = new System.Drawing.Point(6, 48);
            this.labelDtmf.Name = "labelDtmf";
            this.labelDtmf.Size = new System.Drawing.Size(40, 13);
            this.labelDtmf.TabIndex = 19;
            this.labelDtmf.Text = "DTMF:";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(64, 19);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(150, 20);
            this.textBoxName.TabIndex = 18;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(6, 22);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(38, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Name:";
            // 
            // groupBoxFile
            // 
            this.groupBoxFile.Controls.Add(this.labelArguments);
            this.groupBoxFile.Controls.Add(this.textBoxArguments);
            this.groupBoxFile.Controls.Add(this.openExeFile);
            this.groupBoxFile.Controls.Add(this.fileTextBox);
            this.groupBoxFile.Controls.Add(this.labelFilename);
            this.groupBoxFile.Location = new System.Drawing.Point(12, 118);
            this.groupBoxFile.Name = "groupBoxFile";
            this.groupBoxFile.Size = new System.Drawing.Size(372, 104);
            this.groupBoxFile.TabIndex = 16;
            this.groupBoxFile.TabStop = false;
            // 
            // labelArguments
            // 
            this.labelArguments.AutoSize = true;
            this.labelArguments.Location = new System.Drawing.Point(6, 71);
            this.labelArguments.Name = "labelArguments";
            this.labelArguments.Size = new System.Drawing.Size(60, 13);
            this.labelArguments.TabIndex = 19;
            this.labelArguments.Text = "Arguments:";
            // 
            // textBoxArguments
            // 
            this.textBoxArguments.Location = new System.Drawing.Point(72, 68);
            this.textBoxArguments.Name = "textBoxArguments";
            this.textBoxArguments.Size = new System.Drawing.Size(150, 20);
            this.textBoxArguments.TabIndex = 18;
            // 
            // openExeFile
            // 
            this.openExeFile.Location = new System.Drawing.Point(64, 39);
            this.openExeFile.Name = "openExeFile";
            this.openExeFile.Size = new System.Drawing.Size(75, 23);
            this.openExeFile.TabIndex = 17;
            this.openExeFile.Text = "Open File...";
            this.openExeFile.UseVisualStyleBackColor = true;
            this.openExeFile.Click += new System.EventHandler(this.openExeFile_Click);
            // 
            // fileTextBox
            // 
            this.fileTextBox.Location = new System.Drawing.Point(64, 13);
            this.fileTextBox.Name = "fileTextBox";
            this.fileTextBox.Size = new System.Drawing.Size(150, 20);
            this.fileTextBox.TabIndex = 15;
            // 
            // labelFilename
            // 
            this.labelFilename.AutoSize = true;
            this.labelFilename.Location = new System.Drawing.Point(6, 16);
            this.labelFilename.Name = "labelFilename";
            this.labelFilename.Size = new System.Drawing.Size(52, 13);
            this.labelFilename.TabIndex = 16;
            this.labelFilename.Text = "Filename:";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(228, 228);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 17;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(309, 228);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 18;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // DTMFForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(398, 263);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.groupBoxFile);
            this.Controls.Add(this.groupBoxDetails);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(414, 302);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(414, 302);
            this.Name = "DTMFForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DTMF Command";
            this.groupBoxDetails.ResumeLayout(false);
            this.groupBoxDetails.PerformLayout();
            this.groupBoxFile.ResumeLayout(false);
            this.groupBoxFile.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxDetails;
        private System.Windows.Forms.GroupBox groupBoxFile;
        private System.Windows.Forms.Button openExeFile;
        private System.Windows.Forms.TextBox fileTextBox;
        private System.Windows.Forms.Label labelFilename;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox textBoxDtmfSequence;
        private System.Windows.Forms.Label labelDtmf;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.Label labelArguments;
        private System.Windows.Forms.TextBox textBoxArguments;
    }
}