// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using System.Windows.Forms;
using Common_Library.GUI.WinForms.Forms;

namespace Common_Library.Utils
{
    [Flags]
    public enum ActionType
    {
        None = 0,
        Select = 1,
        Copy = 2,
        ReadOnly = 3,
        Paste = 4,
        Cut = 8,
        Swap = 16,
        Add = 32,
        Remove = 64,
        Change = 128,
        Basic = 255,
        ChangeStatus = 256,
        Replace = 512,
        Additional1 = 1024,
        Additional2 = 2048,
        Additional3 = 4096,
        All = 8191
    }

    public static class GUIUtils
    {
        public static DialogResult ToMessageBox(this Object str, Object caption = null, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Warning)
        {
            return MessageBox.Show(str.GetString(), caption?.GetString(), buttons, icon);
        }

        public static DialogResult ToMessageForm(this Object str, Object title = null, Image icon = null, Image messageIcon = null, MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            return new MessageForm(str.GetString(), title?.GetString(), icon, messageIcon, buttons).ShowDialog();
        }
    }
}