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
        Edit = 128,
        Change = 256,
        Basic = 511,
        ChangeStatus = 512,
        Replace = 1024,
        Reset = 2048,
        Additional1 = 4096,
        Additional2 = 8192,
        Additional3 = 16383,
        All = 32767
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