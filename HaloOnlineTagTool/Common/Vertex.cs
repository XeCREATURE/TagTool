using HaloOnlineTagTool.Endian;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HaloOnlineTagTool.Common
{
    [Serializable]
    public class Vertex
    {
        public string FormatName { get; set; }
        public List<VertexValue> Values { get; }

        public Vertex()
        {
            Values = new List<VertexValue>();
        }

        public Vertex(EndianReader reader, XmlNode formatNode)
            : this()
        {
            if (!formatNode.HasChildNodes)
                throw new NotSupportedException(formatNode.Attributes["type"].Value + ":" + formatNode.Attributes["name"].Value + " has an empty definition.");
            
            var origin = (int)reader.Position;

            foreach (XmlNode val in formatNode.ChildNodes)
            {
                reader.SeekTo(origin + Convert.ToInt32(val.Attributes["offset"].Value, 16));
                Values.Add(new VertexValue(val, reader));
            }

            FormatName = formatNode.Attributes["name"].Value;
        }

        public bool TryGetValue(string Usage, int UsageIndex, out VertexValue val)
        {
            foreach (var value in Values)
                if (value.Usage == Usage && value.UsageIndex == UsageIndex)
                {
                    val = value;
                    return true;
                }

            val = new VertexValue(new Vector(), 0, Usage, UsageIndex);
            return false;
        }

        public VertexValue this[string Usage]
        {
            get
            {
                foreach (var v in Values)
                    if (v.Usage.ToLower() == Usage.ToLower()) return v;

                return new VertexValue(new Vector(), VertexValue.ValueType.None, Usage, 0);
            }
        }
    }

    [Serializable]
    public class VertexValue
    {
        public string Usage;
        public ValueType Type;
        public int UsageIndex;
        public Vector Data;

        public VertexValue(XmlNode Node, EndianReader reader)
        {
            if (Convert.ToInt32(Node.Attributes["stream"].Value) > 0) throw new NotSupportedException("Multi-streamed vertices not supported");
            Type = (ValueType)Enum.Parse(typeof(ValueType), Node.Attributes["type"].Value);
            Usage = Node.Attributes["usage"].Value;
            UsageIndex = Convert.ToInt32(Node.Attributes["usageIndex"].Value);
            
            switch (Type)
            {
                case ValueType.Float32_2:
                    Data = new Vector(reader.ReadSingle(), reader.ReadSingle());
                    break;

                case ValueType.Float32_3:
                    Data = new Vector(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    break;

                case ValueType.Float32_4:
                    Data = new Vector(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    break;

                case ValueType.Int8_N4:
                    Data = new Vector((float)reader.ReadByte() / (float)0x7F, (float)reader.ReadByte() / (float)0x7F, (float)reader.ReadByte() / (float)0x7F, (float)reader.ReadByte() / (float)0x7F);
                    break;

                case ValueType.UInt8_2:
                    Data = new Vector(reader.ReadByte(), reader.ReadByte(), 0, 0);
                    break;

                case ValueType.UInt8_3:
                    Data = new Vector(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), 0);
                    break;

                case ValueType.UInt8_4:
                    Data = Vector.FromUByte4(reader.ReadUInt32());
                    break;

                case ValueType.UInt8_N2:
                    Data = new Vector((float)reader.ReadByte() / (float)0xFF, (float)reader.ReadByte() / (float)0xFF, 0, 0);
                    break;

                case ValueType.UInt8_N3:
                    Data = new Vector((float)reader.ReadByte() / (float)0xFF, (float)reader.ReadByte() / (float)0xFF, (float)reader.ReadByte() / (float)0xFF, 0);
                    break;

                case ValueType.UInt8_N4:
                    Data = Vector.FromUByteN4(reader.ReadUInt32());
                    break;

                case ValueType.Int16_N3:
                    Data = new Vector(((float)reader.ReadInt16() + (float)0x7FFF) / (float)0xFFFF, ((float)reader.ReadInt16() + (float)0x7FFF) / (float)0xFFFF, ((float)reader.ReadInt16() + (float)0x7FFF) / (float)0xFFFF, 0);
                    break;

                case ValueType.Int16_N4:
                    Data = new Vector(((float)reader.ReadInt16() + (float)0x7FFF) / (float)0xFFFF, ((float)reader.ReadInt16() + (float)0x7FFF) / (float)0xFFFF, ((float)reader.ReadInt16() + (float)0x7FFF) / (float)0xFFFF, ((float)reader.ReadInt16() + (float)0x7FFF) / (float)0xFFFF);
                    break;

                case ValueType.UInt16_2:
                    Data = new Vector(reader.ReadUInt16(), reader.ReadUInt16());
                    break;

                case ValueType.UInt16_4:
                    Data = new Vector(reader.ReadUInt16(), reader.ReadUInt16(), reader.ReadUInt16(), reader.ReadUInt16());
                    break;

                case ValueType.UInt16_N2:
                    Data = new Vector((float)reader.ReadUInt16() / (float)0xFFFF, (float)reader.ReadUInt16() / (float)0xFFFF);
                    break;

                case ValueType.UInt16_N4:
                    Data = new Vector((float)reader.ReadUInt16() / (float)0xFFFF, (float)reader.ReadUInt16() / (float)0xFFFF, (float)reader.ReadUInt16() / (float)0xFFFF, (float)reader.ReadUInt16() / (float)0xFFFF);
                    break;

                case ValueType.DecN4:
                    Data = Vector.FromDecN4(reader.ReadUInt32());
                    break;

                case ValueType.UDecN4:
                    Data = Vector.FromUDecN4(reader.ReadUInt32());
                    break;

                case ValueType.DHenN3:
                    Data = Vector.FromDHenN3(reader.ReadUInt32());
                    break;

                case ValueType.UDHenN3:
                    Data = Vector.FromUDHenN3(reader.ReadUInt32());
                    break;

                case ValueType.HenDN3:
                    Data = Vector.FromHenDN3(reader.ReadUInt32());
                    break;

                case ValueType.UHenDN3:
                    Data = Vector.FromUHenDN3(reader.ReadUInt32());
                    break;

                case ValueType.Float16_2:
                    Data = new Vector(Half.ToHalf(reader.ReadUInt16()), Half.ToHalf(reader.ReadUInt16()));
                    break;

                case ValueType.Float16_4:
                    Data = new Vector(Half.ToHalf(reader.ReadUInt16()), Half.ToHalf(reader.ReadUInt16()), Half.ToHalf(reader.ReadUInt16()), Half.ToHalf(reader.ReadUInt16()));
                    break;

                case ValueType.D3DColour:
                    reader.ReadUInt32();
                    break;
            }
        }

        public VertexValue(Vector Data, ValueType Type, string Usage, int UsageIndex)
        {
            this.Data = Data;
            this.Type = Type;
            this.Usage = Usage;
            this.UsageIndex = UsageIndex;
        }

        public enum ValueType : byte
        {
            None,

            Float16_2,
            Float16_4,

            Float32_2,
            Float32_3,
            Float32_4,

            DHenN3,
            UDHenN3,

            HenDN3,
            UHenDN3,

            DecN4,
            UDecN4,

            Int8_N4,
            UInt8_2,
            UInt8_3,
            UInt8_4,
            UInt8_N2,
            UInt8_N3,
            UInt8_N4,

            Int16_N2,
            Int16_N3,
            Int16_N4,
            UInt16_2,
            UInt16_4,
            UInt16_N2,
            UInt16_N4,

            D3DColour,
        }

        public override string ToString() => Usage;
    }
}
