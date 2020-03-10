// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Utils
{
    [Flags]
    public enum ActionType : Int64
    {
        None = 0,
        Select = 1,
        Copy = 2,
        Paste = 4,
        Cut = 8,
        Add = 16,
        Remove = 32,
        Change = 64,
        All = 127
    }
    
    public class GUIUtils
    {
        
    }
}