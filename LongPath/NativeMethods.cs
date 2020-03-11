// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;
using DWORD = System.UInt32;
using System.Runtime.ConstrainedExecution;
using System.Security.Principal;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable NotAccessedField.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable UnusedMethodReturnValue.Global

namespace Common_Library.LongPath
{
    internal static class NativeMethods
    {
        internal const Int32 ErrorSuccess = 0;
        internal const Int32 ErrorFileNotFound = 0x2;
        internal const Int32 ErrorPathNotFound = 0x3;
        internal const Int32 ErrorAccessDenied = 0x5;
        internal const Int32 ErrorInvalidHandle = 0x6;
        internal const Int32 ErrorNotEnoughMemory = 0x8;
        internal const Int32 ErrorInvalidDrive = 0xf;
        internal const Int32 ErrorNoMoreFiles = 0x12;
        internal const Int32 ErrorNotReady = 0x15;
        internal const Int32 ErrorSharingViolation = 0x20;
        internal const Int32 ErrorBadNetpath = 0x35;
        internal const Int32 ErrorNetnameDeleted = 0x40;
        internal const Int32 ErrorFileExists = 0x50;
        internal const Int32 ErrorInvalidParameter = 0x57;
        internal const Int32 ErrorInvalidName = 0x7B;
        internal const Int32 ErrorBadPathname = 0xA1;
        internal const Int32 ErrorAlreadyExists = 0xB7;
        internal const Int32 ErrorFilenameExcedRange = 0xCE; // filename too long.
        internal const Int32 ErrorDirectory = 0x10B;
        internal const Int32 ErrorOperationAborted = 0x3e3;
        internal const Int32 ErrorNoToken = 0x3f0;
        internal const Int32 ErrorNotAllAssigned = 0x514;
        internal const Int32 ErrorInvalidOwner = 0x51B;
        internal const Int32 ErrorInvalidPrimaryGroup = 0x51C;
        internal const Int32 ErrorNoSuchPrivilege = 0x521;
        internal const Int32 ErrorPrivilegeNotHeld = 0x522;
        internal const Int32 ErrorLogonFailure = 0x52E;
        internal const Int32 ErrorCantOpenAnonymous = 0x543;
        internal const Int32 ErrorNoSecurityOnObject = 0x546;

        internal const Int32 InvalidFileAttributes = -1;
        internal const Int32 FileAttributeDirectory = 0x00000010;
        internal const Int32 FileWriteAttributes = 0x0100;
        internal const Int32 FileFlagBackupSemantics = 0x02000000;
        internal const Int32 ReplacefileWriteThrough = 0x1;
        internal const Int32 ReplacefileIgnoreMergeErrors = 0x2;

        internal const Int32 MaxPath = 260;

        // While Windows allows larger paths up to a maximum of 32767 characters, because this is only an approximation and
        // can vary across systems and OS versions, we choose a limit well under so that we can give a consistent behavior.
        internal const Int32 MaxLongPath = 32000;
        internal const Int32 MaxAlternate = 14;

        public const Int32 FormatMessageIgnoreInserts = 0x00000200;
        public const Int32 FormatMessageFromSystem = 0x00001000;
        public const Int32 FormatMessageArgumentArray = 0x00002000;

        [Flags]
        internal enum EFileAccess : uint
        {
            GenericRead = 0x80000000,
            GenericWrite = 0x40000000,
            GenericExecute = 0x20000000,
            GenericAll = 0x10000000
        }

        [Serializable]
        internal struct Win32FileAttributeData
        {
            internal System.IO.FileAttributes FileAttributes;

            internal UInt32 FtCreationTimeLow;

            internal UInt32 FtCreationTimeHigh;

            internal UInt32 FtLastAccessTimeLow;

            internal UInt32 FtLastAccessTimeHigh;

            internal UInt32 FtLastWriteTimeLow;

            internal UInt32 FtLastWriteTimeHigh;

