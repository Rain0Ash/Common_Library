// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Common_Library.Interfaces;
using Common_Library.Utils.IO;

namespace Common_Library.GUI.WinForms.ListViews
{
    public class ValidableListView : FixedListView, IAutoValidable
    {
        public Color InvalidColor { get; set; } = Color.Coral;
        
        public Func<Object, Boolean> ValidateFunc { get; set; } = obj => true;
        
        protected virtual Boolean CheckValidFormatItem(ListViewItem item)
        {
            return ValidateFunc?.Invoke(item.Text) ?? true;
        }

        public Boolean CheckValidFormatIndex(Int32 index)
        {
            return CheckValidFormatItem(Items[index]);
        }

        public Boolean CheckValidFormat()
        {
            return Items.OfType<ListViewItem>().All(CheckValidFormatItem);
        }
    }
}