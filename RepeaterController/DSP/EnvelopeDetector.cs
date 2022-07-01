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

namespace RepeaterController.DSP
{
    /// <summary>
    /// 
    /// </summary>
    public class EnvelopeDetector
    {
        /**
         * Fields
         */
        private double sampleRate;
        private double ms;
        private double coeff;

        /** 
         * Properties
         */
        /// <summary>
        /// Gets or sets the detector time constant.
        /// </summary>
        public double TimeConstant
        {
            get { return ms; }
            set
            {
                System.Diagnostics.Debug.Assert(value > 0.0);
                this.ms = value;
                SetCoef();
            }
        }

        /// <summary>
        /// Gets or sets the detector sample rate.
        /// </summary>
        public double SampleRate
        {
            get { return sampleRate; }
            set
            {
                System.Diagnostics.Debug.Assert(value > 0.0);
                this.sampleRate = value;
                SetCoef();
            }
        }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="EnvelopeDetector"/> class.
        /// </summary>
        public EnvelopeDetector() : this(1.0, 44100.0)
        {
            /* stub */
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="EnvelopeDetector"/> class.
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="sampleRate"></param>
        public EnvelopeDetector(double ms, double sampleRate)
        {
            System.Diagnostics.Debug.Assert(sampleRate > 0.0);
            System.Diagnostics.Debug.Assert(ms > 0.0);
            this.sampleRate = sampleRate;
            this.ms = ms;
            SetCoef();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inValue"></param>
        /// <param name="state"></param>
        public void Run(double inValue, ref double state)
        {
            state = inValue + coeff * (state - inValue);
        }

        /// <summary>
        /// Internal helper to set the detector coefficient.
        /// </summary>
        private void SetCoef()
        {
            coeff = Math.Exp(-1.0 / (0.001 * ms * sampleRate));
        }
    } // public class EnvelopeDetector

    /// <summary>
    /// 
    /// </summary>
    public class AttRelEnvelope
    {
        /**
         * Fields
         */
        // DC offset to prevent denormal
        protected const double DC_OFFSET = 1.0E-25;

        private readonly EnvelopeDetector attack;
        private readonly EnvelopeDetector release;

        /**
         * Properties
         */
        /// <summary>
        /// Gets or sets the attack coefficent.
        /// </summary>
        public double Attack
        {
            get { return attack.TimeConstant; }
            set { attack.TimeConstant = value; }
        }

        /// <summary>
        /// Gets or sets the release coefficent.
        /// </summary>
        public double Release
        {
            get { return release.TimeConstant; }
            set { release.TimeConstant = value; }
        }

        /// <summary>
        /// Gets or sets the sample rate.
        /// </summary>
        public double SampleRate
        {
            get { return attack.SampleRate; }
            set { attack.SampleRate = release.SampleRate = value; }
        }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="AttRelEnvelope"/> class.
        /// </summary>
        /// <param name="attackMilliseconds"></param>
        /// <param name="releaseMilliseconds"></param>
        /// <param name="sampleRate"></param>
        public AttRelEnvelope(double attackMilliseconds, double releaseMilliseconds, double sampleRate)
        {
            attack = new EnvelopeDetector(attackMilliseconds, sampleRate);
            release = new EnvelopeDetector(releaseMilliseconds, sampleRate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inValue"></param>
        /// <param name="state"></param>
        public void Run(double inValue, ref double state)
        {
            // assumes that:
            // positive delta = attack
            // negative delta = release
            // good for linear & log values
            if (inValue > state)
                attack.Run(inValue, ref state);   // attack
            else
                release.Run(inValue, ref state);  // release
        }
    } // public class AttRelEnvelope
} // namespace RepeaterController
