/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
/**
 * Based on code from the CSCore project. (https://github.com/filoe/cscore)
 * Licensed under the Ms-PL license.
 * These implementations are based on http://www.earlevel.com/main/2011/01/02/biquad-formulas/
 */
using System;

namespace RepeaterController.DSP
{
    /// <summary>
    /// Represents a biquad-filter.
    /// </summary>
    public abstract class BiQuad
    {
        /**
         * Fields
         */
        /// <summary>
        /// The a0 value.
        /// </summary>
        protected double A0;
        /// <summary>
        /// The a1 value.
        /// </summary>
        protected double A1;
        /// <summary>
        /// The a2 value.
        /// </summary>
        protected double A2;
        /// <summary>
        /// The b1 value.
        /// </summary>
        protected double B1;
        /// <summary>
        /// The b2 value.
        /// </summary>
        protected double B2;
        /// <summary>
        /// The z1 value.
        /// </summary>
        protected double Z1;
        /// <summary>
        /// The z2 value.
        /// </summary>
        protected double Z2;

        private double q;
        private double gainDB;
        private double frequency;

        /**
         * Properties
         */
        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">value;The samplerate has to be bigger than 2 * frequency.</exception>
        public double Frequency
        {
            get { return frequency; }
            set
            {
                if (SampleRate < value * 2)
                    throw new ArgumentOutOfRangeException("value", "The samplerate has to be bigger than 2 * frequency.");
                frequency = value;
                CalculateBiQuadCoefficients();
            }
        }

        /// <summary>
        /// Gets the sample rate.
        /// </summary>
        public int SampleRate { get; private set; }

        /// <summary>
        /// The q value.
        /// </summary>
        public double Q
        {
            get { return q; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");
                q = value;
                CalculateBiQuadCoefficients();
            }
        }

        /// <summary>
        /// Gets or sets the gain value in dB.
        /// </summary>
        public double GainDB
        {
            get { return gainDB; }
            set
            {
                gainDB = value;
                CalculateBiQuadCoefficients();
            }
        }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="BiQuad"/> class.
        /// </summary>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="frequency">The frequency.</param>
        protected BiQuad(int sampleRate, double frequency) : this(sampleRate, frequency, 1.0 / Math.Sqrt(2))
        {
            /* stub */
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BiQuad"/> class.
        /// </summary>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="frequency">The frequency.</param>
        /// <param name="q">The q.</param>
        protected BiQuad(int sampleRate, double frequency, double q)
        {
            if (sampleRate <= 0)
                throw new ArgumentOutOfRangeException("sampleRate");
            if (frequency <= 0)
                throw new ArgumentOutOfRangeException("frequency");
            if (q <= 0)
                throw new ArgumentOutOfRangeException("q");

            this.SampleRate = sampleRate;
            this.Frequency = frequency;
            this.Q = q;
            this.GainDB = 6;
        }

        /// <summary>
        /// Processes a single <paramref name="input"/> sample and returns the result.
        /// </summary>
        /// <param name="input">The input sample to process.</param>
        /// <returns>The result of the processed <paramref name="input"/> sample.</returns>
        public float Process(float input)
        {
            double o = input * A0 + Z1;
            Z1 = input * A1 + Z2 - B1 * o;
            Z2 = input * A2 - B2 * o;
            return (float)o;
        }

        /// <summary>
        /// Processes multiple <paramref name="input"/> samples.
        /// </summary>
        /// <param name="input">The input samples to process.</param>
        /// <remarks>The result of the calculation gets stored within the <paramref name="input"/> array.</remarks>
        public void Process(float[] input)
        {
            for (int i = 0; i < input.Length; i++)
                input[i] = Process(input[i]);
        }

        /// <summary>
        /// Calculates all coefficients.
        /// </summary>
        protected abstract void CalculateBiQuadCoefficients();
    } // public abstract class BiQuad

    /// <summary>
    /// Used to apply a lowpass-filter to a signal.
    /// </summary>
    public class LowpassFilter : BiQuad
    {
        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="LowpassFilter"/> class.
        /// </summary>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="frequency">The filter's corner frequency.</param>
        public LowpassFilter(int sampleRate, double frequency) : base(sampleRate, frequency)
        {
            /* stub */
        }

