// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Common_Library.Config.REG;
using Common_Library.Registry;
using Common_Library.Utils;

namespace Common_Library.Workstation
{
    public enum OSVersion
    {
        Unknown,
        Win32S,
        Win95,
        Win98,
        WinME,
        NT351,
        NT40,
        Win2000,
        WinXP,
        Win2003,
        Vista,
        Win7,
        Win8,
        Win81,
        Win10,
        WinCE,
        Unix,
        MacOS,
        XBox
    }

    public enum OSType
    {
        Unknown,
        WindowsVeryOld,
        WindowsOld,
        Windows,
        Unix,
        MacOS,
        Xbox
    }

    public enum SoftwareArchitecture
    {
        Unknown,
        Bit32,
        Bit64
    }

    public static partial class SoftwareInfo
    {
        public struct OSData
        {
            public OSType OSType { get; }
            public OSVersion OSVersion { get; }

            public OSData(OSType type, OSVersion version)
            {
                OSType = type;
                OSVersion = version;
            }
        }

        public static SoftwareArchitecture GetSoftwareArchitecture()
        {
            return (IntPtr.Size * 8) switch
            {
                64 => SoftwareArchitecture.Bit64,
                32 => SoftwareArchitecture.Bit32,
                _ => SoftwareArchitecture.Unknown
            };
        }

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr LoadLibrary(String libraryName);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr GetProcAddress(IntPtr hwnd, String procedureName);

        private delegate Boolean IsWow64ProcessDelegate([In] IntPtr handle, [Out] out Boolean isWow64Process);

        private static IsWow64ProcessDelegate GetIsWow64ProcessDelegate()
        {
            IntPtr handle = LoadLibrary("kernel32");

            if (handle == IntPtr.Zero)
            {
                return null;
            }

            IntPtr fnPtr = GetProcAddress(handle, "IsWow64Process");

            if (fnPtr != IntPtr.Zero)
            {
                return (IsWow64ProcessDelegate) Marshal.GetDelegateForFunctionPointer(fnPtr, typeof(IsWow64ProcessDelegate));
            }

            return null;
        }

        private static Boolean Is32BitProcessOn64BitProcessor()
        {
            IsWow64ProcessDelegate fnDelegate = GetIsWow64ProcessDelegate();

            Boolean retVal = fnDelegate.Invoke(Process.GetCurrentProcess().Handle, out Boolean isWow64);

            return retVal && isWow64;
        }

        public static SoftwareArchitecture GetOSArchitecture()
        {
            SoftwareArchitecture bit = (IntPtr.Size * 8) switch
            {
                64 => SoftwareArchitecture.Bit64,
                32 => (Is32BitProcessOn64BitProcessor() ? SoftwareArchitecture.Bit64 : SoftwareArchitecture.Bit32),
                _ => SoftwareArchitecture.Unknown
            };

            return bit;
        }

        private static readonly Config.Config Config = new REGConfig(BaseKey.LocalMachine, "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion");

        private static Boolean IsWindows10()
        {
            String productName = Config.GetValue("ProductName");

            return productName.StartsWith("Windows 10", StringComparison.OrdinalIgnoreCase);
        }

        private static Int32 MajorVersion
        {
            get
            {
                if (IsWindows10())
                {
                    return 10;
                }

                String exactVersion = OSVersionRegistry;

                if (String.IsNullOrEmpty(exactVersion))
                {
                    return Environment.OSVersion.Version.Major;
                }

                String splitVersion = exactVersion.Split('.').TryGetValue(1, "0");
                Int32.TryParse(splitVersion, out Int32 value);
                return value;
            }
        }

        private static Int32 MinorVersion
        {
            get
            {
                if (IsWindows10())
                {
                    return 0;
                }

                String exactVersion = OSVersionRegistry;

                if (String.IsNullOrEmpty(exactVersion))
                {
                    return Environment.OSVersion.Version.Minor;
                }

                String splitVersion = exactVersion.Split('.').TryGetValue(1, "0");
                Int32.TryParse(splitVersion, out Int32 value);
                return value;
            }
        }

        private static String OSVersionRegistry
        {
            get
            {
                return Config.GetValue("CurrentVersion", String.Empty);
            }
        }

        public static Version Version
        {
            get
            {
                return new Version(MajorVersion, MinorVersion, BuildVersion, RevisionVersion);
            }
        }

        private static Int32 BuildVersion
        {
            get
            {
                return Config.GetValue("CurrentBuildNumber", 0);
            }
        }

        private static Int32 RevisionVersion
        {
            get
            {
                return IsWindows10() ? 0 : Environment.OSVersion.Version.Revision;
            }
        }

        public static String ServicePack
        {
            get
            {
                OSVERSIONINFOEX osVersionInfo = new OSVERSIONINFOEX {dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX))};
                
                return GetVersionEx(ref osVersionInfo) ? osVersionInfo.szCSDVersion : String.Empty;
            }
        }

        public static OSData GetOSVersion()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32S:
                    return new OSData(OSType.WindowsVeryOld, OSVersion.Win32S);
                case PlatformID.Win32Windows:
                    switch (Environment.OSVersion.Version.Minor)
                    {
                        case 0:
                            return new OSData(OSType.WindowsVeryOld, OSVersion.Win95);
                        case 10:
                            return new OSData(OSType.WindowsVeryOld, OSVersion.Win98);
                        case 90:
                            return new OSData(OSType.WindowsVeryOld, OSVersion.WinME);
                    }

                    break;

                case PlatformID.Win32NT:
                    switch (Environment.OSVersion.Version.Major)
                    {
                        case 3:
                            return new OSData(OSType.WindowsVeryOld, OSVersion.NT351);
                        case 4:
                            return new OSData(OSType.WindowsVeryOld, OSVersion.NT40);
                        case 5:
                            switch (Environment.OSVersion.Version.Minor)
                            {
                                case 0:
                                    return new OSData(OSType.WindowsVeryOld, OSVersion.Win2000);
                                case 1:
                                    return new OSData(OSType.WindowsOld, OSVersion.WinXP);
                                case 2:
                                    return new OSData(OSType.WindowsVeryOld, OSVersion.Win2003);
                            }

                            break;

                        case 6:
                            switch (Environment.OSVersion.Version.Minor)
                            {
                                case 0:
                                    return new OSData(OSType.WindowsOld, OSVersion.Vista);
                                case 1:
                                    return new OSData(OSType.Windows, OSVersion.Win7);
                                case 2:
                                    return new OSData(OSType.Windows, OSVersion.Win8);
                                case 3:
                                    return new OSData(OSType.Windows, OSVersion.Win81);
                            }

                            break;
                        case 10
                            : //this will only show up if the application has a manifest file allowing W10, otherwise a 6.2 version will be used
                            return new OSData(OSType.Windows, OSVersion.Win10);
                    }

                    break;

                case PlatformID.WinCE:
                    return new OSData(OSType.Windows, OSVersion.WinCE);
                case PlatformID.Unix:
                    return new OSData(OSType.Unix, OSVersion.Unix);
                case PlatformID.Xbox:
                    return new OSData(OSType.Xbox, OSVersion.XBox);
                case PlatformID.MacOSX:
                    return new OSData(OSType.MacOS, OSVersion.MacOS);
                default:
                    break;
            }

            return new OSData(OSType.Unknown, OSVersion.Unknown);
        }
    }
}