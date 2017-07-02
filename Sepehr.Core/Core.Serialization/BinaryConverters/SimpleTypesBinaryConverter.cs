using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Serialization.BinaryConverters
{
    public class IntBinaryConverter : BinaryConverter<int>
    {
        protected override void SerializeBase(int objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
        protected override int DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadInt32();
        }

        protected override BinaryConverter<int> CopyBase()
        {
            return new IntBinaryConverter();
        }
    }

    public class CharArrayBinaryConverter : BinaryConverter<char[]>
    {
        protected override void SerializeBase(char[] objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem.Count());
            writer.Write(objectItem);
        }
        protected override char[] DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            var count = reader.ReadInt32();
            return reader.ReadChars(count);
        }
        protected override BinaryConverter<char[]> CopyBase()
        {
            return new CharArrayBinaryConverter();
        }
    }

    public class LongBinaryConverter : BinaryConverter<long>
    {
        protected override void SerializeBase(long objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
        protected override long DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadInt64();
        }
        protected override BinaryConverter<long> CopyBase()
        {
            return new LongBinaryConverter();
        }
    }

    public class UintBinaryConverter : BinaryConverter<uint>
    {
        protected override void SerializeBase(uint objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
        protected override uint DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadUInt32();
        }
        protected override BinaryConverter<uint> CopyBase()
        {
            return new UintBinaryConverter();
        }
    }

    public class UshortBinaryConverter : BinaryConverter<ushort>
    {
        protected override void SerializeBase(ushort objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
        protected override ushort DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadUInt16();
        }
        protected override BinaryConverter<ushort> CopyBase()
        {
            return new UshortBinaryConverter();
        }
    }

    public class ShortBinaryConverter : BinaryConverter<short>
    {
        protected override void SerializeBase(short objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
        protected override short DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadInt16();
        }
        protected override BinaryConverter<short> CopyBase()
        {
            return new ShortBinaryConverter();
        }
    }

    public class DecimalBinaryConverter : BinaryConverter<decimal>
    {
        protected override void SerializeBase(decimal objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
        protected override decimal DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadDecimal();
        }
        protected override BinaryConverter<decimal> CopyBase()
        {
            return new DecimalBinaryConverter();
        }
    }

    public class DoubleBinaryConverter : BinaryConverter<double>
    {
        protected override void SerializeBase(double objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
        protected override double DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadDouble();
        }

        protected override BinaryConverter<double> CopyBase()
        {
            return new DoubleBinaryConverter();
        }
    }

    public class FloatBinaryConverter : BinaryConverter<float>
    {
        protected override void SerializeBase(float objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
        protected override float DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadSingle();
        }
        protected override BinaryConverter<float> CopyBase()
        {
            return new FloatBinaryConverter();
        }
    }

    public class CharBinaryConverter : BinaryConverter<char>
    {
        protected override void SerializeBase(char objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
        protected override char DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadChar();
        }
        protected override BinaryConverter<char> CopyBase()
        {
            return new CharBinaryConverter();
        }
    }

    public class StringBinaryConverter : BinaryConverter<string>
    {
        protected override void SerializeBase(string objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
        protected override string DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadString();
        }
        protected override BinaryConverter<string> CopyBase()
        {
            return new StringBinaryConverter();
        }
    }

    public class ByteArrayBinaryConverter : BinaryConverter<byte[]>
    {
        protected override void SerializeBase(byte[] objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem.Count());
            writer.Write(objectItem);
        }
        protected override byte[] DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            var count = reader.ReadInt32();
            return reader.ReadBytes(count);
        }
        protected override BinaryConverter<byte[]> CopyBase()
        {
            return new ByteArrayBinaryConverter();
        }
    }

    public class SbyteBinaryConverter : BinaryConverter<sbyte>
    {
        protected override void SerializeBase(sbyte objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
        protected override sbyte DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadSByte();
        }
        protected override BinaryConverter<sbyte> CopyBase()
        {
            return new SbyteBinaryConverter();
        }
    }

    public class ByteBinaryConverter : BinaryConverter<byte>
    {
        protected override void SerializeBase(byte objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
        protected override byte DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadByte();
        }
        protected override BinaryConverter<byte> CopyBase()
        {
            return new ByteBinaryConverter();
        }
    }

    public class BoolBinaryConverter : BinaryConverter<bool>
    {
        protected override void SerializeBase(bool objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
        protected override bool DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadBoolean();
        }
        protected override BinaryConverter<bool> CopyBase()
        {
            return new BoolBinaryConverter();
        }
    }

    public class UlongBinaryConverter : BinaryConverter<ulong>
    {
        protected override void SerializeBase(ulong objectItem, BinaryWriter writer, SerializationContext context)
        {
            writer.Write(objectItem);
        }
        protected override ulong DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return reader.ReadUInt64();
        }
        protected override BinaryConverter<ulong> CopyBase()
        {
            return new UlongBinaryConverter();
        }
    }

    //public class DateTimeBinaryConverter : BinaryConverter<DateTime>
    //{
    //    private const byte Unspecified = 0;
    //    private const byte Utc = 1;
    //    private const byte Local = 2;
    //    protected override void SerializeBase(DateTime objectItem, BinaryWriter writer, SerializationContext context)
    //    {
    //        switch (objectItem.Kind)
    //        {
    //            case DateTimeKind.Unspecified:
    //                {
    //                    writer.Write(Unspecified);
    //                    writer.Write(objectItem.Ticks);
    //                    break;
    //                }
    //            case DateTimeKind.Utc:
    //                {
    //                    writer.Write(Utc);
    //                    writer.Write(objectItem.Ticks);
    //                    break;
    //                }
    //            case DateTimeKind.Local:
    //                {
    //                    writer.Write(Local);
    //                    writer.Write(objectItem.ToUniversalTime().Ticks);
    //                    break;
    //                }
    //        }
    //    }
    //    protected override DateTime DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context)
    //    {
    //        var kind = reader.ReadByte();
    //        var ticks = reader.ReadInt64();
    //        switch (kind)
    //        {
    //            case Unspecified:
    //                {
    //                    return new DateTime(ticks, DateTimeKind.Unspecified);
    //                }
    //            case Utc:
    //                {
    //                    return new DateTime(ticks, DateTimeKind.Utc);
    //                }
    //            case Local:
    //                {
    //                    return new DateTime(ticks, DateTimeKind.Utc).ToLocalTime();
    //                }
    //            default:
    //                {
    //                    throw new ArgumentOutOfRangeException($"Expect 'DateTimeKind' value would be 0 until 2 but the value is {kind}!");
    //                }
    //        }
    //    }
    //    protected override BinaryConverter<DateTime> CopyBase()
    //    {
    //        return new DateTimeBinaryConverter();
    //    }
    //}
}
