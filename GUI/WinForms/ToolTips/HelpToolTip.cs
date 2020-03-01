// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Windows.Forms;

namespace Common_Library.GUI.WinForms.ToolTips
{
    public sealed class HelpToolTip : ToolTip
    {
        public HelpToolTip()
        {
            IsBalloon = true;
            UseAnimation = true;
            UseFading = true;
            ShowAlways = true;
            AutoPopDelay = 30000;
        }
    }
}