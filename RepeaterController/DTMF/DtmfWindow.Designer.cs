namespace RepeaterController.DSP.DTMF
{
    partial class DTMFWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DTMFWindow));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.entryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtmfSequence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.instruction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.command = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.argument = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addEditDTMFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteDTMFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executeDTMFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripAddEditButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripDeleteButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripExecuteButton = new System.Windows.Forms.ToolStripButton();
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
            this.menuStrip1.Size = new System.Drawing.Size(722, 24);
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
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(722, 373);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.Location = new System.Drawing.Point(0, 24);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.Size = new System.Drawing.Size(722, 398);
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
            this.entryName,
            this.dtmfSequence,
            this.instruction,
            this.command,
            this.argument});
            this.dataGridView.ContextMenuStrip = this.contextMenuStrip;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.Size = new System.Drawing.Size(722, 373);
            this.dataGridView.TabIndex = 0;
            // 
            // entryName
            // 
            this.entryName.HeaderText = "Name";
            this.entryName.Name = "entryName";
            this.entryName.ReadOnly = true;
            this.entryName.Width = 128;
            // 
            // dtmfSequence
            // 
            this.dtmfSequence.HeaderText = "DTMF Seq.";
            this.dtmfSequence.Name = "dtmfSequence";
            this.dtmfSequence.ReadOnly = true;
            // 
            // instruction
            // 
            this.instruction.HeaderText = "Instruction";
            this.instruction.Name = "instruction";
            this.instruction.ReadOnly = true;
            this.instruction.Width = 150;
            // 
            // command
            // 
            this.command.HeaderText = "Command";
            this.command.Name = "command";
            this.command.ReadOnly = true;
            this.command.Width = 150;
            // 
            // argument
            // 
            this.argument.HeaderText = "Arguments";
            this.argument.Name = "argument";
            this.argument.ReadOnly = true;
            this.argument.Width = 150;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addEditDTMFToolStripMenuItem,
            this.deleteDTMFToolStripMenuItem,
            this.executeDTMFToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(216, 70);
            // 
            // addEditDTMFToolStripMenuItem
            // 
            this.addEditDTMFToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addEditDTMFToolStripMenuItem.Image")));
            this.addEditDTMFToolStripMenuItem.Name = "addEditDTMFToolStripMenuItem";
            this.addEditDTMFToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.addEditDTMFToolStripMenuItem.Text = "Add/Edit DTMF Command";
            this.addEditDTMFToolStripMenuItem.Click += new System.EventHandler(this.addEditDTMFToolStripMenuItem_Click);
            // 
            // deleteDTMFToolStripMenuItem
            // 
            this.deleteDTMFToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteDTMFToolStripMenuItem.Image")));
            this.deleteDTMFToolStripMenuItem.Name = "deleteDTMFToolStripMenuItem";
            this.deleteDTMFToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.deleteDTMFToolStripMenuItem.Text = "Delete DTMF Command";
            this.deleteDTMFToolStripMenuItem.Click += new System.EventHandler(this.deleteDTMFToolStripMenuItem_Click);
            // 
            // executeDTMFToolStripMenuItem
            // 
            this.executeDTMFToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("executeDTMFToolStripMenuItem.Image")));
            this.executeDTMFToolStripMenuItem.Name = "executeDTMFToolStripMenuItem";
            this.executeDTMFToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.executeDTMFToolStripMenuItem.Text = "Execute DTMF Command";
            this.executeDTMFToolStripMenuItem.Click += new System.EventHandler(this.executeDTMFToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripAddEditButton,
            this.toolStripDeleteButton,
            this.toolStripExecuteButton});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(266, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripAddEditButton
            // 
            this.toolStripAddEditButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripAddEditButton.Image")));
            this.toolStripAddEditButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAddEditButton.Name = "toolStripAddEditButton";
            this.toolStripAddEditButton.Size = new System.Drawing.Size(177, 22);
            this.toolStripAddEditButton.Text = "Add/Edit DTMF Command...";
            this.toolStripAddEditButton.Click += new System.EventHandler(this.toolStripAddEditButton_Click);
            // 
            // toolStripDeleteButton
            // 
            this.toolStripDeleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDeleteButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDeleteButton.Image")));
            this.toolStripDeleteButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDeleteButton.Name = "toolStripDeleteButton";
            this.toolStripDeleteButton.Size = new System.Drawing.Size(23, 22);
            this.toolStripDeleteButton.Text = "Delete DTMF Command";
            this.toolStripDeleteButton.Click += new System.EventHandler(this.toolStripDeleteButton_Click);
            // 
            // toolStripExecuteButton
            // 
            this.toolStripExecuteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripExecuteButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripExecuteButton.Image")));
            this.toolStripExecuteButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripExecuteButton.Name = "toolStripExecuteButton";
            this.toolStripExecuteButton.Size = new System.Drawing.Size(23, 22);
            this.toolStripExecuteButton.Text = "Execute DTMF Command";
            this.toolStripExecuteButton.Click += new System.EventHandler(this.toolStripExecuteButton_Click);
            // 
            // DTMFWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 422);
            this.Controls.Add(this.toolStripContainer);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(738, 461);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(738, 461);
            this.Name = "DTMFWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RepeaterController - DTMF Commands";
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
        private System.Windows.Forms.ToolStripButton toolStripExecuteButton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addEditDTMFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteDTMFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem executeDTMFToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn entryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dtmfSequence;
        private System.Windows.Forms.DataGridViewTextBoxColumn instruction;
        private System.Windows.Forms.DataGridViewTextBoxColumn command;
        private System.Windows.Forms.DataGridViewTextBoxColumn argument;
    }
}