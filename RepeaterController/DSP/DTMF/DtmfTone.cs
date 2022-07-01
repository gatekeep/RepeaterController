/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;

namespace RepeaterController.DSP.DTMF
{
    /// <summary>
    /// 
    /// </summary>
    public enum PhoneKey
    {
        None,
        Zero,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Star,
        Hash,
        A,
        B,
        C,
        D
    }

    /// <summary>
    /// Helper class that represents individual DTMF tones.
    /// </summary>
    public struct DtmfTone : IEquatable<DtmfTone>
    {
        /**
         * Properties
         */
        /// <summary>
        /// No DTMF
        /// </summary>
        public static DtmfTone None { get; } = new DtmfTone(0, 0, PhoneKey.None, string.Empty);

        /// <summary>
        /// Digit 0
        /// </summary>
        public static DtmfTone Zero { get; } = new DtmfTone(1336, 941, PhoneKey.Zero, "0");

        /// <summary>
        /// Digit 1
        /// </summary>
        public static DtmfTone One { get; } = new DtmfTone(1209, 697, PhoneKey.One, "1");

        /// <summary>
        /// Digit 2
        /// </summary>
        public static DtmfTone Two { get; } = new DtmfTone(1336, 697, PhoneKey.Two, "2");

        /// <summary>
        /// Digit 3
        /// </summary>
        public static DtmfTone Three { get; } = new DtmfTone(1477, 697, PhoneKey.Three, "3");

        /// <summary>
        /// Digit 4
        /// </summary>
        public static DtmfTone Four { get; } = new DtmfTone(1209, 770, PhoneKey.Four, "4");

        /// <summary>
        /// Digit 5
        /// </summary>
        public static DtmfTone Five { get; } = new DtmfTone(1336, 770, PhoneKey.Five, "5");

        /// <summary>
        /// Digit 6
        /// </summary>
        public static DtmfTone Six { get; } = new DtmfTone(1477, 770, PhoneKey.Six, "6");

        /// <summary>
        /// Digit 7
        /// </summary>
        public static DtmfTone Seven { get; } = new DtmfTone(1209, 852, PhoneKey.Seven, "7");

        /// <summary>
        /// Digit 8
        /// </summary>
        public static DtmfTone Eight { get; } = new DtmfTone(1336, 852, PhoneKey.Eight, "8");

        /// <summary>
        /// Digit 9
        /// </summary>
        public static DtmfTone Nine { get; } = new DtmfTone(1477, 852, PhoneKey.Nine, "9");

        /// <summary>
        /// Asterick (*)
        /// </summary>
        public static DtmfTone Star { get; } = new DtmfTone(1209, 941, PhoneKey.Star, "*");

        /// <summary>
        /// Pound/Hash (#)
        /// </summary>
        public static DtmfTone Hash { get; } = new DtmfTone(1477, 941, PhoneKey.Hash, "#");

        /// <summary>
        /// Digit A
        /// </summary>
        public static DtmfTone A { get; } = new DtmfTone(1633, 697, PhoneKey.A, "A");

        /// <summary>
        /// Digit B
        /// </summary>
        public static DtmfTone B { get; } = new DtmfTone(1633, 770, PhoneKey.B, "B");

        /// <summary>
        /// Digit C
        /// </summary>
        public static DtmfTone C { get; } = new DtmfTone(1633, 852, PhoneKey.C, "C");

        /// <summary>
        /// Digit D
        /// </summary>
        public static DtmfTone D { get; } = new DtmfTone(1633, 941, PhoneKey.D, "D");

        /// <summary>
        /// 
        /// </summary>
        public PhoneKey Key { get; }

        /// <summary>
        /// 
        /// </summary>
        public int HighTone { get; }

        /// <summary>
        /// 
        /// </summary>
        public int LowTone { get; }

        /// <summary>
        /// 
        /// </summary>
        public string KeyString { get; }

        /**
         * Operators
         */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(DtmfTone a, DtmfTone b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(DtmfTone a, DtmfTone b)
        {
            return !(a == b);
        }

        /** 
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="DtmfTone"/> class.
        /// </summary>
        /// <param name="highTone"></param>
        /// <param name="lowTone"></param>
        /// <param name="key"></param>
        /// <param name="keyString"></param>
        private DtmfTone(int highTone, int lowTone, PhoneKey key, string keyString)
        {
            HighTone = highTone;
            LowTone = lowTone;
            Key = key;
            KeyString = keyString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Key.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            if (obj is DtmfTone)
                return Equals((DtmfTone)obj);

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(DtmfTone other)
        {
            return Key == other.Key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    } // public struct DtmfTone : IEquatable<DtmfTone>
} // namespace RepeaterController.DSP.DTMF
