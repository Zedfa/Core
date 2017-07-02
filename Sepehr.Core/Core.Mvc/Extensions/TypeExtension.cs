using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Extensions
{
   internal static class TypeExtension
    {
       internal static string ToJavaScriptType(this Type type)
       {
           if (type == null)
           {
               return "Object";
           }

           if (type == typeof(char) || type == typeof(char?))
           {
               return "String";
           }

           if (IsNumericType(type))
           {
               return "Number";
           }

           if (type == typeof(DateTime) || type == typeof(DateTime?))
           {
               return "Date";
           }

           if (type == typeof(string))
           {
               return "String";
           }

           if (type == typeof(bool) || type == typeof(bool?))
           {
               return "Boolean";
           }

           if (type.GetNonNullableType().IsEnum)
           {
               return "Number";
           }

           if (type.GetNonNullableType() == typeof(Guid))
           {
               return "String";
           }

           return "Object";
       }

       internal static bool IsNumericType(this Type type)
       {
           return GetNumericTypeKind(type) != 0;
       }

       internal static int GetNumericTypeKind(this Type type)
       {
           if (type == null)
           {
               return 0;
           }

           type = GetNonNullableType(type);

           if (type.IsEnum)
           {
               return 0;
           }

           switch (Type.GetTypeCode(type))
           {
               case TypeCode.Char:
               case TypeCode.Single:
               case TypeCode.Double:
               case TypeCode.Decimal:
                   return 1;
               case TypeCode.SByte:
               case TypeCode.Int16:
               case TypeCode.Int32:
               case TypeCode.Int64:
                   return 2;
               case TypeCode.Byte:
               case TypeCode.UInt16:
               case TypeCode.UInt32:
               case TypeCode.UInt64:
                   return 3;
               default:
                   return 0;
           }
       }

       internal static Type GetNonNullableType(this Type type)
       {
           return IsNullableType(type) ? type.GetGenericArguments()[0] : type;
       }

       internal static bool IsNullableType(this Type type)
       {
           return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
       }
    }
}
