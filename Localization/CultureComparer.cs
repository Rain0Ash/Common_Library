// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using Common_Library.Comparers;
using Common_Library.Utils;

namespace Common_Library.Localization
{
    public class CultureComparer : OrderedComparer
    {
        public CultureComparer(IEnumerable<String> languageOrderList = null)
            : base((languageOrderList ?? new[] {"EN"}).SelectWhere(code => (
                CountryData.TryGetName(code.ToUpper(), out String name), name)))
        {
        }
    }
}