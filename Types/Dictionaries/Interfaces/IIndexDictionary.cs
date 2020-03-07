// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace System.Collections.Generic.Interfaces
{
    // ReSharper disable once PossibleInterfaceMemberAmbiguity
    public interface IIndexDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyIndexDictionary<TKey, TValue>
    {
        public void Insert(TKey key, TValue value);

        public void Insert(Int32 index, TKey key, TValue value);

        public Boolean TryInsert(TKey key, TValue value);
        
        public Boolean TryInsert(Int32 index, TKey key, TValue value);

        public void Swap(Int32 index1, Int32 index2);

        public void Reverse();

        public void Reverse(Int32 index, Int32 count);

        public void Sort();

        public void Sort(Comparison<TKey> comparison);

        public void Sort(IComparer<TKey>? comparer);

        public void Sort(Int32 index, Int32 count, IComparer<TKey>? comparer);
    }
}