using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Serialization
{
    public static class TypeExtensions
    {

        /// <summary>
        /// Return true for all Simple type such as int, int?, string,decimal,decimal? and ...
        /// </summary>
        /// <param name="type">a type for detemining "is it SimpleType?".</param>
        /// <returns>Return true for all Simple type such as int, int?, string,decimal,decimal? and ...</returns>
        public static bool IsSimple(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimple(type.GetGenericArguments()[0]);
            }
            return type.IsPrimitive
              || type.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal))
              || (typeof(Enum).IsAssignableFrom(type) && typeof(Enum) != type);
        }
        public static bool IsNullable(this Type type)
        {
            if (((Nullable.GetUnderlyingType(type) != null) || !type.IsValueType) && !((typeof(Enum)).IsAssignableFrom(type) && typeof(Enum) != type))
            {
                return true;
            }
            return false;
        }

        public static object GetDefaultValue(this Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
        public static Boolean IsAnonymousType(this Type type)
        {
            Boolean hasCompilerGeneratedAttribute = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Count() > 0;
            Boolean nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
            Boolean isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;
            return isAnonymousType;
        }
    }
}
