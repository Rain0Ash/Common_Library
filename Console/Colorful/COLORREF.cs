using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Common_Library.Colorful
{
    /// <summary>
    /// A Win32 COLORREF, used to specify an RGB color.  See MSDN for more information:
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/dd183449(v=vs.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct COLORREF
    {
        private readonly UInt32 ColorDWORD;

        internal COLORREF(Color color)
        {
            ColorDWORD = color.R + ((UInt32)color.G << 8) + ((UInt32)color.B << 16);
        }

        internal COLORREF(UInt32 r, UInt32 g, UInt32 b)
        {
            ColorDWORD = r + (g << 8) + (b << 16);
        }

        public override String ToString()
        {
            return ColorDWORD.ToString();
        }
    }
}
