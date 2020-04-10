// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Common_Library.Interfaces;

namespace Common_Library.GUI.WinForms.ListViews
{
    public class ValidableListView : FixedListView, IMultiValidable
    {
        public Color InvalidColor { get; set; } = Color.Coral;

        public event Handlers.EmptyHandler ValidateItemChanged;  
        
        private Func<Object, Boolean> _validateItem;
        public Func<Object, Boolean> ValidateItem
        {
            get
            {
                return _validateItem;
            }
            set
            {
                if (_validateItem == value)
                {
                    return;
                }
                
                _validateItem = value;
                
                ValidateItemChanged?.Invoke();
            }
        }
        
        public Boolean IsValid
        {
            get
            {
                return Items.OfType<ListViewItem>().All(IsValidItem);
            }
        }

        public ValidableListView()
        {
            ValidateItemChanged += Refresh;
        }

        public virtual Boolean IsValidItem(Object item)
        {
            return ValidateItem?.Invoke(item) ?? true;
        }

        public Boolean IsValidIndex(Int32 index)
        {
            return IsValidItem(Items[index]);
        }
    }
}