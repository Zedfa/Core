
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Serialization.BinaryConverters
{
    public class UserDefinedTypeBinaryConverter : BinaryConverter<object>
    {
        private static StreamingContext _context;
        protected override BinaryConverter<object> CopyBase()
        {
            return new UserDefinedTypeBinaryConverter();
        }
        protected BinaryConverterBase[] BinaryConverters { get; private set; }
        protected Type[] BinaryConverterTypesForISerializable { get; private set; }
        protected string[] ItemNamesForISerializable { get; private set; }
        public Type ObjectType { get; private set; }
        public ObjectMetaData EntityMetaData { get; private set; }
        public object UserDefinedObject { get; private set; }
        public bool IsAssingbleFromISerializable { get; private set; }
        protected override void BeforDeserialize(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            if (BinaryConverters == null)
            {
                ObjectType = objectType;
                CurrentType = ObjectType;
                IsAssingbleFromISerializable = typeof(ISerializable).IsAssignableFrom(CurrentType);
                EntityMetaData = ObjectMetaData.GetEntityMetaData(CurrentType);
                if (IsAssingbleFromISerializable)
                {
                    UserDefinedObject = Activator.CreateInstance(CurrentType);
                    SerializationInfo serializationInfo = new SerializationInfo(CurrentType, new FormatterConverter());
                    ((ISerializable)UserDefinedObject).GetObjectData(serializationInfo, _context);
                    ItemNamesForISerializable = new string[serializationInfo.MemberCount];
                    BinaryConverters = new BinaryConverterBase[serializationInfo.MemberCount];
                    BinaryConverterTypesForISerializable = new Type[serializationInfo.MemberCount];
                    var i = 0;
                    foreach (SerializationEntry serializationEntry in serializationInfo)
                    {
                        BinaryConverters[i] = BinaryConverterBase.GetBinaryConverter(serializationEntry.ObjectType).Copy();
                        BinaryConverterTypesForISerializable[i] = serializationEntry.ObjectType;
                        ItemNamesForISerializable[i] = serializationEntry.Name;
                        i++;
                    }
                }
                else
                {
                    BinaryConverters = EntityMetaData.BinaryConverterList;
                }
            }
        }

        protected override void BeforSerialize(object obj, BinaryWriter writer, SerializationContext context)
        {
            if (BinaryConverters == null)
            {
                ObjectType = obj.GetType();
                CurrentType = ObjectType;
                IsAssingbleFromISerializable = typeof(ISerializable).IsAssignableFrom(CurrentType);
                EntityMetaData = ObjectMetaData.GetEntityMetaData(CurrentType);
                if (IsAssingbleFromISerializable)
                {
                    UserDefinedObject = Activator.CreateInstance(CurrentType);
                    SerializationInfo serializationInfo = new SerializationInfo(CurrentType, new FormatterConverter());
                    ((ISerializable)UserDefinedObject).GetObjectData(serializationInfo, _context);
                    ItemNamesForISerializable = new string[serializationInfo.MemberCount];
                    BinaryConverters = new BinaryConverterBase[serializationInfo.MemberCount];
                    BinaryConverterTypesForISerializable = new Type[serializationInfo.MemberCount];
                    var i = 0;
                    foreach (SerializationEntry serializationEntry in serializationInfo)
                    {
                        BinaryConverters[i] = BinaryConverterBase.GetBinaryConverter(serializationEntry.ObjectType).Copy();
                        BinaryConverterTypesForISerializable[i] = serializationEntry.ObjectType;
                        ItemNamesForISerializable[i] = serializationEntry.Name;
                        i++;
                    }
                }
                else
                {
                    BinaryConverters = EntityMetaData.BinaryConverterList;
                    //  UserDefinedObject = Activator.CreateInstance(ObjectType);
                }
            }
        }
        protected override object DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            object result;
            if (IsAssingbleFromISerializable)
            {
                if (!FullyTrusted)
                {
                    string message = $@"Type '{CurrentType}' implements ISerializable but cannot be serialized using the ISerializable interface because the current application is not fully trusted and ISerializable can expose secure data." + Environment.NewLine +
                                     @"To fix this error either change the environment to be fully trusted, change the application to not deserialize the type, add JsonObjectAttribute to the type or change the JsonSerializer setting ContractResolver to use a new DefaultContractResolver with IgnoreSerializableInterface set to true." + Environment.NewLine;
                    throw new SerializationException(message);
                }

                var memberCount = BinaryConverters.Length;
                SerializationInfo serializationInfo = new SerializationInfo(CurrentType, new FormatterConverter());
                for (var i = 0; i < memberCount; i++)
                {
                    //  var fullTypeName = (string)new StringBinaryConverter().Deserialize(reader, typeof(string), context);
                    //  var type = Type.GetType(fullTypeName);
                    //  var name = (string)new StringBinaryConverter().Deserialize(reader, typeof(string), context);
                    object value;
                    //  BinaryConverterBase serializationPlan;
                    //if (!SerializationPlan.SerializationPlan.TryGetBinaryConverter(type, out serializationPlan))
                    //{
                    //    serializationPlan = BinaryConverterBase.GetBinaryConverter(type).Copy();
                    //    value = serializationPlan.Deserialize(reader, type, context);
                    //    SerializationPlan.SerializationPlan.SetSerializePlan(type, serializationPlan);
                    //}
                    //else
                    //{
                    value = BinaryConverters[i].Deserialize(reader, BinaryConverterTypesForISerializable[i], context);
                    //}

                    serializationInfo.AddValue(ItemNamesForISerializable[i], value, BinaryConverterTypesForISerializable[i]);
                }

                result = EntityMetaData.ReflectionEmitPropertyAccessor.CreateInstanceForISerialzableObject(serializationInfo, _context);
            }
            else
            {
                //if (CurrentType.IsAnonymousType())
                //{
                //    result = null;
                //}
                //else
                {
                    //result = EntityMetaData.ProxyObject.CreateObject();
                    result = EntityMetaData.ReflectionEmitPropertyAccessor.EmittedObjectInstanceCreator();
                    var count = EntityMetaData.WritableProperties.Count();
                    var entityMetaData = EntityMetaData;
                    for (int i = 0; i < count; i++)
                    {
                        if (entityMetaData.IsSerializablePropertyByIndexList[i])
                        {
                            var type = entityMetaData.WritablePropertyList[i].PropertyType;
                            //EntityMetaData.ProxyObject.ProxyPropertyList[i].SetProperty(result, BinaryConverters[i].Deserialize(reader, type, context));
                            EntityMetaData.ReflectionEmitPropertyAccessor.EmittedPropertySetters[i](result, BinaryConverters[i].Deserialize(reader, type, context));
                        }
                    }
                }
            }
            return result;
        }
        protected override void SerializeBase(object objectItem, BinaryWriter writer, SerializationContext context)
        {
            if (IsAssingbleFromISerializable)
            {
                if (!FullyTrusted)
                {
                    string message = $@"Type '{CurrentType}' implements ISerializable but cannot be serialized using the ISerializable interface because the current application is not fully trusted and ISerializable can expose secure data." + Environment.NewLine +
                                     @"To fix this error either change the environment to be fully trusted, change the application to not deserialize the type, add JsonObjectAttribute to the type or change the JsonSerializer setting ContractResolver to use a new DefaultContractResolver with IgnoreSerializableInterface set to true." + Environment.NewLine;
                    throw new SerializationException(message);
                }

                SerializationInfo serializationInfo = new SerializationInfo(CurrentType, new FormatterConverter());
                ((ISerializable)objectItem).GetObjectData(serializationInfo, _context);
                //     writer.Write(serializationInfo.MemberCount);
                var i = 0;
                foreach (SerializationEntry serializationEntry in serializationInfo)
                {
                    //new StringBinaryConverter().Serialize(serializationEntry.ObjectType.FullName + "," + serializationEntry.ObjectType.Assembly.FullName, writer, context);
                    //   new StringBinaryConverter().Serialize(serializationEntry.Name, writer, context);
                    //   var type = serializationEntry.ObjectType;
                    //    BinaryConverterBase serializationPlan;
                    //   if (!SerializationPlan.SerializationPlan.TryGetBinaryConverter(type, out serializationPlan))
                    // {
                    //      serializationPlan = BinaryConverterBase.GetBinaryConverter(type).Copy();
                    BinaryConverters[i].Serialize(serializationEntry.Value, writer, context);
                    //     SerializationPlan.SerializationPlan.SetSerializePlan(type, serializationPlan);
                    //  }
                    //  else
                    // {
                    //  serializationPlan.Serialize(serializationEntry.Value, writer, context);
                    // }
                    i++;
                }
            }
            else
            {
                for (int i = 0; i < EntityMetaData.WritablePropertyList.Count; i++)
                {
                    if (EntityMetaData.IsSerializablePropertyByIndexList[i])
                    {
                        // BinaryConverters[i].Serialize(EntityMetaData.ProxyObject.ProxyPropertyList[i].GetProperty(objectItem), writer, context);
                        BinaryConverters[i].Serialize(EntityMetaData.ReflectionEmitPropertyAccessor.EmittedPropertyGetters[i](objectItem), writer, context);

                    }
                }
            }
        }
    }
}
