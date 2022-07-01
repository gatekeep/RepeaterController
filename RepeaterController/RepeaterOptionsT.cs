/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Windows.Forms;

using RepeaterController.DSP;
using RepeaterController.Xml;

namespace RepeaterController
{
    /// <summary>
    /// This structure defines the various repeater options.
    /// </summary>
    [DataContract]
    public class repeater_options_t
    {
        /**
         * Fields
         */
        // ID and MDC settings
        /// <summary>
        /// Configured MDC1200 ID of the repeater.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public ushort MyID;
        /// <summary>
        /// Configured callsign of the repeater.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public string Callsign;

        /// <summary>
        /// Interval in minutes the repeater should ID itself.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public int IdInterval;
        /// <summary>
        /// Flag indicating whether we should ID ourselves.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public bool NoId;
        /// <summary>
        /// Flag indicating whether we should ID ourselves on application startup.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public bool NoIdStartup;
        /// <summary>
        /// Flag indicating whether when we have Tx PL/DPL enabled, we should transmit the ID without PL/DPL.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public bool DisablePLForId;

        /// <summary>
        /// Indicates the number of MDC1200 preambles to send for a MDC1200 packet.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public int NumberOfPreambles;

        /// <summary>
        /// Key used to trigger MDC PTT ID transmit.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public int PTTKey;

        /// <summary>
        /// Flag indicating whether we should automatically acknowledge emergency packets.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public bool AutoAckEmergency;

        /// <summary>
        /// Flag indicating whether the repeater must be access using an MDC1200 RAC code.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public bool UseRAC;
        /// <summary>
        /// MDC1200 Repeater Access Code
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public ushort AccessRAC;

        // Rx Settings
        /// <summary>
        /// Serial port to use for receiver activity detection
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public string RxSerialPort;
        /// <summary>
        /// Flag indicating whether to use PL to use while receiving.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public bool EnableRxPL;
        /// <summary>
        /// PL to use while receiving.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public int RxPL;
        /// <summary>
        /// DPL to use while receiving.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public int RxDPL;
        /// <summary>
        /// Flag indicating whether to use DPL while receiving.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public bool UseRxDPL;
        /// <summary>
        /// Pin asserted when the receiver is receiving
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public SerialPortPin RxAssertPin;
        /// <summary>
        /// Receiver is using VOX for activity detection
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public bool RxVOX;

        /// <summary>
        /// Audio detection level.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public double VOXDetectLevel;

        // Tx Settings
        /// <summary>
        /// Serial port to use for transmitter activity detection
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public string TxSerialPort;
        /// <summary>
        /// Flag indicating whether to use PL to use while transmitting.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public bool EnableTxPL;
        /// <summary>
        /// PL to use while transmitting.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public int TxPL;
        /// <summary>
        /// DPL to use while transmitting.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public int TxDPL;
        /// <summary>
        /// Flag indicating whether to use DPL while transmitting.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public bool UseTxDPL;
        /// <summary>
        /// Gain of the transmitted PL/DPL signal.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public double TxPLGain;
        /// <summary>
        /// Pin asserted when the transmitter is receiving
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public SerialPortPin TxAssertPin;
        /// <summary>
        /// Transmitter is using VOX for activity detection
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public bool TxVOX;
        /// <summary>
        /// Transmit Watch Dog Timer (sec)
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public int WatchDogTime;

        // Timers & CT
        /// <summary>
        /// Maximum length of a transmission.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public int MaxTransmissionTime;
        /// <summary>
        /// Tail time before dropping transmitter.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public int TailTime;

        /// <summary>
        /// Amount of time between DTMF digits for detection.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public double DTMFDigitTime;

        /// <summary>
        /// Flag indicating whether we should have a courtesy tone.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public bool NoCourtesyTone;

        /// <summary>
        /// Initial delay before playing courtesy tone (ms).
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public int CourtesyDelay;
        /// <summary>
        /// Pitch of courtesy tone (Hz).
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public int CourtesyPitch;
        /// <summary>
        /// Length of courtesy tone (ms).
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public int CourtesyLength;

        /// <summary>
        /// Flag indicating if the courtesy tone consists of multiple tones.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public bool CourtesyMultiTone;
        /// <summary>
        /// List of tones to use for a multi-tone courtesy tone.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public List<Multitone> CourtesyTones;

        /// <summary>
        /// Flag indicating whether to playback a wave file for courtesy tone.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public bool CourtesyUseFile;
        /// <summary>
        /// Wave file to use as courtesy tone.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public string CourtesyFile;

        /// <summary>
        /// Flag indicating whether announcments are disabled or not.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public bool DisableAnnouncements;
        /// <summary>
        /// Selected synthesized voice announcment voice.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public string AnnouncementVoice;
        /// <summary>
        /// Selected synthesized voice audio gain.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public double SynthAnncGain;
        /// <summary>
        /// Use the sysetm name during time announcement.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public bool UseSystemName;
        /// <summary>
        /// String containing the name of the 'system'.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public string SystemName;

        /// <summary>
        /// Flag indicating whether or not database logging is enabled.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public bool EnableDatabaseLogging;

        /// <summary>
        /// String containing the name of the database on the database server.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public string DatabaseName;
        /// <summary>
        /// String containing the IP/hostname of the database server.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public string DatabaseServer;
        /// <summary>
        /// String containing the username to authenticate with the database server.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public string DatabaseUsername;
        /// <summary>
        /// String containing the password to authenticate with the database server.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public string DatabasePassword;

        // Global Settings
        /// <summary>
        /// Flag indicating whether announcments and DTMF are enabled in MDC console mode.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public bool AllowConsoleAnncDTMF;

        /// <summary>
        /// Flag indicating whether we should operate in MDC console only mode.
        /// </summary>
        [DataMember]
        [XmlImportExport]
        public bool MDCConsoleOnly;
    } // public struct repeater_options_t
} //namespace RepeaterController
