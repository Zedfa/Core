using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Core.Serialization.BinaryConverters
{
    public class ArrayBinaryConverter : BinaryConverter<Array>
    {
        public override Type ItemType
        {
            get
            {
                return typeof(Array);
            }
        }
        public BinaryConverterBase ElementItem { get; private set; }
        public Type ElementType { get; private set; }
        protected override BinaryConverter<Array> CopyBase()
        {
            return new ArrayBinaryConverter();
        }
        protected override Array DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            Array result;
            var count = reader.ReadInt32();
            result = Activator.CreateInstance(objectType, count) as Array;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var value = ElementItem.Deserialize(reader, ElementType, context);
                    result.SetValue(value, i);
                }
            }
            return result;
        }
        protected override void BeforDeserialize(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            if (ElementItem == null)
            {
                ElementType = objectType.GetElementType();
                CurrentType = objectType;
                ElementItem = GetBinaryConverter(ElementType).Copy();
            }
        }
        protected override void BeforSerialize(Array obj, BinaryWriter writer, SerializationContext context)
        {
            if (ElementItem == null)
            {
                var objType = obj.GetType();
                ElementType = objType.GetElementType();
                CurrentType = objType;
                ElementItem = GetBinaryConverter(ElementType).Copy();
            }
        }
        protected override void SerializeBase(Array objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem.Length);
            foreach (var item in objectItem)
            {
                ///Remark: => ToDo In array if generic type is same for all valuse and we detect it, so we could use for all values by same serializeItem and it does not need to find BinaryConverter for every value.
                SerializeChildItem(ElementItem, item, writer, context);
            }
        }
    }
}

