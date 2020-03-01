// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Common_Library.Localization;
using Common_Library.Utils;

namespace Common_Library.Logger
{
    public class LogMessage : IConsoleMsg
    {
        public static readonly Dictionary<MessageType, ConsoleColor> MessageColors = new Dictionary<MessageType, ConsoleColor>
        {
            {MessageType.Default, ConsoleColor.White},
            {MessageType.Debug, ConsoleColor.Cyan},
            {MessageType.Action, ConsoleColor.Blue},
            {MessageType.Good, ConsoleColor.Green},
            {MessageType.Warning, ConsoleColor.DarkYellow},
            {MessageType.CriticalWarning, ConsoleColor.Red},
            {MessageType.Error, ConsoleColor.DarkRed},
            {MessageType.CriticalError, ConsoleColor.Magenta},
            {MessageType.FatalError, ConsoleColor.DarkMagenta},
            {MessageType.UnknownError, ConsoleColor.Gray}
        };
        
        public CultureStringsBase Message { get; }
        private Object[] FormatList { get; }
        public MessageType MessageType { get; }
        public ConsoleColor MessageColor { get; }
        public Int32 Priority { get; }
        public MessageAdditions MessageAdditions { get; }
        public Boolean NewLine { get; }
        public DateTime DateTime { get; }

        public static implicit operator String(LogMessage obj)
        {
            return obj.ToString();
        }
        
        public LogMessage(String message, MessageType messageType = MessageType.Default, IEnumerable<Object> formatList = null, ConsoleColor? messageColor = null, Int32 priority = 0,
            MessageAdditions messageAdditions = MessageAdditions.CurrentTime, Boolean newLine = true)
            : this(new CultureStringsBase(message), messageType, formatList, messageColor, priority, messageAdditions, newLine)
        {
        }

        public LogMessage(CultureStringsBase message, MessageType messageType = MessageType.Default, IEnumerable<Object> formatList = null, ConsoleColor? messageColor = null, Int32 priority = 0,
            MessageAdditions messageAdditions = MessageAdditions.CurrentTime, Boolean newLine = true)
        {
            Message = message;
            FormatList = formatList?.ToArray() ?? new Object[0];
            MessageType = messageType;
            MessageColor = messageColor ?? MessageColors[MessageType];
            Priority = priority;
            MessageAdditions = messageAdditions;
            NewLine = newLine;
            
            DateTime = DateTime.Now;
        }

        public void ToConsole()
        {
            ToConsole(NewLine);
        }
        
        public void ToConsole(Boolean newLine)
        {
            ToConsole(newLine, MessageColor);
        }
        
        public void ToConsole(Boolean newLine, ConsoleColor color)
        {
            this.ToConsole(color, newLine);
        }

        public Color GetColor()
        {
            return ConsoleUtils.GetColor(MessageColor);
        }

        public override String ToString()
        {
            CultureInfo cultureInfo = LocalizationBase.GetLocalizationCulture();
            
            String dateTime = MessageAdditions switch
            {
                MessageAdditions.CurrentDate => DateTime.Date.ToString(cultureInfo),
                MessageAdditions.CurrentTime => $"{DateTime.Hour}:{DateTime.Minute}:{DateTime.Second}",
                MessageAdditions.CurrentDateTime => DateTime.ToString(cultureInfo),
                _ => String.Empty
            };
            
            return $"{dateTime}{(dateTime.Any() ? " " : String.Empty)}{String.Format(Message, FormatList)}";
        }
    }
}