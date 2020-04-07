// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using System.Linq;
using Common_Library.Interfaces;

namespace Common_Library.GUI.WinForms.ListBoxes
{
    public class ValidableListBox : EventListBox, IAutoValidable
    {
        public Func<Object, Boolean> ValidateFunc { get; set; } = obj => true;
        
        public ValidableListBox()
        {
            ItemsChanged += CheckValidFormatColor;
        }

        protected virtual void CheckValidFormatColor()
        {
            SuspendLayout();

            foreach (Object item in Items)
            {
                if (!CheckValidFormatItem(item))
                {
                    SetColor(item, Brushes.Coral);
                    continue;
                }

                RemoveColor(item);
            }

            ResumeLayout(false);
            PerformLayout();
        }

        private Boolean CheckValidFormatItem(Object item)
        {
            return ValidateFunc?.Invoke(item) ?? true;
        }

        public Boolean CheckValidFormatIndex(Int32 index)
        {
            return CheckValidFormatItem(Items[index]);
        }

        public Boolean CheckValidFormat()
        {
            return Items.OfType<Object>().All(CheckValidFormatItem);
        }
    }
}