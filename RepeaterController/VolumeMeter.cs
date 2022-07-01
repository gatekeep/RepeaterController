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
    /// Implements a rudimentary volume meter.
    /// </summary>
    public partial class VolumeMeter : Control
    {
        /**
         * Fields
         */
        private Brush foregroundBrush;
        private Brush detectBrush;
        private float amplitude;

        /**
         * Properties
         */
        /// <summary>
        /// Current Value
        /// </summary>
        [DefaultValue(-3.0)]
        public float Amplitude
        {
            get { return amplitude; }
            set
            {
                amplitude = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Minimum decibels
        /// </summary>
        [DefaultValue(-60.0)]
        public float MinDb { get; set; }

        /// <summary>
        /// Maximum decibels
        /// </summary>
        [DefaultValue(18.0)]
        public float MaxDb { get; set; }

        /// <summary>
        /// Detector decibels
        /// </summary>
        [DefaultValue(9.0)]
        public float DetectDb { get; set; }

        /// <summary>
        /// Meter orientation
        /// </summary>
        [DefaultValue(Orientation.Vertical)]
        public Orientation Orientation { get; set; }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeMeter"/> class.
        /// </summary>
        public VolumeMeter()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            MinDb = -60;
            MaxDb = 18;
            DetectDb = 9;
            Amplitude = 0;
            Orientation = Orientation.Vertical;
            InitializeComponent();
            OnForeColorChanged(EventArgs.Empty);
        }

        /// <summary>
        /// On Fore Color Changed
        /// </summary>
        protected override void OnForeColorChanged(EventArgs e)
        {
            foregroundBrush = new SolidBrush(ForeColor);
            detectBrush = new SolidBrush(Color.Red);
            base.OnForeColorChanged(e);
        }

        /// <summary>
        /// Paints the volume meter
        /// </summary>
        protected override void OnPaint(PaintEventArgs pe)
        {
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;

            pe.Graphics.DrawRectangle(Pens.Black, 0, 0, this.Width - 1, this.Height - 1);

            double db = 20 * Math.Log10(Amplitude);
            if (db < MinDb)
                db = MinDb;
            if (db > MaxDb)
                db = MaxDb;

            try
            {
                int width = this.Width - 2;
                int height = this.Height - 2;
                double percent = (db - MinDb) / (MaxDb - MinDb);
                if (double.IsNaN(percent))
                    goto OnPaint_End; // NO
                if (double.IsNegativeInfinity(percent))
                    goto OnPaint_End; // NO

                if (Orientation == Orientation.Horizontal)
                {
                    width = (int)(width * percent);

                    pe.Graphics.FillRectangle(foregroundBrush, 1, 1, width, height);
                }
                else
                {
                    height = (int)(height * percent);
                    pe.Graphics.FillRectangle(foregroundBrush, 1, this.Height - 1 - height, width, height);
                }

                width = this.Width - 2;
                height = this.Height - 2;
                double detectPercent = (DetectDb - MinDb) / (MaxDb - MinDb);
                if (double.IsNaN(detectPercent))
                    goto OnPaint_End; // NO
                if (double.IsNegativeInfinity(detectPercent))
                    goto OnPaint_End; // NO

                if (Orientation == Orientation.Horizontal)
                {
                    width = (int)(width * detectPercent);
                    pe.Graphics.FillRectangle(detectBrush, width, 1, 2, height);
                }
                else
                {
                    height = (int)(height * detectPercent);
                    pe.Graphics.FillRectangle(detectBrush, 1, (this.Height - 1 - height) + 1, width, 2);
                }
            }
            catch (OverflowException)
            {
                /* ignore */
            }

// bryanb: why are we doing this? this is stupid...
OnPaint_End:
            string dbValue = String.Format("{0:F2} dB", db);
            if (Double.IsNegativeInfinity(db))
                dbValue = String.Format("{0:F2} dB", MinDb);
            if (Double.IsNaN(db))
                dbValue = String.Format("{0:F2} dB", MinDb);

            pe.Graphics.DrawString(dbValue, this.Font,
                Brushes.Black, this.ClientRectangle, format);
        }
    } // public partial class VolumeMeter : Control
} // namespace RepeaterController
