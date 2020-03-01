// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Common_Library.Comparers;
using Common_Library.Utils;

namespace Common_Library.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class FormatedField : Attribute
    {
        public readonly Boolean Uniqueness;
        
        private readonly OrderedSet<String> _linkedNames = new OrderedSet<String>();

        public String[] LinkedNames
        {
            get
            {
                String[] array = _linkedNames.ToArray();
                return array;
            }
        }
        
        private readonly OrderedSet<String> _attributes = new OrderedSet<String>();
        
        public String[] Attributes
        {
            get
            {
                String[] array = _attributes.ToArray();
                return array;
            }
        }

        public FormatedField([CallerMemberName] String linkedName = null, Boolean uniqueness = false)
            : this(new []{linkedName ?? throw new NullReferenceException()}, uniqueness)
        {
        }

        // ReSharper disable once ParameterTypeCanBeEnumerable.Local
        public FormatedField(String[] linkedNames, Boolean uniqueness = false)
        {
            Initialize(linkedNames);
            
            Uniqueness = uniqueness;
        }

        private void Initialize(IEnumerable<String> linkedNames)
        {
            foreach (String name in linkedNames)
            {
                String[] names = name.Split(':');
                
                for (Int32 i = 0; i < names.Length; i++)
                {
                    String str = names[i];
                    
                    if (i == 0)
                    {
                        if (str.Length == 0)
                        {
                            break;
                        }

                        _linkedNames.Add(str);
                        continue;
                    }

                    if (str.Length > 0)
                    {
                        _attributes.Add(str);
                    }
                }
            }
        }

        public static String Format(Object obj, String str, String enumerableSeparator = "_")
        {
            if (obj == null || String.IsNullOrEmpty(str))
            {
                return String.Empty;
            }
            
            try
            {
                Dictionary<String, Object> replaceDictionary = new Dictionary<String, Object>();
                foreach (PropertyInfo field in obj.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    IEnumerable<FormatedField> attributes = field.GetCustomAttributes<FormatedField>();

                    foreach (FormatedField attribute in attributes)
                    {
                        foreach (String linkedName in attribute.LinkedNames)
                        {
                            Object fieldValue = field.GetValue(obj);
                            String value = fieldValue is IEnumerable<Object> enumerable
                                ? String.Join(enumerableSeparator, enumerable)
                                : fieldValue.ToString();
                            
                            if (String.IsNullOrEmpty(value))
                            {
                                continue;
                            }
                            
                            replaceDictionary[linkedName] = value;
                        }
                    }
                }

                return str.FormatFromDictionary(replaceDictionary);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}