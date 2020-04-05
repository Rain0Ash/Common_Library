// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Linq;
using Common_Library.App;
using Common_Library.Utils.IO;

namespace Common_Library
{
    public static class Library
    {
        public static AppVersion Version { get; } = new AppVersion(1, App.App.Status.OpenBeta);

        public static void Main()
        {
            $"Library version: {Version}".ToConsole(ConsoleColor.Green);
        }
    }
}