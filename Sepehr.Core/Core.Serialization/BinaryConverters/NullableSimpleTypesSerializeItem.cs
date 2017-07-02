using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Serialization.Serialization
{

    public class NullableIntSerializeItem : SerializeItem<int?>
    {
        protected override void SerializeBase(int? objectItem, BinaryWriter writer, SerializeContext context)
        {
            var isNull = objectItem == null; writer.Write(isNull); if (!isNull) writer.Write(objectItem.Value);
        }
        protected override int? DeserializeBase(BinaryReader reader, Type objectType, DeserializeContext context)
        {
            var isNull = reader.ReadBoolean();
            if (isNull) return null;
            else
                return reader.ReadInt32();
        }
    }

    public class NullableLongSerializeItem : SerializeItem<long?>
    {
        protected override void SerializeBase(long? objectItem, BinaryWriter writer, SerializeContext context)
        {
            var isNull = objectItem == null; writer.Write(isNull); if (!isNull) writer.Write(objectItem.Value);
        }

        protected override long? DeserializeBase(BinaryReader reader, Type objectType, DeserializeContext context)
        {
            var isNull = reader.ReadBoolean();
            if (isNull) return null;
            else
                return reader.ReadInt64();
        }
    }

    public class NullableUintSerializeItem : SerializeItem<uint?>
    {
        protected override void SerializeBase(uint? objectItem, BinaryWriter writer, SerializeContext context)
        {
            var isNull = objectItem == null; writer.Write(isNull); if (!isNull) writer.Write(objectItem.Value);
        }
        protected override uint? DeserializeBase(BinaryReader reader, Type objectType, DeserializeContext context)
        {
            var isNull = reader.ReadBoolean();
            if (isNull) return null;
            else
                return reader.ReadUInt32();
        }
    }

    public class NullableUshortSerializeItem : SerializeItem<ushort?>
    {
        protected override void SerializeBase(ushort? objectItem, BinaryWriter writer, SerializeContext context)
        {
            var isNull = objectItem == null; writer.Write(isNull); if (!isNull) writer.Write(objectItem.Value);
        }
        protected override ushort? DeserializeBase(BinaryReader reader, Type objectType, DeserializeContext context)
        {
            var isNull = reader.ReadBoolean();
            if (isNull) return null;
            else
                return reader.ReadUInt16();
        }
    }

    public class NullableShortSerializeItem : SerializeItem<short?>
    {
        protected override void SerializeBase(short? objectItem, BinaryWriter writer, SerializeContext context)
        {
            var isNull = objectItem == null; writer.Write(isNull); if (!isNull) writer.Write(objectItem.Value);
        }
        protected override short? DeserializeBase(BinaryReader reader, Type objectType, DeserializeContext context)
        {
            var isNull = reader.ReadBoolean();
            if (isNull) return null;
            else
                return reader.ReadInt16();
        }
    }

    public class NullableDecimalSerializeItem : SerializeItem<decimal?>
    {
        protected override void SerializeBase(decimal? objectItem, BinaryWriter writer, SerializeContext context)
        {
            var isNull = objectItem == null; writer.Write(isNull); if (!isNull) writer.Write(objectItem.Value);
        }
        protected override decimal? DeserializeBase(BinaryReader reader, Type objectType, DeserializeContext context)
        {
            var isNull = reader.ReadBoolean();
            if (isNull) return null;
            else
                return reader.ReadDecimal();
        }
    }

    public class NullableDoubleSerializeItem : SerializeItem<double?>
    {
        protected override void SerializeBase(double? objectItem, BinaryWriter writer, SerializeContext context)
        {
            var isNull = objectItem == null; writer.Write(isNull); if (!isNull) writer.Write(objectItem.Value);
        }
        protected override double? DeserializeBase(BinaryReader reader, Type objectType, DeserializeContext context)
        {
            var isNull = reader.ReadBoolean();
            if (isNull) return null;
            else
                return reader.ReadDouble();
        }
    }

    public class NullableFloatSerializeItem : SerializeItem<float?>
    {
        protected override void SerializeBase(float? objectItem, BinaryWriter writer, SerializeContext context)
        {
            var isNull = objectItem == null; writer.Write(isNull); if (!isNull) writer.Write(objectItem.Value);
        }
        protected override float? DeserializeBase(BinaryReader reader, Type objectType, DeserializeContext context)
        {
            var isNull = reader.ReadBoolean();
            if (isNull) return null;
            else
                return reader.ReadSingle();
        }
    }

    public class NullableCahrSerializeItem : SerializeItem<char?>
    {
        protected override void SerializeBase(char? objectItem, BinaryWriter writer, SerializeContext context)
        {
            var isNull = objectItem == null; writer.Write(isNull); if (!isNull) writer.Write(objectItem.Value);
        }
        protected override char? DeserializeBase(BinaryReader reader, Type objectType, DeserializeContext context)
        {
            var isNull = reader.ReadBoolean();
            if (isNull) return null;
            else
                return reader.ReadChar();
        }
    }

    public class NullableSbyteSerializeItem : SerializeItem<sbyte?>
    {
        protected override void SerializeBase(sbyte? objectItem, BinaryWriter writer, SerializeContext context)
        {
            var isNull = objectItem == null; writer.Write(isNull); if (!isNull) writer.Write(objectItem.Value);
        }
        protected override sbyte? DeserializeBase(BinaryReader reader, Type objectType, DeserializeContext context)
        {
            var isNull = reader.ReadBoolean();
            if (isNull) return null;
            else
                return reader.ReadSByte();
        }
    }

    public class NullableByteSerializeItem : SerializeItem<byte?>
    {
        protected override void SerializeBase(byte? objectItem, BinaryWriter writer, SerializeContext context)
        {
            var isNull = objectItem == null; writer.Write(isNull); if (!isNull) writer.Write(objectItem.Value);
        }
        protected override byte? DeserializeBase(BinaryReader reader, Type objectType, DeserializeContext context)
        {
            var isNull = reader.ReadBoolean();
            if (isNull) return null;
            else
                return reader.ReadByte();
        }
    }

    public class NullableBoolSerializeItem : SerializeItem<bool?>
    {
        protected override void SerializeBase(bool? objectItem, BinaryWriter writer, SerializeContext context)
        {
            var isNull = objectItem == null; writer.Write(isNull); if (!isNull) writer.Write(objectItem.Value);
        }
        protected override bool? DeserializeBase(BinaryReader reader, Type objectType, DeserializeContext context)
        {
            var isNull = reader.ReadBoolean();
            if (isNull) return null;
            else
                return reader.ReadBoolean();
        }
    }

    public class NullableUlongSerializeItem : SerializeItem<ulong?>
    {
        protected override void SerializeBase(ulong? objectItem, BinaryWriter writer, SerializeContext context)
        {
            var isNull = objectItem == null; writer.Write(isNull); if (!isNull) writer.Write(objectItem.Value);
        }
        protected override ulong? DeserializeBase(BinaryReader reader, Type objectType, DeserializeContext context)
        {
            var isNull = reader.ReadBoolean();
            if (isNull) return null;
            else
                return reader.ReadUInt64();
        }
    }

}
