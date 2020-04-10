// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Interfaces
{
    public interface IValidable
    {
        public event Handlers.EmptyHandler ValidateChanged;
        public Func<Boolean> Validate { get; set; }

        public Boolean IsValid
        {
            get
            {
                return Validate?.Invoke() != false;
            }
        }
    }
}