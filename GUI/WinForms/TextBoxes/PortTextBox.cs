// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Windows.Forms;
using Common_Library.Utils;
using Common_Library.Utils.Math;
using Common_Library.Utils.Network;

namespace Common_Library.GUI.WinForms.TextBoxes
{
    public class PortTextBox : HidenTextBox
    {
        private Int32 _defaultPort;

        public Int32 DefaultPort
        {
            get
            {
                return _defaultPort;
            }
            set
            {
                _defaultPort = MathUtils.Range(value, 1, 65535);
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
                base.Text = String.IsNullOrEmpty(value) || !NetworkUtils.CheckPort(value) ? DefaultPort.ToString() : value;
            }
        }

        public PortTextBox()
        {
            PasswdChar = '\0';
            MaxLength = 5;
            Text = DefaultPort.ToString();
            Leave += (sender, args) => Text = CheckValidFormat() ? Text : DefaultPort.ToString();
        }

        public override Boolean CheckValidFormat()
        {
            return NetworkUtils.CheckPort(Text);
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

        protected override Boolean IsAllowedChar(Char c)
        {
            return CharUtils.IsControl(c) || Char.IsDigit(c);
        }
    }
}