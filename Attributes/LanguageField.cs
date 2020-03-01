// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class LanguageField : Attribute
    {
        public readonly Boolean Enabled;
        public LanguageField(Boolean enabled = true)
        {
            Enabled = enabled;
        }
    }
}