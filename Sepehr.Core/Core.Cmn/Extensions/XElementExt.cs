using System.Xml;
using System.Xml.Linq;

namespace Core.Cmn.Extensions
{
    public static class XElementExt
    {
        public static XmlElement ToXmlElement(this XElement el)
        {
            var doc = new XmlDocument();
            doc.Load(el.CreateReader());
            return doc.DocumentElement;

        }
    }
}
