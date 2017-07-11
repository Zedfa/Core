using Core.Serialization.Utility;
using System.IO;

namespace Core.Serialization
{
    public class BinaryWriterCore : BinaryWriter
    {
        public BinaryWriterCore(Stream output) : base(output)
        {
        }

        public override void Close()
        {
            base.Close();
        }

        public override void Write(bool value)
        {
            base.Write(value);
            //Debuging.WriteLine(value);
        }

        public override void Write(byte value)
        {
            base.Write(value);
            //Debuging.WriteLine(value);
        }

        public override void Write(byte[] buffer)
        {
            base.Write(buffer);
            //Debuging.WriteLine(buffer);
        }

        public override void Write(byte[] buffer, int index, int count)
        {
            base.Write(buffer, index, count);
            //Debuging.WriteLine(buffer);
        }

        public override void Write(char ch)
        {
            base.Write(ch);
            //Debuging.WriteLine(ch);
        }

        public override void Write(char[] chars)
        {
            base.Write(chars);
            //Debuging.WriteLine(chars);
        }

        public override void Write(char[] chars, int index, int count)
        {
            base.Write(chars, index, count);
            //Debuging.WriteLine(chars);
        }

        public override void Write(decimal value)
        {
            base.Write(value);
            //Debuging.WriteLine(value);
        }

        public override void Write(double value)
        {
            base.Write(value);
            //Debuging.WriteLine(value);
        }

        public override void Write(float value)
        {
            base.Write(value);
            //Debuging.WriteLine(value);
        }

        public override void Write(int value)
        {
            base.Write(value);
            //Debuging.WriteLine(value);
        }

        public override void Write(long value)
        {
            base.Write(value);
            //Debuging.WriteLine(value);
        }

        public override void Write(sbyte value)
        {
            base.Write(value);
            //Debuging.WriteLine(value);
        }

        public override void Write(short value)
        {
            base.Write(value);
            //Debuging.WriteLine(value);
        }

        public override void Write(string value)
        {
            base.Write(value);
            //Debuging.WriteLine(value);
        }

        public override void Write(uint value)
        {
            base.Write(value);
            //Debuging.WriteLine(value);
        }

        public override void Write(ulong value)
        {
            base.Write(value);
            //Debuging.WriteLine(value);
        }

        public override void Write(ushort value)
        {
            base.Write(value);
            //Debuging.WriteLine(value);
        }
    }
}