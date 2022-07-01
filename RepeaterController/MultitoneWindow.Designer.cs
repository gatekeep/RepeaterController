namespace RepeaterController
{
    partial class MultitoneWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultitoneWindow));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addEditToneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripAddEditButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripDeleteButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripPlayButton = new System.Windows.Forms.ToolStripButton();
            this.index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entryFreq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entryFreq2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.silenceLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1.SuspendLayout();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(572, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.Controls.Add(this.dataGridView);
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(572, 373);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.Location = new System.Drawing.Point(0, 24);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.Size = new System.Drawing.Size(572, 398);
            this.toolStripContainer.TabIndex = 1;
            this.toolStripContainer.Text = "toolStripContainer1";
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.index,
            this.entryFreq,
            this.entryFreq2,
            this.length,
            this.silenceLength});
            this.dataGridView.ContextMenuStrip = this.contextMenuStrip;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.Size = new System.Drawing.Size(572, 373);
            this.dataGridView.TabIndex = 0;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addEditToneToolStripMenuItem,
            this.deleteToneToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(151, 48);
            // 
            // addEditToneToolStripMenuItem
            // 
            this.addEditToneToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addEditToneToolStripMenuItem.Image")));
            this.addEditToneToolStripMenuItem.Name = "addEditToneToolStripMenuItem";
            this.addEditToneToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.addEditToneToolStripMenuItem.Text = "Add/Edit Tone";
            this.addEditToneToolStripMenuItem.Click += new System.EventHandler(this.addEditToneToolStripMenuItem_Click);
            // 
            // deleteToneToolStripMenuItem
            // 
            this.deleteToneToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteToneToolStripMenuItem.Image")));
            this.deleteToneToolStripMenuItem.Name = "deleteToneToolStripMenuItem";
            this.deleteToneToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.deleteToneToolStripMenuItem.Text = "Delete Tone";
            this.deleteToneToolStripMenuItem.Click += new System.EventHandler(this.deleteToneToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripAddEditButton,
            this.toolStripDeleteButton,
            this.toolStripPlayButton});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(170, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripAddEditButton
            // 
            this.toolStripAddEditButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripAddEditButton.Image")));
            this.toolStripAddEditButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAddEditButton.Name = "toolStripAddEditButton";
            this.toolStripAddEditButton.Size = new System.Drawing.Size(112, 22);
            this.toolStripAddEditButton.Text = "Add/Edit Tone...";
            this.toolStripAddEditButton.Click += new System.EventHandler(this.toolStripAddEditButton_Click);
            // 
            // toolStripDeleteButton
            // 
            this.toolStripDeleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDeleteButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDeleteButton.Image")));
            this.toolStripDeleteButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDeleteButton.Name = "toolStripDeleteButton";
            this.toolStripDeleteButton.Size = new System.Drawing.Size(23, 22);
            this.toolStripDeleteButton.Text = "Delete Tone";
            this.toolStripDeleteButton.Click += new System.EventHandler(this.toolStripDeleteButton_Click);
            // 
            // toolStripPlayButton
            // 
            this.toolStripPlayButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripPlayButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripPlayButton.Image")));
            this.toolStripPlayButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripPlayButton.Name = "toolStripPlayButton";
            this.toolStripPlayButton.Size = new System.Drawing.Size(23, 22);
            this.toolStripPlayButton.Text = "Play Multitone Sequence";
            this.toolStripPlayButton.Click += new System.EventHandler(this.toolStripPlayButton_Click);
            // 
            // index
            // 
            this.index.HeaderText = "Index";
            this.index.Name = "index";
            this.index.ReadOnly = true;
            this.index.Width = 150;
            // 
            // entryFreq
            // 
            this.entryFreq.HeaderText = "Frequency";
            this.entryFreq.Name = "entryFreq";
            this.entryFreq.ReadOnly = true;
            this.entryFreq.Width = 128;
            // 
            // entryFreq2
            // 
            this.entryFreq2.HeaderText = "Func Frequency";
            this.entryFreq2.Name = "entryFreq2";
            this.entryFreq2.ReadOnly = true;
            this.entryFreq2.Width = 128;
            // 
            // length
            // 
            this.length.HeaderText = "Length";
            this.length.Name = "length";
            this.length.ReadOnly = true;
            // 
            // silenceLength
            // 
            this.silenceLength.HeaderText = "Silence Length";
            this.silenceLength.Name = "silenceLength";
            this.silenceLength.ReadOnly = true;
            this.silenceLength.Width = 150;
            // 
            // MultitoneWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 422);
            this.Controls.Add(this.toolStripContainer);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(588, 461);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(588, 461);
            this.Name = "MultitoneWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RepeaterController - Multitone";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripAddEditButton;
        private System.Windows.Forms.ToolStripButton toolStripDeleteButton;
        private System.Windows.Forms.ToolStripButton toolStripPlayButton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addEditToneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToneToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn index;
        private System.Windows.Forms.DataGridViewTextBoxColumn entryFreq;
        private System.Windows.Forms.DataGridViewTextBoxColumn entryFreq2;
        private System.Windows.Forms.DataGridViewTextBoxColumn length;
        private System.Windows.Forms.DataGridViewTextBoxColumn silenceLength;
    }
}