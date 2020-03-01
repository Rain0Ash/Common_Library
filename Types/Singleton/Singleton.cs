// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Types
{
    public class Singleton<T> where T : Singleton<T>, new()
    {
        private static readonly Lazy<Singleton<T>> Lazy = 
            new Lazy<Singleton<T>>(() => new Singleton<T>());

        public static Singleton<T> GetInstance()
        {
            return Lazy.Value;
        }
    }
}