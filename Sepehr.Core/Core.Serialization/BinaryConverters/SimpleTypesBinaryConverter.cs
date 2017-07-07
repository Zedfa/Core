using System;
using System.IO;
using System.Linq;

namespace Core.Serialization.BinaryConverters
{
    public class BoolBinaryConverter : BinaryConverter<bool>
    {
        public override BinaryConverterBase Copy(Type type)
        {
            return new BoolBinaryConverter().Init(type);
        }

        protected override bool DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadBoolean();
        }

        protected override void SerializeBase(bool objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
    }

    public class ByteArrayBinaryConverter : BinaryConverter<byte[]>
    {
        public override BinaryConverterBase Copy(Type type)
        {
            return new ByteArrayBinaryConverter().Init(type);
        }

        protected override byte[] DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            var count = reader.ReadInt32();
            return reader.ReadBytes(count);
        }

        protected override void SerializeBase(byte[] objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem.Count());
            writer.Write(objectItem);
        }
    }

    public class ByteBinaryConverter : BinaryConverter<byte>
    {
        public override BinaryConverterBase Copy(Type type)
        {
            return new ByteBinaryConverter().Init(type);
        }

        protected override byte DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadByte();
        }

        protected override void SerializeBase(byte objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
    }

    public class CharArrayBinaryConverter : BinaryConverter<char[]>
    {
        public override BinaryConverterBase Copy(Type type)
        {
            return new CharArrayBinaryConverter().Init(type);
        }

        protected override char[] DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            var count = reader.ReadInt32();
            return reader.ReadChars(count);
        }

        protected override void SerializeBase(char[] objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem.Count());
            writer.Write(objectItem);
        }
    }

    public class CharBinaryConverter : BinaryConverter<char>
    {
        public override BinaryConverterBase Copy(Type type)
        {
            return new CharBinaryConverter().Init(type);
        }

        protected override char DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadChar();
        }

        protected override void SerializeBase(char objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
    }

    public class DateTimeBinaryConverter : BinaryConverter<DateTime>
    {
        private const byte Local = 2;
        private const byte Unspecified = 0;
        private const byte Utc = 1;
        public override BinaryConverterBase Copy(Type type)
        {
            return new DateTimeBinaryConverter().Init(type);
        }

        protected override DateTime DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            var kind = reader.ReadByte();
            var ticks = reader.ReadInt64();
            switch (kind)
            {
                case Unspecified:
                    {
                        return new DateTime(ticks, DateTimeKind.Unspecified);
                    }
                case Utc:
                    {
                        return new DateTime(ticks, DateTimeKind.Utc);
                    }
                case Local:
                    {
                        return new DateTime(ticks, DateTimeKind.Utc).ToLocalTime();
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException($"Expect 'DateTimeKind' value would be 0 until 2 but the value is {kind}!");
                    }
            }
        }

        protected override void SerializeBase(DateTime objectItem, BinaryWriter writer, SerializationContext context)
        {
            switch (objectItem.Kind)
            {
                case DateTimeKind.Unspecified:
                    {
                        writer.Write(Unspecified);
                        writer.Write(objectItem.Ticks);
                        break;
                    }
                case DateTimeKind.Utc:
                    {
                        writer.Write(Utc);
                        writer.Write(objectItem.Ticks);
                        break;
                    }
                case DateTimeKind.Local:
                    {
                        writer.Write(Local);
                        writer.Write(objectItem.ToUniversalTime().Ticks);
                        break;
                    }
            }
        }
    }

    public class DecimalBinaryConverter : BinaryConverter<decimal>
    {
        public override BinaryConverterBase Copy(Type type)
        {
            return new DecimalBinaryConverter().Init(type);
        }

        protected override decimal DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadDecimal();
        }

        protected override void SerializeBase(decimal objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
    }

    public class DoubleBinaryConverter : BinaryConverter<double>
    {
        public override BinaryConverterBase Copy(Type type)
        {
            return new DoubleBinaryConverter().Init(type);
        }

        protected override double DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadDouble();
        }

        protected override void SerializeBase(double objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
    }

    public class FloatBinaryConverter : BinaryConverter<float>
    {
        public override BinaryConverterBase Copy(Type type)
        {
            return new FloatBinaryConverter().Init(type);
        }

        protected override float DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadSingle();
        }

        protected override void SerializeBase(float objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
    }

    public class IntBinaryConverter : BinaryConverter<int>
    {
        public override BinaryConverterBase Copy(Type type)
        {
            return new IntBinaryConverter().Init(type);
        }

        protected override int DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadInt32();
        }

        protected override void SerializeBase(int objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
    }
    public class LongBinaryConverter : BinaryConverter<long>
    {
        public override BinaryConverterBase Copy(Type type)
        {
            return new LongBinaryConverter().Init(type);
        }

        protected override long DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadInt64();
        }

        protected override void SerializeBase(long objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
    }

    public class SbyteBinaryConverter : BinaryConverter<sbyte>
    {
        public override BinaryConverterBase Copy(Type type)
        {
            return new SbyteBinaryConverter().Init(type);
        }

        protected override sbyte DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadSByte();
        }

        protected override void SerializeBase(sbyte objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
    }

    public class ShortBinaryConverter : BinaryConverter<short>
    {
        public override BinaryConverterBase Copy(Type type)
        {
            return new ShortBinaryConverter().Init(type);
        }

        protected override short DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadInt16();
        }

        protected override void SerializeBase(short objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
    }

    public class StringBinaryConverter : BinaryConverter<string>
    {
        public override BinaryConverterBase Copy(Type type)
        {
            return new StringBinaryConverter().Init(type);
        }

        protected override string DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadString();
        }

        protected override void SerializeBase(string objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
    }

    public class UintBinaryConverter : BinaryConverter<uint>
    {
        public override BinaryConverterBase Copy(Type type)
        {
            return new UintBinaryConverter().Init(type);
        }

        protected override uint DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadUInt32();
        }

        protected override void SerializeBase(uint objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
    }

    public class UlongBinaryConverter : BinaryConverter<ulong>
    {
        public override BinaryConverterBase Copy(Type type)
        {
            return new UlongBinaryConverter().Init(type);
        }

        protected override ulong DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadUInt64();
        }

        protected override void SerializeBase(ulong objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
    }

    public class UshortBinaryConverter : BinaryConverter<ushort>
    {
        public override BinaryConverterBase Copy(Type type)
        {
            return new UshortBinaryConverter().Init(type);
        }

        protected override ushort DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadUInt16();
        }

        protected override void SerializeBase(ushort objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
    }

    public class GuidBinaryConverter : BinaryConverter<Guid>
    {
        public override BinaryConverterBase Copy(Type type)
        {
            return new GuidBinaryConverter().Init(type);
        }

        protected override Guid DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return new Guid(reader.ReadBytes(16));
        }

        protected override void SerializeBase(Guid objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem.ToByteArray());
        }
    }
}