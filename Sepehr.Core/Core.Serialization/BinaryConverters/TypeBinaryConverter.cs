using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Serialization.BinaryConverters
{
    public class TypeBinaryConverter : BinaryConverter<Type>
    {
        public override Type ItemType
        {
            get
            {
                return typeof(Type);
            }
        }

        public override BinaryConverterBase Copy(Type objectType)
        {
            return new TypeBinaryConverter() { CurrentType = typeof(Type) };
        }

        public override object CreateInstance(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            var obj = Type.GetType(reader.ReadString());
            context.CurrentReferenceTypeObject = obj;
            return obj;
        }

        protected override Type DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            var obj = context.CurrentReferenceTypeObject;
            return (Type)obj;
        }

        protected override void SerializeBase(Type objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem.FullName + "," + objectItem.Assembly.FullName);
        }
    }
}
