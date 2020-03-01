// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Linq;
using Common_Library;

namespace System.Collections.Generic
{
    public class EventList<T> : List<T>
    {
        public event Handlers.TypeHandler<T> OnAdd;
        public event Handlers.TypeHandler<T> OnRemove;
        public event Handlers.EmptyHandler OnClear;
        public event Handlers.EmptyHandler ItemsChanged;

        public EventList()
        {
        }
        
        public EventList(IEnumerable<T> collection)
            : base(collection)
        {
        }
        
        public EventList(Int32 capacity)
            : base(capacity)
        {
        }
        
        public new void Add(T item)
        {
            base.Add(item);
            OnAdd?.Invoke(item);
            ItemsChanged?.Invoke();
        }

        public new void Remove(T item)
        {
            if (!Contains(item))
            {
                return;
            }

            base.Remove(item);
            OnRemove?.Invoke(item);
            ItemsChanged?.Invoke();
        }
        
        public new void RemoveAt(Int32 index)
        {
            T item = this[index];
            base.RemoveAt(index);
            OnRemove?.Invoke(item);
            ItemsChanged?.Invoke();
        }

        public new void Clear()
        {
            Boolean any = this.Any();
            base.Clear();
            if (!any)
            {
                return;
            }

            OnClear?.Invoke();
            ItemsChanged?.Invoke();
        }
    }
}