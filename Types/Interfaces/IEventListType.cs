// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace Common_Library.Types.Interfaces
{
    public interface IEventListType<out T> : IEventType
    {
        public event Handlers.TypeHandler<T> OnAdd;

        public event Handlers.TypeHandler<T> OnSet; 
        
        public event Handlers.TypeHandler<T> OnRemove;
        
        public event Handlers.TypeHandler<T> OnChange;
    }
}