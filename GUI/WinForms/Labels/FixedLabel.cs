// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Windows.Forms;

namespace Common_Library.GUI.WinForms.Labels
{
    public class FixedLabel : Label
    {
        public FixedLabel()
        {
            DoubleBuffered = true;
        }
    }
}