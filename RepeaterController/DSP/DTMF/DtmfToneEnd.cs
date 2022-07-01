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
    /// Helper class ot hold a 'ending' DTMF tone.
    /// </summary>
    public class DtmfToneEnd
    {
        /**
         * Properties
         */
        /// <summary>
        /// Gets the DTMF tone.
        /// </summary>
        public DtmfTone DtmfTone { get; }

        /// <summary>
        /// Gets the duration of the DTMF tone.
        /// </summary>
        public TimeSpan Duration { get; }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="DtmfToneEnd"/> class.
        /// </summary>
        /// <param name="dtmfTone"></param>
        /// <param name="duration"></param>
        public DtmfToneEnd(DtmfTone dtmfTone, TimeSpan duration)
        {
            DtmfTone = dtmfTone;
            Duration = duration;
        }
    } // public class DtmfToneEnd
} // namespace RepeaterController.DSP.DTMF
