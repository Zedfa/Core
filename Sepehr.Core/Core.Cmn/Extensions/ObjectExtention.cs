using Core.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace Core.Cmn.Extensions
{
    public static class ObjectExtention
    {
        public static T DeepCopy<T>(this T obj, bool parameterlessConstructor = false) where T : class, new()
        {
            object clonedObj = null;
            if (!parameterlessConstructor)
            {
                var binary = BinaryConverter.Serialize(obj);
                clonedObj = BinaryConverter.Deserialize(binary, obj.GetType());
            }
            else
            {
                using (var ms = new MemoryStream())
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(ms, obj);
                    ms.Position = 0;
                    clonedObj = formatter.Deserialize(ms);
                }

            }
            return (T)clonedObj;

        }


        public static IDictionary<string, object> ToDictionary(this object obj)
        {
            var dictionary = new Dictionary<string, object>();
            if (obj != null)
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj))
                {
                    dictionary.Add(property.Name.Replace("_", "-"), property.GetValue(obj));
                }
            }
            return dictionary;

        }


        public static TValue GetValue<TObj, TValue>(this TObj obj, Func<TObj, TValue> member, TValue defaultValueOnNull = default(TValue))
        {
            if (member == null)
                throw new ArgumentNullException("member");

            if (obj == null)
                throw new ArgumentNullException("obj");

            try
            {
                return member(obj);
            }
            catch (NullReferenceException)
            {
                return defaultValueOnNull;
            }
            catch (IndexOutOfRangeException)
            {
                return defaultValueOnNull;
            }
        }


    }
}
