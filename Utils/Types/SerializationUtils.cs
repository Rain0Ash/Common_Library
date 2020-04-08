// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Common_Library.Utils
{
    public static class SerializationUtils
    {
        public static Byte[] Serialize(this Object obj)
        {
            if(obj == null)
            {
                return null;
            }

            BinaryFormatter bf = new BinaryFormatter();
            using MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        public static Object Deserialize(this Byte[] bytes)
        {
            using MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(bytes, 0, bytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = binForm.Deserialize(memStream);

            return obj;
        }

        public static T Deserialize<T>(this Byte[] bytes)
        {
            return (T)Deserialize(bytes);
        }
    }
}