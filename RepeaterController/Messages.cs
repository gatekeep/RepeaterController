/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.ServiceModel;
using System.Windows.Forms;

namespace RepeaterController
{
    /// <summary>
    /// Enum that represents the individual trace flags.
    /// </summary>
    [Flags]
    public enum LogFilter : ushort
    {
        ALL_TRACE_OFF = 0x0000,           // Dec: 0

        GENERAL_TRACE = 0x0001,           // Dec: 1
        SERVICE_VERB_TRACE = 0x0002,      // Dec: 2
        PL_DPL_TRACE = 0x0004,            // Dec: 4
        DTMF_TRACE = 0x0008,              // Dec: 8

        VERBOSE_TRACE = 0x0010,           // Dec: 16
        PCKT_TRACE = 0x0020,              // Dec: 32

        HTTP_TRACE = 0x0040,              // Dec: 64
        NETWORK_TRACE = 0x0080,           // Dec: 128

        TEMPLATE_TRACE = 0x1000,          // Dec: 4096
        AUTH_TRACE = 0x2000,              // Dec: 8192
        VAR_DUMP_TRACE = 0x4000,          // Dec: 16384
        CMD_DUMP_TRACE = 0x8000,          // Dec: 32768

        ENABLE_ALL_TRACE = 0xFFFF,        // Dec: 65535
    }

    /// <summary>
    /// Implements simple handler to dump an exception.
    /// </summary>
    public class Messages
    {
        /**
         * Fields
         */
        public static FileStream logStream = null;
        public static bool DisplayToConsole = false;
        private static TextWriter tw;
        private static LogFilter traceFilter = LogFilter.GENERAL_TRACE;
        private const string LOG_TIME_FORMAT = "MM/dd/yyyy HH:mm:ss";

        private static ConsoleColor defColor = Console.ForegroundColor;

        /**
         * Properties
         */
        /// <summary>
        /// Gets the trace filter or sets/unsets messages to trace.
        /// </summary>
        public static LogFilter TraceFilter
        {
            get { return traceFilter; }
            set
            {
                if (traceFilter.HasFlag(value))
                    traceFilter &= ~value;
                else
                    traceFilter |= value;

                DumpFilterSettingToLog();
            }
        }

        /**
         * Methods
         */
        /// <summary>
        /// Sets up the trace logging.
        /// </summary>
        /// <param name="directoryPath"></param>
        public static void SetupTextWriter(string directoryPath, string logFile)
        {
            // destroy existing log file
            if (File.Exists(directoryPath + Path.DirectorySeparatorChar + logFile))
                File.Delete(directoryPath + Path.DirectorySeparatorChar + logFile);

            // open a new log stream
            logStream = new FileStream(directoryPath + Path.DirectorySeparatorChar + logFile, FileMode.CreateNew, FileAccess.Write);
            tw = new StreamWriter(logStream);
        }

        /// <summary>
        /// Writes a log entry to the text log.
        /// </summary>
        /// <param name="message"></param>
        public static void WriteLog(string message)
        {
            string logTime = DateTime.Now.ToString(LOG_TIME_FORMAT) + " ";
            if (tw != null)
            {
                tw.WriteLine(logTime + message);
                tw.Flush();
            }

            System.Diagnostics.Trace.WriteLine(logTime + message);
            if (DisplayToConsole)
            {
                Console.Write(logTime);

                if (message.StartsWith("CTRL_WARN"))
                    Console.ForegroundColor = ConsoleColor.Yellow;
                else if (message.StartsWith("CTRL_FAIL") || message.StartsWith("CTRL_ERROR"))
                    Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine(message);

                Console.ForegroundColor = defColor;
            }
        }

        public static void DirectSetTraceFilter(ushort myTraceFilter)
        {
            traceFilter = (LogFilter)myTraceFilter;
            DumpFilterSettingToLog();
        }

