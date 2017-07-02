using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Serialization.BinaryConverters
{
    public class ListBinaryConverter : BinaryConverter<IList>
    {
        public override Type ItemType
        {
            get
            {
                return typeof(IList);
            }
        }

        public BinaryConverterBase ElementItem { get; private set; }
        public Type ElementType { get; private set; }

        protected override BinaryConverter<IList> CopyBase()
        {
            return new ListBinaryConverter();
        }

        protected override void BeforDeserialize(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            if (ElementItem == null)
            {
                ElementType = objectType.GetGenericArguments().First();
                CurrentType = objectType;
                ElementItem = GetBinaryConverter(ElementType).Copy();
            }
        }
        protected override void BeforSerialize(IList obj, BinaryWriter writer, SerializationContext context)
        {
            if (ElementItem == null)
            {
                var objType = obj.GetType();
                ElementType = objType.GetGenericArguments().First();
                CurrentType = objType;
                ElementItem = GetBinaryConverter(ElementType).Copy();
            }

        }
        protected override IList DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            IList result;
            result = (IList)Activator.CreateInstance(objectType);
            var count = reader.ReadInt32();
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                   // var aaa = typeof(IList).IsAssignableFrom(ElementType);
                    var value = ElementItem.Deserialize(reader, ElementType, context);
                    result.Add(value);
                }
            }

            return result;
        }

        protected override void SerializeBase(IList objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem.Count);
            foreach (var item in objectItem)
            {
                ///Remark: => ToDo In array if generic type is same for all valuse and we detect it, so we could use for all values by same serializeItem and it does not need to find BinaryConverter for every value.
                SerializeChildItem(ElementItem, item, writer, context);
            }
        }
    }
}
