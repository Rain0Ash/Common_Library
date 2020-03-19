// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Collections.Generic.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using Common_Library.Types.Map;
using Common_Library.Utils;

namespace Common_Library.Localization
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public abstract class LocalizationBase
    {
        public static event Handlers.EmptyHandler LanguageChanged;

        public static String NewLine
        {
            get
            {
                return Environment.NewLine;
            }
        }

        static LocalizationBase()
        {
            DefaultCulture = new CultureInfoFixed(0x409) {CustomName = "English"};

            CultureInfoFixed[] array =
            {
                DefaultCulture,
                new CultureInfoFixed(0x419) {CustomName = "Русский"},
                new CultureInfoFixed(0x407) {CustomName = "Deutsch"},
                new CultureInfoFixed(0x40c) {CustomName = "Française"}
            };

            CultureLCIDDictionary =
                new IndexDictionary<Int32, CultureInfoFixed>(array.Select(culture =>
                    new KeyValuePair<Int32, CultureInfoFixed>(culture.LCID, culture)));

            CodeByLCIDMap =
                new Map<Int32, String>(CultureLCIDDictionary.ToDictionary(pair => pair.Value.LCID, pair => pair.Value.Code.ToLower()));

            DefaultComparer = new CultureComparer(CultureLCIDDictionary.Select(pair => pair.Value));

            SystemCulture = CultureLCIDDictionary.TryGetValue(CultureInfo.CurrentUICulture.LCID, DefaultCulture);

            CurrentCulture = SystemCulture;
        }

        public static CultureComparer DefaultComparer { get; }

        private static readonly IndexDictionary<Int32, CultureInfoFixed> CultureLCIDDictionary;

        private static readonly Map<Int32, String> CodeByLCIDMap;

        public static IReadOnlyMap<Int32, String> CodeByLCID
        {
            get
            {
                return CodeByLCIDMap;
            }
        }

        public static IReadOnlyIndexDictionary<Int32, CultureInfoFixed> CultureByLCID
        {
            get
            {
                return CultureLCIDDictionary;
            }
        }

        public static void AddLanguage(Int32 lcid, CultureInfoFixed culture)
        {
            CultureLCIDDictionary.Add(lcid, culture);
            CodeByLCIDMap.Add(culture.Code, lcid);
        }

        public static void RemoveLanguage(Int32 lcid)
        {
            CultureLCIDDictionary.Remove(lcid);
            CodeByLCIDMap.Remove(lcid);
        }

        public static CultureInfoFixed DefaultCulture { get; }

        public static CultureInfoFixed SystemCulture { get; }

        public static CultureInfoFixed BasicCulture
        {
            get
            {
                return UseSystemCulture ? SystemCulture : DefaultCulture;
            }
        }

        public static CultureInfoFixed CurrentCulture { get; protected set; }

        public static Boolean ChangeUIThreadLanguage { get; set; } = true;

        public static Boolean UseSystemCulture { get; set; } = true;

        private readonly CultureStringsBase _currentCultureStrings;

        public IEnumerable<Int32> AvailableLocalization
        {
            get
            {
                return _currentCultureStrings.AvailableLocalization;
            }
        }

        protected LocalizationBase(Int32 lcid, CultureStringsBase currentCultureStrings = null)
        {
            _currentCultureStrings = currentCultureStrings ?? new CultureStringsBase(null);
            InitializeLanguage();
            UpdateLocalization(lcid, AvailableLocalization);
        }

        public static void UpdateLocalization(Int32 lcid, IEnumerable<Int32> avlcid = null)
        {
            if (!CultureByLCID.ContainsKey(lcid) && avlcid?.Contains(lcid) != false)
            {
                lcid = BasicCulture.LCID;
            }

            if (CurrentCulture.LCID == lcid)
            {
                return;
            }

            CurrentCulture = CultureByLCID[lcid];

            if (ChangeUIThreadLanguage)
            {
                SetUILanguage();
            }

            LanguageChanged?.Invoke();
        }

        protected virtual void InitializeLanguage()
        {
            //Override by language strings;
        }

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        private static extern UInt16 SetThreadUILanguage(UInt16 langId);

        public static void SetUILanguage()
        {
            UInt16 lcid;
            try
            {
                lcid = CurrentCulture.LCID16;
            }
            catch (Exception)
            {
                lcid = DefaultCulture.LCID16;
            }

            SetUILanguage(lcid);
        }

        public static void SetUILanguage(UInt16 lcid)
        {
            SetThreadUILanguage(lcid);
        }

        public IEnumerable<CultureInfoFixed> GetCultures()
        {
            return _currentCultureStrings.GetCultures();
        }

        public static Int32 GetLanguageOrderID(Int32 lcid)
        {
            return DefaultComparer.GetLanguageOrderID(lcid);
        }

        public static String GetCultureCode()
        {
            return CurrentCulture.Code;
        }

        public static String GetCultureCode(Int32 lcid)
        {
            return CodeByLCID.TryGetValue(lcid, out String code) ? code : DefaultCulture.Code;
        }
    }
}