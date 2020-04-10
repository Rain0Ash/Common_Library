// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using Common_Library.Interfaces;

namespace Common_Library.GUI.WinForms.TextBoxes
{
    public class ValidableTextBox : HistoryTextBox, IValidable
    {
        public event Handlers.EmptyHandler ValidateChanged;
        
        private Func<Boolean> _validate;
        public Func<Boolean> Validate
        {
            get
            {
                return _validate;
            }
            set
            {
                if (_validate == value)
                {
                    return;
                }

                _validate = value;
                
                ValidateChanged?.Invoke();
            }
        }

        public ValidableTextBox()
        {
            TextChanged += (sender, args) => ItemValidateColor();
            ValidateChanged += ItemValidateColor;
        }

        public Boolean IsValid
        {
            get
            {
                return Validate?.Invoke() != false;
            }
        }

        protected virtual void ItemValidateColor()
        {
            BackColor = IsValid ? Color.White : Color.Coral;
        }
    }
}