// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Utils
{
    [Flags]
    public enum ActionType
    {
        None = 0,
        Select = 1,
        Copy = 2,
        Paste = 4,
        Cut = 8,
        Swap = 16,
        Add = 32,
        Remove = 64,
        Change = 128,
        All = 255
    }

    public static class GUIUtils
    {
    }
}