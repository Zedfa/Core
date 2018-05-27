using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Core.Cmn
{
    public static class CmnHelper
    {
        public static string GenerateJavaScriptOfSubmitForm(
            string formActionUrl,
            NameValueCollection formFields
            )
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<script language='javascript' type='text/javascript'>");
            sb.AppendLine("var dynamicForm = document.createElement('form');");
            sb.AppendLine("dynamicForm.setAttribute('method', 'POST');");
            sb.AppendLine($"dynamicForm.setAttribute('action', '{formActionUrl}');");
            sb.AppendLine("dynamicForm.setAttribute('target', '_self');");

            foreach (string key in formFields)
            {                
                sb.AppendLine($"var htmlInput{key} = document.createElement('input');");
                sb.AppendLine($"htmlInput{key}.setAttribute('name', '{key}');");                
                sb.AppendLine($"htmlInput{key}.setAttribute('value', '{formFields[key]}');");                
                sb.AppendLine($"dynamicForm.appendChild(htmlInput{key});");
            }

            sb.AppendLine("document.body.appendChild(dynamicForm);");
            sb.AppendLine("dynamicForm.submit();");
            sb.AppendLine("document.body.removeChild(dynamicForm);");
            sb.AppendLine("</script>");

            return sb.ToString();
        }

        public static T DeserializeFromXmlElement<T>(XmlElement element)
        {
            var serializer = new XmlSerializer(typeof(T));

            return (T)serializer.Deserialize(new XmlNodeReader(element));
        }

    }
}
