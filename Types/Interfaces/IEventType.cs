// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace Common_Library.Types.Interfaces
{
    public interface IEventType
    {
        public event Handlers.EmptyHandler ItemsChanged;

        public event Handlers.EmptyHandler OnClear;
    }
}