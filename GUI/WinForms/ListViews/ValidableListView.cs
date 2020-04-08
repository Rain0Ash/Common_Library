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

        public event Handlers.FuncHandler<Object, Boolean> ValidateFuncChanged;  
        
        private Func<Object, Boolean> _validateFunc = obj => true;

        public Func<Object, Boolean> ValidateFunc
        {
            get
            {
                return _validateFunc;
            }
            set
            {
                if (_validateFunc == value)
                {
                    return;
                }
                
                _validateFunc = value;
                
                ValidateFuncChanged?.Invoke(_validateFunc);
            }
        }
        
        protected virtual Boolean CheckValidItem(Object item)
        {
            return ValidateFunc?.Invoke(item) ?? true;
        }
        
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