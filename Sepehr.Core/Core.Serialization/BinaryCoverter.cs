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
            lock (obj)
            {
                Core.Serialization.Utility.Debuging.WriteLine("Start Binary Deserializaing ...");
                using (var memoryStream = new MemoryStream(binary))
                {
                    using (var reader = new BinaryReaderCore(memoryStream))
                    {
                        BinaryConverterBase serializationPlan;
                        var context = new DeserializationContext();
                        object obj;
                        serializationPlan = BinaryConverterBase.GetBinaryConverter(objectType);
                        obj = serializationPlan.Deserialize(reader, objectType, context);
                        return obj;
                    }
                }
            }
        }

        public static T Deserialize<T>(byte[] binary)
        {
            return (T)Deserialize(binary, typeof(T));
        }

        public static byte[] Serialize(object obj)
        {
            System.Diagnostics.Debug.WriteLine("Start Binary Serializaing ...");
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new BinaryWriterCore(memoryStream))
                {
                    var type = obj.GetType();
                    BinaryConverterBase serializationPlan;
                    var context = new SerializationContext();
                    serializationPlan = BinaryConverterBase.GetBinaryConverter(type);
                    serializationPlan.Serialize(obj, writer, context);
                    return memoryStream.ToArray();
                }
            }
        }
    }
}