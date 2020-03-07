﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Common_Library.Colorful
{
    /// <summary>
    /// Exposes a collection of style classifications which can be used to style text.
    /// </summary>
    public sealed class StyleSheet
    {
        /// <summary>
        /// The StyleSheet's collection of style classifications.
        /// </summary>
        public List<StyleClass<TextPattern>> Styles { get; private set; }
        /// <summary>
        /// The color to be associated with unstyled text.
        /// </summary>
        public Color UnstyledColor;

        /// <summary>
        /// Exposes a collection of style classifications which can be used to style text.
        /// </summary>
        /// <param name="defaultColor">The color to be associated with unstyled text.</param>
        public StyleSheet(Color defaultColor)
        {
            Styles = new List<StyleClass<TextPattern>>();
            UnstyledColor = defaultColor;
        }

        /// <summary>
        /// Adds a style classification to the StyleSheet.
        /// </summary>
        /// <param name="target">The string to be styled.</param>
        /// <param name="color">The color to be applied to the target.</param>
        /// <param name="matchHandler">A delegate instance which describes a transformation that
        /// can be applied to the target.</param>
        public void AddStyle(String target, Color color, Styler.MatchFound matchHandler)
        {
            Styler styler = new Styler(target, color, matchHandler);

            Styles.Add(styler);
        }

        /// <summary>
        /// Adds a style classification to the StyleSheet.
        /// </summary>
        /// <param name="target">The string to be styled.</param>
        /// <param name="color">The color to be applied to the target.</param>
        /// <param name="matchHandler">A delegate instance which describes a simpler transformation that
        /// can be applied to the target.</param>
        public void AddStyle(String target, Color color, Styler.MatchFoundLite matchHandler)
        {
            String Wrapper(String s, MatchLocation l, String m) => matchHandler.Invoke(m);
            Styler styler = new Styler(target, color, Wrapper);

            Styles.Add(styler);
        }

        /// <summary>
        /// Adds a style classification to the StyleSheet.
        /// </summary>
        /// <param name="target">The string to be styled.</param>
        /// <param name="color">The color to be applied to the target.</param>
        public void AddStyle(String target, Color color)
        {
            String Handler(String s, MatchLocation l, String m) => m;
            Styler styler = new Styler(target, color, Handler);

            Styles.Add(styler);
        }
    }
}