        public static string DumpFilterSettingToLog()
        {
            ushort value = (ushort)traceFilter;

            // dump current trace level to log
            string outMsg = "traceFilter set to (" + value.ToString("X4") + ")[";
            if (traceFilter.HasFlag(LogFilter.GENERAL_TRACE))
                outMsg += "GENERAL_TRACE, ";
            if (traceFilter.HasFlag(LogFilter.SERVICE_VERB_TRACE))
                outMsg += "SERVICE_VERB_TRACE, ";
            if (traceFilter.HasFlag(LogFilter.PL_DPL_TRACE))
                outMsg += "PL_DPL_TRACE, ";
            if (traceFilter.HasFlag(LogFilter.DTMF_TRACE))
                outMsg += "DTMF_TRACE, ";

            if (traceFilter.HasFlag(LogFilter.VERBOSE_TRACE))
                outMsg += "VERBOSE_TRACE, ";
            if (traceFilter.HasFlag(LogFilter.PCKT_TRACE))
                outMsg += "PCKT_TRACE, ";

            if (traceFilter.HasFlag(LogFilter.AUTH_TRACE))
                outMsg += "AUTH_TRACE, ";

            if (traceFilter.HasFlag(LogFilter.VAR_DUMP_TRACE))
                outMsg += "VAR_DUMP_TRACE, ";
            if (traceFilter.HasFlag(LogFilter.CMD_DUMP_TRACE))
                outMsg += "CMD_DUMP_TRACE, ";

            outMsg = outMsg.TrimEnd(new char[] { ',', ' ' }) + "]";
            WriteLog(outMsg);

            return outMsg;
        }

