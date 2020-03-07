// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;

namespace Common_Library.Types.Map
{
    // ReSharper disable once PossibleInterfaceMemberAmbiguity
    public interface IMap<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyMap<TKey, TValue>
    {
        void Add(TValue key, TKey value);

        Boolean TryAdd(TValue key, TKey value);

        public void Remove(TValue key);

        public void Remove(TValue key, out TKey value);

        public new TKey this[TValue key] { get; set; }
    }
}