/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

using RepeaterController.Announcements;
using RepeaterController.Xml;

namespace RepeaterController.DSP.DTMF
{
    /// <summary>
    /// 
    /// </summary>
    public enum DtmfInstruction
    {
        External,
        RepeaterKnockdown,
        PlayTimeAnnouncement,
        PlayAnnouncement,
    } // public enum DtmfInstruction

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public struct dtmf_ret_t
    {
        /**
         * Fields
         */
        /// <summary>
        /// Name of the announcement.
        /// </summary>
        [DataMember]
        public string Name;
        /// <summary>
        /// DTMF Sequence
        /// </summary>
        [DataMember]
        public string DTMFSequence;
        /// <summary>
        /// Instruction to execute.
        /// </summary>
        [DataMember]
        public DtmfInstruction Instruction;
        /// <summary>
        /// Name of the executable file to execute.
        /// </summary>
        [DataMember]
        public string Filename;
        /// <summary>
        /// Executable arguments.
        /// </summary>
        [DataMember]
        public string Arguments;
    } // public struct dtmf_ret_t

    /// <summary>
    /// Implements the management of DTMF commands.
    /// </summary>
    public partial class DTMFWindow : Form
    {
        /**
         * Fields
         */
        private MainWindow wnd;
        private XmlResource rsrc;
        private bool shown = false;
        private List<dtmf_ret_t> commands;

        private string toneSequence = string.Empty;

        /// <summary>
        /// Flag indicating whether the form was canceled.
        /// </summary>
        public bool Canceled;

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="DTMFWindow"/> class.
        /// </summary>
        /// <param name="wnd"></param>
        public DTMFWindow(MainWindow wnd)
        {
            InitializeComponent();

            this.wnd = wnd;
            wnd.DTMFToneDetected += Wnd_DTMFToneDetected;

            this.deleteDTMFToolStripMenuItem.Enabled = false;
            this.toolStripDeleteButton.Enabled = false;
            this.executeDTMFToolStripMenuItem.Enabled = false;
            this.toolStripExecuteButton.Enabled = false;

            this.Shown += DTMFWindow_Shown;
            this.FormClosing += DTMFWindow_FormClosing;

            this.dataGridView.SelectionChanged += DataGridView_SelectionChanged;
            this.dataGridView.MouseDown += DataGridView_MouseDown;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DTMFWindow"/> class.
        /// </summary>
        /// <param name="rsrc"></param>
        /// <param name="wnd"></param>
        public DTMFWindow(XmlResource rsrc, MainWindow wnd) : this(wnd)
        {
            this.rsrc = rsrc;

            LoadXml();
            UpdateModal();
        }

        /// <summary>
        /// Helper to load/generate the user XML configuration data.
        /// </summary>
        public void LoadXml()
        {
            if (rsrc.HasNode("DTMFConfig"))
            {
                XmlResource dtmfConfig = rsrc.Get<XmlResource>("DTMFConfig");

                // get data from XML
                try
                {
                    this.commands = rsrc.Get<List<dtmf_ret_t>>("Commands");
                }
                catch (ArgumentException ae)
                {
                    // for announcements we'll just reset
                    Messages.TraceException(ae.Message, ae);
                    SetDefaults();
                    SaveXml();
                }
            }
            else
            {
                // reset defaults
                SetDefaults();
                SaveXml();
            }
            UpdateModal();
        }

        /// <summary>
        /// Helper to save the XML.
        /// </summary>
        public void SaveXml()
        {
            XmlResource dtmfConfig = rsrc.CreateNode("DTMFConfig");
            dtmfConfig.Submit("Commands", commands);

            rsrc.SaveXml(Environment.CurrentDirectory + Path.DirectorySeparatorChar + MainWindow.XML_FILE);
        }

        /// <summary>
        /// Event that occurs when a DTMF tone is detected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="tone"></param>
        private void Wnd_DTMFToneDetected(object sender, DtmfToneEnd tone)
        {
            // no function if MDC console
            if (wnd.RepeaterOptions.Options.MDCConsoleOnly && !wnd.RepeaterOptions.Options.AllowConsoleAnncDTMF)
                return;

            toneSequence += tone.DtmfTone.KeyString;

            lock (commands)
            {
                foreach (dtmf_ret_t a in commands)
                    if (a.DTMFSequence == toneSequence)
                    {
                        wnd.Invoke(new Action(() =>
                        {
                            wnd.NewLogMessage("Detected DTMF Sequence: " + toneSequence + ", " + a.Instruction.ToString());
                        }));

                        if (a.Instruction != DtmfInstruction.RepeaterKnockdown)
                        {
                            // wait for repeater to stop Tx before executing command
                            do
                            {
                                ;
                            } while (MainWindow.IsRepeaterTx);
                        }

                        ExecuteDTMFCommand(a);
                        toneSequence = string.Empty;
                        break;
                    }
            }
        }

        /// <summary>
        /// Convenience function to execute arbitrary programs, using the specified full
        /// command path on the system shell.
        /// </summary>
        /// <param name='proc_name'>Command to execute</param>
        /// <param name='arg'>Arguments to command</param>
        private void ExecProc(string proc_name, string arg)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = proc_name;
            proc.StartInfo.Arguments = arg;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;

            proc.Start();
            proc.WaitForExit();
            return;
        }

