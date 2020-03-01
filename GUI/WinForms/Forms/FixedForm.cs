// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Common_Library.GUI.WinForms.Forms
{
    public abstract class FixedForm : Form
    {
        protected FixedForm()
        {
            DoubleBuffered = true;
            FormClosing += OnForm_Closing;
        }
        
        private void OnForm_Closing(Object sender, FormClosingEventArgs e)
        {
            Size iconSize = new Size(32,32);
            if (new Rectangle(Location, iconSize).Contains(Cursor.Position) && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
            }
        }

        protected override void Dispose(Boolean disposing)
        {
            FormClosing -= OnForm_Closing;
            base.Dispose(disposing);
        }
    }
}