        /// <summary>
        /// Calculates all coefficients.
        /// </summary>
        protected override void CalculateBiQuadCoefficients()
        {
            double k = Math.Tan(Math.PI * Frequency / SampleRate);
            var norm = 1 / (1 + k / Q + k * k);
            A0 = k * k * norm;
            A1 = 2 * A0;
            A2 = A0;
            B1 = 2 * (k * k - 1) * norm;
            B2 = (1 - k / Q + k * k) * norm;
        }
    } // public class LowpassFilter : BiQuad

    /// <summary>
    /// Used to apply a highpass-filter to a signal.
    /// </summary>
    public class HighpassFilter : BiQuad
    {
        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="HighpassFilter"/> class.
        /// </summary>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="frequency">The filter's corner frequency.</param>
        public HighpassFilter(int sampleRate, double frequency) : base(sampleRate, frequency)
        {
            /* stub */
        }

        /// <summary>
        /// Calculates all coefficients.
        /// </summary>
        protected override void CalculateBiQuadCoefficients()
        {
            double k = Math.Tan(Math.PI * Frequency / SampleRate);
            var norm = 1 / (1 + k / Q + k * k);
            A0 = 1 * norm;
            A1 = -2 * A0;
            A2 = A0;
            B1 = 2 * (k * k - 1) * norm;
            B2 = (1 - k / Q + k * k) * norm;
        }
    } // public class HighpassFilter : BiQuad

    /// <summary>
    /// Used to apply a bandpass-filter to a signal.
    /// </summary>
    public class BandpassFilter : BiQuad
    {
        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="BandpassFilter"/> class.
        /// </summary>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="frequency">The filter's corner frequency.</param>
        public BandpassFilter(int sampleRate, double frequency) : base(sampleRate, frequency)
        {
            /* stub */
        }

        /// <summary>
        /// Calculates all coefficients.
        /// </summary>
        protected override void CalculateBiQuadCoefficients()
        {
            double k = Math.Tan(Math.PI * Frequency / SampleRate);
            double norm = 1 / (1 + k / Q + k * k);
            A0 = k / Q * norm;
            A1 = 0;
            A2 = -A0;
            B1 = 2 * (k * k - 1) * norm;
            B2 = (1 - k / Q + k * k) * norm;
        }
    } // public class BandpassFilter : BiQuad

    /// <summary>
    /// Used to apply a notch-filter to a signal.
    /// </summary>
    public class NotchFilter : BiQuad
    {
        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="NotchFilter"/> class.
        /// </summary>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="frequency">The filter's corner frequency.</param>
        public NotchFilter(int sampleRate, double frequency) : base(sampleRate, frequency)
        {
            /* stub */
        }

        /// <summary>
        /// Calculates all coefficients.
        /// </summary>
        protected override void CalculateBiQuadCoefficients()
        {
            double k = Math.Tan(Math.PI * Frequency / SampleRate);
            double norm = 1 / (1 + k / Q + k * k);
            A0 = (1 + k * k) * norm;
            A1 = 2 * (k * k - 1) * norm;
            A2 = A0;
            B1 = A1;
            B2 = (1 - k / Q + k * k) * norm;
        }
    } // public class NotchFilter : BiQuad

    /// <summary>
    /// Used to apply a lowshelf-filter to a signal.
    /// </summary>
    public class LowShelfFilter : BiQuad
    {
        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="LowShelfFilter"/> class.
        /// </summary>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="frequency">The filter's corner frequency.</param>
        /// <param name="gainDB">Gain value in dB.</param>
        public LowShelfFilter(int sampleRate, double frequency, double gainDB) : base(sampleRate, frequency)
        {
            this.GainDB = gainDB;
        }

        /// <summary>
        /// Calculates all coefficients.
        /// </summary>
        protected override void CalculateBiQuadCoefficients()
        {
            const double sqrt2 = 1.4142135623730951;
            double k = Math.Tan(Math.PI * Frequency / SampleRate);
            double v = Math.Pow(10, Math.Abs(GainDB) / 20.0);
            double norm;
            if (GainDB >= 0)
            {   
                // boost
                norm = 1 / (1 + sqrt2 * k + k * k);
                A0 = (1 + Math.Sqrt(2 * v) * k + v * k * k) * norm;
                A1 = 2 * (v * k * k - 1) * norm;
                A2 = (1 - Math.Sqrt(2 * v) * k + v * k * k) * norm;
                B1 = 2 * (k * k - 1) * norm;
                B2 = (1 - sqrt2 * k + k * k) * norm;
            }
            else
            {   
                // cut
                norm = 1 / (1 + Math.Sqrt(2 * v) * k + v * k * k);
                A0 = (1 + sqrt2 * k + k * k) * norm;
                A1 = 2 * (k * k - 1) * norm;
                A2 = (1 - sqrt2 * k + k * k) * norm;
                B1 = 2 * (v * k * k - 1) * norm;
                B2 = (1 - Math.Sqrt(2 * v) * k + v * k * k) * norm;
            }
        }
    } // public class LowShelfFilter : BiQuad