        /// <summary>
        /// Helper to execute a DTMF command.
        /// </summary>
        /// <param name="cmnd"></param>
        public void ExecuteDTMFCommand(dtmf_ret_t cmnd)
        {
            // execute arbitrary script
            if (cmnd.Instruction == DtmfInstruction.External)
            {
                wnd.NewLogMessage("DTMF handling external script: " + cmnd.Filename + " " + cmnd.Arguments);
                new Thread(() =>
                {
                    ExecProc(cmnd.Filename, cmnd.Arguments);
                }).Start();
            }
            
            // handle internal commands
            switch (cmnd.Instruction)
            {
                case DtmfInstruction.RepeaterKnockdown:
                    {
                        wnd.NewLogMessage("DTMF handling Repeater Knockdown");
                        if (wnd.DisableTransmitter)
                            wnd.DisableTransmitter = false;
                        else
                            wnd.DisableTransmitter = true;
                    }
                    break;
                case DtmfInstruction.PlayTimeAnnouncement:
                    {
                        wnd.NewLogMessage("DTMF handling Play Time Announcement");
                        wnd.AutomaticAnnc.PlayTimeAnnc();
                    }
                    break;
                case DtmfInstruction.PlayAnnouncement:
                    {
                        wnd.NewLogMessage("DTMF handling Play Announcement");

                        int idx = -1;
                        foreach (annc_ret_t a in AutomaticAnnc.Announcements)
                            if (a.Name.ToLower() == cmnd.Arguments.ToLower())
                            {
                                idx = AutomaticAnnc.Announcements.IndexOf(a);
                                break;
                            }

                        if (idx != -1)
                            wnd.AutomaticAnnc.PlayRepeaterAnnc(AutomaticAnnc.Announcements[idx], DateTime.Now);
                        else
                            Messages.Trace("couldn't find [" + cmnd.Arguments + "] announcement in the list! got idx " + idx);
                    }
                    break;
                default:
                    Messages.Trace("unhandled DTMF sequence [" + cmnd.DTMFSequence + "]");
                    break;
            }
        }

        /// <summary>
        /// Sets default preset values.
        /// </summary>
        public void SetDefaults()
        {
            this.commands = new List<dtmf_ret_t>();
        }

        /// <summary>
        /// Updates the modal to the state of the internal variables.
        /// </summary>
        public void UpdateModal()
        {
            lock (commands)
            {
                if (shown)
                {
                    this.Invoke(new Action(() =>
                    {
                        dataGridView.Rows.Clear();
                        foreach (dtmf_ret_t cmnd in commands)
                            dataGridView.Rows.Add(new object[] { cmnd.Name, cmnd.DTMFSequence, cmnd.Instruction, cmnd.Filename, cmnd.Arguments });
                    }));
                }
            }
        }

        /// <summary>
        /// Occurs when the form is shown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DTMFWindow_Shown(object sender, EventArgs e)
        {
            this.shown = true;
            UpdateModal();   
        }

        /// <summary>
        /// Occurs when the form is closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DTMFWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.shown = false;
            if (DialogResult == DialogResult.Cancel)
                this.Canceled = true;
            else
                closeToolStripMenuItem_Click(sender, new EventArgs());
        }

        /// <summary>
        /// Occurs when the "Close" toolstrip menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateModal();
            SaveXml();

            DialogResult = DialogResult.OK;
            Canceled = false;

