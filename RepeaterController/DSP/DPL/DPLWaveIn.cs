/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.Collections.Generic;
using System.Threading;

using NAudio.Wave;

namespace RepeaterController.DSP.DPL
{
    /// <summary>
    /// Implements a worker that waits for and decodes samples for a DPL signal.
    /// </summary>
    public class DPLWaveIn : IDisposable
    {
        /**
         * Fields
         */
        private const double LOW_PASS_FREQ = 280;
        private const int SAMPLE_BEFORE_DROP = 16;

        private WaveIn waveIn;
        private bool disposed = false;
        private Thread captureWorker;

        private readonly ISampleSource source;
        private readonly DPLDetector plDetector = new DPLDetector();

        private int lastDplTone;

        /**
         * Properties
         */
        /// <summary>
        /// Milliseconds for the buffer. Recommended value is 100ms
        /// </summary>
        public int BufferMilliseconds
        {
            get { return waveIn.BufferMilliseconds; }
            set { waveIn.BufferMilliseconds = value; }
        }

        /// <summary>
        /// Number of Buffers to use (usually 2 or 3)
        /// </summary>
        public int NumberOfBuffers
        {
            get { return waveIn.NumberOfBuffers; }
            set { waveIn.NumberOfBuffers = value; }
        }

        /// <summary>
        /// Flag indicating whether the live analyzer is capturing samples.
        /// </summary>
        public bool IsRecording { get; private set; }

        /// <summary>
        /// Gets the last detected DPL tone.
        /// </summary>
        public List<int> LastDPLTones { get; private set; }

        /**
         * Events
         */
        /// <summary>
        /// 
        /// </summary>
        public event Action<object, List<int>> DPLTonesDetected;

        /// <summary>
        /// 
        /// </summary>
        public event Action<object> DPLToneLost;

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="DPLWaveIn"/> class.
        /// </summary>
        /// <param name="waveFormat"></param>
        /// <param name="deviceNumber"></param>
        public DPLWaveIn(WaveFormat waveFormat, int deviceNumber)
        {
            this.waveIn = new WaveIn();
            this.waveIn.WaveFormat = waveFormat;
            this.waveIn.DeviceNumber = deviceNumber;

            this.LastDPLTones = new List<int>();
            this.source = new StreamingSampleSource(Buffer(waveIn), DPLDetector.SampleRate);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (IsRecording)
                    this.StopRecording();

                if (waveIn != null)
                    waveIn.Dispose();

                if (captureWorker != null)
                {
                    if (captureWorker.IsAlive)
                    {
                        captureWorker.Abort();
                        captureWorker.Join();
                    }

                    captureWorker = null;
                }
            }

            disposed = true;
        }

        /// <summary>
        /// Starts capturing samples for analysis.
        /// </summary>
        public void StartRecording()
        {
            if (IsRecording)
                return;

            waveIn.StartRecording();

            IsRecording = true;
            captureWorker = new Thread(Detect);
            captureWorker.Name = "DPLWaveIn";
            captureWorker.Start();
        }

