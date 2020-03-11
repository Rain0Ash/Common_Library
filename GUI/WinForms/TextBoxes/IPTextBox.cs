// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Common_Library.Utils;

namespace Common_Library.GUI.WinForms.TextBoxes
{
    public class IPTextBox : HidenTextBox
    {
        private String _defaultHost = @"127.0.0.1";

        public String DefaultHost
        {
            get
            {
                return _defaultHost;
            }
            set
            {
                if (NetworkUtils.ValidateIPv4(value))
                {
                    _defaultHost = value;
                }
            }
        }

        public override String Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = String.IsNullOrEmpty(value) || !NetworkUtils.ValidateIPv4(value) ? DefaultHost : value;
            }
        }

        public IPTextBox()
        {
            MaxLength = 15;
            PasswdChar = '\0';
            Leave += (sender, args) => Text = CheckValidFormat() ? Text : DefaultHost;
        }

        public override Boolean CheckValidFormat()
        {
            return NetworkUtils.ValidateIPv4(Text);
        }

        protected override void CheckValidFormatColor()
        {
            BackColor = CheckValidFormat() ? Color.White : Color.Coral;
        }

        protected override Boolean IsAllowedChar(Char c)
        {
            return CharUtils.IsControl(c) || Char.IsDigit(c) || c == '.' && Text.Count(chr => chr == '.') < 3;
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (IsAllowedChar(e.KeyChar))
            {
                base.OnKeyPress(e);
                return;
            }

            e.Handled = true;
        }
    }
}