            internal Int32 FileSizeHigh;

            internal Int32 FileSizeLow;

            public void PopulateFrom(Win32FindData findData)
            {
                FileAttributes = findData.dwFileAttributes;
                FtCreationTimeLow = (UInt32) findData.ftCreationTime.dwLowDateTime;
                FtCreationTimeHigh = (UInt32) findData.ftCreationTime.dwHighDateTime;
                FtLastAccessTimeLow = (UInt32) findData.ftLastAccessTime.dwLowDateTime;
                FtLastAccessTimeHigh = (UInt32) findData.ftLastAccessTime.dwHighDateTime;
                FtLastWriteTimeLow = (UInt32) findData.ftLastWriteTime.dwLowDateTime;
                FtLastWriteTimeHigh = (UInt32) findData.ftLastWriteTime.dwHighDateTime;
                FileSizeHigh = findData.nFileSizeHigh;
                FileSizeLow = findData.nFileSizeLow;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct Win32FindData
        {
            internal System.IO.FileAttributes dwFileAttributes;
            internal FILETIME ftCreationTime;
            internal FILETIME ftLastAccessTime;
            internal FILETIME ftLastWriteTime;
            internal Int32 nFileSizeHigh;
            internal Int32 nFileSizeLow;
            internal Int32 dwReserved0;
            internal Int32 dwReserved1;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MaxPath)]
            internal String cFileName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MaxAlternate)]
            internal String cAlternate;
        }

        internal static Int32 MakeHrFromErrorCode(Int32 errorCode)
        {
            return unchecked((Int32) 0x80070000 | errorCode);
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern Boolean CopyFile(String src, String dst, [MarshalAs(UnmanagedType.Bool)] Boolean failIfExists);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false)]
        internal static extern Boolean ReplaceFile(String replacedFileName, String replacementFileName, String backupFileName,
            Int32 dwReplaceFlags, IntPtr lpExclude, IntPtr lpReserved);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern SafeFindHandle FindFirstFile(String lpFileName, out Win32FindData lpFindFileData);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern Boolean FindNextFile(SafeFindHandle hFindFile, out Win32FindData lpFindFileData);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern Boolean FindClose(IntPtr hFindFile);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern UInt32 GetFullPathName(String lpFileName, UInt32 nBufferLength,
            StringBuilder lpBuffer, IntPtr mustBeNull);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern Boolean DeleteFile(String lpFileName);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern Boolean RemoveDirectory(String lpPathName);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern Boolean CreateDirectory(String lpPathName,
            IntPtr lpSecurityAttributes);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern Boolean MoveFile(String lpPathNameFrom, String lpPathNameTo);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern SafeFileHandle CreateFile(
            String lpFileName,
            EFileAccess dwDesiredAccess,
            UInt32 dwShareMode,
            IntPtr lpSecurityAttributes,
            UInt32 dwCreationDisposition,
            UInt32 dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern System.IO.FileAttributes GetFileAttributes(String lpFileName);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern Boolean SetFileAttributes(String lpFileName,
            [MarshalAs(UnmanagedType.U4)] System.IO.FileAttributes dwFileAttributes);

        internal static Int64 SetFilePointer(SafeFileHandle handle, Int64 offset, System.IO.SeekOrigin origin)
        {
            Int32 num1 = (Int32) (offset >> 32);
            Int32 num2 = SetFilePointerWin32(handle, (Int32) offset, ref num1, (Int32) origin);
            if (num2 == -1 && Marshal.GetLastWin32Error() != 0)
            {
                return -1L;
            }

            return ((Int64) (UInt32) num1 << 32) | (UInt32) num2;
        }

        [DllImport("kernel32.dll", EntryPoint = "SetFilePointer", SetLastError = true)]
        internal static extern Int32 SetFilePointerWin32(SafeFileHandle handle, Int32 lo, ref Int32 hi, Int32 origin);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern Int32 FormatMessage(Int32 dwFlags, IntPtr lpSource, Int32 dwMessageId, Int32 dwLanguageId,
            StringBuilder lpBuffer, Int32 nSize, IntPtr vaListArguments);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false)]
        internal static extern Boolean DecryptFile(String path, Int32 reservedMustBeZero);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false)]
        internal static extern Boolean EncryptFile(String path);

        public static String GetMessage(Int32 errorCode)
        {
            StringBuilder sb = new StringBuilder(512);
            Int32 result = FormatMessage(FormatMessageIgnoreInserts |
                                         FormatMessageFromSystem | FormatMessageArgumentArray,
                IntPtr.Zero, errorCode, 0, sb, sb.Capacity, IntPtr.Zero);
            return result != 0 ? sb.ToString() : $"Unknown error: {errorCode}";
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct FileTime
        {
            public FileTime(Int64 fileTime)
            {
                ftTimeLow = (UInt32) fileTime;
                ftTimeHigh = (UInt32) (fileTime >> 32);
            }

            public Int64 ToTicks()
            {
                return ((Int64) ftTimeHigh << 32) + ftTimeLow;
            }

            internal UInt32 ftTimeLow;
            internal UInt32 ftTimeHigh;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern unsafe Boolean SetFileTime(SafeFileHandle hFile, FileTime* creationTime,
            FileTime* lastAccessTime, FileTime* lastWriteTime);

        [DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, ExactSpelling = false, SetLastError = true)]
        internal static extern Boolean GetFileAttributesEx(String name, Int32 fileInfoLevel, ref Win32FileAttributeData lpFileInformation);

        [DllImport("kernel32.dll", CharSet = CharSet.None, EntryPoint = "SetErrorMode", ExactSpelling = true)]
        private static extern Int32 SetErrorMode_VistaAndOlder(Int32 newMode);

        private static readonly Version ThreadErrorModeMinOsVersion = new Version(6, 1, 7600);

        [DllImport("kernel32.dll", CharSet = CharSet.None, EntryPoint = "SetThreadErrorMode", ExactSpelling = false, SetLastError = true)]
        private static extern Boolean SetErrorMode_Win7AndNewer(Int32 newMode, out Int32 oldMode);

        internal static Int32 SetErrorMode(Int32 newMode)
        {
            if (Environment.OSVersion.Version < ThreadErrorModeMinOsVersion)
            {
                return SetErrorMode_VistaAndOlder(newMode);
            }

            SetErrorMode_Win7AndNewer(newMode, out Int32 num);
            return num;
        }

        [DllImport("advapi32.dll",
            EntryPoint = "GetNamedSecurityInfoW",
            CallingConvention = CallingConvention.Winapi,
            SetLastError = true,
            ExactSpelling = true,
            CharSet = CharSet.Unicode)]
        internal static extern DWORD GetSecurityInfoByName(
            String name,
            DWORD objectType,
            DWORD securityInformation,
            out IntPtr sidOwner,
            out IntPtr sidGroup,
            out IntPtr dacl,
            out IntPtr sacl,
            out IntPtr securityDescriptor);

        [DllImport(
            "advapi32.dll",
            EntryPoint = "SetNamedSecurityInfoW",
            CallingConvention = CallingConvention.Winapi,
            SetLastError = true,
            ExactSpelling = true,
            CharSet = CharSet.Unicode)]
        internal static extern DWORD SetSecurityInfoByName(
            String name,
            DWORD objectType,
            DWORD securityInformation,
            Byte[] owner,
            Byte[] group,
            Byte[] dacl,
            Byte[] sacl);

        [DllImport(
            "advapi32.dll",
            EntryPoint = "SetSecurityInfo",
            CallingConvention = CallingConvention.Winapi,
            SetLastError = true,
            ExactSpelling = true,
            CharSet = CharSet.Unicode)]
        internal static extern DWORD SetSecurityInfoByHandle(
            SafeHandle handle,
            DWORD objectType,
            DWORD securityInformation,
            Byte[] owner,
            Byte[] group,
            Byte[] dacl,
            Byte[] sacl);

        [DllImport(
            "advapi32.dll",
            EntryPoint = "GetSecurityDescriptorLength",
            CallingConvention = CallingConvention.Winapi,
            SetLastError = true,
            ExactSpelling = true,
            CharSet = CharSet.Unicode)]
        internal static extern DWORD GetSecurityDescriptorLength(
            IntPtr byteArray);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr LocalFree(IntPtr handle);

        [DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, ExactSpelling = false, SetLastError = true)]
        internal static extern Boolean SetCurrentDirectory(String path);


        internal enum SecurityImpersonationLevel
        {
            Anonymous = 0,
            Identification = 1,
            Impersonation = 2,
            Delegation = 3
        }

        internal enum TokenType
        {
            Primary = 1,
            Impersonation = 2
        }

        internal const UInt32 SePrivilegeDisabled = 0x00000000;
        internal const UInt32 SePrivilegeEnabled = 0x00000002;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct Luid
        {
            internal UInt32 LowPart;
            internal UInt32 HighPart;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct LuidAndAttributes
        {
            internal Luid Luid;
            internal UInt32 Attributes;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct TokenPrivilege
        {
            internal UInt32 PrivilegeCount;
            internal LuidAndAttributes Privilege;
        }


        [DllImport(
            "kernel32.dll",
            SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        internal static extern Boolean CloseHandle(IntPtr handle);

        [DllImport(
            "advapi32.dll",
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        internal static extern Boolean AdjustTokenPrivileges(
            [In] SafeTokenHandle tokenHandle,
            [In] Boolean disableAllPrivileges,
            [In] ref TokenPrivilege newState,
            [In] UInt32 bufferLength,
            [In] [Out] ref TokenPrivilege previousState,
            [In] [Out] ref UInt32 returnLength);

        [DllImport(
            "advapi32.dll",
            CharSet = CharSet.Auto,
            SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        internal static extern
            Boolean RevertToSelf();

        [DllImport(
            "advapi32.dll",
            EntryPoint = "LookupPrivilegeValueW",
            CharSet = CharSet.Auto,
            SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        internal static extern
            Boolean LookupPrivilegeValue(
                [In] String lpSystemName,
                [In] String lpName,
                [In] [Out] ref Luid luid);

        [DllImport(
            "kernel32.dll",
            CharSet = CharSet.Auto,
            SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        internal static extern
            IntPtr GetCurrentProcess();

        [DllImport(
            "kernel32.dll",
            CharSet = CharSet.Auto,
            SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        internal static extern
            IntPtr GetCurrentThread();

        [DllImport(
            "advapi32.dll",
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        internal static extern
            Boolean OpenProcessToken(
                [In] IntPtr processToken,
                [In] TokenAccessLevels desiredAccess,
                [In] [Out] ref SafeTokenHandle tokenHandle);

        [DllImport
        ("advapi32.dll",
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        internal static extern
            Boolean OpenThreadToken(
                [In] IntPtr threadToken,
                [In] TokenAccessLevels desiredAccess,
                [In] Boolean openAsSelf,
                [In] [Out] ref SafeTokenHandle tokenHandle);

        [DllImport
        ("advapi32.dll",
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        internal static extern
            Boolean DuplicateTokenEx(
                [In] SafeTokenHandle existingToken,
                [In] TokenAccessLevels desiredAccess,
                [In] IntPtr tokenAttributes,
                [In] SecurityImpersonationLevel impersonationLevel,
                [In] TokenType tokenType,
                [In] [Out] ref SafeTokenHandle newToken);

        [DllImport
        ("advapi32.dll",
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        internal static extern
            Boolean SetThreadToken(
                [In] IntPtr thread,
                [In] SafeTokenHandle token);
    }
}