// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Objects
{
    public interface IPauseable
    {
        event Handlers.EmptyHandler Resumed;
        event Handlers.EmptyHandler Paused;

        Boolean IsPaused { get; set; }

        void Pause();
        void Resume();
    }
}