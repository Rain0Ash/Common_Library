// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Common_Library.Types.Deque
{
    internal static class DequeHelpers
    {
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IReadOnlyCollection<T> ReifyCollection<T>(IEnumerable<T> source)
        {
            return source switch
            {
                null => throw new ArgumentNullException(nameof(source)),
                IReadOnlyCollection<T> result => result,
                ICollection<T> collection => new CollectionWrapper<T>(collection),
                ICollection nongenericCollection => new NongenericCollectionWrapper<T>(nongenericCollection),
                _ => new List<T>(source)
            };
        }

        private sealed class NongenericCollectionWrapper<T> : IReadOnlyCollection<T>
        {
            private readonly ICollection _collection;

            public NongenericCollectionWrapper(ICollection collection)
            {
                _collection = collection ?? throw new ArgumentNullException(nameof(collection));
            }

            public Int32 Count
            {
                get
                {
                    return _collection.Count;
                }
            }

            public IEnumerator<T> GetEnumerator()
            {
                return _collection.Cast<T>().GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _collection.GetEnumerator();
            }
        }

        private sealed class CollectionWrapper<T> : IReadOnlyCollection<T>
        {
            private readonly ICollection<T> _collection;

            public CollectionWrapper(ICollection<T> collection)
            {
                _collection = collection ?? throw new ArgumentNullException(nameof(collection));
            }

            public Int32 Count
            {
                get
                {
                    return _collection.Count;
                }
            }

            public IEnumerator<T> GetEnumerator()
            {
                return _collection.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _collection.GetEnumerator();
            }
        }
    }
}