// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Net;

namespace Common_Library
{
    public static class Handlers
    {
        public delegate void EmptyHandler();

        public delegate void StringHandler(String str);

        public delegate void EnumerableHandler(IEnumerable<Object> list);

        public delegate void Int32Handler(Int32 number);

        public delegate void Int32ArrayHandler(Int32[] number);

        public delegate void BooleanHandler(Boolean boolean);

        public delegate void HttpStatusCodeHandler(HttpStatusCode statusCode);

        public delegate void FuncHandler<out T, in TOut>(Func<T, TOut> function);

        public delegate void ObjectHandler(Object obj);

        public delegate void IndexObjectHandler(Int32 index, Object obj);

        public delegate void KeyValueHandler(Object key, Object value);

        public delegate void KeyValueHandler<in T1, in T2>(T1 key, T2 value);
        
        public delegate void ObjectArrayHandler(Object[] obj);

        public delegate void TypeHandler<in T>(T type);

        public delegate void IndexTypeHandler<in T>(Int32 index, T type);

        public delegate void RTypeHandler<T>(ref T type);

        public delegate void IndexRTypeHandler<T>(Int32 index, ref T type);

        public delegate void TypeArrayHandler<in T>(T[] array);

        public delegate void TypeKeyValueHandler<in TKey, in TValue>(TKey key, TValue value);

        public delegate void IndexTypeKeyValueHandler<in TKey, in TValue>(Int32 index, TKey key, TValue value);

        public delegate void RTypeKeyValueHandler<TKey, TValue>(ref TKey key, ref TValue value);

        public delegate void IndexRTypeKeyValueHandler<TKey, TValue>(Int32 index, ref TKey key, ref TValue value);
    }
}