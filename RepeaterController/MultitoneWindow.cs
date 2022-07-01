/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;

using RepeaterController.DSP;

namespace RepeaterController
{
    /// <summary>
    /// Implements the management of multi-tone tone data.
    /// </summary>
    public partial class MultitoneWindow : Form
    {
        /**
         * Fields
         */
        private MainWindow wnd;
        private RepeaterOptions options;
        private bool shown = false;

        /// <summary>
        /// Flag indicating whether the form was canceled.
        /// </summary>
        public bool Canceled;

        /// <summary>
        /// List of configured tones.
        /// </summary>
        public List<Multitone> Tones;

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="MultitoneWindow"/> class.
        /// </summary>
        /// <param name="wnd"></param>
        /// <param name="options"></param>
        public MultitoneWindow(MainWindow wnd, RepeaterOptions options)
        {
            InitializeComponent();

            this.wnd = wnd;
            this.options = options;

            this.deleteToneToolStripMenuItem.Enabled = false;
            this.toolStripDeleteButton.Enabled = false;

            this.Shown += MultitoneWindow_Shown;
            this.FormClosing += MultitoneWindow_FormClosing;

            this.dataGridView.SelectionChanged += DataGridView_SelectionChanged;
            this.dataGridView.MouseDown += DataGridView_MouseDown;
        }

        /// <summary>
        /// Updates the modal to the state of the internal variables.
        /// </summary>
        public void UpdateModal()
        {
            lock (Tones)
            {
                if (shown)
                {
                    this.Invoke(new Action(() =>
                    {
                        dataGridView.Rows.Clear();
                        foreach (Multitone tone in Tones)
                            dataGridView.Rows.Add(new object[] { tone.Index.ToString(), tone.Frequency.ToString(), tone.FrequencyTwo.ToString(), tone.Length.ToString(), tone.SilenceLength.ToString() });
                    }));

                    options.UpdateMultiTone();
                    options.UpdateModal();
                }
            }
        }

        /// <summary>
        /// Occurs when the form is shown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MultitoneWindow_Shown(object sender, EventArgs e)
        {
            this.shown = true;
            UpdateModal();   
        }

        /// <summary>
        /// Occurs when the form is closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MultitoneWindow_FormClosing(object sender, FormClosingEventArgs e)
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
                this.deleteToneToolStripMenuItem.Enabled = true;
                this.toolStripDeleteButton.Enabled = true;
            }
            else
            {
                this.deleteToneToolStripMenuItem.Enabled = false;
                this.toolStripDeleteButton.Enabled = false;
            }
        }

        /// <summary>
        /// Occurs when the "Add/Edit" toolstrip button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripAddEditButton_Click(object sender, EventArgs e)
        {
            addEditToneToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Occurs when the "Add/Edit Tone" context menu toolstrip menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addEditToneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MultitoneForm form = new MultitoneForm();
            if (dataGridView.SelectedRows.Count > 0)
            {
                // edit mode
                form.IsEditing = true;
                DataGridViewRow row = dataGridView.SelectedRows[0];
                form.Tone.Index = Convert.ToUInt16((string)row.Cells[0].Value);

                // find the tone we're updating
                foreach (Multitone tone in Tones)
                {
                    if (tone.Index == form.Tone.Index)
                    {
                        form.Tone.Index = tone.Index;
                        form.Tone.Frequency = tone.Frequency;
                        form.Tone.FrequencyTwo = tone.FrequencyTwo;
                        form.Tone.Length = tone.Length;
                        form.Tone.SilenceLength = tone.SilenceLength;
                        break;
                    }
                }

                form.UpdateModal();
                form.IndexChanged = false;
            }
            else
            {
                if (Tones.Count > 0)
                {
                    int idx = Tones.Count + 1;
                    form.Tone.Index = idx;
                }

                form.UpdateModal();
                form.IndexChanged = false;
            }

            form.ShowDialog();

            // perform add/edit
            if (!form.Canceled)
            {
                if (form.IsEditing)
                {
                    int idx = -1;
                    foreach (Multitone a in Tones)
                        if (a.Index == form.Tone.Index)
                        {
                            idx = Tones.IndexOf(a);
                            break;
                        }

                    Multitone tone = Tones[idx];
                    tone.Index = form.Tone.Index;
                    tone.Frequency = form.Tone.Frequency;
                    tone.FrequencyTwo = form.Tone.FrequencyTwo;
                    tone.Length = form.Tone.Length;
                    tone.SilenceLength = form.Tone.SilenceLength;

                    // lets not allow doubles now...
                    if (form.IndexChanged)
                    {
                        foreach (Multitone a in Tones)
                            if (a.Index == form.Tone.Index)
                            {
                                MessageBox.Show("Cannot edit tone! Tone order index was changed, and a tone with the given index already exists!", AssemblyVersion._NAME, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                    }

                    lock (Tones)
                    {
                        Tones[idx] = tone;
                    }

                    UpdateModal();
                }
                else
                {
                    // we're adding a new tone
                    Multitone tone = new Multitone()
                    {
                        Index = form.Tone.Index,
                        Frequency = form.Tone.Frequency,
                        FrequencyTwo = form.Tone.FrequencyTwo,
                        Length = form.Tone.Length,
                        SilenceLength = form.Tone.SilenceLength,
                    };

                    lock (Tones)
                    {
                        // lets not allow doubles now...
                        foreach (Multitone a in Tones)
                            if (a.Index == form.Tone.Index)
                            {
                                MessageBox.Show("Cannot edit tone! Tone order index was changed, and a tone with the given index already exists!", AssemblyVersion._NAME, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                        Tones.Add(tone);
                    }

                    UpdateModal();
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
            deleteToneToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Occurs when the "Delete Tone" context menu toolstrip menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteToneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView.SelectedRows[0];
                int toneIdx = Convert.ToInt32((string)row.Cells[0].Value);

                int idx = -1;
                foreach (Multitone a in Tones)
                    if (a.Index == toneIdx)
                    {
                        idx = Tones.IndexOf(a);
                        break;
                    }

                lock (Tones)
                {
                    Tones.RemoveAt(idx);
                }

                UpdateModal();
            }
        }

        /// <summary>
        /// Occurs when the "Play" toolstrip button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripPlayButton_Click(object sender, EventArgs e)
        {
            UpdateModal();
            wnd.GenerateCourtesyTone();
        }
    } // public partial class MultitoneWindow : Form
} // namespace RepeaterController
