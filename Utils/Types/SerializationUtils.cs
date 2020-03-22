// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Common_Library.Utils
{
    public static class SerializationUtils
    {
        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        public static String Serialize<T>(T serializableObject)
        {
            if (serializableObject == null)
            {
                return null;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using MemoryStream stream = new MemoryStream();
                serializer.Serialize(stream, serializableObject);
                stream.Position = 0;
                return Encoding.ASCII.GetString(stream.ToArray());
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Boolean TryDeserialize<T>(String obj, out T value)
        {
            if (String.IsNullOrEmpty(obj))
            {
                value = default;
                return false;
            }

            try
            {
                using StringReader read = new StringReader(obj);
                Type outType = typeof(T);

                XmlSerializer serializer = new XmlSerializer(outType);
                using XmlReader reader = new XmlTextReader(read);

                value = (T) serializer.Deserialize(reader);

                return true;
            }
            catch (Exception)
            {
                value = default;
                return false;
            }
        }
    }
}