            this.Hide();
        }

        /// <summary>
        /// Event that occurs when the mouse is clicked on the data grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var hti = dataGridView.HitTest(e.X, e.Y);
                dataGridView.ClearSelection();
                if (hti.RowIndex != -1)
                    dataGridView.Rows[hti.RowIndex].Selected = true;
                return;
            }

            if (e.Button == MouseButtons.Right)
            {
                var hti = dataGridView.HitTest(e.X, e.Y);
                dataGridView.ClearSelection();
                if (hti.RowIndex != -1)
                    dataGridView.Rows[hti.RowIndex].Selected = true;
                return;
            }
        }

        /// <summary>
        /// Occurs when items are selected in the data grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                this.deleteDTMFToolStripMenuItem.Enabled = true;
                this.toolStripDeleteButton.Enabled = true;
                this.executeDTMFToolStripMenuItem.Enabled = true;
                this.toolStripExecuteButton.Enabled = true;
            }
            else
            {
                this.deleteDTMFToolStripMenuItem.Enabled = false;
                this.toolStripDeleteButton.Enabled = false;
                this.executeDTMFToolStripMenuItem.Enabled = false;
                this.toolStripExecuteButton.Enabled = false;
            }
        }

        /// <summary>
        /// Occurs when the "Add/Edit" toolstrip button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripAddEditButton_Click(object sender, EventArgs e)
        {
            addEditDTMFToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Occurs when the "Add/Edit DTMF Command" context menu toolstrip menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addEditDTMFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DTMFForm form = new DTMFForm();
            if (dataGridView.SelectedRows.Count > 0)
            {
                // edit mode
                form.IsEditing = true;
                DataGridViewRow row = dataGridView.SelectedRows[0];
                form.DtmfName = (string)row.Cells[0].Value;

                // find the dtmf command we're updating
                foreach (dtmf_ret_t cmnd in commands)
                {
                    if (cmnd.Name == form.DtmfName)
                    {
                        form.DtmfName = cmnd.Name;
                        form.DTMFSequence = cmnd.DTMFSequence;
                        form.Instruction = cmnd.Instruction;
                        form.Filename = cmnd.Filename;
                        form.Arguments = cmnd.Arguments;
                        break;
                    }
                }

                form.UpdateModal();
            }

            form.ShowDialog();

            // perform add/edit
            if (!form.Canceled)
            {
                if (form.IsEditing)
                {
                    int idx = -1;
                    foreach (dtmf_ret_t a in commands)
                        if (a.Name == form.DtmfName)
                        {
                            idx = commands.IndexOf(a);
                            break;
                        }

                    dtmf_ret_t cmnd = commands[idx];
                    cmnd.DTMFSequence = form.DTMFSequence;
                    cmnd.Instruction = form.Instruction;
                    cmnd.Filename = form.Filename;
                    cmnd.Arguments = form.Arguments;

                    // make sure we specified an announcement name if we are trying to play an announcement
                    if ((cmnd.Instruction == DtmfInstruction.PlayAnnouncement) && (cmnd.Arguments == string.Empty))
                    {
                        MessageBox.Show("DTMF command to play announcement must specify the announcement as the command argument! Cannot add an DTMF command to play announcement with no announcement specified.", AssemblyVersion._NAME, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    lock (commands)
                    {
                        commands[idx] = cmnd;
                    }

                    UpdateModal();
                    SaveXml();
                }
                else
                {
                    // we're adding a new dtmf command
                    dtmf_ret_t cmnd = new dtmf_ret_t()
                    {
                        Name = form.DtmfName,
                        DTMFSequence = form.DTMFSequence,
                        Instruction = form.Instruction,
                        Filename = form.Filename,
                        Arguments = form.Arguments,
                    };

                    // make sure a name was defined
                    if (cmnd.Name == string.Empty)
                    {
                        MessageBox.Show("DTMF commands must have a name! Cannot add an DTMF command with no name.", AssemblyVersion._NAME, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // make sure we specified an announcement name if we are trying to play an announcement
                    if ((cmnd.Instruction == DtmfInstruction.PlayAnnouncement) && (cmnd.Arguments == string.Empty))
                    {
                        MessageBox.Show("DTMF command to play announcement must specify the announcement as the command argument! Cannot add an DTMF command to play announcement with no announcement specified.", AssemblyVersion._NAME, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    lock (commands)
                    {
                        // lets not allow doubles now...
                        foreach (dtmf_ret_t a in commands)
                            if (a.Name == form.DtmfName)
                            {
                                MessageBox.Show("Cannot add DTMF command named " + form.DtmfName + " it already exists!", AssemblyVersion._NAME, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                        commands.Add(cmnd);
                    }

                    UpdateModal();
                    SaveXml();
                }
            }
        }

        /// <summary>
        /// Occurs when the "Delete" toolstrip button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripDeleteButton_Click(object sender, EventArgs e)
        {
            deleteDTMFToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Occurs when the "Delete DTMF Command" context menu toolstrip menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteDTMFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView.SelectedRows[0];
                string dtmfName = (string)row.Cells[0].Value;

                int idx = -1;
                foreach (dtmf_ret_t a in commands)
                    if (a.Name == dtmfName)
                    {
                        idx = commands.IndexOf(a);
                        break;
                    }

                lock (commands)
                {
                    commands.RemoveAt(idx);
                }

                UpdateModal();
                SaveXml();
            }
        }

        /// <summary>
        /// Occurs when the "Execute" toolstrip button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripExecuteButton_Click(object sender, EventArgs e)
        {
            executeDTMFToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Occurs when the "Execute DTMF Command" context menu toolstrip menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void executeDTMFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView.SelectedRows[0];
                string dtmfName = (string)row.Cells[0].Value;

                int idx = -1;
                foreach (dtmf_ret_t a in commands)
                    if (a.Name == dtmfName)
                    {
                        idx = commands.IndexOf(a);
                        break;
                    }

                lock (commands)
                {
                    ExecuteDTMFCommand(commands[idx]);
                }

                UpdateModal();
            }
        }
    } // public partial class DTMFWindow : Form
} // namespace RepeaterController.DSP.DTMF
