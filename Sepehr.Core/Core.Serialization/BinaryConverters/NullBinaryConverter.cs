using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Serialization.BinaryConverters
{
    public class NullBinaryConverter : BinaryConverter<object>
    {
        public override Type ItemType
        {
            get
            {
                return typeof(object);
            }
        }

        public override BinaryConverterBase Copy(Type objectType)
        {
            return new NullBinaryConverter() { CurrentType = typeof(object) };
        }

        protected override object DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return null;
        }

        protected override void SerializeBase(object objectItem, BinaryWriter writer, SerializationContext context)
        {
            
        }
    }
}
