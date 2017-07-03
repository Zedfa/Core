using System;
using System.IO;
using System.Linq;

namespace Core.Serialization.BinaryConverters
{
    public class BoolBinaryConverter : BinaryConverter<bool>
    {
        protected override BinaryConverter<bool> CopyBase()
        {
            return new BoolBinaryConverter();
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
        protected override BinaryConverter<byte[]> CopyBase()
        {
            return new ByteArrayBinaryConverter();
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
        protected override BinaryConverter<byte> CopyBase()
        {
            return new ByteBinaryConverter();
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
        protected override BinaryConverter<char[]> CopyBase()
        {
            return new CharArrayBinaryConverter();
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
        protected override BinaryConverter<char> CopyBase()
        {
            return new CharBinaryConverter();
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
        protected override BinaryConverter<DateTime> CopyBase()
        {
            return new DateTimeBinaryConverter();
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
        protected override BinaryConverter<decimal> CopyBase()
        {
            return new DecimalBinaryConverter();
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
        protected override BinaryConverter<double> CopyBase()
        {
            return new DoubleBinaryConverter();
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
        protected override BinaryConverter<float> CopyBase()
        {
            return new FloatBinaryConverter();
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
        protected override BinaryConverter<int> CopyBase()
        {
            return new IntBinaryConverter();
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
        protected override BinaryConverter<long> CopyBase()
        {
            return new LongBinaryConverter();
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
        protected override BinaryConverter<sbyte> CopyBase()
        {
            return new SbyteBinaryConverter();
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
        protected override BinaryConverter<short> CopyBase()
        {
            return new ShortBinaryConverter();
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
        protected override BinaryConverter<string> CopyBase()
        {
            return new StringBinaryConverter();
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
        protected override BinaryConverter<uint> CopyBase()
        {
            return new UintBinaryConverter();
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
        protected override BinaryConverter<ulong> CopyBase()
        {
            return new UlongBinaryConverter();
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
        protected override BinaryConverter<ushort> CopyBase()
        {
            return new UshortBinaryConverter();
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
}