    /// <summary>
    /// Used to apply a highshelf-filter to a signal.
    /// </summary>
    public class HighShelfFilter : BiQuad
    {
        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="HighShelfFilter"/> class.
        /// </summary>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="frequency">The filter's corner frequency.</param>
        /// <param name="gainDB">Gain value in dB.</param>
        public HighShelfFilter(int sampleRate, double frequency, double gainDB) : base(sampleRate, frequency)
        {
            this.GainDB = gainDB;
        }

        /// <summary>
        /// Calculates all coefficients.
        /// </summary>
        protected override void CalculateBiQuadCoefficients()
        {
            const double sqrt2 = 1.4142135623730951;
            double k = Math.Tan(Math.PI * Frequency / SampleRate);
            double v = Math.Pow(10, Math.Abs(GainDB) / 20.0);
            double norm;
            if (GainDB >= 0)
            {    
                // boost
                norm = 1 / (1 + sqrt2 * k + k * k);
                A0 = (v + Math.Sqrt(2 * v) * k + k * k) * norm;
                A1 = 2 * (k * k - v) * norm;
                A2 = (v - Math.Sqrt(2 * v) * k + k * k) * norm;
                B1 = 2 * (k * k - 1) * norm;
                B2 = (1 - sqrt2 * k + k * k) * norm;
            }
            else
            {   
                // cut
                norm = 1 / (v + Math.Sqrt(2 * v) * k + k * k);
                A0 = (1 + sqrt2 * k + k * k) * norm;
                A1 = 2 * (k * k - 1) * norm;
                A2 = (1 - sqrt2 * k + k * k) * norm;
                B1 = 2 * (k * k - v) * norm;
                B2 = (v - Math.Sqrt(2 * v) * k + k * k) * norm;
            }
        }
    } // public class HighShelfFilter : BiQuad

    /// <summary>
    /// Used to apply an peak-filter to a signal.
    /// </summary>
    public class PeakFilter : BiQuad
    {
        /**
         * Properties
         */
        /// <summary>
        /// Gets or sets the bandwidth.
        /// </summary>
        public double BandWidth
        {
            get { return Q; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");
                Q = value;
            }
        }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="PeakFilter"/> class.
        /// </summary>
        /// <param name="sampleRate">The sampleRate of the audio data to process.</param>
        /// <param name="frequency">The center frequency to adjust.</param>
        /// <param name="bandWidth">The bandWidth.</param>
        /// <param name="peakGainDB">The gain value in dB.</param>
        public PeakFilter(int sampleRate, double frequency, double bandWidth, double peakGainDB) : base(sampleRate, frequency, bandWidth)
        {
            this.GainDB = peakGainDB;
        }

        /// <summary>
        /// Calculates all coefficients.
        /// </summary>
        protected override void CalculateBiQuadCoefficients()
        {
            double norm;
            double v = Math.Pow(10, Math.Abs(GainDB) / 20.0);
            double k = Math.Tan(Math.PI * Frequency / SampleRate);
            double q = Q;

            if (GainDB >= 0)
            {
                // boost
                norm = 1 / (1 + 1 / q * k + k * k);
                A0 = (1 + v / q * k + k * k) * norm;
                A1 = 2 * (k * k - 1) * norm;
                A2 = (1 - v / q * k + k * k) * norm;
                B1 = A1;
                B2 = (1 - 1 / q * k + k * k) * norm;
            }
            else
            {
                // cut
                norm = 1 / (1 + v / q * k + k * k);
                A0 = (1 + 1 / q * k + k * k) * norm;
                A1 = 2 * (k * k - 1) * norm;
                A2 = (1 - 1 / q * k + k * k) * norm;
                B1 = A1;
                B2 = (1 - v / q * k + k * k) * norm;
            }
        }
    } // public class PeakFilter : BiQuad
} // namespace RepeaterController
