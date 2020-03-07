// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Linq;
using Newtonsoft.Json;

namespace System.Collections.Generic
{
    [JsonObject(MemberSerialization.OptOut)]
    public class NestedDictionary<TKey, TValue> : Dictionary<TKey, NestedDictionary<TKey, TValue>>
    {
        public TValue Value { set; get; }

        public new NestedDictionary<TKey, TValue> this[TKey key]
        {
            get
            {
                if (!Keys.Contains(key))
                {
                    base[key] = new NestedDictionary<TKey, TValue>();
                }
                
                return base[key];
            }
            set
            {
                base[key] = value;
            }
        }

        public NestedDictionary<TKey, TValue> this[TKey key, params TKey[] sections]
        {
            get
            {
                NestedDictionary<TKey, TValue> dict = this;
                
                if (sections.Length <= 0)
                {
                    return dict[key];
                }

                dict = sections.Aggregate(dict, (current, section) => current[section]);

                return dict[key];
            }
            set
            {
                NestedDictionary<TKey, TValue> dict = this;
                
                if (sections.Length <= 0)
                {
                    dict[key] = value;
                }

                dict = sections.Aggregate(dict, (current, section) => current[section]);

                dict[key] = value;
            }
        }
    }
}