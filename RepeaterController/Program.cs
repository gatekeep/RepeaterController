/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.Windows.Forms;

namespace RepeaterController
{
    /// <summary>
    /// This class serves as the entry point for the C# application.
    /// </summary>
    public static class Program
    {
        /**
         * Fields
         */
        public static ushort OverrideTraceLevel = 0;

        public static ushort APIPort = 9292;
        public static ushort WebPort = 18080;

        /**
         * Methods
         */
        /// <summary>
        /// Internal helper to prints the program usage.
        /// </summary>
        private static void Usage(OptionSet p)
        {
            Console.WriteLine("usage: RepeaterController [--trace <trace level>]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
            Environment.Exit(0);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            bool showHelp = false;

            // configure trace logger
            Messages.SetupTextWriter(Environment.CurrentDirectory, "Trace.log");

            // command line parameters
            OptionSet options = new OptionSet()
            {
                { "h|help", "show this message and exit", v => showHelp = v != null },

                { "trace=", "sets the trace level to execute with", v => ushort.TryParse(v, out OverrideTraceLevel) },
            };

            // attempt to parse the commandline
            try
            {
                options.Parse(args);
            }
            catch (OptionException)
            {
                Console.WriteLine("error: invalid arguments");
                Usage(options);
                Environment.Exit(1);
            }

            if (OverrideTraceLevel != 0)
            {
                Messages.Trace("commandline trace level override [" + OverrideTraceLevel + "]");
                Messages.DirectSetTraceFilter(OverrideTraceLevel);
            }

            if (showHelp)
            {
                Console.WriteLine(AssemblyVersion._VERSION_STRING + " (Built: " + AssemblyVersion._BUILD_DATE + ")");
                Console.WriteLine(AssemblyVersion._COPYRIGHT + " All Rights Reserved.");
                Console.WriteLine();
            }

            // show help
            if (showHelp)
                Usage(options);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    } // public static class Program
} // namespace RepeaterController
