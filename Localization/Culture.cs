// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using System.Linq;
using Common_Library.Images.flags;

namespace Common_Library.Localization
{
    public class Culture
    {
        public readonly String CultureCode;
        public String CultureName;
        public Image CultureImage;

        public Culture(String cultureCode = null, String cultureName = null, Image cultureImage = null)
        {
            cultureCode = cultureCode?.ToLower();

            CultureCode = cultureCode;
            CultureName = cultureName;
            CultureImage = cultureImage;

            switch (cultureCode)
            {
                case null when cultureName == null:
                    SetDefaultCulture();
                    break;
                case null:
                    String cultureKey = CountryData.LanguageNameByISO2.FirstOrDefault(x => x.Value.CustomName == cultureName).Key;
                
                    if (cultureKey == null)
                    {
                        SetDefaultCulture();
                        break;
                    }

                    cultureCode = cultureKey.ToLower();
                    CultureCode = cultureCode;
                    CultureName = cultureName;
                    CultureImage = GetImage(cultureCode);
                    
                    break;
                default:
                    if (cultureName == null)
                    {
                        CountryData.TryGetName(cultureCode.ToUpper(), out String value);
                        CultureName = value ?? "null";
                        CultureImage = GetImage(cultureCode);
                    }

                    break;
            }

            if (cultureImage == null)
            {
                CultureImage = GetImage(CultureCode);
            }
        }

        private void SetDefaultCulture()
        {
            CountryData.TryGetName(LocalizationBase.DefaultCultureCode, out String cultureName);
            CultureName = cultureName ?? "English";
            CultureImage = GetImage(LocalizationBase.DefaultCultureCode);
        }
        
        private static Image GetImage(String cultureCode)
        {
            if (String.IsNullOrEmpty(cultureCode))
            {
                return Images.Images.Basic.Null;
            }
            
            return (Image)(FlagsImages.ResourceManager.GetObject(cultureCode) ?? FlagsImages.ResourceManager.GetObject($"_{cultureCode}")) ?? Images.Images.Basic.Null;
        }
    }
}