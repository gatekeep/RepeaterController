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
    public class SimpleCompressor : AttRelEnvelope
    {
        /**
         * Fields
         */
        private double envdB;			// over-threshold envelope (dB)

        /**
         * Properties
         */
        /// <summary>
        /// 
        /// </summary>
        public double MakeUpGain { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Threshold { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Ratio { get; set; }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCompressor"/> class.
        /// </summary>
        public SimpleCompressor()
            : base(10.0, 10.0, 44100.0)
        {
            this.Threshold = 0.0;
            this.Ratio = 1.0;
            this.MakeUpGain = 0.0;
            this.envdB = DC_OFFSET;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCompressor"/> class.
        /// </summary>        
        /// <param name="attackTime"></param>
        /// <param name="releaseTime"></param>
        /// <param name="sampleRate"></param>
        public SimpleCompressor(double attackTime, double releaseTime, double sampleRate)
            : base(attackTime, releaseTime, sampleRate)
        {
            this.Threshold = 0.0;
            this.Ratio = 1.0;
            this.MakeUpGain = 0.0;
            this.envdB = DC_OFFSET;
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitRuntime()
        {
            this.envdB = DC_OFFSET;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="in1"></param>
        /// <param name="in2"></param>
        public void Process(ref double in1, ref double in2)
        {
            // sidechain

            // rectify input
            double rect1 = Math.Abs(in1);	// n.b. was fabs
            double rect2 = Math.Abs(in2); // n.b. was fabs

            // if desired, one could use another EnvelopeDetector to smooth
            // the rectified signal.

            double link = Math.Max(rect1, rect2);	// link channels with greater of 2

            link += DC_OFFSET;					// add DC offset to avoid log( 0 )
            double keydB = Decibels.LinearToDecibels(link);		// convert linear -> dB

            // threshold
            double overdB = keydB - Threshold;	// delta over threshold
            if (overdB < 0.0)
                overdB = 0.0;

            // attack/release

            overdB += DC_OFFSET;					// add DC offset to avoid denormal

            Run(overdB, ref envdB);	// run attack/release envelope

            overdB = envdB - DC_OFFSET;         // subtract DC offset

            // Regarding the DC offset: In this case, since the offset is added before 
            // the attack/release processes, the envelope will never fall below the offset,
            // thereby avoiding denormals. However, to prevent the offset from causing
            // constant gain reduction, we must subtract it from the envelope, yielding
            // a minimum value of 0dB.

            // transfer function
            double gr = overdB * (Ratio - 1.0);	// gain reduction (dB)
            gr = Decibels.DecibelsToLinear(gr) * Decibels.DecibelsToLinear(MakeUpGain); // convert dB -> linear

            // output gain
            in1 *= gr;	// apply gain reduction to input
            in2 *= gr;
        }
    } // public class SimpleCompressor : AttRelEnvelope
} // namespace RepeaterController
