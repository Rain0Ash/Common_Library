// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;

namespace Common_Library.Colorful
{
    /// <summary>
    /// A StyleClass instance that exposes a delegate instance which can be used for more 
    /// customized styling.
    /// </summary>
    public sealed class Styler : StyleClass<TextPattern>, IEquatable<Styler>
    {
        /// <summary>
        /// Defines a string transformation.
        /// </summary>
        /// <param name="unstyledInput">The entire input string being matched against, before
        /// styling has taken place.</param>
        /// <param name="matchLocation">The location of the target in the input string.</param>
        /// <param name="match">The "matching" portion of the input string.</param>
        /// <returns>A transformed version of the 'match' parameter.</returns>
        public delegate String MatchFound(String unstyledInput, MatchLocation matchLocation, String match);
        /// <summary>
        /// Defines a simpler string transformation.
        /// </summary>
        /// <param name="match">The "matching" portion of the input string.</param>
        /// <returns>A transformed version of the 'match' parameter.</returns>
        public delegate String MatchFoundLite(String match);

        /// <summary>
        /// A delegate instance which can be used for custom styling.
        /// </summary>
        public MatchFound MatchFoundHandler { get; private set; }

        /// <summary>
        /// A StyleClass instance that exposes a delegate instance which can be used for more 
        /// customized styling.
        /// </summary>
        /// <param name="target">The string to be styled.</param>
        /// <param name="color">The color to be applied to the target.</param>
        /// <param name="matchHandler">A delegate instance which describes a transformation that
        /// can be applied to the target.</param>
        public Styler(String target, Color color, MatchFound matchHandler)
        {
            Target = new TextPattern(target);
            Color = color;
            MatchFoundHandler = matchHandler;
        }

        public Boolean Equals(Styler other)
        {
            if (other == null)
            {
                return false;
            }

            return base.Equals(other)
                && MatchFoundHandler == other.MatchFoundHandler;
        }

        public override Boolean Equals(Object obj) => Equals(obj as Styler);

        public override Int32 GetHashCode()
        {
            Int32 hash = base.GetHashCode();

            hash *= 79 + MatchFoundHandler.GetHashCode();

            return hash;
        }
    }
}
