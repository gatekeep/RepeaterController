namespace RepeaterController.Announcements
{
    partial class AnnouncementWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnnouncementWindow));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.automatedTimeAnnouncementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.everyHalfHourToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.everyHourToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.every3HoursToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.every12HoursToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.greetingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.militaryTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playTimeAnnouncmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.useCourtesyToneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addEditAnnouncmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAnnouncmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executeAnnouncmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripAddEditButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripDeleteButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripExecuteButton = new System.Windows.Forms.ToolStripButton();
            this.entryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entryDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startRun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastRun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nextRun = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.optionsToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(721, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.automatedTimeAnnouncementToolStripMenuItem,
            this.playTimeAnnouncmentToolStripMenuItem,
            this.toolStripMenuItem2,
            this.useCourtesyToneToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // automatedTimeAnnouncementToolStripMenuItem
            // 
            this.automatedTimeAnnouncementToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.everyHalfHourToolStripMenuItem,
            this.everyHourToolStripMenuItem,
            this.every3HoursToolStripMenuItem,
            this.every12HoursToolStripMenuItem,
            this.toolStripMenuItem1,
            this.greetingToolStripMenuItem,
            this.militaryTimeToolStripMenuItem});
            this.automatedTimeAnnouncementToolStripMenuItem.Name = "automatedTimeAnnouncementToolStripMenuItem";
            this.automatedTimeAnnouncementToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.automatedTimeAnnouncementToolStripMenuItem.Text = "Automated Time Announcement";
            this.automatedTimeAnnouncementToolStripMenuItem.Click += new System.EventHandler(this.automatedTimeAnnouncementToolStripMenuItem_Click);
            // 
            // everyHalfHourToolStripMenuItem
            // 
            this.everyHalfHourToolStripMenuItem.Name = "everyHalfHourToolStripMenuItem";
            this.everyHalfHourToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.everyHalfHourToolStripMenuItem.Text = "Every Half Hour";
            this.everyHalfHourToolStripMenuItem.Click += new System.EventHandler(this.everyHalfHourToolStripMenuItem_Click);
            // 
            // everyHourToolStripMenuItem
            // 
            this.everyHourToolStripMenuItem.Name = "everyHourToolStripMenuItem";
            this.everyHourToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.everyHourToolStripMenuItem.Text = "Every Hour";
            this.everyHourToolStripMenuItem.Click += new System.EventHandler(this.everyHourToolStripMenuItem_Click);
            // 
            // every3HoursToolStripMenuItem
            // 
            this.every3HoursToolStripMenuItem.Name = "every3HoursToolStripMenuItem";
            this.every3HoursToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.every3HoursToolStripMenuItem.Text = "Every 3 Hours";
            this.every3HoursToolStripMenuItem.Click += new System.EventHandler(this.every3HoursToolStripMenuItem_Click);
            // 
            // every12HoursToolStripMenuItem
            // 
            this.every12HoursToolStripMenuItem.Name = "every12HoursToolStripMenuItem";
            this.every12HoursToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.every12HoursToolStripMenuItem.Text = "Every 12 Hours";
            this.every12HoursToolStripMenuItem.Click += new System.EventHandler(this.every12HoursToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(154, 6);
            // 
            // greetingToolStripMenuItem
            // 
            this.greetingToolStripMenuItem.Name = "greetingToolStripMenuItem";
            this.greetingToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.greetingToolStripMenuItem.Text = "Greeting";
            this.greetingToolStripMenuItem.Click += new System.EventHandler(this.greetingToolStripMenuItem_Click);
            // 
            // militaryTimeToolStripMenuItem
            // 
            this.militaryTimeToolStripMenuItem.Name = "militaryTimeToolStripMenuItem";
            this.militaryTimeToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.militaryTimeToolStripMenuItem.Text = "Military Time";
            this.militaryTimeToolStripMenuItem.Click += new System.EventHandler(this.militaryTimeToolStripMenuItem_Click);
            // 
            // playTimeAnnouncmentToolStripMenuItem
            // 
            this.playTimeAnnouncmentToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("playTimeAnnouncmentToolStripMenuItem.Image")));
            this.playTimeAnnouncmentToolStripMenuItem.Name = "playTimeAnnouncmentToolStripMenuItem";
            this.playTimeAnnouncmentToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.playTimeAnnouncmentToolStripMenuItem.Text = "Play Time Announcment";
            this.playTimeAnnouncmentToolStripMenuItem.Click += new System.EventHandler(this.playTimeAnnouncmentToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(247, 6);
            // 
            // useCourtesyToneToolStripMenuItem
            // 
            this.useCourtesyToneToolStripMenuItem.Name = "useCourtesyToneToolStripMenuItem";
            this.useCourtesyToneToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.useCourtesyToneToolStripMenuItem.Text = "Use Courtesy Tone";
            this.useCourtesyToneToolStripMenuItem.Click += new System.EventHandler(this.useCourtesyToneToolStripMenuItem_Click);
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
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(721, 373);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.Location = new System.Drawing.Point(0, 24);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.Size = new System.Drawing.Size(721, 398);
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
            this.entryDescription,
            this.startRun,
            this.lastRun,
            this.nextRun});
            this.dataGridView.ContextMenuStrip = this.contextMenuStrip;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.Size = new System.Drawing.Size(721, 373);
            this.dataGridView.TabIndex = 0;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addEditAnnouncmentToolStripMenuItem,
            this.deleteAnnouncmentToolStripMenuItem,
            this.executeAnnouncmentToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(211, 70);
            // 
            // addEditAnnouncmentToolStripMenuItem
            // 
            this.addEditAnnouncmentToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addEditAnnouncmentToolStripMenuItem.Image")));
            this.addEditAnnouncmentToolStripMenuItem.Name = "addEditAnnouncmentToolStripMenuItem";
            this.addEditAnnouncmentToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.addEditAnnouncmentToolStripMenuItem.Text = "Add/Edit Announcment...";
            this.addEditAnnouncmentToolStripMenuItem.Click += new System.EventHandler(this.addEditAnnouncmentToolStripMenuItem_Click);
            // 
            // deleteAnnouncmentToolStripMenuItem
            // 
            this.deleteAnnouncmentToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteAnnouncmentToolStripMenuItem.Image")));
            this.deleteAnnouncmentToolStripMenuItem.Name = "deleteAnnouncmentToolStripMenuItem";
            this.deleteAnnouncmentToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.deleteAnnouncmentToolStripMenuItem.Text = "Delete Announcment";
            this.deleteAnnouncmentToolStripMenuItem.Click += new System.EventHandler(this.deleteAnnouncmentToolStripMenuItem_Click);
            // 
            // executeAnnouncmentToolStripMenuItem
            // 
            this.executeAnnouncmentToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("executeAnnouncmentToolStripMenuItem.Image")));
            this.executeAnnouncmentToolStripMenuItem.Name = "executeAnnouncmentToolStripMenuItem";
            this.executeAnnouncmentToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.executeAnnouncmentToolStripMenuItem.Text = "Execute Announcment";
            this.executeAnnouncmentToolStripMenuItem.Click += new System.EventHandler(this.executeAnnouncmentToolStripMenuItem_Click);
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
            this.toolStrip1.Size = new System.Drawing.Size(221, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripAddEditButton
            // 
            this.toolStripAddEditButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripAddEditButton.Image")));
            this.toolStripAddEditButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAddEditButton.Name = "toolStripAddEditButton";
            this.toolStripAddEditButton.Size = new System.Drawing.Size(163, 22);
            this.toolStripAddEditButton.Text = "Add/Edit Announcment...";
            this.toolStripAddEditButton.Click += new System.EventHandler(this.toolStripAddEditButton_Click);
            // 
            // toolStripDeleteButton
            // 
            this.toolStripDeleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDeleteButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDeleteButton.Image")));
            this.toolStripDeleteButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDeleteButton.Name = "toolStripDeleteButton";
            this.toolStripDeleteButton.Size = new System.Drawing.Size(23, 22);
            this.toolStripDeleteButton.Text = "Delete Announcement";
            this.toolStripDeleteButton.Click += new System.EventHandler(this.toolStripDeleteButton_Click);
            // 
            // toolStripExecuteButton
            // 
            this.toolStripExecuteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripExecuteButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripExecuteButton.Image")));
            this.toolStripExecuteButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripExecuteButton.Name = "toolStripExecuteButton";
            this.toolStripExecuteButton.Size = new System.Drawing.Size(23, 22);
            this.toolStripExecuteButton.Text = "Execute Announcement";
            this.toolStripExecuteButton.Click += new System.EventHandler(this.toolStripExecuteButton_Click);
            // 
            // entryName
            // 
            this.entryName.HeaderText = "Name";
            this.entryName.Name = "entryName";
            this.entryName.ReadOnly = true;
            this.entryName.Width = 128;
            // 
            // entryDescription
            // 
            this.entryDescription.HeaderText = "Interval";
            this.entryDescription.Name = "entryDescription";
            this.entryDescription.ReadOnly = true;
            // 
            // startRun
            // 
            this.startRun.HeaderText = "Start Run";
            this.startRun.Name = "startRun";
            this.startRun.ReadOnly = true;
            this.startRun.Width = 150;
            // 
            // lastRun
            // 
            this.lastRun.HeaderText = "Last Run";
            this.lastRun.Name = "lastRun";
            this.lastRun.ReadOnly = true;
            this.lastRun.Width = 150;
            // 
            // nextRun
            // 
            this.nextRun.HeaderText = "Next Run";
            this.nextRun.Name = "nextRun";
            this.nextRun.ReadOnly = true;
            this.nextRun.Width = 150;
            // 
            // AnnouncementWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 422);
            this.Controls.Add(this.toolStripContainer);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(737, 461);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(737, 461);
            this.Name = "AnnouncementWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RepeaterController - Announcements";
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
        private System.Windows.Forms.ToolStripMenuItem addEditAnnouncmentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteAnnouncmentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem executeAnnouncmentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem automatedTimeAnnouncementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playTimeAnnouncmentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem everyHourToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem every3HoursToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem every12HoursToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem greetingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem everyHalfHourToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem useCourtesyToneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem militaryTimeToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn entryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn entryDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn startRun;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastRun;
        private System.Windows.Forms.DataGridViewTextBoxColumn nextRun;
    }
}