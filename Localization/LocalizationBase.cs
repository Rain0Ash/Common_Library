﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

namespace Common_Library.Localization
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public abstract class LocalizationBase
    {
        public const String DefaultCultureCode = @"en";
        private const UInt16 DefaultCultureLCID = 0x409;
        public static event Handlers.EmptyHandler LanguageChanged;

        public static readonly String NewLine = Environment.NewLine;

        public static Boolean ChangeUIThreadLanguage { get; set; } = true;
        
        public static String LocalizationCultureCode { get; protected set; }

        private readonly CultureStringsBase _currentCultureStrings;

        protected LocalizationBase(String cultureInfo = null, CultureStringsBase currentCultureStrings = null)
        {
            _currentCultureStrings = currentCultureStrings ?? new CultureStringsBase(null);
            Init(cultureInfo);
        }

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        private static extern UInt16 SetThreadUILanguage(UInt16 langId);

        public static void UpdateLocalization(String cultureInfo = null)
        {
            if (LocalizationCultureCode == cultureInfo)
            {
                return;
            }

            LocalizationCultureCode = cultureInfo ?? GetCurrentCultureCode();
            if (ChangeUIThreadLanguage)
            {
                SetUILanguage();
            }
            LanguageChanged?.Invoke();
        }

        private void Init(String cultureInfo)
        {
            InitializeLanguage();
            UpdateLocalization(GetCultures().Select(culture => culture.CultureCode).Contains(cultureInfo) ? cultureInfo : DefaultCultureCode);
        }

        protected virtual void InitializeLanguage()
        {
            //Override;
        }

        public static String GetCurrentCultureCode()
        {
            return CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        }
        
        public static CultureInfo GetCurrentCulture()
        {
            return CultureInfo.CurrentCulture;
        }

        public static CultureInfo GetLocalizationCulture()
        {
            return GetCultureByISOCode(LocalizationCultureCode);
        }

        public static void SetUILanguage()
        {
            CountryData.TryGetCultureInfo(LocalizationCultureCode, out CultureInfoFixed info);
            if (info == null)
            {
                CountryData.TryGetCultureInfo(DefaultCultureCode, out info);
            }

            UInt16 lcid;
            try
            {
                lcid = (UInt16) info.LCID;
            }
            catch (Exception)
            {
                lcid = DefaultCultureLCID;
            }
            
            SetUILanguage(lcid);
        }
        
        public static void SetUILanguage(UInt16 lcid)
        {
            SetThreadUILanguage(lcid);
        }

        public static CultureInfo GetCultureByISOCode(String code)
        {
            if (code == null)
            {
                return GetCurrentCulture();
            }
            return CultureInfo
                       .GetCultures(CultureTypes.InstalledWin32Cultures)
                       .First(culture => String.Equals(culture.TwoLetterISOLanguageName, code, StringComparison.OrdinalIgnoreCase)) ?? GetCurrentCulture();
        }

        public Int32 GetLanguageID(String languageCodeOrName = null)
        {
            languageCodeOrName ??= GetLocalizationCultureCode();
            Int32 index = GetCultures().ToList().FindIndex(culture =>
                String.Equals(culture.CultureCode, languageCodeOrName, StringComparison.CurrentCultureIgnoreCase) ||
                String.Equals(culture.CultureName, languageCodeOrName, StringComparison.CurrentCultureIgnoreCase));
            return index;
        }

        public IEnumerable<Culture> GetCultures()
        {
            return _currentCultureStrings.GetCultures();
        }

        public static String GetLocalizationCultureCode()
        {
            return LocalizationCultureCode;
        }
    }
}
