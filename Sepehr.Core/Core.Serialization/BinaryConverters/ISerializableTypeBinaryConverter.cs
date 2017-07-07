using System;
using System.IO;
using System.Runtime.Serialization;

namespace Core.Serialization.BinaryConverters
{
    public class ISerializableTypeBinaryConverter : BinaryConverter<ISerializable>
    {
        private static StreamingContext _context;

        public ObjectMetaData EntityMetaData { get; private set; }

        public Type ObjectType { get; private set; }

        public object UserDefinedObject { get; private set; }

        protected BinaryConverterBase[] BinaryConverters { get; private set; }

        protected Type[] BinaryConverterTypesForISerializable { get; private set; }

        protected string[] ItemNamesForISerializable { get; private set; }
        public override BinaryConverterBase Copy(Type type)
        {
            var binaryConverter = new ISerializableTypeBinaryConverter();
            binaryConverter.ObjectType = type;
            binaryConverter.Init(type);
            binaryConverter.EntityMetaData = ObjectMetaData.GetEntityMetaData(type);
            binaryConverter.UserDefinedObject = Activator.CreateInstance(type);
            SerializationInfo serializationInfo = new SerializationInfo(type, new FormatterConverter());
            ((ISerializable)binaryConverter.UserDefinedObject).GetObjectData(serializationInfo, _context);
            binaryConverter.ItemNamesForISerializable = new string[serializationInfo.MemberCount];
            binaryConverter.BinaryConverters = new BinaryConverterBase[serializationInfo.MemberCount];
            binaryConverter.BinaryConverterTypesForISerializable = new Type[serializationInfo.MemberCount];
            var i = 0;
            foreach (SerializationEntry serializationEntry in serializationInfo)
            {
                binaryConverter.BinaryConverters[i] = BinaryConverterBase.GetBinaryConverter(serializationEntry.ObjectType);
                binaryConverter.BinaryConverterTypesForISerializable[i] = serializationEntry.ObjectType;
                binaryConverter.ItemNamesForISerializable[i] = serializationEntry.Name;
                i++;
            }
            return binaryConverter;
        }

        protected override ISerializable DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            ISerializable result;
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
                object value;
                value = BinaryConverters[i].Deserialize(reader, BinaryConverterTypesForISerializable[i], context);
                serializationInfo.AddValue(ItemNamesForISerializable[i], value, BinaryConverterTypesForISerializable[i]);
            }

            result = (ISerializable)EntityMetaData.ReflectionEmitPropertyAccessor.CreateInstanceForISerialzableObject(serializationInfo, _context);
            return result;
        }

        protected override void SerializeBase(ISerializable objectItem, BinaryWriter writer, SerializationContext context)
        {
            if (!FullyTrusted)
            {
                string message = $@"Type '{CurrentType}' implements ISerializable but cannot be serialized using the ISerializable interface because the current application is not fully trusted and ISerializable can expose secure data." + Environment.NewLine +
                                 @"To fix this error either change the environment to be fully trusted, change the application to not deserialize the type, add JsonObjectAttribute to the type or change the JsonSerializer setting ContractResolver to use a new DefaultContractResolver with IgnoreSerializableInterface set to true." + Environment.NewLine;
                throw new SerializationException(message);
            }

            SerializationInfo serializationInfo = new SerializationInfo(CurrentType, new FormatterConverter());
            ((ISerializable)objectItem).GetObjectData(serializationInfo, _context);
            var i = 0;
            foreach (SerializationEntry serializationEntry in serializationInfo)
            {
                BinaryConverters[i].Serialize(serializationEntry.Value, writer, context);
                i++;
            }
        }
    }
}