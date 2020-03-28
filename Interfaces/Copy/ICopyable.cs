// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Interfaces
{
    public interface ICopyable : ICopyable<Object>
    {
    }
    
    public interface ICopyable<out T>
    {
        public T Copy();
    }
}