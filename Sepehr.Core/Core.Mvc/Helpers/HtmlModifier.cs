using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Core.Mvc.Helpers
{
    public static class HtmlModifier
    {
        public static string SanitizeId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return string.Empty;

            StringBuilder builder = new StringBuilder(id.Length);
            int index = id.IndexOf("#");
            int num2 = id.LastIndexOf("#");
            if (num2 > index)
            {
                ReplaceInvalidCharacters(id.Substring(0, index), builder);
                builder.Append(id.Substring(index, (num2 - index) + 1));
                ReplaceInvalidCharacters(id.Substring(num2 + 1), builder);
            }
            else
                ReplaceInvalidCharacters(id, builder);

            return builder.ToString();
        }

        private static void ReplaceInvalidCharacters(string part, StringBuilder builder)
        {
            for (int i = 0; i < part.Length; i++)
            {
                char c = part[i];
                if (IsValidCharacter(c))
                {
                    builder.Append(c);
                }
                else
                {
                    builder.Append(HtmlHelper.IdAttributeDotReplacement);
                }
            }
        }

        private static bool IsValidCharacter(char c)
        {
            return (((c != '?') && (c != '!')) && ((c != '#') && (c != '.')));
        }

        //-------------------------------------
        internal static string ModifyId(string Id)
        {
            StringBuilder result = new StringBuilder(Id);
            char[] ilegalSymbol = new char[] { '[', ']', '\'', '"', '.', ',', '<', '>' };
            foreach (var item in ilegalSymbol)
            {
                result.Replace(item, '_');
            }
            return result.ToString();

        }

        private static Dictionary<string, object> _DefaultHtmlAttributes = new Dictionary<string, object> { 
                                                   { "id", new Func<string,string,object>(
                                                       (styleName,id)=>
                                                   {
                                                       return ModifyId(id); 
                                                   })
                                                   },
                                                   
                                                   {"class", new Func<string,string,object>(
                                                      (styleName,cssClass)=>
                                                      {
                                                          return string.Format("{0} {1}", styleName,cssClass);
                                                      })  
                                                  //    },
                                                  
                                                  //{"autofocus", new Func<string,string,object>(
                                                  //    (styleName,id)=>
                                                  //    {
                                                  //        return string.Format(@"<script> $('#{0}').focus(); </script>" ,id  );
                                                  //    })  
                                                  // 
                                                  }
                                                  
        };


        internal static void ManageHtmlAttributes(Dictionary<string, object> attributes, string styleKindName)
        {

            if (attributes.Count > 0)
            {
                foreach (var item in attributes.ToList())
                {
                    if (_DefaultHtmlAttributes.ContainsKey(item.Key))
                    {
                        attributes[item.Key] = (_DefaultHtmlAttributes[item.Key] as Func<string, string, object>).Invoke(styleKindName, attributes[item.Key].ToString());
                    }

                }

            }

            if (!attributes.ContainsKey("class"))
            {
                attributes.Add("class", styleKindName);
            }
        }

        internal static Dictionary<string, object> ManageHtmlAttributes(string styleKindName)
        {
            Dictionary<string, object> attributes = new Dictionary<string, object>();
            ManageHtmlAttributes(attributes, styleKindName);
            return attributes;
        }

        internal static void ManageAttributesWithPermissions(Dictionary<string, object> attributes, string kendoStyle, bool disable)
        {
            if (disable)
            {
                attributes.Add("Style", "opacity:0.4");
                attributes.Add("disabled", "disabled");
            }
            HtmlModifier.ManageHtmlAttributes(attributes, disable ? string.Empty : kendoStyle);

        }

    }
}
