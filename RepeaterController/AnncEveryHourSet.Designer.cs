namespace RepeaterController
{
    partial class AnncEveryHourSet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnncEveryHourSet));
            this.labelTime = new System.Windows.Forms.Label();
            this.setButton = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.labelNext = new System.Windows.Forms.Label();
            this.labelNextHour = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(12, 15);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(63, 13);
            this.labelTime.TabIndex = 0;
            this.labelTime.Text = "Every Hour:";
            // 
            // setButton
            // 
            this.setButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.setButton.Location = new System.Drawing.Point(241, 10);
            this.setButton.Name = "setButton";
            this.setButton.Size = new System.Drawing.Size(75, 23);
            this.setButton.TabIndex = 0;
            this.setButton.Text = "Set";
            this.setButton.UseVisualStyleBackColor = true;
            this.setButton.Click += new System.EventHandler(this.setButton_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "hh:mm:ss tt";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(81, 12);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.ShowUpDown = true;
            this.dateTimePicker1.Size = new System.Drawing.Size(97, 20);
            this.dateTimePicker1.TabIndex = 1;
            // 
            // labelNext
            // 
            this.labelNext.AutoSize = true;
            this.labelNext.Location = new System.Drawing.Point(12, 42);
            this.labelNext.Name = "labelNext";
            this.labelNext.Size = new System.Drawing.Size(58, 13);
            this.labelNext.TabIndex = 2;
            this.labelNext.Text = "Next Hour:";
            // 
            // labelNextHour
            // 
            this.labelNextHour.AutoSize = true;
            this.labelNextHour.Location = new System.Drawing.Point(81, 42);
            this.labelNextHour.Name = "labelNextHour";
            this.labelNextHour.Size = new System.Drawing.Size(50, 13);
            this.labelNextHour.TabIndex = 3;
            this.labelNextHour.Text = "nextHour";
            // 
            // AnncEveryHourSet
            // 
            this.AcceptButton = this.setButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 74);
            this.Controls.Add(this.labelNextHour);
            this.Controls.Add(this.labelNext);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.setButton);
            this.Controls.Add(this.labelTime);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(344, 83);
            this.Name = "AnncEveryHourSet";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set \'Every Hour\'...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Button setButton;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label labelNext;
        private System.Windows.Forms.Label labelNextHour;
    }
}