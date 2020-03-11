// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace Common_Library.Types.Interfaces
{
    public interface IEventCollection<T> : IEventType
    {
        public event Handlers.RTypeHandler<T> OnAdd;

        public event Handlers.RTypeHandler<T> OnSet;

        public event Handlers.RTypeHandler<T> OnRemove;

        public event Handlers.RTypeHandler<T> OnChange;
    }
}