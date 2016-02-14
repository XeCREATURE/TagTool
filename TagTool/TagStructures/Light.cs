using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.TagStructures
{
    [TagStructure(Name = "light", Class = "ligh", Size = 0x94)]
    public class Light
    {
        public uint Flags;
        public TypeValue Type;
        public short Unknown;
        public float LightRange;
        public float NearWidth;
        public float HeightStretch;
        public Angle FieldOfView;
        public StringID FunctionName;
        public StringID FunctionName2;
        public short Unknown2;
        public short Unknown3;
        public uint Unknown4;
        public byte[] Function;
        public StringID FunctionName3;
        public StringID FunctionName4;
        public short Unknown5;
        public short Unknown6;
        public uint Unknown7;
        public byte[] Function2;
        public TagInstance GelMap;
        public uint Unknown8;
        public uint Unknown9;
        public uint Unknown10;
        public uint Unknown11;
        public sbyte Unknown12;
        public sbyte Unknown13;
        public sbyte Unknown14;
        public sbyte Unknown15;
        public TagInstance LensFlare;

        public enum TypeValue : short
        {
            Sphere,
            Projective,
        }
    }
}
