/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;

namespace RepeaterController.DSP.DPL
{
    /// <summary>
    /// Implements a encoder for DPL bitstream in a audio stream.
    /// </summary>
    public class DPLEncoder
    {
        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="DPLEncoder"/> class.
        /// </summary>
        public DPLEncoder()
        {
            /* stub */
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inp"></param>
        /// <returns></returns>
        private byte[] ReverseBitArray(byte[] inp)
        {
            byte[] rev = new byte[inp.Length];
            for (int i = inp.Length; i > 0; i--)
                rev[rev.Length - i] = inp[i - 1];
            return rev;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        private byte[] GenerateParityBits(byte[] bits)
        {
            byte[] golay = new byte[11];

            // second word
            byte w2B3 = bits[11];
            byte w2B2 = bits[10];
            byte w2B1 = bits[9];

            // third word
            byte w3B3 = bits[8];
            byte w3B2 = bits[7];
            byte w3B1 = bits[6];

            // first word
            byte w1B3 = bits[5];
            byte w1B2 = bits[4];
            byte w1B1 = bits[3];

            // calculate bits
            golay[0] = (byte)((w2B3 + w2B2 + w2B1 + w3B3 + w3B2 + w1B2) % 2);
            golay[1] = (byte)((~((w2B2 + w2B1 + w3B3 + w3B2 + w3B1 + w1B1) % 2)) & 0x01);
            golay[2] = (byte)((w2B3 + w2B2 + w3B1 + w1B3 + w1B2) % 2);
            golay[3] = (byte)((~((w2B2 + w2B1 + w1B3 + w1B2 + w1B1) % 2)) & 0x01);
            golay[4] = (byte)((~((w2B3 + w2B2 + w3B2 + w1B1) % 2)) & 0x01);
            golay[5] = (byte)((~((w2B3 + w3B3 + w3B2 + w3B1 + w1B2) % 2)) & 0x01);
            golay[6] = (byte)((w2B3 + w2B1 + w3B3 + w3B1 + w1B3 + w1B2 + w1B1) % 2);
            golay[7] = (byte)((w2B2 + w3B3 + w3B2 + w1B3 + w1B2 + w1B1) % 2);
            golay[8] = (byte)((w2B1 + w3B2 + w3B1 + w1B2 + w1B1) % 2);
            golay[9] = (byte)((~((w3B3 + w3B1 + w1B3 + w1B1) % 2)) & 0x01);
            golay[10] = (byte)((~((w2B3 + w2B2 + w2B1 + w3B3 + w1B3) % 2)) & 0x01);

            return golay;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="digit1"></param>
        /// <param name="digit2"></param>
        /// <param name="digit3"></param>
        /// <returns></returns>
        private byte[] GenerateRawBits(byte digit1, byte digit2, byte digit3)
        {
            if (digit1 > 7)
                throw new ArgumentOutOfRangeException();
            if (digit2 > 7)
                throw new ArgumentOutOfRangeException();
            if (digit3 > 7)
                throw new ArgumentOutOfRangeException();

            byte[] dcs = new byte[12];
            int pos = 3;

            // DCS signature marker (4)
            dcs[0] = 1;
            dcs[1] = 0;
            dcs[2] = 0;

            // generate bit pattern for word 1
            string d1B = Convert.ToString(digit1, 2);
            if (d1B.Length <= 1)
                d1B = d1B.Insert(0, "00");
            if (d1B.Length <= 2)
                d1B = d1B.Insert(0, "0");

            // insert bit pattern
            for (int i = 0; i < d1B.Length; i++)
            {
                if (d1B[i] == '1')
                    dcs[pos] = 1;
                else
                    dcs[pos] = 0;
                pos++;
            }

            // generate bit pattern for word 2
            string d2B = Convert.ToString(digit2, 2);
            if (d2B.Length <= 1)
                d2B = d2B.Insert(0, "00");
            if (d2B.Length <= 2)
                d2B = d2B.Insert(0, "0");

            // insert bit pattern
            for (int i = 0; i < d2B.Length; i++)
            {
                if (d2B[i] == '1')
                    dcs[pos] = 1;
                else
                    dcs[pos] = 0;
                pos++;
            }

            // generate bit pattern for word 3
            string d3B = Convert.ToString(digit3, 2);
            if (d3B.Length <= 1)
                d3B = d3B.Insert(0, "00");
            if (d3B.Length <= 2)
                d3B = d3B.Insert(0, "0");

            // insert bit pattern
            for (int i = 0; i < d3B.Length; i++)
            {
                if (d3B[i] == '1')
                    dcs[pos] = 1;
                else
                    dcs[pos] = 0;
                pos++;
            }

            return dcs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="digit1"></param>
        /// <param name="digit2"></param>
        /// <param name="digit3"></param>
        /// <returns></returns>
        public byte[] GenerateDPLBits(byte digit1, byte digit2, byte digit3)
        {
            byte[] dcs = new byte[23];
            byte[] raw = GenerateRawBits(digit1, digit2, digit3);
            byte[] parity = ReverseBitArray(GenerateParityBits(raw));
            Buffer.BlockCopy(parity, 0, dcs, 0, parity.Length);
            Buffer.BlockCopy(raw, 0, dcs, parity.Length, raw.Length);

            return ReverseBitArray(dcs);
        }
    } // public class DPLEncoder
} // namespace RepeaterController.DSP.DPL
