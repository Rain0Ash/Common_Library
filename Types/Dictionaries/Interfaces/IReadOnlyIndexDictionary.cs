// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace System.Collections.Generic.Interfaces
{
    public interface IReadOnlyIndexDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        public IReadOnlyList<TKey> OrderedKeys { get; }

        public TValue GetByIndex(Int32 index);

        public KeyValuePair<TKey, TValue> GetPairByIndex(Int32 index);

        public Boolean TryGetPairByIndex(Int32 index, out KeyValuePair<TKey, TValue> pair);

        public Int32 IndexOf(TKey key);
    }
}