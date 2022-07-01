/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.Collections.Generic;

namespace RepeaterController.DSP.DTMF
{
    /// <summary>
    /// Helper class to classify the raw DTMF 2-tone into an appropriate tone.
    /// </summary>
    public static class DtmfClassification
    {
        /**
         * Fields
         */
        private static readonly Dictionary<Tuple<int, int>, DtmfTone> DtmfTones = new Dictionary<Tuple<int, int>, DtmfTone>
        { 
            [KeyOf(DtmfTone.One)]   = DtmfTone.One,   [KeyOf(DtmfTone.Two)]   = DtmfTone.Two,   [KeyOf(DtmfTone.Three)] = DtmfTone.Three, [KeyOf(DtmfTone.A)] = DtmfTone.A,
            [KeyOf(DtmfTone.Four)]  = DtmfTone.Four,  [KeyOf(DtmfTone.Five)]  = DtmfTone.Five,  [KeyOf(DtmfTone.Six)]   = DtmfTone.Six,   [KeyOf(DtmfTone.B)] = DtmfTone.B,
            [KeyOf(DtmfTone.Seven)] = DtmfTone.Seven, [KeyOf(DtmfTone.Eight)] = DtmfTone.Eight, [KeyOf(DtmfTone.Nine)]  = DtmfTone.Nine,  [KeyOf(DtmfTone.C)] = DtmfTone.C,
            [KeyOf(DtmfTone.Star)]  = DtmfTone.Star,  [KeyOf(DtmfTone.Zero)]  = DtmfTone.Zero,  [KeyOf(DtmfTone.Hash)]  = DtmfTone.Hash,  [KeyOf(DtmfTone.D)] = DtmfTone.D
        };

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="DtmfClassification"/> class.
        /// </summary>
        /// <param name="highTone"></param>
        /// <param name="lowTone"></param>
        public static DtmfTone For(int highTone, int lowTone)
        {
            DtmfTone tone;
            return DtmfTones.TryGetValue(KeyOf(highTone, lowTone), out tone) ? tone : DtmfTone.None;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static Tuple<int, int> KeyOf(DtmfTone t) => Tuple.Create(t.HighTone, t.LowTone);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="highTone"></param>
        /// <param name="lowTone"></param>
        /// <returns></returns>
        private static Tuple<int, int> KeyOf(int highTone, int lowTone) => Tuple.Create(highTone, lowTone);
    } // public static class DtmfClassification
} // namespace RepeaterController.DSP.DTMF
