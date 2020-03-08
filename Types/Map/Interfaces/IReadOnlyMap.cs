// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;

namespace Common_Library.Types.Map
{
    public interface IReadOnlyMap<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        public Boolean ContainsKey(TValue value);
        public Boolean TryGetValue(TValue key, out TKey value);
        public TKey this[TValue key] { get; }

        public IEnumerator<KeyValuePair<TValue, TKey>> GetReversedEnumerator();
    }
}