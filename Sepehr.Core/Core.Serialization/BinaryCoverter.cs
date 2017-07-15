using Core.Serialization.BinaryConverters;
using System;
using System.IO;

namespace Core.Serialization
{
    public static class BinaryConverter
    {
        public static object obj = new object();
        public static object Deserialize(byte[] binary, Type objectType)
        {
            //   Core.Serialization.Utility.Debuging.WriteLine($"Start Binary Deserializaing at {DateTime.Now} ...");
            //  Console.WriteLine($"Start Binary Deserializaing at {DateTime.Now} ...");
            using (var memoryStream = new MemoryStream(binary))
            {
                using (var reader = new BinaryReaderCore(memoryStream))
                {
                    BinaryConverterBase serializationPlan;
                    var context = new DeserializationContext();
                    object obj;
                    serializationPlan = BinaryConverterBase.GetBinaryConverter(objectType);
                    obj = serializationPlan.Deserialize(reader, objectType, context);
                    //   Core.Serialization.Utility.Debuging.WriteLine($"End Binary Deserializaing at {DateTime.Now} ...");
                    //    Console.WriteLine($"End Binary Deserializaing at {DateTime.Now} ...");
                    return obj;
                }
            }
        }

        public static T Deserialize<T>(byte[] binary)
        {
            return (T)Deserialize(binary, typeof(T));
        }

        public static byte[] Serialize(object obj)
        {
            //  Core.Serialization.Utility.Debuging.WriteLine($"Start Binary Serializaing at {DateTime.Now} ...");
            //   Console.WriteLine($"Start Binary Serializaing at {DateTime.Now} ...");
            var context = new SerializationContext() { IsFirstItem = true };
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new BinaryWriterCore(memoryStream))
                {
                    if (obj == null)
                    {
                        new NullBinaryConverter().Serialize(obj, writer, context);
                    }
                    else
                    {
                        var type = obj.GetType();
                        BinaryConverterBase serializationPlan;

                        serializationPlan = BinaryConverterBase.GetBinaryConverter(type);
                        serializationPlan.Serialize(obj, writer, context);
                        //        Core.Serialization.Utility.Debuging.WriteLine($"End Binary Serializaing at {DateTime.Now} ...");
                        //         Console.WriteLine($"End Binary Serializaing at {DateTime.Now} ...");
                    }
                    return memoryStream.ToArray();
                }
            }
        }
    }
}