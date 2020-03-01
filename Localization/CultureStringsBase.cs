// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common_Library.Attributes;

namespace Common_Library.Localization
{
    public class CultureStringsBase
        {
            private readonly IComparer<String> _comparer;

            public static implicit operator String(CultureStringsBase obj)
            {
                return obj.ToString();
            }
            
            private const String StringMissing = @"String is missing";
            
            // ReSharper disable once InconsistentNaming
            [LanguageField] public String en;

            public CultureStringsBase()
                : this(StringMissing)
            {
            }
            
            public CultureStringsBase(String english, IComparer<String> comparer = null)
            {
                en = english ?? StringMissing;
                _comparer = comparer ?? new CultureComparer();
            }
            
            private IEnumerable<Culture> GetAvailableCultures()
            {
                return GetType()
                    .GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                    .Where(field =>
                        (field.GetCustomAttribute<LanguageField>()?.Enabled ?? false) &&
                        CountryData.LanguageNameByISO2.ContainsKey(field.Name.ToUpper()))
                    .Select(field => new Culture(field.Name));
            }
            
            public IEnumerable<Culture> GetCultures()
            {
                return GetAvailableCultures()
                    .OrderBy(culture => culture.CultureName, _comparer);
            }

            public override String ToString()
            {
                Type type = GetType();
                const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
                String str = type.GetField(String.IsNullOrEmpty(LocalizationBase.LocalizationCultureCode) ? LocalizationBase.DefaultCultureCode : LocalizationBase.LocalizationCultureCode, flags)?.GetValue(this)?.ToString() ?? 
                             type.GetField(LocalizationBase.DefaultCultureCode, flags)?.GetValue(this)?.ToString() ?? StringMissing;

                return str;
            }
        }
}