// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Concurrent;
using System.Drawing;

namespace Common_Library.Colorful
{
    public sealed class ColorManagerFactory
    {
        public ColorManager GetManager(ColorStore colorStore, Int32 maxColorChanges, Int32 initialColorChangeCountValue, Boolean isInCompatibilityMode)
        {
            ColorMapper colorMapper = GetColorMapperSafe(ColorManager.IsWindows());

            return new ColorManager(colorStore, colorMapper, maxColorChanges, initialColorChangeCountValue, isInCompatibilityMode);
        }

        public ColorManager GetManager(ConcurrentDictionary<Color, ConsoleColor> colorMap, ConcurrentDictionary<ConsoleColor, Color> consoleColorMap, Int32 maxColorChanges, Int32 initialColorChangeCountValue, Boolean isInCompatibilityMode)
        {
            ColorStore colorStore = new ColorStore(colorMap, consoleColorMap);
            ColorMapper colorMapper = GetColorMapperSafe(ColorManager.IsWindows());

            return new ColorManager(colorStore, colorMapper, maxColorChanges, initialColorChangeCountValue, isInCompatibilityMode);
        }

        private ColorMapper GetColorMapperSafe(Boolean isWindows)
        {
            return isWindows ? new ColorMapper() : null;
        }
    }
}