        /// <summary>
        /// Returns a HTML formatted string for the given exception.
        /// </summary>
        /// <param name="throwable"></param>
        /// <returns></returns>
        public static string HtmlStackTrace(Exception throwable)
        {
            string exMessage = string.Empty;
            Exception inner = throwable.InnerException;

            exMessage += "<code>---- TRACE SNIP ----<br />";
            exMessage += throwable.Message + (inner != null ? " (Inner: " + inner.Message + ")" : "") + "<br />";
            exMessage += throwable.GetType().ToString() + "<br />";

            FaultException fe = throwable as FaultException;
            if (fe != null)
            {
                exMessage += "<br />Service Fault Code [" + fe.Code.Name + "]<br />";
                exMessage += "Service Fault Action [" + fe.Action + "]<br />";
                exMessage += "Service Fault Reason [" + fe.Reason.ToString() + "]<br />";
            }

            exMessage += "<br />" + throwable.Source + "<br />";
            foreach (string str in throwable.StackTrace.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
                exMessage += str + "<br />";
            if (inner != null)
                foreach (string str in throwable.StackTrace.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
                    exMessage += "inner trace: " + str + "<br />";
            exMessage += "---- TRACE SNIP ----</code>";

            return exMessage;
        }

        /// <summary>
        /// Writes the exception stack trace to the console/trace log
        /// </summary>
        /// <param name="throwable">Exception to obtain information from</param>
        /// <param name="reThrow"></param>
        public static void StackTrace(Exception throwable, bool reThrow = true)
        {
            StackTrace(string.Empty, throwable, reThrow);
        }

        /// <summary>
        /// Writes the exception stack trace to the console/trace log
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="throwable">Exception to obtain information from</param>
        /// <param name="reThrow"></param>
        public static void StackTrace(string msg, Exception throwable, bool reThrow = true)
        {
            MethodBase mb = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod();
            ParameterInfo[] param = mb.GetParameters();
            string funcParams = string.Empty;
            for (int i = 0; i < param.Length; i++)
                if (i < param.Length - 1)
                    funcParams += param[i].ParameterType.Name + ", ";
                else
                    funcParams += param[i].ParameterType.Name;

            Exception inner = throwable.InnerException;

            WriteLog("CTRL_FAIL: caught an unrecoverable exception! " + msg);
            WriteLog("---- TRACE SNIP ----");
            WriteLog(throwable.Message + (inner != null ? " (Inner: " + inner.Message + ")" : ""));
            WriteLog(throwable.GetType().ToString());

            FaultException fe = throwable as FaultException;
            if (fe != null)
            {
                WriteLog("Service Fault Code [" + fe.Code.ToString() + "]");
                WriteLog("Service Fault Action [" + fe.Action + "]");
                WriteLog("Service Fault Reason [" + fe.Reason.ToString() + "]");
            }

            WriteLog("<" + mb.ReflectedType.Name + "::" + mb.Name + "(" + funcParams + ")>");
            WriteLog(throwable.Source);
            foreach (string str in throwable.StackTrace.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
                WriteLog(str);
            if (inner != null)
                foreach (string str in throwable.StackTrace.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
                    WriteLog("inner trace: " + str);
            WriteLog("---- TRACE SNIP ----");

            if (reThrow)
                throw throwable;
        }

        /// <summary>
        /// Writes the exception stack trace to the received stream
        /// </summary>
        /// <param name="errorMessage">Error message</param>
        /// <param name="throwable">Exception to obtain information from</param>
        public static void Write(string errorMessage, Exception throwable)
        {
            StringBuilder sb = new StringBuilder();
            MethodBase mb = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod();
            ParameterInfo[] param = mb.GetParameters();
            string funcParams = string.Empty;
            for (int i = 0; i < param.Length; i++)
                if (i < param.Length - 1)
                    funcParams += param[i].ParameterType.Name + ", ";
                else
                    funcParams += param[i].ParameterType.Name;

            sb.AppendLine(errorMessage + " " + throwable.Message + "\n");
            sb.AppendLine("<" + mb.ReflectedType.Name + "::" + mb.Name + "(" + funcParams + ")>");
            sb.AppendLine(throwable.GetType().ToString());
            sb.AppendLine(throwable.Source);
            sb.AppendLine("\n" + throwable.StackTrace);
            sb.AppendLine("\n");

            string trace = "---- SNIP ----\n" + sb.ToString() + "\n" + "---- SNIP ----";
#if TRACE
            System.Diagnostics.Trace.WriteLine(trace);
#endif
            WriteLog(trace);
            MessageBox.Show(sb.ToString(), "RepeaterController Exception Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Writes the exception stack trace to the received stream
        /// </summary>
        /// <param name="errorMessage">Error message</param>
        /// <param name="throwable">Exception to obtain information from</param>
        public static void TraceException(string errorMessage, Exception throwable)
        {
            StringBuilder sb = new StringBuilder();
            MethodBase mb = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod();
            ParameterInfo[] param = mb.GetParameters();
            string funcParams = string.Empty;
            for (int i = 0; i < param.Length; i++)
                if (i < param.Length - 1)
                    funcParams += param[i].ParameterType.Name + ", ";
                else
                    funcParams += param[i].ParameterType.Name;

            sb.AppendLine(errorMessage + "\n" + throwable.Message + "\n");
            sb.AppendLine("<" + mb.ReflectedType.Name + "::" + mb.Name + "(" + funcParams + ")>");
            sb.AppendLine(throwable.GetType().ToString());
            sb.AppendLine(throwable.Source);
            sb.AppendLine("\n" + throwable.StackTrace);

            string trace = "---- SNIP ----\n" + sb.ToString() + "\n" + "---- SNIP ----";
#if TRACE
            System.Diagnostics.Trace.WriteLine(trace);
#endif
            WriteLog(trace);
        }

        /// <summary>
        /// Writes a error trace message w/ calling function information.
        /// </summary>
        /// <param name='message'>Message to print</param>
        public static void WriteWarning(string message)
        {
            WriteLog("CTRL_WARN: " + message);
        }

        /// <summary>
        /// Writes a error trace message w/ calling function information.
        /// </summary>
        /// <param name='message'>Message to print</param>
        public static void WriteError(string message)
        {
            WriteLog("CTRL_ERROR: " + message);
        }

        /// <summary>
        /// Writes a trace message for an unimplemented RPC.
        /// </summary>
        public static void Unimplemented()
        {
            Trace("unimplemented RPC routine called", LogFilter.GENERAL_TRACE, 2);
        }

        /// <summary>
        /// Converts a trace filter into a textual name.
        /// </summary>
        /// <param name="traceFilter"></param>
        /// <returns></returns>
        private static string TraceFilterToString(LogFilter myTraceFilter)
        {
            switch (myTraceFilter)
            {
                case LogFilter.AUTH_TRACE:
                    return "[AUTH] ";
                case LogFilter.VAR_DUMP_TRACE:
                    return "[VAR_DUMP] ";
                case LogFilter.CMD_DUMP_TRACE:
                    return "[CMD_DUMP] ";

                case LogFilter.PL_DPL_TRACE:
                    return "[PL/DPL] ";
                case LogFilter.DTMF_TRACE:
                    return "[DTMF] ";

                case LogFilter.PCKT_TRACE:
                    return "[PCKT] ";

                case LogFilter.GENERAL_TRACE:
                case LogFilter.VERBOSE_TRACE:
                case LogFilter.SERVICE_VERB_TRACE:
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Writes a trace message w/ calling function information.
        /// </summary>
        /// <param name="message">Message to print to debug window</param>
        public static void Trace(string message, LogFilter myTraceFilter = LogFilter.GENERAL_TRACE, int frame = 1)
        {
            // print trace if we have a level higher then or equal to 1
            if (traceFilter.HasFlag(myTraceFilter))
            {
                string trace = TraceFilterToString(myTraceFilter);

                if (myTraceFilter != LogFilter.GENERAL_TRACE)
                {
                    System.Diagnostics.StackFrame stackFrame = new System.Diagnostics.StackTrace(0, true).GetFrame(frame);
                    MethodBase mb = stackFrame.GetMethod();
                    ParameterInfo[] param = mb.GetParameters();
                    string funcParams = string.Empty;
                    for (int i = 0; i < param.Length; i++)
                        if (i < param.Length - 1)
                            funcParams += param[i].ParameterType.Name + ", ";
                        else
                            funcParams += param[i].ParameterType.Name;

                    trace += "<" + mb.ReflectedType.Name + "::" + mb.Name + "(" + funcParams + "), line " + stackFrame.GetFileLineNumber() + "> ";
                }
                trace += message;
                WriteLog(trace);
            }
        }

        /// <summary>
        /// Helper to dump a hex trace for a buffer.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="buffer"></param>
        /// <param name="maxLength"></param>
        /// <param name="startOffset"></param>
        public static void TraceHex(string message, byte[] buffer, int maxLength = 32, int startOffset = 0)
        {
#if DEBUG
            int bCount = 0, j = 0, lenCount = 0;

            // iterate through buffer printing all the stored bytes
            string traceMsg = message + " Off [" + j.ToString("X4") + "] -> [";
            for (int i = startOffset; i < buffer.Length; i++)
            {
                byte b = buffer[i];

                // split the message every 16 bytes...
                if (bCount == 16)
                {
                    traceMsg += "]";
                    Console.WriteLine(traceMsg);

                    bCount = 0;
                    j += 16;
                    traceMsg = message + " Off [" + j.ToString("X4") + "] -> [";
                }
                else
                    traceMsg += (bCount > 0) ? " " : "";

                traceMsg += b.ToString("X2");

                bCount++;

                // increment the length counter, and check if we've exceeded the specified
                // maximum, then break the loop
                lenCount++;
                if (lenCount > maxLength)
                    break;
            }

            // if the byte count at this point is non-zero print the message
            if (bCount != 0)
            {
                traceMsg += "]";
                Console.WriteLine(traceMsg);
            }
#endif
        }
    } // public class Messages
} // namespace RepeaterController
