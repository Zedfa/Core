using System;
using System.IO;
using System.Linq;

namespace Core.Serialization.BinaryConverters
{
    public class UserDefinedTypeBinaryConverter : BinaryConverter<object>
    {
        public ObjectMetaData EntityMetaData { get; private set; }

        public Type ObjectType { get; private set; }

        public object UserDefinedObject { get; private set; }

        protected BinaryConverterBase[] BinaryConverters { get; private set; }

        public override BinaryConverterBase Copy(Type type)
        {
            var binaryConverter = new UserDefinedTypeBinaryConverter();
            binaryConverter.Init(type);
            binaryConverter.ObjectType = type;
            binaryConverter.EntityMetaData = ObjectMetaData.GetEntityMetaData(type);
            binaryConverter.BinaryConverters = binaryConverter.EntityMetaData.BinaryConverterList;
            return binaryConverter;
        }

        protected override object DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            object result;
            result = EntityMetaData.ReflectionEmitPropertyAccessor.EmittedObjectInstanceCreator();
            var count = EntityMetaData.WritableProperties.Count();
            var entityMetaData = EntityMetaData;
            for (int i = 0; i < count; i++)
            {
                if (entityMetaData.IsSerializablePropertyByIndexList[i])
                {
                    var type = entityMetaData.WritablePropertyList[i].PropertyType;
                    EntityMetaData.ReflectionEmitPropertyAccessor.EmittedPropertySetters[i](result, BinaryConverters[i].Deserialize(reader, type, context));
                }
            }
            return result;
        }

        protected override void SerializeBase(object objectItem, BinaryWriter writer, SerializationContext context)
        {
            for (int i = 0; i < EntityMetaData.WritablePropertyList.Count; i++)
            {
                if (EntityMetaData.IsSerializablePropertyByIndexList[i])
                {
                    var currentObjToSerialize = EntityMetaData.ReflectionEmitPropertyAccessor.EmittedPropertyGetters[i](objectItem);
                    BinaryConverters[i].Serialize(currentObjToSerialize, writer, context);
                }
            }
        }
    }
}