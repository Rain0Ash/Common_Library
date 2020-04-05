// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Common_Library.Types.Strings.Interfaces;
using Common_Library.Utils;

namespace Common_Library.Localization
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
    public class CultureStringsBase : IString
    {
        public static CultureComparer Comparer
        {
            get
            {
                return LocalizationBase.DefaultComparer;
            }
        }

        public static implicit operator String(CultureStringsBase obj)
        {
            return obj.ToString();
        }

        protected const String StringMissing = @"String is missing";

        internal static IEnumerable<String> DefaultLocalization { get; } = new[] {LocalizationBase.DefaultCulture.Code};

        public virtual IEnumerable<Int32> AvailableLocalization
        {
            get
            {
                return DefaultLocalization.Select(GetLCID);
            }
        }

        protected readonly Dictionary<Int32, String> Localization;

        protected static Int32 GetLCID(String code)
        {
            return LocalizationBase.CodeByLCID.TryGetValue(code, LocalizationBase.DefaultCulture.LCID);
        }

        private static readonly Int32 DefaultLCID = GetLCID(LocalizationBase.DefaultCulture.Code);

        private String Default
        {
            get
            {
                return Localization.TryGetValue(DefaultLCID, StringMissing) ?? StringMissing;
            }
            set
            {
                Localization[DefaultLCID] = value ?? StringMissing;
            }
        }

        public String en
        {
            get
            {
                return Default;
            }
            protected set
            {
                Default = value;
            }
        }

        public CultureStringsBase()
            : this(StringMissing)
        {
        }

        public CultureStringsBase([NotNull] String english)
        {
            Localization = AvailableLocalization.ToDictionary(lcid => lcid, lcid => (String) null);
            en = english ?? StringMissing;
        }

        public IEnumerable<CultureInfoFixed> GetCultures()
        {
            return Localization.Keys.Select(lcid => LocalizationBase.CultureByLCID[lcid]).OrderBy(culture => culture.Code, Comparer);
        }

        public override String ToString()
        {
            return ToString(LocalizationBase.CurrentCulture.LCID);
        }
        
        public String ToString(Int32 lcid)
        {
            return Localization.TryGetValue(lcid, Default) ?? Default;
        }
    }
}