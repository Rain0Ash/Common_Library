// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace Common_Library.Types.Interfaces
{
    public interface IEventPairType<out TKey, out TValue> : IEventType
    {
        public event Handlers.TypeKeyValueHandler<TKey, TValue> OnAdd;
        
        public event Handlers.TypeKeyValueHandler<TKey, TValue> OnSet;
        
        public event Handlers.TypeKeyValueHandler<TKey, TValue> OnRemove;
        
        public event Handlers.TypeKeyValueHandler<TKey, TValue> OnChange;
    }
}