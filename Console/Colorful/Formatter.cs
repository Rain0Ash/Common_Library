// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;

namespace Common_Library.Colorful
{
    /// <summary>
    /// Exposes properties representing an object and its color.  This is a convenience wrapper around
    /// the StyleClass type, so you don't have to provide the type argument each time.
    /// </summary>
    public sealed class Formatter
    {
        /// <summary>
        /// The object to be styled.
        /// </summary>
        public Object Target => _backingClass.Target;

        /// <summary>
        /// The color to be applied to the target.
        /// </summary>
        public Color Color => _backingClass.Color;

        private readonly StyleClass<Object> _backingClass;

        /// <summary>
        /// Exposes properties representing an object and its color.  This is a convenience wrapper around
        /// the StyleClass type, so you don't have to provide the type argument each time.
        /// </summary>
        /// <param name="target">The object to be styled.</param>
        /// <param name="color">The color to be applied to the target.</param>
        public Formatter(Object target, Color color)
        {
            _backingClass = new StyleClass<Object>(target, color);
        }
    }
}
