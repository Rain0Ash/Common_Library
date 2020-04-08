// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Drawing;

namespace Common_Library.Types.Drawing
{
    public class DrawingData
    {
        public Color BackgroundColor { get; set; }
        public Color ForegroundColor { get; set; }
        public Font Font { get; set; }

        public DrawingData(Color background, Color foreground, Font font = null)
        {
            BackgroundColor = background;
            ForegroundColor = foreground;
            
            Font = font;
        }
    }
}