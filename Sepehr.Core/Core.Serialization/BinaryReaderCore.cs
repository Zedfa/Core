using Core.Serialization.Utility;
using System.IO;

namespace Core.Serialization
{
    public class BinaryReaderCore : BinaryReader
    {
        //
        // Summary:
        //     Initializes a new instance of the System.IO.BinaryReader class based on the specified
        //     stream and using UTF-8 encoding.
        //
        // Parameters:
        //   input:
        //     The input stream.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     The stream does not support reading, is null, or is already closed.
        public BinaryReaderCore(Stream input) : base(input)
        { }

        public override void Close()
        {
            base.Close();
        }

        public override int Read(byte[] buffer, int index, int count)
        {
            var result = base.Read(buffer, index, count);
            //Debuging.WriteLine(result);
            return result;
        }

        public override int Read(char[] buffer, int index, int count)
        {
            var result = base.Read(buffer, index, count);
            //Debuging.WriteLine(result);
            return result;
        }

        public override bool ReadBoolean()
        {
            var result = base.ReadBoolean();
            //Debuging.WriteLine(result);
            return result;
        }

        public override byte ReadByte()
        {
            var result = base.ReadByte();
            //Debuging.WriteLine(result);
            return result;
        }

        public override byte[] ReadBytes(int count)
        {
            var result = base.ReadBytes(count);
            //Debuging.WriteLine(result);
            return result;
        }

        public override char ReadChar()
        {
            var result = base.ReadChar();
            //Debuging.WriteLine(result);
            return result;
        }

        public override char[] ReadChars(int count)
        {
            var result = base.ReadChars(count);
            //Debuging.WriteLine(result);
            return result;
        }

        public override decimal ReadDecimal()
        {
            var result = base.ReadDecimal();
            //Debuging.WriteLine(result);
            return result;
        }

        public override double ReadDouble()
        {
            var result = base.ReadDouble();
            //Debuging.WriteLine(result);
            return result;
        }

        public override short ReadInt16()
        {
            var result = base.ReadInt16();
            //Debuging.WriteLine(result);
            return result;
        }

        public override int ReadInt32()
        {
            var result = base.ReadInt32();
            //Debuging.WriteLine(result);
            return result;
        }

        public override long ReadInt64()
        {
            var result = base.ReadInt64();
            //Debuging.WriteLine(result);
            return result;
        }

        public override sbyte ReadSByte()
        {
            var result = base.ReadSByte();
            //Debuging.WriteLine(result);
            return result;
        }

        public override float ReadSingle()
        {
            var result = base.ReadSingle();
            //Debuging.WriteLine(result);
            return result;
        }

        public override string ReadString()
        {
            var result = base.ReadString();
            //Debuging.WriteLine(result);
            return result;
        }

        public override ushort ReadUInt16()
        {
            var result = base.ReadUInt16();
            //Debuging.WriteLine(result);
            return result;
        }

        public override uint ReadUInt32()
        {
            var result = base.ReadUInt32();
            //Debuging.WriteLine(result);
            return result;
        }

        public override ulong ReadUInt64()
        {
            var result = base.ReadUInt64();
            //Debuging.WriteLine(result);
            return result;
        }
    }
}