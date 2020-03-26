// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Common_Library.Localization;
using Common_Library.Logger;
using Common_Library.Utils.Math;
using Common_Library.Utils.OS;

namespace Common_Library.GUI.WinForms.RichTextBoxes
{
    public class LoggerRichTextBox : RichTextBox
    {
        private readonly EventQueueList<LogMessage> _messages = new EventQueueList<LogMessage>();

        public Int32 MaximumLength
        {
            get
            {
                return _messages.MaximumLength;
            }
            set
            {
                _messages.MaximumLength = MathUtils.ToRange(value);
                UpdateLog();
            }
        }

        private Boolean _reversed = true;

        public Boolean Reversed
        {
            get
            {
                return _reversed;
            }
            set
            {
                if (_reversed == value)
                {
                    return;
                }

                _reversed = value;
                UpdateLog();
            }
        }

        public LoggerRichTextBox()
        {
            ReadOnly = true;
            LocalizationBase.LanguageChanged += UpdateLog;
            //Enabled = false;
        }

        /*protected override void WndProc(ref Message m) {
            if(m.Msg == 0x0007) m.Msg = 0x0008;

            base.WndProc (ref m);
        }*/

        protected override void OnLinkClicked(LinkClickedEventArgs e)
        {
            ProcessUtils.OpenBrowser(e.LinkText);
            base.OnLinkClicked(e);
        }

        public void UpdateLog()
        {
            Clear();
            Boolean first = true;
            foreach (LogMessage item in Reversed ? _messages.AsEnumerable().Reverse() : _messages)
            {
                if (first)
                {
                    Display(item, item.GetColor(), false);
                    first = false;
                    continue;
                }

                Display(item, item.GetColor(), item.NewLine);
            }
        }

        public void ClearLog()
        {
            _messages.Clear();
            UpdateLog();
        }

        public void Log(LogMessage logMessage)
        {
            _messages.Add(logMessage);
            UpdateLog();
        }

        public void Log(String text, IEnumerable<Object> formatList = null, Boolean newLine = true)
        {
            Log(text, MessageType.Default, formatList, ConsoleColor.White, 0, MessageAdditions.CurrentTime, newLine);
        }

        public void Log(String message, MessageType messageType = MessageType.Default, IEnumerable<Object> formatList = null,
            ConsoleColor? color = null, Int32 priority = 0, MessageAdditions messageAdditions = MessageAdditions.CurrentTime,
            Boolean newLine = true)
        {
            foreach (String msg in message.Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries))
            {
                if (MaximumLength > 0 && _messages.Count >= MaximumLength - 1)
                {
                    _messages.RemoveRange(0, _messages.Count - MaximumLength + 1);
                }

                // ReSharper disable once PossibleMultipleEnumeration
                _messages.Add(new LogMessage(msg, messageType, formatList, color, priority, messageAdditions, newLine));
            }

            UpdateLog();
        }

        private void Display(String message, Color color, Boolean newLine)
        {
            if (newLine)
            {
                message = Environment.NewLine + message;
            }

            SelectionStart = TextLength;
            SelectionLength = 0;

            SelectionColor = color;
            AppendText(message);
            SelectionColor = ForeColor;
            SelectionStart = Reversed ? 0 : Text.Length;
            ScrollToCaret();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            UpdateLog();
        }

        protected override void Dispose(Boolean disposing)
        {
            Clear();
            LocalizationBase.LanguageChanged -= UpdateLog;
            base.Dispose(disposing);
        }
    }
}