using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Core.Mvc.Extensions
{
    public static class StringExt
    {
        public static string ScriptSerializer(this string script)
        {
            var str = script.Replace("\r\n", "").Replace("</script>", @"<\/script>").Replace("#", "\\\\\\#");
            return new JavaScriptSerializer().Serialize(str);
        }

        public static int ConvertToInt(this string toBeConventor)
        {
            int result = 0;
            if (int.TryParse(toBeConventor, out result))
                return result;
            return result;
        }

        public static decimal ConvertToDecimal(this string toBeConventor)
        {
            decimal result = 0;
            if (decimal.TryParse(toBeConventor, out result))
                return result;
            return result;
        }
    }
}
