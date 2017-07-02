using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Serialization.BinaryConverters
{
    public class DictionaryBinaryConverter : BinaryConverter<IDictionary>
    {
        public override Type ItemType
        {
            get
            {
                return typeof(IDictionary);
            }
        }
        public BinaryConverterBase KeyItem { get; private set; }
        public BinaryConverterBase ValueItem { get; private set; }
        public Type KeyType { get; private set; }
        public Type ValueType { get; private set; }

        protected override BinaryConverter<IDictionary> CopyBase()
        {
            return new DictionaryBinaryConverter();
        }
        protected override void BeforDeserialize(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            if (KeyItem == null)
            {
                CurrentType = objectType;
                KeyType = objectType.GetGenericArguments().First();
                ValueType = objectType.GetGenericArguments().Last();
                KeyItem = GetBinaryConverter(KeyType).Copy();
                ValueItem = GetBinaryConverter(ValueType).Copy();
            }
        }
        protected override void BeforSerialize(IDictionary obj, BinaryWriter writer, SerializationContext context)
        {
            if (KeyItem == null)
            {
                var objectType = obj.GetType();
                CurrentType = objectType;
                KeyType = objectType.GetGenericArguments().First();
                ValueType = objectType.GetGenericArguments().Last();
                KeyItem = GetBinaryConverter(KeyType).Copy();
                ValueItem = GetBinaryConverter(ValueType).Copy();
            }
        }
        protected override IDictionary DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            IDictionary result;
            result = (IDictionary)Activator.CreateInstance(objectType);
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
