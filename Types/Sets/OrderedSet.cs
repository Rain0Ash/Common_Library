// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace System.Collections.Generic
{
    public static class OrderedSetLINQ
    {
        public static OrderedSet<T> ToOrderedSet<T>(this IEnumerable<T> source)
        {
            OrderedSet<T> orderedSet = new OrderedSet<T>();
            foreach (T item in source)
            {
                orderedSet.Add(item);
            }

            return orderedSet;
        }
    }
    public class OrderedSet<T> : ICollection<T>
    {
        private readonly IDictionary<T, LinkedListNode<T>> _mDictionary;
        private readonly LinkedList<T> _mLinkedList;

        public OrderedSet()
            : this(EqualityComparer<T>.Default)
        {
        }

        public OrderedSet(IEqualityComparer<T> comparer)
        {
            _mDictionary = new Dictionary<T, LinkedListNode<T>>(comparer);
            _mLinkedList = new LinkedList<T>();
        }

        public OrderedSet(IEnumerable<T> collection, IEqualityComparer<T> comparer = null)
        {
            foreach (T item in collection)
            {
                Add(item);
            }
            _mDictionary = new Dictionary<T, LinkedListNode<T>>(comparer ?? EqualityComparer<T>.Default);
            _mLinkedList = new LinkedList<T>();
        }

        public Int32 Count
        {
            get { return _mDictionary.Count; }
        }

        public Boolean IsReadOnly
        {
            get { return _mDictionary.IsReadOnly; }
        }

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        public void Clear()
        {
            _mLinkedList.Clear();
            _mDictionary.Clear();
        }

        public Boolean Remove(T item)
        {
            Boolean found = _mDictionary.TryGetValue(item, out LinkedListNode<T> node);
            if (!found)
            {
                return false;
            }

            _mDictionary.Remove(item);
            _mLinkedList.Remove(node);
            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _mLinkedList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Boolean Contains(T item)
        {
            return item != null && _mDictionary.ContainsKey(item);
        }

        public void CopyTo(T[] array, Int32 arrayIndex)
        {
            _mLinkedList.CopyTo(array, arrayIndex);
        }

        public Boolean Add(T item)
        {
            if (_mDictionary.ContainsKey(item))
            {
                return false;
            }

            LinkedListNode<T> node = _mLinkedList.AddLast(item);
            _mDictionary.Add(item, node);
            return true;
        }
    }
}
