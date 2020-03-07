// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Collections.Generic.Interfaces;

namespace Common_Library.Types.Map
{
    public interface IReadOnlyIndexMap<TKey, TValue> : IReadOnlyMap<TKey, TValue>, IReadOnlyIndexDictionary<TKey, TValue>
    {
        public Int32 IndexOf(TValue key);
        
        public TKey GetReversedByIndex(Int32 index);

        public KeyValuePair<TValue, TKey> GetReversedPairByIndex(Int32 index);

        public Boolean TryGetReversedPairByIndex(Int32 index, out KeyValuePair<TValue, TKey> pair);
    }
}