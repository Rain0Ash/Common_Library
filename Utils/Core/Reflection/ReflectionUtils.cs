// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common_Library.Utils
{
    public static class ReflectionUtils
    {
        public static IEnumerable<Type> GetTypesInNamespace(String nameSpace)
        {
            return GetTypesInNamespace(Assembly.GetEntryAssembly(), nameSpace);
        }

        public static IEnumerable<Type> GetTypesInNamespace(Assembly assembly, String nameSpace)
        {
            return assembly.GetTypes().Where(type => String.Equals(type.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
        }
    }
}