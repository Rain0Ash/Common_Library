// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Linq;
using System.Xml;

namespace Common_Library.Utils.IO
{
    public static class XMLUtils
    {
        private static void SplitOnce(this String value, String separator, out String part1, out String part2)
        {
            if (value!=null)
            {
                Int32 idx=value.IndexOf(separator, StringComparison.InvariantCulture);
                if (idx>=0)
                {
                    part1=value.Substring(0, idx);
                    part2=value.Substring(idx+separator.Length);
                }
                else
                {
                    part1=value;
                    part2=null;
                }
            }
            else
            {
                part1="";
                part2=null;
            }
        }
        
        private static XmlNode CreateXPath(this XmlDocument doc, String xpath)
        {
            XmlNode node=doc;
            foreach (String part in xpath.Substring(1).Split('/'))
            {
                XmlNodeList nodes=node.SelectNodes(part);
                if (nodes.Count>1)
                {
                    throw new XmlException("Xpath '"+xpath+"' was not found multiple times!");
                }

                if (nodes.Count == 1)
                {
                    node=nodes[0]; continue;
                }

                if (part.StartsWith("@"))
                {
                    XmlAttribute anode=doc.CreateAttribute(part.Substring(1));
                    node.Attributes.Append(anode);
                    node=anode;
                }
                else
                {
                    String elName, attrib=null;
                    if (part.Contains("["))
                    {
                        part.SplitOnce("[", out elName, out attrib);
                        if (!attrib.EndsWith("]"))
                        {
                            throw new XmlException("Unsupported XPath (missing ]): "+part);
                        }

                        attrib=attrib.Substring(0, attrib.Length-1);
                    }
                    else
                    {
                        elName=part;
                    }

                    XmlNode next=doc.CreateElement(elName);
                    node.AppendChild(next);
                    node=next;

                    if (attrib == null)
                    {
                        continue;
                    }

                    if (!attrib.StartsWith("@"))
                    {
                        throw new XmlException("Unsupported XPath attrib (missing @): "+part);
                    }

                    attrib.Substring(1).SplitOnce("='", out String name, out String value);
                    if (String.IsNullOrEmpty(value) || !value.EndsWith("'"))
                    {
                        throw new XmlException("Unsupported XPath attrib: "+part);
                    }

                    value=value.Substring(0, value.Length-1);
                    XmlAttribute anode=doc.CreateAttribute(name);
                    anode.Value=value;
                    node.Attributes.Append(anode);
                }
            }
            return node;
        }
        
        public static void Set(this XmlDocument doc, String xpath, String value)
        {
            if (doc==null)
            {
                throw new ArgumentNullException("doc");
            }

            if (String.IsNullOrEmpty(xpath))
            {
                throw new ArgumentNullException("xpath");
            }

            XmlNodeList nodes=doc.SelectNodes(xpath);
            if (nodes.Count > 1)
            {
                throw new XmlException("Xpath '" + xpath + "' was not found multiple times!");
            }

            if (nodes.Count == 0)
            {
                CreateXPath(doc, xpath).InnerText = value;
            }
            else
            {
                nodes[0].InnerText = value;
            }
        }
    }
}