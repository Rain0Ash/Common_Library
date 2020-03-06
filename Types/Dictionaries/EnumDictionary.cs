// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using Common_Library.Comparers.Enum;

namespace System.Collections.Generic
{
    public class EnumDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TKey : struct, IConvertible
    {
        static EnumDictionary()
        {
            if (!typeof(TKey).IsEnum)
            {
                throw new ArgumentException("TKey must be an enum type.");
            }
        }
        
        public EnumDictionary()
            : base(new EnumEqualityComparer<TKey>())
        {
        }

        public EnumDictionary(IDictionary<TKey, TValue> dictionary)
            : base(dictionary, new EnumEqualityComparer<TKey>())
        {
        }
        
        public EnumDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
            : base(collection, new EnumEqualityComparer<TKey>())
        {
        }
        
        public EnumDictionary(Int32 capacity)
            : base(capacity, new EnumEqualityComparer<TKey>())
        {
        }
    }
}