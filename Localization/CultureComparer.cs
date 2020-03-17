// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Linq;
using Common_Library.Comparers;
using Common_Library.Utils;
using Common_Library.Utils.Math;

namespace Common_Library.Localization
{
    public class CultureComparer : OrderedComparer<String>
    {
        public CultureComparer(IEnumerable<CultureInfoFixed> cultureOrder)
            : this((cultureOrder ?? new[] {LocalizationBase.DefaultCulture}).Select(culture => culture.Code))
        {
        }

        public CultureComparer(IEnumerable<Int32> lcidOrder)
            : this(lcidOrder?.SelectWhere(lcid => (LocalizationBase.CodeByLCID.TryGetValue(lcid, out String code), code)))
        {
        }

        public CultureComparer(IEnumerable<String> languageOrder = null)
            : base((languageOrder ?? CultureStringsBase.DefaultLocalization).Select(code => code.ToLower()))
        {
        }

        public Int32 GetLanguageOrderID(Int32 lcid)
        {
            return GetLanguageOrderID(LocalizationBase.CodeByLCID.TryGetValue(lcid, LocalizationBase.DefaultCulture.Code));
        }

        public Int32 GetLanguageOrderID(String code)
        {
            return MathUtils.Range(Order.IndexOf(code.ToLower()));
        }
    }
}