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
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using NAudio.Wave;

namespace RepeaterController
{
    /// <summary>
    /// Extension class for adding bit rotation.
    /// </summary>
    public static class BitRotateExtension
    {
        public static int RotateLeft(this int value, int count)
        {
            return (value << count) | (value >> (32 - count));
        }

        public static int RotateRight(this int value, int count)
        {
            return (value >> count) | (value << (32 - count));
        }

        public static int SparseBitCount(this int value)
        {
            int count = 0;
            while (value != 0)
            {
                count++;
                value &= (value - 1);
            }
            return count;
        }

        public static long RotateLeft(this long value, int count)
        {
            return (value << count) | (value >> (64 - count));
        }

        public static long RotateRight(this long value, int count)
        {
            return (value >> count) | (value << (64 - count));
        }

        public static long SparseBitCount(this long value)
        {
            int count = 0;
            while (value != 0)
            {
                count++;
                value &= (value - 1);
            }
            return count;
        }
    }

    /// <summary>
    /// A util class for conversions
    /// </summary>
    public class Decibels
    {
        /**
         * Fields
         */
        // 20 / ln( 10 )
        private const double LOG_2_DB = 8.6858896380650365530225783783321;

        // ln( 10 ) / 20
        private const double DB_2_LOG = 0.11512925464970228420089957273422;

        /** 
         * Methods
         */
        /// <summary>
        /// linear to dB conversion
        /// </summary>
        /// <param name="lin">linear value</param>
        /// <returns>decibel value</returns>
        public static double LinearToDecibels(double lin)
        {
            return Math.Log(lin) * LOG_2_DB;
        }

        /// <summary>
        /// dB to linear conversion
        /// </summary>
        /// <param name="dB">decibel value</param>
        /// <returns>linear value</returns>
        public static double DecibelsToLinear(double dB)
        {
            return Math.Exp(dB * DB_2_LOG);
        }
    } // public class Decibels

    /// <summary>
    /// 
    /// </summary>
    public class SamplesToMS
    {
        /**
         * Methods
         */
        /// <summary>
        /// (ms) to sample count conversion
        /// </summary>
        /// <param name="format">Wave format</param>
        /// <param name="ms">Number of milliseconds</param>
        /// <returns>Number of samples</returns>
        public static int MSToSamples(WaveFormat format, int ms)
        {
            return (int)(((long)ms) * format.SampleRate * format.Channels / 1000);
        }

        /// <summary>
        /// samples to bytes conversion
        /// </summary>
        /// <param name="format">Wave format</param>
        /// <param name="samples">Number of samples</param>
        /// <returns>Number of bytes for the number of samples</returns>
        public static int SamplesToBytes(WaveFormat format, int samples)
        {
            return samples * (format.BitsPerSample / 8);
        }

        /// <summary>
        /// (ms) to bytes conversion
        /// </summary>
        /// <param name="format">Wave format</param>
        /// <param name="ms">Number of milliseconds</param>
        /// <returns>Number of bytes for the amount of audio in (ms)</returns>
        public static int MSToSampleBytes(WaveFormat format, int ms)
        {
            return SamplesToBytes(format, MSToSamples(format, ms));
        }
    } // public class SamplesToMS

    /// <summary>
    /// Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx
    /// Provides a method for performing a deep copy of an object.
    /// Binary Serialization is used to perform the copy.
    /// </summary>
    public static class ObjectCopier
    {
        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    } // public static class ObjectCopier
} // namespace RepeaterController
