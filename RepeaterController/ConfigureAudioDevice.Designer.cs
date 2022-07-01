namespace RepeaterController
{
    partial class ConfigureAudioDevice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigureAudioDevice));
            this.okButton = new System.Windows.Forms.Button();
            this.labelInputAudioDevice = new System.Windows.Forms.Label();
            this.inputDeviceComboBox = new System.Windows.Forms.ComboBox();
            this.outputDeviceComboBox = new System.Windows.Forms.ComboBox();
            this.labelOutputAudioDevice = new System.Windows.Forms.Label();
            this.labelBufferLength = new System.Windows.Forms.Label();
            this.bufferLengthTextBox = new System.Windows.Forms.TextBox();
            this.dataBufferLengthTextBox = new System.Windows.Forms.TextBox();
            this.labelDataBufferLength = new System.Windows.Forms.Label();
            this.dataBufferCountTextBox = new System.Windows.Forms.TextBox();
            this.labelDataBufferCount = new System.Windows.Forms.Label();
            this.bufferCountTextBox = new System.Windows.Forms.TextBox();
            this.labelBufferCount = new System.Windows.Forms.Label();
            this.bufferDetailLabel = new System.Windows.Forms.Label();
            this.mdcBufferDetail = new System.Windows.Forms.Label();
            this.linkDeviceComboBox = new System.Windows.Forms.ComboBox();
            this.labelLinkAudioDevice = new System.Windows.Forms.Label();
            this.checkBoxSeparateLinkAudio = new System.Windows.Forms.CheckBox();
            this.checkBoxDisableFilter = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okButton.Location = new System.Drawing.Point(367, 175);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(59, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // labelInputAudioDevice
            // 
            this.labelInputAudioDevice.AutoSize = true;
            this.labelInputAudioDevice.Location = new System.Drawing.Point(12, 15);
            this.labelInputAudioDevice.Name = "labelInputAudioDevice";
            this.labelInputAudioDevice.Size = new System.Drawing.Size(127, 13);
            this.labelInputAudioDevice.TabIndex = 2;
            this.labelInputAudioDevice.Text = "Local Input Audio Device";
            // 
            // inputDeviceComboBox
            // 
            this.inputDeviceComboBox.FormattingEnabled = true;
            this.inputDeviceComboBox.Location = new System.Drawing.Point(145, 12);
            this.inputDeviceComboBox.Name = "inputDeviceComboBox";
            this.inputDeviceComboBox.Size = new System.Drawing.Size(281, 21);
            this.inputDeviceComboBox.TabIndex = 1;
            // 
            // outputDeviceComboBox
            // 
            this.outputDeviceComboBox.FormattingEnabled = true;
            this.outputDeviceComboBox.Location = new System.Drawing.Point(124, 89);
            this.outputDeviceComboBox.Name = "outputDeviceComboBox";
            this.outputDeviceComboBox.Size = new System.Drawing.Size(302, 21);
            this.outputDeviceComboBox.TabIndex = 2;
            // 
            // labelOutputAudioDevice
            // 
            this.labelOutputAudioDevice.AutoSize = true;
            this.labelOutputAudioDevice.Location = new System.Drawing.Point(12, 92);
            this.labelOutputAudioDevice.Name = "labelOutputAudioDevice";
            this.labelOutputAudioDevice.Size = new System.Drawing.Size(106, 13);
            this.labelOutputAudioDevice.TabIndex = 4;
            this.labelOutputAudioDevice.Text = "Output Audio Device";
            // 
            // labelBufferLength
            // 
            this.labelBufferLength.AutoSize = true;
            this.labelBufferLength.Location = new System.Drawing.Point(12, 119);
            this.labelBufferLength.Name = "labelBufferLength";
            this.labelBufferLength.Size = new System.Drawing.Size(71, 13);
            this.labelBufferLength.TabIndex = 6;
            this.labelBufferLength.Text = "Buffer Length";
            // 
            // bufferLengthTextBox
            // 
            this.bufferLengthTextBox.Location = new System.Drawing.Point(89, 116);
            this.bufferLengthTextBox.Name = "bufferLengthTextBox";
            this.bufferLengthTextBox.Size = new System.Drawing.Size(65, 20);
            this.bufferLengthTextBox.TabIndex = 3;
            // 
            // dataBufferLengthTextBox
            // 
            this.dataBufferLengthTextBox.Location = new System.Drawing.Point(116, 142);
            this.dataBufferLengthTextBox.Name = "dataBufferLengthTextBox";
            this.dataBufferLengthTextBox.Size = new System.Drawing.Size(65, 20);
            this.dataBufferLengthTextBox.TabIndex = 5;
            // 
            // labelDataBufferLength
            // 
            this.labelDataBufferLength.AutoSize = true;
            this.labelDataBufferLength.Location = new System.Drawing.Point(12, 145);
            this.labelDataBufferLength.Name = "labelDataBufferLength";
            this.labelDataBufferLength.Size = new System.Drawing.Size(97, 13);
            this.labelDataBufferLength.TabIndex = 8;
            this.labelDataBufferLength.Text = "Data Buffer Length";
            // 
            // dataBufferCountTextBox
            // 
            this.dataBufferCountTextBox.Location = new System.Drawing.Point(297, 142);
            this.dataBufferCountTextBox.Name = "dataBufferCountTextBox";
            this.dataBufferCountTextBox.Size = new System.Drawing.Size(65, 20);
            this.dataBufferCountTextBox.TabIndex = 6;
            // 
            // labelDataBufferCount
            // 
            this.labelDataBufferCount.AutoSize = true;
            this.labelDataBufferCount.Location = new System.Drawing.Point(193, 145);
            this.labelDataBufferCount.Name = "labelDataBufferCount";
            this.labelDataBufferCount.Size = new System.Drawing.Size(92, 13);
            this.labelDataBufferCount.TabIndex = 12;
            this.labelDataBufferCount.Text = "Data Buffer Count";
            // 
            // bufferCountTextBox
            // 
            this.bufferCountTextBox.Location = new System.Drawing.Point(270, 116);
            this.bufferCountTextBox.Name = "bufferCountTextBox";
            this.bufferCountTextBox.Size = new System.Drawing.Size(65, 20);
            this.bufferCountTextBox.TabIndex = 4;
            // 
            // labelBufferCount
            // 
            this.labelBufferCount.AutoSize = true;
            this.labelBufferCount.Location = new System.Drawing.Point(193, 119);
            this.labelBufferCount.Name = "labelBufferCount";
            this.labelBufferCount.Size = new System.Drawing.Size(66, 13);
            this.labelBufferCount.TabIndex = 10;
            this.labelBufferCount.Text = "Buffer Count";
            // 
            // bufferDetailLabel
            // 
            this.bufferDetailLabel.AutoSize = true;
            this.bufferDetailLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bufferDetailLabel.Location = new System.Drawing.Point(12, 167);
            this.bufferDetailLabel.Name = "bufferDetailLabel";
            this.bufferDetailLabel.Size = new System.Drawing.Size(51, 15);
            this.bufferDetailLabel.TabIndex = 14;
            this.bufferDetailLabel.Text = "bufDetail";
            // 
            // mdcBufferDetail
            // 
            this.mdcBufferDetail.AutoSize = true;
            this.mdcBufferDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mdcBufferDetail.Location = new System.Drawing.Point(12, 188);
            this.mdcBufferDetail.Name = "mdcBufferDetail";
            this.mdcBufferDetail.Size = new System.Drawing.Size(51, 15);
            this.mdcBufferDetail.TabIndex = 15;
            this.mdcBufferDetail.Text = "bufDetail";
            // 
            // linkDeviceComboBox
            // 
            this.linkDeviceComboBox.FormattingEnabled = true;
            this.linkDeviceComboBox.Location = new System.Drawing.Point(145, 62);
            this.linkDeviceComboBox.Name = "linkDeviceComboBox";
            this.linkDeviceComboBox.Size = new System.Drawing.Size(281, 21);
            this.linkDeviceComboBox.TabIndex = 16;
            // 
            // labelLinkAudioDevice
            // 
            this.labelLinkAudioDevice.AutoSize = true;
            this.labelLinkAudioDevice.Location = new System.Drawing.Point(12, 65);
            this.labelLinkAudioDevice.Name = "labelLinkAudioDevice";
            this.labelLinkAudioDevice.Size = new System.Drawing.Size(121, 13);
            this.labelLinkAudioDevice.TabIndex = 17;
            this.labelLinkAudioDevice.Text = "Link Input Audio Device";
            // 
            // checkBoxSeparateLinkAudio
            // 
            this.checkBoxSeparateLinkAudio.AutoSize = true;
            this.checkBoxSeparateLinkAudio.Location = new System.Drawing.Point(145, 39);
            this.checkBoxSeparateLinkAudio.Name = "checkBoxSeparateLinkAudio";
            this.checkBoxSeparateLinkAudio.Size = new System.Drawing.Size(122, 17);
            this.checkBoxSeparateLinkAudio.TabIndex = 18;
            this.checkBoxSeparateLinkAudio.Text = "Separate Link Audio";
            this.checkBoxSeparateLinkAudio.UseVisualStyleBackColor = true;
            // 
            // checkBoxDisableFilter
            // 
            this.checkBoxDisableFilter.AutoSize = true;
            this.checkBoxDisableFilter.Location = new System.Drawing.Point(15, 39);
            this.checkBoxDisableFilter.Name = "checkBoxDisableFilter";
            this.checkBoxDisableFilter.Size = new System.Drawing.Size(100, 17);
            this.checkBoxDisableFilter.TabIndex = 19;
            this.checkBoxDisableFilter.Text = "Disable Filtering";
            this.checkBoxDisableFilter.UseVisualStyleBackColor = true;
            // 
            // ConfigureAudioDevice
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.okButton;
            this.ClientSize = new System.Drawing.Size(438, 210);
            this.Controls.Add(this.checkBoxDisableFilter);
            this.Controls.Add(this.checkBoxSeparateLinkAudio);
            this.Controls.Add(this.linkDeviceComboBox);
            this.Controls.Add(this.labelLinkAudioDevice);
            this.Controls.Add(this.mdcBufferDetail);
            this.Controls.Add(this.bufferDetailLabel);
            this.Controls.Add(this.dataBufferCountTextBox);
            this.Controls.Add(this.labelDataBufferCount);
            this.Controls.Add(this.bufferCountTextBox);
            this.Controls.Add(this.labelBufferCount);
            this.Controls.Add(this.dataBufferLengthTextBox);
            this.Controls.Add(this.labelDataBufferLength);
            this.Controls.Add(this.bufferLengthTextBox);
            this.Controls.Add(this.labelBufferLength);
            this.Controls.Add(this.outputDeviceComboBox);
            this.Controls.Add(this.labelOutputAudioDevice);
            this.Controls.Add(this.inputDeviceComboBox);
            this.Controls.Add(this.labelInputAudioDevice);
            this.Controls.Add(this.okButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(454, 249);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(454, 249);
            this.Name = "ConfigureAudioDevice";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configure Audio Device";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label labelInputAudioDevice;
        private System.Windows.Forms.ComboBox inputDeviceComboBox;
        private System.Windows.Forms.ComboBox outputDeviceComboBox;
        private System.Windows.Forms.Label labelOutputAudioDevice;
        private System.Windows.Forms.Label labelBufferLength;
        private System.Windows.Forms.TextBox bufferLengthTextBox;
        private System.Windows.Forms.TextBox dataBufferLengthTextBox;
        private System.Windows.Forms.Label labelDataBufferLength;
        private System.Windows.Forms.TextBox dataBufferCountTextBox;
        private System.Windows.Forms.Label labelDataBufferCount;
        private System.Windows.Forms.TextBox bufferCountTextBox;
        private System.Windows.Forms.Label labelBufferCount;
        private System.Windows.Forms.Label bufferDetailLabel;
        private System.Windows.Forms.Label mdcBufferDetail;
        private System.Windows.Forms.ComboBox linkDeviceComboBox;
        private System.Windows.Forms.Label labelLinkAudioDevice;
        private System.Windows.Forms.CheckBox checkBoxSeparateLinkAudio;
        private System.Windows.Forms.CheckBox checkBoxDisableFilter;
    }
}