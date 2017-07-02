using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Extensions
{
    public static class AttributeExtensions
    {
        public static TValue GetAttributeValue<TAttribute, TValue>(
            this Type type,
            Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute
        {
            var att = type.GetCustomAttributes(
                typeof(TAttribute), true
            ).FirstOrDefault() as TAttribute;
            if (att != null)
            {
                return valueSelector(att);
            }
            return default(TValue);
        }
        public static TValue GetAttributeValue<TAttribute, TValue>(
   this MemberInfo member,
   Func<TAttribute, TValue> valueSelector)
   where TAttribute : Attribute
        {
            var att = member.GetCustomAttributes(
                typeof(TAttribute), true
            ).FirstOrDefault() as TAttribute;
            if (att != null)
            {
                return valueSelector(att);
            }
            return default(TValue);
        }
        /// <summary>
        /// Return true for all Simple type such as int, int?, string,decimal,decimal? and ...
        /// </summary>
        /// <param name="type">a type for detemining "is it SimpleType?".</param>
        /// <returns>Return true for all Simple type such as int, int?, string,decimal,decimal? and ...</returns>
        public static bool IsSimple(this Type type)
        {
            return Core.Serialization.TypeExtensions.IsSimple(type);
        }
    }
}
