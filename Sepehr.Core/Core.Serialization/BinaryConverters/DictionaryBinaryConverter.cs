using System;
using System.Collections;
using System.IO;
using System.Linq;

namespace Core.Serialization.BinaryConverters
{
    public class DictionaryBinaryConverter : BinaryConverter<IDictionary>
    {
        private BinaryConverterBase _keyItem;
        private BinaryConverterBase _valueItem;

        public ObjectMetaData EntityMetaData { get; private set; }
        public BinaryConverterBase KeyItem
        {
            get
            {
                if (_keyItem == null)
                    _keyItem = GetBinaryConverter(KeyType);
                return _keyItem;
            }
        }
        public Type KeyType { get; private set; }
        public BinaryConverterBase ValueItem
        {
            get
            {
                if (_valueItem == null)
                    _valueItem = GetBinaryConverter(ValueType);
                return _valueItem;
            }
        }
        public Type ValueType { get; private set; }

        public override BinaryConverterBase Copy(Type type)
        {
            var binaryConverter = new DictionaryBinaryConverter();
            binaryConverter.Init(type);
            binaryConverter.KeyType = binaryConverter.CurrentType.GetGenericArguments().First();
            binaryConverter.ValueType = binaryConverter.CurrentType.GetGenericArguments().Last();
            binaryConverter.EntityMetaData = ObjectMetaData.GetEntityMetaData(type);
            return binaryConverter;
        }

        public override IDictionary CreateInstanceBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            var obj = (IDictionary)EntityMetaData.ReflectionEmitPropertyAccessor.EmittedObjectInstanceCreator();
            context.CurrentReferenceTypeObject = obj;
            return obj;
        }
        protected override IDictionary DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            IDictionary result = (IDictionary)context.CurrentReferenceTypeObject;
            var count = reader.ReadInt32();
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var key = KeyItem.Deserialize(reader, KeyType, context);
                    var value = ValueItem.Deserialize(reader, ValueType, context);
                    result.Add(key, value);
                }
            }

            return result;
        }

        protected override void SerializeBase(IDictionary objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem.Count);
            foreach (var item in objectItem)
            {
                ///Remark: => ToDo In array if generic type is same for all valuse and we detect it, so we could use for all values by same serializeItem and it does not need to find BinaryConverter for every value.
                SerializeChildItem(KeyItem, ((dynamic)item).Key, writer, context);
                SerializeChildItem(ValueItem, ((dynamic)item).Value, writer, context);
            }
        }
    }
}