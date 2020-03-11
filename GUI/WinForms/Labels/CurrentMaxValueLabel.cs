// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Globalization;
using System.Linq;
using Common_Library.Utils;

// ReSharper disable MemberCanBePrivate.Global

namespace Common_Library.GUI.WinForms.Labels
{
    public class CurrentMaxValueLabel : AdditionalsLabel
    {
        private event Handlers.EmptyHandler ValueChanged;

        private Int32 _currentValue;

        public Int32 CurrentValue
        {
            get
            {
                return _currentValue;
            }
            set
            {
                Int32 val = MathUtils.Range(value, 0, MaximumValue);
                if (_currentValue == val)
                {
                    return;
                }

                _currentValue = val;
                ValueChanged?.Invoke();
            }
        }

        private Int32 _maximumValue = 100;

        public Int32 MaximumValue
        {
            get
            {
                return _maximumValue;
            }
            set
            {
                if (_maximumValue == value)
                {
                    return;
                }

                _maximumValue = value;
                ValueChanged?.Invoke();
            }
        }

        private String _separator = "\\";

        public String Separator
        {
            get
            {
                return _separator;
            }
            set
            {
                if (_separator == value)
                {
                    return;
                }

                _separator = value;
                ValueChanged?.Invoke();
            }
        }

        public MathUtils.DisplayType DisplayType { get; set; } = MathUtils.DisplayType.Value;
        public Byte PercentFractionalCount { get; set; } = 0;
        public MathUtils.RoundType RoundType { get; set; } = MathUtils.RoundType.Banking;

        public Int32 Step { get; set; } = 1;

        public Boolean Loop { get; set; } = false;

        public Boolean FixedDecimalNumber { get; set; } = true;

        public void PerformStep()
        {
            CurrentValue = MathUtils.Range(CurrentValue + Step, 0, MaximumValue, Loop);
        }

        public CurrentMaxValueLabel()
        {
            ValueChanged += Display;
        }

        protected virtual void Display()
        {
            String value = $"{CurrentValue}{Separator}{MaximumValue}";
            Double percent = MathUtils.Round((Double) CurrentValue / MathUtils.ZeroCheck(MaximumValue) * 100, PercentFractionalCount,
                RoundType);
            Int32 digits = MathUtils.GetDigitsAfterPoint(percent);
            String additionalZeros =
                $"{(FixedDecimalNumber ? (digits == 0 ? "." : String.Empty) + String.Concat(Enumerable.Repeat("0", PercentFractionalCount - digits)) : String.Empty)}";
            String percentString = $"{percent.ToString(CultureInfo.InvariantCulture)}{additionalZeros}%";
            String text = DisplayType switch
            {
                MathUtils.DisplayType.ValueAndPercent =>
                $@"{value} ({percentString})",
                MathUtils.DisplayType.Percent =>
                $@"{percentString}",
                _ => $@"{value}"
            };

            Text = text;
        }

        protected override void Dispose(Boolean disposing)
        {
            ValueChanged = null;
            base.Dispose(disposing);
        }
    }
}