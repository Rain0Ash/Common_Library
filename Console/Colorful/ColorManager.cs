﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Linq;
using System.Drawing;

namespace Common_Library.Colorful
{
    /// <summary>
    /// Manages the number of different colors that the Windows console is able to display in a given session.
    /// </summary>
    public sealed class ColorManager
    {
        /// <summary>
        /// Compatibility mode is used when the Win32 API is not able to access the console. In this case,
        /// System.Drawing cannot be used.
        /// </summary>
        public Boolean IsInCompatibilityMode { get; }

        private readonly ColorMapper _colorMapper;
        private readonly ColorStore _colorStore;
        private Int32 _colorChangeCount;
        private readonly Int32 _maxColorChanges;

        /// <summary>
        /// Manages the number of different colors that the Windows console is able to display in a given session.
        /// </summary>
        /// <param name="colorStore">The ColorStore instance in which the ColorManager will store colors.</param>
        /// <param name="colorMapper">The ColorMapper instance the ColorManager will use to relate different color
        /// types to one another.</param>
        /// <param name="maxColorChanges">The maximum number of color changes allowed by the ColorManager.  It's
        /// necessary to keep track of this, because the Windows console can only display 16 different colors in
        /// a given session.</param>
        /// <param name="initialColorChangeCountValue">The number of color changes which have already occurred.</param>
        /// <param name="isInCompatibilityMode"></param>
        public ColorManager(ColorStore colorStore, ColorMapper colorMapper, Int32 maxColorChanges , Int32 initialColorChangeCountValue, Boolean isInCompatibilityMode)
        {
            _colorStore = colorStore;
            _colorMapper = colorMapper;

            _colorChangeCount = initialColorChangeCountValue;
            _maxColorChanges = maxColorChanges;
            IsInCompatibilityMode = isInCompatibilityMode;
        }

        /// <summary>
        /// Gets the System.Drawing.Color mapped to the ConsoleColor provided as an argument.
        /// </summary>
        /// <param name="color">The ConsoleColor alias under which the desired System.Drawing.Color is stored.</param>
        /// <returns>The corresponding System.Drawing.Color.</returns>
        public Color GetColor(ConsoleColor color)
        {
            return _colorStore.ConsoleColors[color];
        }

        /// <summary>
        /// Replaces one System.Drawing.Color in the ColorManager with another.
        /// </summary>
        /// <param name="oldColor">The color to be replaced.</param>
        /// <param name="newColor">The replacement color.</param>
        public void ReplaceColor(Color oldColor, Color newColor)
        {
            // If the console exists and Common_Library.Colorful.Console is running on Windows.
            if (IsInCompatibilityMode || !IsWindows()) return;
            ConsoleColor consoleColor = _colorStore.Replace(oldColor, newColor);
            _colorMapper.MapColor(consoleColor, newColor);
        }

        /// <summary>
        /// Gets the ConsoleColor mapped to the System.Drawing.Color provided as an argument.
        /// </summary>
        /// <param name="color">The System.Drawing.Color whose ConsoleColor alias should be retrieved.</param>
        /// <returns>The corresponding ConsoleColor.</returns>
        public ConsoleColor GetConsoleColor(Color color)
        {
            if (IsInCompatibilityMode)
            {
                return color.ToNearestConsoleColor();
            }

            try
            {
#if NETSTANDARD2_0
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
#endif
                    return GetConsoleColorNative(color);

#if NETSTANDARD2_0
                }
                else
                {
                    return color.ToNearestConsoleColor();
                }
#endif
            }
            // If no NETSTANDARD2_0, but still not running on Windows, catch the exception and approximate the requested color.
            catch
            {
                return color.ToNearestConsoleColor();
            }
        }

        /// <summary>
        /// Returns true if Common_Library.Colorful.Console is being run on Windows; returns false otherwise.
        /// The usage of this method in Common_Library.Colorful.Console is meant to prevent the ColorMapper class
        /// from being instantiated if Common_Library.Colorful.Console is not being run on Windows, due to an
        /// implementation detail of the CLR.  Namely, we'd like to prevent ColorMapper's static
        /// constructor from running if not on Windows, because that would attempt to import functions
        /// from kernel32.dll, which won't exist if not on Windows.  For more info on this
        /// implementation detail, see http://csharpindepth.com/Articles/General/Beforefieldinit.aspx
        /// </summary>
        /// <returns></returns>
        public static Boolean IsWindows()
        {
            Boolean isWindows = true;

#if NETSTANDARD2_0
            isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif

            return isWindows;
        }

        private ConsoleColor GetConsoleColorNative(Color color)
        {
            if (CanChangeColor() && _colorStore.RequiresUpdate(color))
            {
                ConsoleColor oldColor = (ConsoleColor)_colorChangeCount;

                _colorMapper.MapColor(oldColor, color);
                _colorStore.Update(oldColor, color);

                _colorChangeCount++;
            }

            ConsoleColor nativeColor;
            return _colorStore.Colors.TryGetValue(color, out nativeColor) ? nativeColor : _colorStore.Colors.Last().Value;
        }

        private Boolean CanChangeColor()
        {
            return _colorChangeCount < _maxColorChanges;
        }
    }
}