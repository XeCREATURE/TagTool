using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static System.BitConverter;
using static TagTool.IO.EndianFormat;

namespace TagTool.IO
{
    public class EndianReader : BinaryReader
    {
        public EndianFormat Format { get; set; }

        public long Origin { get; set; }
        
        public EndianReader(Stream stream, EndianFormat format)
            : base(stream)
        {
            Format = format;
            Origin = 0;
        }

        public override short ReadInt16() =>
            ReadInt16(Format);

        public override int ReadInt32() =>
            ReadInt32(Format);

        public override long ReadInt64() =>
            ReadInt64(Format);

        public override ushort ReadUInt16() =>
            ReadUInt16(Format);

        public override uint ReadUInt32() =>
            ReadUInt32(Format);

        public override ulong ReadUInt64() =>
            ReadUInt64(Format);

        public override float ReadSingle() =>
            ReadSingle(Format);

        public override double ReadDouble() =>
            ReadDouble(Format);

        public short ReadInt16(EndianFormat format) =>
            format == Big ?
                ToInt16(ReadBytes(2).Reverse().ToArray(), 0) :
                base.ReadInt16();

        public int ReadInt32(EndianFormat format) =>
            format == Big ?
                ToInt32(ReadBytes(4).Reverse().ToArray(), 0) :
                base.ReadInt32();

        public long ReadInt64(EndianFormat format) =>
            format == Big ?
                ToInt64(ReadBytes(8).Reverse().ToArray(), 0) :
                base.ReadInt64();

        public ushort ReadUInt16(EndianFormat format) =>
            format == Big ?
                ToUInt16(ReadBytes(2).Reverse().ToArray(), 0) :
                base.ReadUInt16();

        public uint ReadUInt32(EndianFormat format) =>
            format == Big ?
                ToUInt32(ReadBytes(4).Reverse().ToArray(), 0) :
                base.ReadUInt32();

        public ulong ReadUInt64(EndianFormat format) =>
            format == Big ?
                ToUInt64(ReadBytes(8).Reverse().ToArray(), 0) :
                base.ReadUInt64();

        public float ReadSingle(EndianFormat format) =>
            format == Big ?
                ToSingle(ReadBytes(4).Reverse().ToArray(), 0) :
                base.ReadSingle();

        public double ReadDouble(EndianFormat format) =>
            format == Big ?
                ToDouble(ReadBytes(8).Reverse().ToArray(), 0) :
                base.ReadDouble();
        
        public string ReadString(int Length, bool Trim = true)
        {
            string str = Encoding.UTF8.GetString(ReadBytes(Length));

            if (Trim)
                str = str.Trim().Replace("\0", "");

            return str;
        }
        
        public string ReadNullTerminatedString()
        {
            var bytes = new List<byte>();
            byte b;
            while ((b = ReadByte()) != 0)
                bytes.Add(b);

            return Encoding.UTF8.GetString(bytes.ToArray());
        }
        
        public string ReadNullTerminatedString(int MaxLength)
        {
            string str = Encoding.UTF8.GetString(ReadBytes(MaxLength));
            return str.Substring(0, str.IndexOf('\0'));
        }
        
        public ushort PeekUInt16() => PeekUInt16(Format);
        
        public ushort PeekUInt16(EndianFormat Type)
        {
            ushort val;

            if (Type == EndianFormat.Little)
                val = base.ReadUInt16();
            else
            {
                byte[] bytes = base.ReadBytes(2);
                Array.Reverse(bytes);
                val = BitConverter.ToUInt16(bytes, 0);
            }

            Skip(-2);
            return val;
        }

        public int ReadBlock(byte[] buffer, int offset, int size) =>
            BaseStream.Read(buffer, offset, size);

        public void SeekTo(long offset) =>
            BaseStream.Seek(Origin + offset, SeekOrigin.Begin);

        public void Skip(long count) =>
            BaseStream.Seek(count, SeekOrigin.Current);

        public long Position =>
            BaseStream.Position - Origin;

        public long Length =>
            BaseStream.Length - Origin;

        public bool EOF =>
            Position >= Length;
    }
}
