using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Common_Library.Colorful
{
    public sealed class GradientGenerator
    {
        public List<StyleClass<T>> GenerateGradient<T>(IEnumerable<T> input, Color startColor, Color endColor, Int32 maxColorsInGradient)
        {
            List<T> inputAsList = input.ToList();
            Int32 numberOfGrades = inputAsList.Count / maxColorsInGradient;
            Int32 numberOfGradesRemainder = inputAsList.Count % maxColorsInGradient;

            List<StyleClass<T>> gradients = new List<StyleClass<T>>();
            Color previousColor = Color.Empty;
            T previousItem = default(T);
            Int32 SetProgressSymmetrically(Int32 remainder) => remainder > 1 ? -1 : 0; // An attempt to make the gradient symmetric in the event that maxColorsInGradient does not divide input.Count evenly.
            Int32 ResetProgressSymmetrically(Int32 progress) => progress == 0 ? -1 : 0; // An attempt to make the gradient symmetric in the event that maxColorsInGradient does not divide input.Count evenly.
            Int32 colorChangeProgress = SetProgressSymmetrically(numberOfGradesRemainder);
            Int32 colorChangeCount = 0;

            Boolean IsFirstRun(Int32 index) => index == 0;
            Boolean ShouldChangeColor(Int32 index, Int32 progress, T current, T previous) => progress > numberOfGrades - 1 && !current.Equals(previous) || IsFirstRun(index);
            Boolean CanChangeColor(Int32 changeCount) => changeCount < maxColorsInGradient;

            for (Int32 i = 0; i < inputAsList.Count; i++)
            {
                T currentItem = inputAsList[i];
                colorChangeProgress++;

                if (ShouldChangeColor(i, colorChangeProgress, currentItem, previousItem) && CanChangeColor(colorChangeCount))
                {
                    previousColor = GetGradientColor(i, startColor, endColor, inputAsList.Count);
                    previousItem = currentItem;
                    colorChangeProgress = ResetProgressSymmetrically(colorChangeProgress);
                    colorChangeCount++;
                }

                gradients.Add(new StyleClass<T>(currentItem, previousColor));
            }

            return gradients;
        }

        private Color GetGradientColor(Int32 index, Color startColor, Color endColor, Int32 numberOfGrades)
        {
            Int32 numberOfGradesAdjusted = numberOfGrades - 1;

            Int32 rDistance = startColor.R - endColor.R;
            Int32 gDistance = startColor.G - endColor.G;
            Int32 bDistance = startColor.B - endColor.B;

            Double r = startColor.R + -rDistance * ((Double)index / numberOfGradesAdjusted);
            Double g = startColor.G + -gDistance * ((Double)index / numberOfGradesAdjusted);
            Double b = startColor.B + -bDistance * ((Double)index / numberOfGradesAdjusted);

            Color graded = Color.FromArgb((Int32)r, (Int32)g, (Int32)b);
            return graded;
        }
    }
}
