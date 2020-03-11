// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Security;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Runtime.ConstrainedExecution;

// ReSharper disable UnusedMember.Local

namespace Common_Library.LongPath
{
    internal class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private SafeTokenHandle()
            : base(true)
        {
        }

        // 0 is an Invalid Handle
        internal SafeTokenHandle(IntPtr handle)
            : base(true)
        {
            SetHandle(handle);
        }

        internal static SafeTokenHandle InvalidHandle
        {
            get
            {
                return new SafeTokenHandle(IntPtr.Zero);
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        private static extern Boolean CloseHandle(IntPtr handle);

        protected override Boolean ReleaseHandle()
        {
            return CloseHandle(handle);
        }
    }
}