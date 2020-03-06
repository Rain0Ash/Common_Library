// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;
using System.Globalization;
using Common_Library.Images.flags;
using JetBrains.Annotations;

namespace Common_Library.Localization
{
    public class CultureInfoFixed : CultureInfo
    {
        private String _customName;

        public String CustomName
        {
            get
            {
                return _customName ?? NativeName;
            }
            set
            {
                _customName = value;
            }
        }

        public String Code
        {
            get
            {
                return TwoLetterISOLanguageName.ToLower();
            }
        }

        private Image _image;

        public Image Image
        {
            get
            {
                return _image ?? 
                       (Image)(FlagsImages.ResourceManager.GetObject(TwoLetterISOLanguageName) ?? 
                               FlagsImages.ResourceManager.GetObject($"_{TwoLetterISOLanguageName}")) ?? Images.Images.Basic.Null;
            }
            set
            {
                _image = value;
            }
        }

        public UInt16 LCID16
        {
            get
            {
                return (UInt16) LCID;
            }
        }

        public CultureInfoFixed(Int32 lcid)
            : base(lcid)
        {
        }

        public CultureInfoFixed(Int32 lcid, Boolean useUserOverride)
            : base(lcid, useUserOverride)
        {
        }
        
        public CultureInfoFixed([NotNull] String name)
            : base(name)
        {
        }
        
        public CultureInfoFixed([NotNull] String name, Boolean useUserOverride)
            : base(name, useUserOverride)
        {
        }
    }
}