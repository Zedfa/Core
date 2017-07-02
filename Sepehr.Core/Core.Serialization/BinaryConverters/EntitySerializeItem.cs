using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Serialization.Serialization
{
    public class EntitySerializeItem : SerializeItem<SerializableEntity>
    {

        protected override SerializeItem<SerializableEntity> CopyBase()
        {
            return new EntitySerializeItem();
        }

        protected SerializeItemBase[] SerializeItems { get; private set; }
        public Type ObjectType { get; private set; }
        public ObjectMetaData EntityMetaData { get; private set; }
        public SerializableEntity SerializableEntity { get; private set; }

        protected override void BeforDeserialize(BinaryReader reader, Type objectType, DeserializeContext context)
        {
            if (SerializeItems == null)
            {
                ObjectType = objectType;
                CurrentType = ObjectType;
                EntityMetaData = ObjectMetaData.GetEntityMetaData(ObjectType);
                SerializeItems = EntityMetaData.SerializeItemList;
                SerializableEntity = Activator.CreateInstance(objectType) as SerializableEntity;
            }
        }

        protected override void BeforSerialize(SerializableEntity obj, BinaryWriter writer, SerializeContext context)
        {
            if (SerializeItems == null)
            {
                ObjectType = obj.GetEntityMetaData.ObjectType;
                CurrentType = ObjectType;
                EntityMetaData = ObjectMetaData.GetEntityMetaData(ObjectType);
                SerializeItems = EntityMetaData.SerializeItemList;
                SerializableEntity = Activator.CreateInstance(ObjectType) as SerializableEntity;
            }
        }
        protected override SerializableEntity DeserializeBase(BinaryReader reader, Type objectType, DeserializeContext context)
        {
            SerializableEntity result = null;
            result = SerializableEntity.CreateInstance() as SerializableEntity;
            var count = result.PropertyValues.Count();
            var entityMetaData = EntityMetaData;
            for (int i = 0; i < count; i++)
            {
                if (entityMetaData.IsSerializablePropertyByIndexList[i])
                {
                    var type = entityMetaData.WritablePropertyList[i].PropertyType;
                    result.PropertyValues[i] = SerializeItems[i].Deserialize(reader, type, context);
                }
            }


            return result;
        }


        protected override void SerializeBase(SerializableEntity objectItem, BinaryWriter writer, SerializeContext context)
        {
            for (int i = 0; i < objectItem.PropertyValues.Length; i++)
            {
                if (EntityMetaData.IsSerializablePropertyByIndexList[i])
                {
                    SerializeItems[i].Serialize(objectItem.GetValue(i, EntityMetaData), writer, context);
                }
            }
        }


        //protected override SerializableEntity DeserializeBase(BinaryReader reader, Type objectType, DeserializeContext context)
        //{
        //    SerializableEntity result;
        //    if (reader.ReadBoolean())
        //    {
        //        result = null;
        //    }
        //    else
        //    {
        //        result = Activator.CreateInstance(objectType) as SerializableEntity;
        //        var count = result.PropertyValues.Count();
        //        var entityMetaData = EntityMetaData.GetEntityMetaData(objectType);
        //        for (int i = 0; i < count; i++)
        //        {
        //            if (entityMetaData.IsSerializablePropertyByIndexList[i])
        //            {
        //                var type = entityMetaData.WritablePropertyList[i].PropertyType;
        //                result.PropertyValues[i] = SerializeItemBase.GetSerializeItem(objectType, i).Deserialize(reader, type, context);
        //            }
        //        }
        //    }

        //    return result;
        //}

        //protected override void SerializeBase(SerializableEntity objectItem, BinaryWriter writer, SerializeContext context)
        //{
        //    var isNull = objectItem == null;
        //    writer.Write(isNull);
        //    if (!isNull)
        //    {
        //        var objectType = objectItem.GetEntityMetaData.EntityType;
        //        for (int i = 0; i < objectItem.PropertyValues.Length; i++)
        //        {
        //            if (EntityMetaData.GetEntityMetaData(objectType).IsSerializablePropertyByIndexList[i])
        //            {
        //                SerializeItemBase.GetSerializeItem(objectType, i).Serialize(objectItem.PropertyValues[i], writer, context);
        //            }
        //        }
        //    }
        //}
    }
}
