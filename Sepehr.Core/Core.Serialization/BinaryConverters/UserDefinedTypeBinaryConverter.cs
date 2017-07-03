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

        protected override void BeforDeserialize(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            if (BinaryConverters == null)
            {
                ObjectType = objectType;
                CurrentType = ObjectType;
                EntityMetaData = ObjectMetaData.GetEntityMetaData(CurrentType);
                BinaryConverters = EntityMetaData.BinaryConverterList;
            }
        }

        protected override void BeforSerialize(object obj, BinaryWriter writer, SerializationContext context)
        {
            if (BinaryConverters == null)
            {
                ObjectType = obj.GetType();
                CurrentType = ObjectType;
                EntityMetaData = ObjectMetaData.GetEntityMetaData(CurrentType);
                BinaryConverters = EntityMetaData.BinaryConverterList;
            }
        }

        protected override BinaryConverter<object> CopyBase()
        {
            return new UserDefinedTypeBinaryConverter();
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
                    BinaryConverters[i].Serialize(EntityMetaData.ReflectionEmitPropertyAccessor.EmittedPropertyGetters[i](objectItem), writer, context);
                }
            }
        }
    }
}