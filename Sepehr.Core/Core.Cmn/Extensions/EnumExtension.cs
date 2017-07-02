using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Extensions
{
    public static class EnumExt
    {
        public static string GetDescriptionEnum(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                        as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static T FindByDescriptionEnum<T>(this string description) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            var enumType = typeof(T);
            foreach (string value in enumType.GetEnumNames())
            {
                FieldInfo field = enumType.GetField(value);
                DescriptionAttribute attribute
                        = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                            as DescriptionAttribute;
                if (attribute == null)
                    throw new ArgumentException("All Enum items must have DescriptionAttribute!");
                var strDescription = attribute.Description;
                if (description.ToLower() == strDescription.ToLower())
                    return (T)Enum.Parse(enumType, value);
            }

            throw new KeyNotFoundException("description not exist in this enum.");
        }

        public static bool TryFindByDescriptionEnum<T>(this string description, out T result) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            var enumType = typeof(T);
            foreach (string value in enumType.GetEnumNames())
            {
                FieldInfo field = enumType.GetField(value);
                DescriptionAttribute attribute
                        = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                            as DescriptionAttribute;
                if (attribute == null)
                    throw new ArgumentException("All Enum items must have DescriptionAttribute!");
                var strDescription = attribute.Description;
                if (description.ToLower() == strDescription.ToLower())
                {
                    result = (T)Enum.Parse(enumType, value);
                    return true;
                }
            }

            result = default(T);
            return false;
        }

        public static Array GetIntegerEquivalentOfEnumDescription(this Enum enm, Type enumType)
        {
            return System.Enum.GetValues(enumType);
        }

        public static int[] GetEnumIntegerEquivalents(this Type enumType, string[] enumStringConstants)
        {
            var enumConstantsLength = enumStringConstants.Length;
            var intEquivalents = new int[enumConstantsLength];

            for (var i = 0; i < enumConstantsLength; i++)
            {
                intEquivalents[i] = (int)Enum.Parse(enumType, enumStringConstants[i]);
            }

            return intEquivalents;
        }

        public static int[] GetEnumIntegerEquivalents(this Type enumType, Array enumStringConstants)
        {
            var enumConstantsLength = enumStringConstants.Length;
            var intEquivalents = new int[enumConstantsLength];

            for (var i = 0; i < enumConstantsLength; i++)
            {
                intEquivalents[i] = (int)Enum.Parse(enumType, enumStringConstants.GetValue(i).ToString());
            }

            return intEquivalents;
        }

        public static Dictionary<int, string> GetEnumKeyValuePairEquivalents(this Type enumType)
        {
            Dictionary<int, string> enumKeyValDic = null;

            var enumStrConstants = Enum.GetNames(enumType);
            var fieldInfos = enumType.GetFields();
            if (fieldInfos.Length > 1)
            {
                enumKeyValDic = new Dictionary<int, string>();
                for (var i = 1; i < fieldInfos.Length; i++)
                {
                    var fieldDescriptionAttr = Attribute.GetCustomAttribute(fieldInfos[i], typeof(DescriptionAttribute)) as DescriptionAttribute;
                    enumKeyValDic.Add((int)Enum.Parse(enumType, fieldInfos[i].Name), fieldDescriptionAttr.Description);
                }
            }
            return enumKeyValDic;
        }
    }

}
