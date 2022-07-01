namespace RepeaterController.Announcements
{
    partial class AnnouncementForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnnouncementForm));
            this.groupBoxDetails = new System.Windows.Forms.GroupBox();
            this.dateTimePickerInterval = new System.Windows.Forms.DateTimePicker();
            this.checkBoxWave = new System.Windows.Forms.CheckBox();
            this.labelInterval = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.groupBoxFile = new System.Windows.Forms.GroupBox();
            this.speechText = new System.Windows.Forms.TextBox();
            this.radioButtonUseText = new System.Windows.Forms.RadioButton();
            this.radioButtonUseFile = new System.Windows.Forms.RadioButton();
            this.openAnncFile = new System.Windows.Forms.Button();
            this.anncFileTextBox = new System.Windows.Forms.TextBox();
            this.labelFilename = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.labelStart = new System.Windows.Forms.Label();
            this.groupBoxDetails.SuspendLayout();
            this.groupBoxFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxDetails
            // 
            this.groupBoxDetails.Controls.Add(this.dateTimePickerStart);
            this.groupBoxDetails.Controls.Add(this.labelStart);
            this.groupBoxDetails.Controls.Add(this.dateTimePickerInterval);
            this.groupBoxDetails.Controls.Add(this.checkBoxWave);
            this.groupBoxDetails.Controls.Add(this.labelInterval);
            this.groupBoxDetails.Controls.Add(this.textBoxName);
            this.groupBoxDetails.Controls.Add(this.labelName);
            this.groupBoxDetails.Location = new System.Drawing.Point(12, 12);
            this.groupBoxDetails.Name = "groupBoxDetails";
            this.groupBoxDetails.Size = new System.Drawing.Size(372, 100);
            this.groupBoxDetails.TabIndex = 15;
            this.groupBoxDetails.TabStop = false;
            // 
            // dateTimePickerInterval
            // 
            this.dateTimePickerInterval.CustomFormat = "HH:mm:ss";
            this.dateTimePickerInterval.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerInterval.Location = new System.Drawing.Point(208, 45);
            this.dateTimePickerInterval.Name = "dateTimePickerInterval";
            this.dateTimePickerInterval.ShowUpDown = true;
            this.dateTimePickerInterval.Size = new System.Drawing.Size(85, 20);
            this.dateTimePickerInterval.TabIndex = 22;
            this.dateTimePickerInterval.Value = new System.DateTime(1753, 1, 1, 0, 30, 0, 0);
            // 
            // checkBoxWave
            // 
            this.checkBoxWave.AutoSize = true;
            this.checkBoxWave.Location = new System.Drawing.Point(9, 71);
            this.checkBoxWave.Name = "checkBoxWave";
            this.checkBoxWave.Size = new System.Drawing.Size(159, 17);
            this.checkBoxWave.TabIndex = 21;
            this.checkBoxWave.Text = "Prerecorded Announcement";
            this.checkBoxWave.UseVisualStyleBackColor = true;
            // 
            // labelInterval
            // 
            this.labelInterval.AutoSize = true;
            this.labelInterval.Location = new System.Drawing.Point(157, 48);
            this.labelInterval.Name = "labelInterval";
            this.labelInterval.Size = new System.Drawing.Size(45, 13);
            this.labelInterval.TabIndex = 19;
            this.labelInterval.Text = "Interval:";
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
            this.groupBoxFile.Controls.Add(this.speechText);
            this.groupBoxFile.Controls.Add(this.radioButtonUseText);
            this.groupBoxFile.Controls.Add(this.radioButtonUseFile);
            this.groupBoxFile.Controls.Add(this.openAnncFile);
            this.groupBoxFile.Controls.Add(this.anncFileTextBox);
            this.groupBoxFile.Controls.Add(this.labelFilename);
            this.groupBoxFile.Location = new System.Drawing.Point(12, 118);
            this.groupBoxFile.Name = "groupBoxFile";
            this.groupBoxFile.Size = new System.Drawing.Size(372, 238);
            this.groupBoxFile.TabIndex = 16;
            this.groupBoxFile.TabStop = false;
            // 
            // speechText
            // 
            this.speechText.Location = new System.Drawing.Point(9, 68);
            this.speechText.Multiline = true;
            this.speechText.Name = "speechText";
            this.speechText.Size = new System.Drawing.Size(357, 164);
            this.speechText.TabIndex = 20;
            // 
            // radioButtonUseText
            // 
            this.radioButtonUseText.AutoSize = true;
            this.radioButtonUseText.Location = new System.Drawing.Point(303, 42);
            this.radioButtonUseText.Name = "radioButtonUseText";
            this.radioButtonUseText.Size = new System.Drawing.Size(46, 17);
            this.radioButtonUseText.TabIndex = 19;
            this.radioButtonUseText.Text = "Text";
            this.radioButtonUseText.UseVisualStyleBackColor = true;
            this.radioButtonUseText.CheckedChanged += new System.EventHandler(this.radioButtonUseText_CheckedChanged);
            // 
            // radioButtonUseFile
            // 
            this.radioButtonUseFile.AutoSize = true;
            this.radioButtonUseFile.Checked = true;
            this.radioButtonUseFile.Location = new System.Drawing.Point(303, 14);
            this.radioButtonUseFile.Name = "radioButtonUseFile";
            this.radioButtonUseFile.Size = new System.Drawing.Size(63, 17);
            this.radioButtonUseFile.TabIndex = 18;
            this.radioButtonUseFile.TabStop = true;
            this.radioButtonUseFile.Text = "Use File";
            this.radioButtonUseFile.UseVisualStyleBackColor = true;
            this.radioButtonUseFile.CheckedChanged += new System.EventHandler(this.radioButtonUseFile_CheckedChanged);
            // 
            // openAnncFile
            // 
            this.openAnncFile.Location = new System.Drawing.Point(64, 39);
            this.openAnncFile.Name = "openAnncFile";
            this.openAnncFile.Size = new System.Drawing.Size(75, 23);
            this.openAnncFile.TabIndex = 17;
            this.openAnncFile.Text = "Open File...";
            this.openAnncFile.UseVisualStyleBackColor = true;
            this.openAnncFile.Click += new System.EventHandler(this.openAnncFile_Click);
            // 
            // anncFileTextBox
            // 
            this.anncFileTextBox.Location = new System.Drawing.Point(64, 13);
            this.anncFileTextBox.Name = "anncFileTextBox";
            this.anncFileTextBox.Size = new System.Drawing.Size(150, 20);
            this.anncFileTextBox.TabIndex = 15;
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
            this.okButton.Location = new System.Drawing.Point(230, 362);
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
            this.cancelButton.Location = new System.Drawing.Point(311, 362);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 18;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // dateTimePickerStart
            // 
            this.dateTimePickerStart.CustomFormat = "hh:mm:ss tt";
            this.dateTimePickerStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerStart.Location = new System.Drawing.Point(64, 45);
            this.dateTimePickerStart.Name = "dateTimePickerStart";
            this.dateTimePickerStart.ShowUpDown = true;
            this.dateTimePickerStart.Size = new System.Drawing.Size(85, 20);
            this.dateTimePickerStart.TabIndex = 24;
            this.dateTimePickerStart.Value = new System.DateTime(1753, 1, 1, 0, 30, 0, 0);
            // 
            // labelStart
            // 
            this.labelStart.AutoSize = true;
            this.labelStart.Location = new System.Drawing.Point(13, 48);
            this.labelStart.Name = "labelStart";
            this.labelStart.Size = new System.Drawing.Size(32, 13);
            this.labelStart.TabIndex = 23;
            this.labelStart.Text = "Start:";
            // 
            // AnnouncementForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(398, 397);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.groupBoxFile);
            this.Controls.Add(this.groupBoxDetails);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(414, 436);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(414, 436);
            this.Name = "AnnouncementForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Announcement";
            this.groupBoxDetails.ResumeLayout(false);
            this.groupBoxDetails.PerformLayout();
            this.groupBoxFile.ResumeLayout(false);
            this.groupBoxFile.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxDetails;
        private System.Windows.Forms.GroupBox groupBoxFile;
        private System.Windows.Forms.Button openAnncFile;
        private System.Windows.Forms.TextBox anncFileTextBox;
        private System.Windows.Forms.Label labelFilename;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label labelInterval;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.CheckBox checkBoxWave;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.DateTimePicker dateTimePickerInterval;
        private System.Windows.Forms.TextBox speechText;
        private System.Windows.Forms.RadioButton radioButtonUseText;
        private System.Windows.Forms.RadioButton radioButtonUseFile;
        private System.Windows.Forms.DateTimePicker dateTimePickerStart;
        private System.Windows.Forms.Label labelStart;
    }
}