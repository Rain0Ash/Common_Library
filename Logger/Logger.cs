// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Common_Library.Localization;

// ReSharper disable MemberCanBePrivate.Global

namespace Common_Library.Logger
{
    public enum MessageType
    {
        Default,
        Debug,
        Action,
        Good,
        Warning,
        CriticalWarning,
        Error,
        CriticalError,
        FatalError,
        UnknownError
    }
        
    public enum MessageAdditions
    {
        None = 0,
        CurrentDate = 1,
        CurrentTime = 2,
        CurrentDateTime = 3
    }
    
    public class Logger
    {
        public delegate void LogHandler(LogMessage logMessage);

        public event LogHandler Logged;

        public static event LogHandler GlobalLogged;

        public Int32 SavedMessageCount
        {
            get
            {
                return Messages.MaximumLength;
            }
            set
            {
                Messages.MaximumLength = value;
            }
        }
        
        public static readonly EventQueueList<LogMessage> GlobalMessages = new EventQueueList<LogMessage>();
        public readonly EventQueueList<LogMessage> Messages = new EventQueueList<LogMessage>();

        public static void GlobalLog(LogMessage logMessage)
        {
            GlobalMessages.Add(logMessage);
            GlobalLogged?.Invoke(logMessage);
        }

        public static void GlobalLog(CultureStringsBase message, MessageType messageType = MessageType.Default, IEnumerable<Object> formatList = null, ConsoleColor? messageColor = null, Int32 priority = 0, MessageAdditions messageAdditions = MessageAdditions.None)
        {
            LogMessage logMessage = new LogMessage(message, messageType, formatList, messageColor, priority, messageAdditions);
            GlobalMessages.Add(logMessage);
            GlobalLogged?.Invoke(logMessage);
        }
        
        public Logger(Int32 savedMessageCount = 255)
        {
            SavedMessageCount = savedMessageCount;
        }

        public void Log(LogMessage logMessage)
        {
            Messages.Add(logMessage);
            Logged?.Invoke(logMessage);
        }
        
        public void Log(CultureStringsBase message, MessageType messageType = MessageType.Default, IEnumerable<Object> formatList = null, ConsoleColor? messageColor = null, Int32 priority = 0, MessageAdditions messageAdditions = MessageAdditions.None)
        {
            LogMessage logMessage = new LogMessage(message, messageType, formatList, messageColor, priority, messageAdditions);
            Messages.Add(logMessage);
            Logged?.Invoke(logMessage);
        }

        public void Clear()
        {
            Messages.Clear();
        }
    }
}