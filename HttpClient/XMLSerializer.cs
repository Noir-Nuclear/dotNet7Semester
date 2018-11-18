using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace HttpClient
{
    class XMLSerializer : Serializer
    {
        public string serialize<T>(T input)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(T));
            String result;
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, input);
                result = Encoding.UTF8.GetString(stream.ToArray());
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = false;
                doc.LoadXml(result);
                doc.ChildNodes.Item(1).Attributes.RemoveAll();
                doc.RemoveChild(doc.ChildNodes.Item(0));
                return doc.InnerXml;
            }
        }

        public T deserialize<T>(string serializableStr)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringReader stringReader = new StringReader(serializableStr);
            return (T)serializer.Deserialize(stringReader);
        }
    }
}
