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
        
        public delegate void FuncHandler(Func<Type, Type> function);

        public delegate void ObjectHandler(Object obj);
        
        public delegate void ObjectIndexHandler(Object obj, Int32 index);
        
        public delegate void KeyValueHandler(Object key, Object value);
        
        public delegate void ObjectArrayHandler(Object[] obj);
        
        public delegate void TypeHandler<in T>(T type);
        
        public delegate void TypeArrayHandler<in T>(T[] array);

        public delegate void TypeKeyValueHandler<in TKey, in TValue>(TKey key, TValue value);
    }
}