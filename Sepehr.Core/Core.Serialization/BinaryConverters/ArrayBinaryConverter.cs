﻿using System;
using System.IO;

namespace Core.Serialization.BinaryConverters
{
    public class ArrayBinaryConverter : BinaryConverter<Array>
    {
        public BinaryConverterBase ElementItem { get; private set; }

        public Type ElementType { get; private set; }

        public override Type ItemType
        {
            get
            {
                return typeof(Array);
            }
        }

        public override BinaryConverterBase Copy(Type type)
        {
            var binaryConverter = new ArrayBinaryConverter();
            binaryConverter.Init(type);
            binaryConverter.ElementType = binaryConverter.CurrentType.GetElementType();
            binaryConverter.ElementItem = GetBinaryConverter(binaryConverter.ElementType);
            return binaryConverter;
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
        protected override void SerializeBase(Array objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem.Length);
            foreach (var item in objectItem)
            {
                ///Remark: => ToDo In array if generic type is same for all values and we detect it, so we could use for all values by same serializeItem and it does not need to find BinaryConverter for every value.
                SerializeChildItem(ElementItem, item, writer, context);
            }
        }
    }
}