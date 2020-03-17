// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;

namespace Common_Library.Utils
{
    public static class DrawingUtils
    {
        public static void DrawCircle(this Graphics g, Pen pen, Single x, Single y, Single d)
        {
            g.DrawEllipse(pen, x, y, d, d);
        }

        public static void DrawCircle(this Graphics g, Pen pen, Int32 x, Int32 y, Int32 d)
        {
            g.DrawEllipse(pen, x, y, d, d);
        }

        public static void FillCircle(this Graphics g, Brush brush, Single x, Single y, Single d)
        {
            g.FillEllipse(brush, x, y, d, d);
        }

        public static void FillCircle(this Graphics g, Brush brush, Int32 x, Int32 y, Int32 d)
        {
            g.FillEllipse(brush, x, y, d, d);
        }
    }
}