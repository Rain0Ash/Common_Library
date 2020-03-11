// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using Common_Library.Interfaces;

namespace Common_Library.GUI.WinForms.TextBoxes
{
    public abstract class ValidableTextBox : HistoryTextBox, IValidable
    {
        protected ValidableTextBox()
        {
            TextChanged += (sender, args) => CheckValidFormatColor();
        }

        public virtual Boolean CheckValidFormat()
        {
            return true;
        }

        protected virtual void CheckValidFormatColor()
        {
            BackColor = CheckValidFormat() ? Color.White : Color.Coral;
        }
    }
}