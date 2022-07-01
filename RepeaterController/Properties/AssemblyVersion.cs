/**
 * RepeaterController
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@gmail.com>
 */
using System;
using System.Reflection;

namespace RepeaterController
{
    /// <summary>
    /// Static class to build the engine version string.
    /// </summary>
    public class AssemblyVersion
    {
        /**
         * Fields
         */
        public static string _NAME;
        public static string _VERSION;
        public static string _BUILD;
        public static string _BUILD_DATE;
        public static string _COPYRIGHT;
        public static string _VERSION_STRING;

        public const string _CODE_NAME = "Knox";

        /**
         * Methods
         */
        /// <summary>
        /// Initializes static members of the AssemblyVersion class.
        /// </summary>
        static AssemblyVersion()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            AssemblyTitleAttribute asmTitle = asm.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0] as AssemblyTitleAttribute;
            AssemblyDescriptionAttribute asmDesc = asm.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0] as AssemblyDescriptionAttribute;
            AssemblyCopyrightAttribute asmCopyright = asm.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0] as AssemblyCopyrightAttribute;
            DateTime buildDate = new DateTime(2000, 1, 1).AddDays(asm.GetName().Version.Build).AddSeconds(asm.GetName().Version.Revision * 2);
            _NAME = asmTitle.Title;
            _VERSION = asm.GetName().Version.Major + "." + asm.GetName().Version.Minor;
            _BUILD = string.Empty + "B" + asm.GetName().Version.Build + "R" + asm.GetName().Version.Revision;
            _BUILD_DATE = buildDate.ToShortDateString() + " at " + buildDate.ToShortTimeString();
            _COPYRIGHT = asmCopyright.Copyright;

#if DEBUG
            _VERSION_STRING = AssemblyVersion._NAME + " " + AssemblyVersion._VERSION + " DEBUG_DNR (" + _CODE_NAME + ") build " + AssemblyVersion._BUILD;
#else
            _VERSION_STRING = AssemblyVersion._NAME + " " + AssemblyVersion._VERSION + " (" + _CODE_NAME + ") build " + AssemblyVersion._BUILD;
#endif
        }
    } // public class AssemblyVersion
} // namespace RepeaterController
