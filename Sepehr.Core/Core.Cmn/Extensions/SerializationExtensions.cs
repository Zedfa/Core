using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Core.Cmn.Extensions
{
    public static class SerializationExtensions
    {

        public static XmlSerializerNamespaces GetNamespaces()
        {

            XmlSerializerNamespaces ns;
            ns = new XmlSerializerNamespaces();
            ns.Add("xs", "http://www.w3.org/2001/XMLSchema");
            ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            return ns;

        }

        public static string TargetNamespace => "http://www.w3.org/2001/XMLSchema";

        public static string SerializeObjectIntoXML<T>(object toConvertToXmlObj)
        {
            return toConvertToXmlObj.SerializeObjectToXML();
        }
        public static string SerializeObjectToXML(this object toConvertToXmlObj)
        {
            var ser = new XmlSerializer(toConvertToXmlObj.GetType(), SerializationExtensions.TargetNamespace);
            var memStream = new MemoryStream();
            var xmlWriter = new XmlTextWriter(memStream, Encoding.UTF8);
            xmlWriter.Namespaces = true;
            ser.Serialize(xmlWriter, toConvertToXmlObj, GetNamespaces());
            xmlWriter.Close();
            memStream.Close();
            string xml;
            xml = Encoding.UTF8.GetString(memStream.GetBuffer());
            xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
            xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
            return xml;
        }

        public static string SerializetoJSON(this object toConvertToJsonStr)
        {
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(toConvertToJsonStr);
            return str;
        }
        public static T DeSerializeJSONToObject<T>(this string jsonString)
        {
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);
            return obj;

        }

        public static byte[] SerializetoBinary(this object toConvertToJsonStr)
        {
            var binary = Core.Serialization.BinaryConverter.Serialize(toConvertToJsonStr);
            return binary;
        }
        public static T DeSerializeBinaryToObject<T>(this byte[] binary)
        {
            var obj = Core.Serialization.BinaryConverter.Deserialize<T>(binary);
            return obj;

        }
        public static object DeSerializeXMLIntoObject<T>(string xmlConvertedString)
        {
            var ser = new XmlSerializer(typeof(T));
            var stringReader = new StringReader(xmlConvertedString);
            var xmlReader = new XmlTextReader(stringReader);
            object obj = ser.Deserialize(xmlReader);
            xmlReader.Close();
            stringReader.Close();

            return obj;
        }

        public static T DeSerializeXMLToObject<T>(this string xmlConvertedString)
        {
            var ser = new XmlSerializer(typeof(T));
            var stringReader = new StringReader(xmlConvertedString);
            var xmlReader = new XmlTextReader(stringReader);
            object obj = ser.Deserialize(xmlReader);
            xmlReader.Close();
            stringReader.Close();

            return (T)obj;
        }

        public static string StoreViewElementIntoXML(object viewElementComplexTypeObj)
        {
            var xs = new XmlSerializer(viewElementComplexTypeObj.GetType());

            var sw = new StringWriter();
            xs.Serialize(sw, viewElementComplexTypeObj);
            var serializedString = sw.ToString();
            using (var db = new SqlConnection(@"Data Source=Core-TD-24\SQL2012ENT;Initial Catalog=RequestSystem;User Id=sa; Password=123456"))
            {
                db.Open();
                try
                {
                    using (var cmd = new SqlCommand("Update  ViewElements set XMLViewData=@Xml where Id=3 ", db))
                    {
                        cmd.Parameters.AddWithValue("@Xml", serializedString);
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch
                        {

                        }
                    }
                }
                finally { db.Close(); }
            }
            return serializedString;
        }


    }
}
