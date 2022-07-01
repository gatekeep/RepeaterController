namespace RepeaterController
{
    partial class MultitoneForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultitoneForm));
            this.groupBoxDetails = new System.Windows.Forms.GroupBox();
            this.textBoxSilenceLength = new System.Windows.Forms.TextBox();
            this.labelSilenceLength = new System.Windows.Forms.Label();
            this.textBoxLength = new System.Windows.Forms.TextBox();
            this.labelLength = new System.Windows.Forms.Label();
            this.textBoxFrequency = new System.Windows.Forms.TextBox();
            this.labelFrequency = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.labelIndex = new System.Windows.Forms.Label();
            this.numericUpDownIndex = new System.Windows.Forms.NumericUpDown();
            this.textBoxFrequency2 = new System.Windows.Forms.TextBox();
            this.groupBoxDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIndex)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxDetails
            // 
            this.groupBoxDetails.Controls.Add(this.textBoxFrequency2);
            this.groupBoxDetails.Controls.Add(this.textBoxSilenceLength);
            this.groupBoxDetails.Controls.Add(this.labelSilenceLength);
            this.groupBoxDetails.Controls.Add(this.textBoxLength);
            this.groupBoxDetails.Controls.Add(this.labelLength);
            this.groupBoxDetails.Controls.Add(this.textBoxFrequency);
            this.groupBoxDetails.Controls.Add(this.labelFrequency);
            this.groupBoxDetails.Location = new System.Drawing.Point(12, 12);
            this.groupBoxDetails.Name = "groupBoxDetails";
            this.groupBoxDetails.Size = new System.Drawing.Size(226, 100);
            this.groupBoxDetails.TabIndex = 15;
            this.groupBoxDetails.TabStop = false;
            // 
            // textBoxSilenceLength
            // 
            this.textBoxSilenceLength.Location = new System.Drawing.Point(93, 71);
            this.textBoxSilenceLength.Name = "textBoxSilenceLength";
            this.textBoxSilenceLength.Size = new System.Drawing.Size(52, 20);
            this.textBoxSilenceLength.TabIndex = 4;
            // 
            // labelSilenceLength
            // 
            this.labelSilenceLength.AutoSize = true;
            this.labelSilenceLength.Location = new System.Drawing.Point(6, 74);
            this.labelSilenceLength.Name = "labelSilenceLength";
            this.labelSilenceLength.Size = new System.Drawing.Size(81, 13);
            this.labelSilenceLength.TabIndex = 21;
            this.labelSilenceLength.Text = "Silence Length:";
            // 
            // textBoxLength
            // 
            this.textBoxLength.Location = new System.Drawing.Point(55, 45);
            this.textBoxLength.Name = "textBoxLength";
            this.textBoxLength.Size = new System.Drawing.Size(52, 20);
            this.textBoxLength.TabIndex = 3;
            // 
            // labelLength
            // 
            this.labelLength.AutoSize = true;
            this.labelLength.Location = new System.Drawing.Point(6, 48);
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new System.Drawing.Size(43, 13);
            this.labelLength.TabIndex = 19;
            this.labelLength.Text = "Length:";
            // 
            // textBoxFrequency
            // 
            this.textBoxFrequency.Location = new System.Drawing.Point(72, 19);
            this.textBoxFrequency.Name = "textBoxFrequency";
            this.textBoxFrequency.Size = new System.Drawing.Size(65, 20);
            this.textBoxFrequency.TabIndex = 1;
            // 
            // labelFrequency
            // 
            this.labelFrequency.AutoSize = true;
            this.labelFrequency.Location = new System.Drawing.Point(6, 22);
            this.labelFrequency.Name = "labelFrequency";
            this.labelFrequency.Size = new System.Drawing.Size(60, 13);
            this.labelFrequency.TabIndex = 0;
            this.labelFrequency.Text = "Frequency:";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(82, 144);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 6;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(163, 144);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // labelIndex
            // 
            this.labelIndex.AutoSize = true;
            this.labelIndex.Location = new System.Drawing.Point(18, 120);
            this.labelIndex.Name = "labelIndex";
            this.labelIndex.Size = new System.Drawing.Size(64, 13);
            this.labelIndex.TabIndex = 23;
            this.labelIndex.Text = "Tone Index:";
            // 
            // numericUpDownIndex
            // 
            this.numericUpDownIndex.Location = new System.Drawing.Point(88, 118);
            this.numericUpDownIndex.Name = "numericUpDownIndex";
            this.numericUpDownIndex.Size = new System.Drawing.Size(69, 20);
            this.numericUpDownIndex.TabIndex = 5;
            // 
            // textBoxFrequency2
            // 
            this.textBoxFrequency2.Location = new System.Drawing.Point(143, 19);
            this.textBoxFrequency2.Name = "textBoxFrequency2";
            this.textBoxFrequency2.Size = new System.Drawing.Size(65, 20);
            this.textBoxFrequency2.TabIndex = 2;
            // 
            // MultitoneForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(246, 174);
            this.Controls.Add(this.numericUpDownIndex);
            this.Controls.Add(this.labelIndex);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.groupBoxDetails);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(262, 213);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(262, 213);
            this.Name = "MultitoneForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Multi-Tone Tone";
            this.groupBoxDetails.ResumeLayout(false);
            this.groupBoxDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIndex)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxDetails;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox textBoxFrequency;
        private System.Windows.Forms.Label labelFrequency;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox textBoxLength;
        private System.Windows.Forms.Label labelLength;
        private System.Windows.Forms.TextBox textBoxSilenceLength;
        private System.Windows.Forms.Label labelSilenceLength;
        private System.Windows.Forms.Label labelIndex;
        private System.Windows.Forms.NumericUpDown numericUpDownIndex;
        private System.Windows.Forms.TextBox textBoxFrequency2;
    }
}