// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Common_Library.Utils.IO;

namespace Common_Library.Serialization
{
    public static class XML
    {
        /// <summary>
        /// Converts an object to XML
        /// </summary>
        /// <param name="temp">Object to convert</param>
        /// <param name="rootName"></param>
        /// <param name="fileName">File to save the XML to</param>
        /// <returns>string representation of the object in XML format</returns>
        public static String ObjectToXML(Object temp, String rootName, String fileName)
        {
            String xml = ObjectToXML(temp, rootName);
            File.AppendAllText(fileName, xml);
            return xml;
        }

        /// <summary>
        /// Converts an object to XML
        /// </summary>
        /// <param name="temp">Object to convert</param>
        /// <param name="rootName"></param>
        /// <returns>string representation of the object in XML format</returns>
        public static String ObjectToXML(Object temp, String rootName)
        {
            if (temp == null)
            {
                throw new ArgumentException("Object can not be null");
            }

            using MemoryStream stream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(temp.GetType(), new XmlRootAttribute(rootName));
            serializer.Serialize(stream, temp);
            stream.Flush();
            return Encoding.UTF8.GetString(stream.GetBuffer(), 0, (Int32) stream.Position);
        }

        /// <summary>
        /// Takes an XML file and exports the Object it holds
        /// </summary>
        /// <param name="fileName">File name to use</param>
        /// <param name="result">Object to export to</param>
        /// <param name="rootName"></param>
        public static void XMLToObject<T>(String fileName, out T result, String rootName)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("File name can not be null/empty");
            }

            if (!PathUtils.IsExistAsFile(fileName))
            {
                throw new ArgumentException("File does not exist");
            }

            String content = FileUtils.GetFileContents(fileName);
            result = XMLToObject<T>(content, rootName);
        }

        /// <summary>
        /// Converts an XML string to an object
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="xml">XML string</param>
        /// <param name="rootName"></param>
        /// <returns>The object of the specified type</returns>
        public static T XMLToObject<T>(String xml, String rootName)
        {
            if (String.IsNullOrEmpty(xml))
            {
                throw new ArgumentException("XML can not be null/empty");
            }

            using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootName));
            return (T) serializer.Deserialize(stream);
        }

        /// <summary>
        /// Takes an XML file and exports the Object it holds
        /// </summary>
        /// <param name="fileName">File name to use</param>
        /// <param name="result">Object to export to</param>
        /// <param name="type">Object type to export</param>
        /// <param name="rootName"></param>
        public static void XMLToObject(String fileName, out Object result, Type type, String rootName)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("File name can not be null/empty");
            }

            if (!PathUtils.IsExistAsFile(fileName))
            {
                throw new ArgumentException("File does not exist");
            }

            String content = FileUtils.GetFileContents(fileName);
            result = XMLToObject(content, type, rootName);
        }

        /// <summary>
        /// Converts an XML string to an object
        /// </summary>
        /// <param name="xml">XML string</param>
        /// <param name="type">Object type to export</param>
        /// <param name="rootName"></param>
        /// <returns>The object of the specified type</returns>
        public static Object XMLToObject(String xml, Type type, String rootName)
        {
            if (String.IsNullOrEmpty(xml))
            {
                throw new ArgumentException("XML can not be null/empty");
            }

            using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
            XmlSerializer serializer = new XmlSerializer(type, new XmlRootAttribute(rootName));
            return serializer.Deserialize(stream);
        }
        
        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        public static String SerializeXML<T>(T serializableObject)
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
        public static Boolean TryDeserializeXML<T>(String obj, out T value)
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