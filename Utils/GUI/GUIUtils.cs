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
        View = 4,
        ReadOnly = 7,
        Paste = 8,
        Cut = 16,
        Swap = 32,
        Add = 64,
        Remove = 128,
        Edit = 256,
        Change = 512,
        Basic = 1023,
        ChangeStatus = 1024,
        Replace = 2048,
        Reset = 4096,
        Additional1 = 8192,
        Additional2 = 16384,
        Additional3 = 32768,
        All = 65535
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