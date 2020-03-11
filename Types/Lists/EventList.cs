// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Linq;
using Common_Library;
using Common_Library.Types.Interfaces;

namespace System.Collections.Generic
{
    public class EventList<T> : List<T>, IEventIndexCollection<T>
    {
        public event Handlers.RTypeHandler<T> OnAdd;
        public event Handlers.IndexRTypeHandler<T> OnInsert;
        public event Handlers.RTypeHandler<T> OnSet;
        public event Handlers.RTypeHandler<T> OnRemove;
        public event Handlers.RTypeHandler<T> OnChange;

        public event Handlers.IndexRTypeHandler<T> OnChangeIndex;
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
            OnAdd?.Invoke(ref item);
            ItemsChanged?.Invoke();
        }

        public new void Remove(T item)
        {
            if (!Contains(item))
            {
                return;
            }

            base.Remove(item);
            OnRemove?.Invoke(ref item);
            ItemsChanged?.Invoke();
        }

        public new void RemoveAt(Int32 index)
        {
            T item = this[index];
            base.RemoveAt(index);
            OnRemove?.Invoke(ref item);
            OnChangeIndex?.Invoke(index, ref item);
            ItemsChanged?.Invoke();
        }

        public new void Insert(Int32 index, T item)
        {
            base.Insert(index, item);
            OnAdd?.Invoke(ref item);
            OnInsert?.Invoke(index, ref item);
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

        public new T this[Int32 index]
        {
            get
            {
                return base[index];
            }
            set
            {
                base[index] = value;
                OnSet?.Invoke(ref value);
            }
        }
    }
}