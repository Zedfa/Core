using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace Core.Cmn.Extensions
{
    public static class ObjectExtention
    {
        public static T DeepCopy<T>(this T obj) where T : class , new()
        {
            object clonedObj = null;
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                clonedObj = formatter.Deserialize(ms);
            }
            return (T)clonedObj;
        }
        // //public static T DeepCopy<T>(this T obj) where T : class , new()
        // //{
        // //    var newTypeInstance = Activator.CreateInstance<T>();
        // //    var fields = newTypeInstance.GetType().GetProperties();//.GetFields();
        // //    foreach (var field in fields)
        // //    {
        // //        //if (field.GetType().GetMethod("DeepCopy") != null)
        // //        //{
        // //        //    var fieldType = field.PropertyType;
        // //        //    var fObj = field.GetType().GetMethod("DeepCopy").Invoke(field, null) as PropertyInfo;
        // //        //    //var value = field.GetValue(fObj);
        // //        //    field.SetValue(field, fObj);
        // //        //}
        // //        //else
        // //        //{
        // //        var value = field.GetValue(obj);
        // //        field.SetValue(newTypeInstance, value);
        // //        //}
        // //    }
        // //    return newTypeInstance;
        // //}
        ///// <summary>
        // /// Implement deep clone the object using serialization.
        // /// </summary>
        // /// <param name="source">Entity Object needs to be cloned </param>
        // /// <returns>The Cloned object</returns>
        // public static T DeepCopy<T>(this T source) where T : class , new()
        // {
        //     //var ser = new DataContractSerializer(typeof(T));
        //     //using (var stream = new MemoryStream())
        //     //{
        //     //    ser.WriteObject(stream, source);
        //     //    stream.Seek(0, SeekOrigin.Begin);
        //     //    return (T)ser.ReadObject(stream);
        //     //}
        //     return DeserializeXmlString<T>(SerializeToXmlString<T>(source));
        // }
        // /// <summary>
        // /// Serializes an object to Xml as a string.
        // /// </summary>
        // /// <typeparam name="T">Datatype T.</typeparam>
        // /// <param name="ToSerialize">Object of type T to be serialized.</param>
        // /// <returns>Xml string of serialized type T object.</returns>
        // public static string SerializeToXmlString<T>(T ToSerialize)
        // {
        //     string xmlstream = String.Empty;

        //     using (MemoryStream memstream = new MemoryStream())
        //     {
        //         XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        //         XmlTextWriter xmlWriter = new XmlTextWriter(memstream, Encoding.UTF8);

        //         xmlSerializer.Serialize(xmlWriter, ToSerialize);
        //         xmlstream = UTF8ByteArrayToString(((MemoryStream)xmlWriter.BaseStream).ToArray());
        //     }

        //     return xmlstream;
        // }

        // /// <summary>
        // /// Deserializes Xml string of type T.
        // /// </summary>
        // /// <typeparam name="T">Datatype T.</typeparam>
        // /// <param name="XmlString">Input Xml string from which to read.</param>
        // /// <returns>Returns rehydrated object of type T.</returns>
        // public static T DeserializeXmlString<T>(string XmlString)
        // {
        //     T tempObject = default(T);

        //     using (MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(XmlString)))
        //     {
        //         XmlSerializer xs = new XmlSerializer(typeof(T));
        //         XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

        //         tempObject = (T)xs.Deserialize(memoryStream);
        //     }

        //     return tempObject;
        // }

        // // Convert Array to String
        // public static String UTF8ByteArrayToString(Byte[] ArrBytes)
        // { return new UTF8Encoding().GetString(ArrBytes); }
        // // Convert String to Array
        // public static Byte[] StringToUTF8ByteArray(String XmlString)
        // { return new UTF8Encoding().GetBytes(XmlString); }

        // /// <summary>
        // ///  Clear the entity key of the object and all related child objects 
        // /// </summary>
        // /// <param name="source">Entity Object needs to be cleared</param>
        // /// <param name="bCheckHierarchy">
        // ///  Determine whether to clear all the child object
        // /// </param>
        // /// <returns></returns>
        // public static EntityObject ClearEntityReference(this EntityObject source,
        //     bool bCheckHierarchy)
        // {
        //     return source.ClearEntityObject(bCheckHierarchy);
        // }

        // /// <summary>
        // ///  Clear the entity of object and all related child objects 
        // /// </summary>
        // /// <param name="source">Entity Object needs to be cleared</param>
        // /// <param name="bCheckHierarchy">
        // ///  Determine whether to clear all the child object
        // /// </param>
        // /// <returns></returns>
        // private static T ClearEntityObject<T>(this  T source, bool bCheckHierarchy)
        //     where T : class
        // {
        //     // Throw if passed object is null
        //     if (source == null)
        //     {
        //         throw new Exception("Null Object cannot be cloned");
        //     }

        //     // Get the Type of passed object 
        //     Type tObj = source.GetType();

        //     // Check passed object's entity key Attribute 
        //     if (tObj.GetProperty("EntityKey") != null)
        //     {
        //         tObj.GetProperty("EntityKey").SetValue(source, null, null);
        //     }

        //     // bCheckHierarchy is used to check and clear child object releation keys 
        //     if (!bCheckHierarchy)
        //     {
        //         return source;
        //     }

        //     // Clearing the Entity's related Child Objects 
        //     List<PropertyInfo> propertyList = (from a in source.GetType().GetProperties()
        //                                        where a.PropertyType.Name.Equals
        //                                        ("ENTITYCOLLECTION`1",
        //                                        StringComparison.OrdinalIgnoreCase)
        //                                        select a).ToList();

        //     // Loop the list of Child Object and Clear the Entity Reference 
        //     foreach (PropertyInfo prop in propertyList)
        //     {
        //         var keys = (IEnumerable)tObj.GetProperty(prop.Name)
        //             .GetValue(source, null);

        //         foreach (object key in keys)
        //         {
        //             // Clearing Entity Reference from Parent Object 
        //             var childProp = (from a in key.GetType().GetProperties()
        //                              where a.PropertyType.Name.Equals
        //                              ("EntityReference`1",
        //                              StringComparison.OrdinalIgnoreCase)
        //                              select a).SingleOrDefault();

        //             if (childProp != null)
        //             {
        //                 childProp.GetValue(key, null).ClearEntityObject(false);
        //             }

        //             // Recursively clearing the the Entity Reference from Child object 
        //             key.ClearEntityObject(true);
        //         }
        //     }
        //     return source;
        // }

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
