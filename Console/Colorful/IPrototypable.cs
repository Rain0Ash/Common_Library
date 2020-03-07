// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace Common_Library.Colorful
{
    /// <summary>
    /// Exposes methods used for creating (potentially inexact) copies of objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPrototypable<T>
    {
        /// <summary>
        /// Returns a potentially inexact copy of the target object.
        /// </summary>
        /// <returns></returns>
        T Prototype();
    }
}
