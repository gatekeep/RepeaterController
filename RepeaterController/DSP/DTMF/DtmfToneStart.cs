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
    /// Helper class ot hold a 'starting' DTMF tone.
    /// </summary>
    public class DtmfToneStart
    {
        /**
         * Properties
         */
        /// <summary>
        /// Gets the DTMF tone.
        /// </summary>
        public DtmfTone DtmfTone { get; }

        /// <summary>
        /// Gets the position of the DTMF tone in the source stream.
        /// </summary>
        public DateTime Position { get; }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="DtmfToneStart"/> class.
        /// </summary>
        /// <param name="dtmfTone"></param>
        /// <param name="position"></param>
        public DtmfToneStart(DtmfTone dtmfTone, DateTime position)
        {
            DtmfTone = dtmfTone;
            Position = position;
        }
    } // public class DtmfToneStart
} // namespace RepeaterController.DSP.DTMF
