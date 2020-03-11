// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic.Interfaces;

namespace Common_Library.Types.Map
{
    public interface IIndexMap<TKey, TValue> : IMap<TKey, TValue>, IReadOnlyIndexMap<TKey, TValue>, IIndexDictionary<TKey, TValue>
    {
        public void Insert(TValue key, TKey value);

        public void Insert(Int32 index, TValue key, TKey value);

        public Boolean TryInsert(TValue key, TKey value);

        public Boolean TryInsert(Int32 index, TValue key, TKey value);
    }
}