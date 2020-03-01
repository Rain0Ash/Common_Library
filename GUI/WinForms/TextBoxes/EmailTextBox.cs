// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using Common_Library.Utils;

namespace Common_Library.GUI.WinForms.TextBoxes
{
    public class EmailTextBox : HidenTextBox
    {
        public EmailTextBox()
        {
            PasswdChar = '\0';
        }

        public override Boolean CheckValidFormat()
        {
            return EmailUtils.CheckValidEmail(Text);
        }
    }
}