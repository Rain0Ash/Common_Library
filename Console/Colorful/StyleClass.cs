using System;
using System.Drawing;

namespace Common_Library.Colorful
{
    /// <summary>
    /// Exposes methods and properties that represent a style classification.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StyleClass<T> : IEquatable<StyleClass<T>>
    {
        /// <summary>
        /// The object to be styled.
        /// </summary>
        public T Target { get; set; }

        /// <summary>
        /// The color to be applied to the target.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Exposes methods and properties that represent a style classification.
        /// </summary>
        public StyleClass()
        {
        }

        /// <summary>
        /// Exposes methods and properties that represent a style classification.
        /// </summary>
        /// <param name="target">The object to be styled.</param>
        /// <param name="color">The color to be applied to the target.</param>
        public StyleClass(T target, Color color)
        {
            Target = target;
            Color = color;
        }

        public Boolean Equals(StyleClass<T> other)
        {
            if (other == null)
            {
                return false;
            }

            return Target.Equals(other.Target)
                && Color == other.Color;
        }

        public override Boolean Equals(Object obj)=> Equals(obj as StyleClass<T>);
        
        public override Int32 GetHashCode()
        {
            Int32 hash = 163;

            hash *= 79 + Target.GetHashCode();
            hash *= 79 + Color.GetHashCode();

            return hash;
        }
    }
}
