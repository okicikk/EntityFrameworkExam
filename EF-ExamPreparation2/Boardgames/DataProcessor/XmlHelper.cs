using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

public static class XmlHelper
{
    public static T DeserializeFromFile<T>(string filePath)
    {
        try
        {
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stream);
            }
        }
        catch (Exception ex)
        {
            // Handle or log the exception as needed
            throw new InvalidOperationException("Failed to deserialize XML from file", ex);
        }
    }

    public static void SerializeToFile<T>(T obj, string filePath, Encoding encoding = null)
    {
        try
        {
            encoding = encoding ?? Encoding.UTF8;
            using (var writer = new StreamWriter(filePath, false, encoding))
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, obj);
            }
        }
        catch (Exception ex)
        {
            // Handle or log the exception as needed
            throw new InvalidOperationException("Failed to serialize XML to file", ex);
        }
    }

    public static T DeserializeFromString<T>(string xmlString,string root)
    {
        try
        {
            using (var reader = new StringReader(xmlString))
            {
                var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(root));
                return (T)serializer.Deserialize(reader);
            }
        }
        catch (Exception ex)
        {
            // Handle or log the exception as needed
            throw new InvalidOperationException("Failed to deserialize XML from string", ex);
        }
    }

    public static string SerializeToString<T>(T obj, string root)
    {
        try
        {
            using (var stringWriter = new StringWriter())
            {
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add("","");

                var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(root));

                serializer.Serialize(stringWriter, obj,namespaces);
                return stringWriter.ToString();
            }
        }
        catch (Exception ex)
        {
            // Handle or log the exception as needed
            throw new InvalidOperationException("Failed to serialize XML to string", ex);
        }
    }

    private class StringWriterWithEncoding : StringWriter
    {
        private readonly Encoding encoding;

        public StringWriterWithEncoding(Encoding encoding)
        {
            this.encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
        }

        public override Encoding Encoding => encoding;
    }
}
