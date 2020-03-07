// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using Microsoft.Win32.SafeHandles;

namespace Common_Library.LongPath
{
	// ReSharper disable once ClassNeverInstantiated.Global
	internal sealed class SafeFindHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		internal SafeFindHandle()
			: base(true)
		{
		}

		protected override Boolean ReleaseHandle()
		{
			return NativeMethods.FindClose(handle);
		}
	}
}