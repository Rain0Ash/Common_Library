// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;

namespace Common_Library.Utils
{
    public interface IConsoleMsg
    {
        String GetConsoleText(IFormatProvider provider = null)
        {
            return this.Convert(provider);
        }
        
        void ToConsole(Boolean newLine = true, IFormatProvider provider = null)
        {
            GetConsoleText(provider).ToConsole(newLine, provider);
        }

        void ToConsole(ConsoleColor color, Boolean newLine = true, IFormatProvider provider = null)
        {
            GetConsoleText(provider).ToConsole(color, newLine, provider);
        }
        
        void ToConsole(Color color, Boolean newLine = true, IFormatProvider provider = null)
        {
            GetConsoleText(provider).ToConsole(color, newLine, provider);
        }

        void ToConsole(ConsoleColor color, ConsoleColor bColor, Boolean newLine = true, IFormatProvider provider = null)
        {
            GetConsoleText(provider).ToConsole(color, bColor, newLine, provider);
        }
        
        void ToConsole(Color color, Color bColor, Boolean newLine = true, IFormatProvider provider = null)
        {
            GetConsoleText(provider).ToConsole(color, bColor, newLine, provider);
        }
    }
}