        /// <summary>
        /// Stops capturing samples for analysis.
        /// </summary>
        public void StopRecording()
        {
            if (!IsRecording)
                return;

            IsRecording = false;
            captureWorker.Abort();
            captureWorker.Join();

            waveIn.StopRecording();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int CalculateIndex(int index)
        {
            if (index < 0)
                index = 23 + index;

            return (index % 23);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binary"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        private bool ParityCheck(byte[] binary, int startIndex)
        {
            // second word
            byte w2B3  = binary[CalculateIndex(startIndex - 11)];
            byte w2B2 = binary[CalculateIndex(startIndex - 10)];
            byte w2B1 = binary[CalculateIndex(startIndex - 9)];

            // third word
            byte w3B3 = binary[CalculateIndex(startIndex - 8)];
            byte w3B2 = binary[CalculateIndex(startIndex - 7)];
            byte w3B1 = binary[CalculateIndex(startIndex - 6)];

            // first word
            byte w1B3 = binary[CalculateIndex(startIndex - 5)];
            byte w1B2 = binary[CalculateIndex(startIndex - 4)];
            byte w1B1 = binary[CalculateIndex(startIndex - 3)];

            // parity bits
            byte partiyB1 = binary[CalculateIndex(startIndex + 1)];
            byte parityB2 = binary[CalculateIndex(startIndex + 2)];
            byte parityB3 = binary[CalculateIndex(startIndex + 3)];
            byte parityB4 = binary[CalculateIndex(startIndex + 4)];
            byte parityB5 = binary[CalculateIndex(startIndex + 5)];
            byte parityB6 = binary[CalculateIndex(startIndex + 6)];
            byte parityB7 = binary[CalculateIndex(startIndex + 7)];
            byte parityB8 = binary[CalculateIndex(startIndex + 8)];
            byte parityB9 = binary[CalculateIndex(startIndex + 9)];
            byte parityB10 = binary[CalculateIndex(startIndex + 10)];
            byte parityB11 = binary[CalculateIndex(startIndex + 11)];

            // validate parity
            if (partiyB1 != (((((w2B3 ^ w2B2) ^ w2B1) ^ w3B3) ^ w3B2) ^ w1B2))
                return false;
            if (parityB2 == (((((w2B2 ^ w2B1) ^ w3B3) ^ w3B2) ^ w3B1) ^ w1B1))
                return false;
            if (parityB3 != ((((w2B3 ^ w2B2) ^ w3B1) ^ w1B3) ^ w1B2))
                return false;
            if (parityB4 == ((((w2B2 ^ w2B1) ^ w1B3) ^ w1B2) ^ w1B1))
                return false;
            if (parityB5 == (((w2B3 ^ w2B2) ^ w3B2) ^ w1B1))
                return false;
            if (parityB6 == ((((w2B3 ^ w3B3) ^ w3B2) ^ w3B1) ^ w1B2))
                return false;
            if (parityB7 != ((((((w2B3 ^ w2B1) ^ w3B3) ^ w3B1) ^ w1B3) ^ w1B2) ^ w1B1))
                return false;
            if (parityB8 != (((((w2B2 ^ w3B3) ^ w3B2) ^ w1B3) ^ w1B2) ^ w1B1))
                return false;
            if (parityB9 != ((((w2B1 ^ w3B2) ^ w3B1) ^ w1B2) ^ w1B1))
                return false;
            if (parityB10 == (((w3B3 ^ w3B1) ^ w1B3) ^ w1B1))
                return false;
            if (parityB11 == ((((w2B3 ^ w2B2) ^ w2B1) ^ w3B3) ^ w1B3))
                return false;
            return true;
        }

        /// <summary>
        /// Bit flip all values in a given byte array. (i.e. literal 1 to 0, 0 to 1)
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private byte[] InvertBitArray(byte[] inp)
        {
            byte[] inv = new byte[inp.Length];
            for (int i = 0; i < inp.Length; i++)
            {
                if (inp[i] == 1)
                    inv[i] = 0;
                else
                    inv[i] = 1;
            }
            return inv;
        }

        /// <summary>
        /// Decode the 9-bits of octal data.
        /// </summary>
        /// <param name="bin"></param>
        /// <returns></returns>
        private List<int> BinaryToOctal(byte[] bin)
        {
            List<int> dplCodes = new List<int>();
            byte[] inv = InvertBitArray(bin);

            // build DPL list
            for (int i = 0; i < bin.Length; i++)
            {
                // detect the DPL sync bit
                if (((bin[i] == 1) && (bin[CalculateIndex(i - 1)] == 0)) && ((bin[CalculateIndex(i - 2)] == 0) && ParityCheck(bin, i)))
                {
                    // get the 3 octal words
                    byte dcsWord3 = (byte)(((bin[CalculateIndex(i - 6)] * 4) + (bin[CalculateIndex(i - 7)] * 2)) + (bin[CalculateIndex(i - 8)] * 1));
                    byte dcsWord2 = (byte)(((bin[CalculateIndex(i - 9)] * 4) + (bin[CalculateIndex(i - 10)] * 2)) + (bin[CalculateIndex(i - 11)] * 1));
                    byte dcsWord1 = (byte)(((bin[CalculateIndex(i - 3)] * 4) + (bin[CalculateIndex(i - 4)] * 2)) + (bin[CalculateIndex(i - 5)] * 1));

                    // generate code
                    int pCode = (dcsWord1 * 100) + (dcsWord3 * 10) + dcsWord2;
                    dplCodes.Add(pCode);
                }

                // detect the DPL sync bit (inverted)
                if (((inv[i] == 1) && (inv[CalculateIndex(i - 1)] == 0)) && ((inv[CalculateIndex(i - 2)] == 0) && ParityCheck(inv, i)))
                {
                    // get the 3 octal words
                    byte dcsWord3 = (byte)(((inv[CalculateIndex(i - 6)] * 4) + (inv[CalculateIndex(i - 7)] * 2)) + (inv[CalculateIndex(i - 8)] * 1));
                    byte dcsWord2 = (byte)(((inv[CalculateIndex(i - 9)] * 4) + (inv[CalculateIndex(i - 10)] * 2)) + (inv[CalculateIndex(i - 11)] * 1));
                    byte dcsWord1 = (byte)(((inv[CalculateIndex(i - 3)] * 4) + (inv[CalculateIndex(i - 4)] * 2)) + (inv[CalculateIndex(i - 5)] * 1));

                    // generate code
                    int iCode = (dcsWord1 * 100) + (dcsWord3 * 10) + dcsWord2;
                    dplCodes.Add(iCode);
                }
            }
            dplCodes.Sort();

            return dplCodes;
        }

        /// <summary>
        /// Helper to wait for a new DPL code.
        /// </summary>
        /// <returns></returns>
        public List<int> WaitForPLTone()
        {
            while (source.HasSamples)
            {
                plDetector.Analyze(source.Samples);
                if (plDetector.CodeAvailable)
                {
                    List<int> ret = BinaryToOctal(plDetector.ReceivedCode);
                    if (ret.Count > 0)
                    {
                        lastDplTone = ret[0];
                        return ret;
                    }
                }
            }

            return new List<int>();
        }

        /// <summary>
        /// Helper to wait for the end of the last detected PL tone.
        /// </summary>
        public void WaitForEndOfLastPLTone()
        {
            int cycleCount = 0;
            while (source.HasSamples)
            {
                plDetector.Analyze(source.Samples);
                if (plDetector.CodeAvailable)
                {
                    List<int> ret = BinaryToOctal(plDetector.ReceivedCode);
                    if (ret.Count > 0)
                    {
                        int nextCode = ret[0];
                        cycleCount = 0;

                        if (nextCode != lastDplTone)
                        {
                            Messages.Trace("Last DPL code " + lastDplTone + " next code " + nextCode, LogFilter.PL_DPL_TRACE);
                            return;
                        }
                    }
                    else
                    {
                        cycleCount++;
                        if (cycleCount > SAMPLE_BEFORE_DROP)
                            return;
                    }
                }
            }
        }

        /// <summary>
        /// Helper to dump detected tones.
        /// </summary>
        /// <param name="tones"></param>
        /// <returns></returns>
        private string TonesToString(List<int> tones)
        {
            string msg = "Code + ";
            for (int i = 0; i < 3; i++)
            {
                if (i > tones.Count)
                    break;
                msg += tones[i] + " ";
            }

            msg += "Code - ";
            for (int i = 3; i < 6; i++)
            {
                if (i > tones.Count)
                    break;
                msg += tones[i] + " ";
            }

            return msg;
        }

        /// <summary>
        /// Thread method for detecting DPL data.
        /// </summary>
        private void Detect()
        {
            while (true)
            {
                LastDPLTones = WaitForPLTone();
                DPLTonesDetected?.Invoke(this, LastDPLTones);
                Messages.Trace("DPL Detect: " + TonesToString(LastDPLTones), LogFilter.PL_DPL_TRACE);

                WaitForEndOfLastPLTone();
                DPLToneLost?.Invoke(this);
            }
        }

        /// <summary>
        /// Helper to generate a <see cref="AudioWaveProvider"/> wave source.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static AudioWaveProvider Buffer(IWaveIn source)
        {
            BiQuad filter = new LowpassFilter(source.WaveFormat.SampleRate, LOW_PASS_FREQ);
            AudioWaveProvider sourceBuffer = new AudioWaveProvider(source.WaveFormat) { DiscardOnBufferOverflow = true };
            source.DataAvailable += (sender, e) => sourceBuffer.AddSamples(e.Buffer, 0, e.BytesRecorded);
            sourceBuffer.FilterSampleCallback += (ref double leftSmp, ref double rightSmp) =>
            {
                leftSmp = filter.Process((float)leftSmp);
                rightSmp = filter.Process((float)rightSmp);
            };

            return sourceBuffer;
        }
    } // public class DPLWaveIn
} // namespace RepeaterController.DSP.DPL
