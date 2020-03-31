// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Runtime.InteropServices;
using Common_Library.Utils.Core.GC;

namespace Common_Library.GCE
{
    public class GCHandleProvider : IDisposable
    {
        public GCHandleProvider(Object obj)
        {
            Handle = obj.ToGCHandle();
        }

        public GCHandleProvider(GCHandle handle)
        {
            Handle = handle;
        }

        public IntPtr Pointer
        {
            get
            {
                return Handle.ToIntPtr();
            }
        }

        public GCHandle Handle { get; }

        private void ReleaseUnmanagedResources()
        {
            if (Handle.IsAllocated)
            {
                Handle.Free();
            }
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~GCHandleProvider()
        {
            ReleaseUnmanagedResources();
        }
    }
}