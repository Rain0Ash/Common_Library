// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Utils
{
    [Flags]
    public enum ActionType : Byte
    {
        Select = 0,
        Copy = 1,
        Paste = 2,
        Cut = 4,
        Add = 8,
        Remove = 16,
        Change = 32,
        All = 63
    }
    
    public class GUIUtils
    {
        
    }
}