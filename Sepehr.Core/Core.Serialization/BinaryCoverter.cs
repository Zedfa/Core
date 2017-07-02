using Core.Serialization.BinaryConverters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Serialization
{
    public static class BinaryConverter
    {
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
                    if (!SerializationPlan.SerializationPlan.TryGetBinaryConverter(type, out serializationPlan))
                    {
                        serializationPlan = BinaryConverterBase.GetBinaryConverter(type).Copy();
                        serializationPlan.Serialize(obj, writer, context);
                        SerializationPlan.SerializationPlan.SetSerializePlan(type, serializationPlan);
                    }
                    else
                    {
                        serializationPlan.Serialize(obj, writer, context);
                    }


                    return memoryStream.ToArray();
                }
            }
        }

        public static object Deserialize(byte[] binary, Type objectType)
        {
            Core.Serialization.Utility.Debuging.WriteLine("Start Binary Deserializaing ...");
            using (var memoryStream = new MemoryStream(binary))
            {
                using (var reader = new BinaryReaderCore(memoryStream))
                {
                    BinaryConverterBase serializationPlan;
                    var context = new DeserializationContext();
                    object obj;
                    if (!SerializationPlan.SerializationPlan.TryGetBinaryConverter(objectType, out serializationPlan))
                    {
                        serializationPlan = BinaryConverterBase.GetBinaryConverter(objectType).Copy();
                        obj = serializationPlan.Deserialize(reader, objectType, context);
                        SerializationPlan.SerializationPlan.SetSerializePlan(objectType, serializationPlan);
                    }
                    else
                    {
                        obj = serializationPlan.Deserialize(reader, objectType, context);
                    }

                    return obj;
                }
            }
        }
        public static T Deserialize<T>(byte[] binary)
        {
            return (T)Deserialize(binary, typeof(T));
        }
    }
}
