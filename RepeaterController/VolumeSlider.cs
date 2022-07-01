/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
/**
 * Based on code from the NAudio project. (https://github.com/naudio/NAudio)
 * Licensed under the Ms-PL license.
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RepeaterController
{
    /// <summary>
    /// Implements a volume slider control.
    /// </summary>
    public partial class VolumeSlider : UserControl
    {
        /**
         * Fields
         */
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private float volume = 1.0f;
        private float mindb = -48.0f;

        /**
         * Events
         */
        /// <summary>
        /// Volume changed event
        /// </summary>
        public event EventHandler VolumeChanged;

        /**
         * Properties
         */
        /// <summary>
        /// The volume for this control
        /// </summary>
        [DefaultValue(1.0f)]
        public float Volume
        {
            get { return volume; }
            set
            {
                if (value < 0.0f)
                    value = 0.0f;
                if (value > 1.0f)
                    value = 1.0f;
                if (volume != value)
                {
                    volume = value;
                    if (VolumeChanged != null)
                        VolumeChanged(this, EventArgs.Empty);
                    Invalidate();
                }
            }
        }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeSlider"/> class.
        /// </summary>
        public VolumeSlider()
        {
            /* stub */
        }

        /// <summary>
        /// <see cref="Control.OnPaint"/>
        /// </summary>
        protected override void OnPaint(PaintEventArgs pe)
        {
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;

            pe.Graphics.DrawRectangle(Pens.Black, 0, 0, this.Width - 1, this.Height - 1);
            float db = 20 * (float)Math.Log10(Volume);
            float percent = 1 - (db / mindb);

            pe.Graphics.FillRectangle(Brushes.LightGreen, 1, 1, (int)((this.Width - 2) * percent), this.Height - 2);
            string dbValue = String.Format("{0:F2} dB", db);
            if (Double.IsNegativeInfinity(db))
                dbValue = String.Format("{0:F2} dB", mindb);
            if (Double.IsNaN(db))
                dbValue = String.Format("{0:F2} dB", mindb);

            pe.Graphics.DrawString(dbValue, this.Font,
                Brushes.Black, this.ClientRectangle, format);
            // Calling the base class OnPaint
            //base.OnPaint(pe);
        }

        /// <summary>
        /// <see cref="Control.OnMouseMove"/>
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                SetVolumeFromMouse(e.X);
            base.OnMouseMove(e);
        }

        /// <summary>
        /// <see cref="Control.OnMouseDown"/>
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            SetVolumeFromMouse(e.X);
            base.OnMouseDown(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        private void SetVolumeFromMouse(int x)
        {
            // linear Volume = (float) x / this.Width;
            float dbVolume = (1 - (float)x / this.Width) * mindb;
            if (x <= 0)
                Volume = 0;
            else
                Volume = (float)Math.Pow(10, dbVolume / 20);
        }
    } // public partial class VolumeSlider : UserControl
} // namespace RepeaterController
