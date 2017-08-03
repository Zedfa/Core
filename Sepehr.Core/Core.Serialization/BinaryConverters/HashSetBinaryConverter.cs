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
        private BinaryConverterBase _elementItem;
        public BinaryConverterBase ElementItem
        {
            get
            {
                if (_elementItem == null)
                    _elementItem = GetBinaryConverter(ElementType);
                return _elementItem;
            }
        }
        public Type ElementType { get; private set; }
        public ObjectMetaData EntityMetaData { get; private set; }

        public override BinaryConverterBase Copy(Type type)
        {
            var binaryConverter = new HashSetBinaryConverter<T>();
            binaryConverter.Init(type);
            binaryConverter.ElementType = type.GetGenericArguments().First();
            binaryConverter.EntityMetaData = ObjectMetaData.GetEntityMetaData(type);
            return binaryConverter;
        }

        public override HashSet<T> CreateInstanceBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            var obj = EntityMetaData.ReflectionEmitPropertyAccessor.EmittedObjectInstanceCreator();
            context.CurrentReferenceTypeObject = obj;
            return (HashSet<T>)obj;
        }
        protected override HashSet<T> DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            var result = (HashSet<T>)context.CurrentReferenceTypeObject;
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
