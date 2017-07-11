using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Serialization.BinaryConverters
{
    public class HashSetBinaryConverter<T> : BinaryConverter<HashSet<T>>
    {
        public BinaryConverterBase ElementItem { get; private set; }
        public Type ElementType { get; private set; }
        public override BinaryConverterBase Copy(Type type)
        {
            var binaryConverter = new HashSetBinaryConverter<T>();
            binaryConverter.Init(type);
            binaryConverter.ElementType = type.GetGenericArguments().First();
            binaryConverter.ElementItem = GetBinaryConverter(binaryConverter.ElementType);
            return binaryConverter;
        }
        protected override HashSet<T> DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            HashSet<T> result;
            result = (HashSet<T>)Activator.CreateInstance(objectType);
            var count = reader.ReadInt32();
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var value = ElementItem.Deserialize(reader, ElementType, context);
                    result.Add((T)value);
                }
            }

            return result;
        }

        protected override void SerializeBase(HashSet<T> objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem.Count);
            foreach (var item in objectItem)
            {
                SerializeChildItem(ElementItem, item, writer, context);
            }
        }
    }
}
