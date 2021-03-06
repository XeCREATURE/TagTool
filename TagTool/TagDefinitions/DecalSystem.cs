using System.Collections.Generic;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.TagGroups;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "decal_system", Class = "decs", Size = 0x24)]
    public class DecalSystem
    {
        public uint Unknown;
        public uint Unknown2;
        public uint Unknown3;
        public uint Unknown4;
        public uint Unknown5;
        public List<DecalSystemBlock> DecalSystem2;
        public uint Unknown6;

        [TagStructure(Size = 0x74)]
        public class DecalSystemBlock
        {
            public StringID Name;
            public uint Unknown;
            public TagInstance BaseRenderMethod;
            public List<UnknownBlock> Unknown2;
            public List<ImportDatum> ImportData;
            public List<ShaderProperty> ShaderProperties;
            public sbyte Unknown3;
            public sbyte Unknown4;
            public sbyte Unknown5;
            public sbyte Unknown6;
            public uint Unknown7;
            public int Unknown8;
            public uint Unknown9;
            public uint Unknown10;
            public uint Unknown11;
            public uint Unknown12;
            public uint Unknown13;
            public uint Unknown14;
            public uint Unknown15;
            public uint Unknown16;
            public uint Unknown17;
            public uint Unknown18;
            public uint Unknown19;

            [TagStructure(Size = 0x2)]
            public class UnknownBlock
            {
                public short Unknown;
            }

            [TagStructure(Size = 0x3C)]
            public class ImportDatum
            {
                public StringID MaterialType;
                public int Unknown;
                public TagInstance Bitmap;
                public uint Unknown2;
                public int Unknown3;
                public short Unknown4;
                public short Unknown5;
                public short Unknown6;
                public short Unknown7;
                public short Unknown8;
                public short Unknown9;
                public uint Unknown10;
                public List<Function> Functions;

                [TagStructure(Size = 0x24)]
                public class Function
                {
                    public int Unknown;
                    public StringID Name;
                    public uint Unknown2;
                    public uint Unknown3;
                    public byte[] Function2;
                }
            }

            [TagStructure(Size = 0x84)]
            public class ShaderProperty
            {
                public TagInstance Template;
                public List<ShaderMap> ShaderMaps;
                public List<Argument> Arguments;
                public List<UnknownBlock> Unknown;
                public uint Unknown2;
                public List<UnknownBlock2> Unknown3;
                public List<UnknownBlock3> Unknown4;
                public List<UnknownBlock4> Unknown5;
                public List<Function> Functions;
                public int Unknown6;
                public int Unknown7;
                public uint Unknown8;
                public short Unknown9;
                public short Unknown10;
                public short Unknown11;
                public short Unknown12;
                public short Unknown13;
                public short Unknown14;
                public short Unknown15;
                public short Unknown16;

                [TagStructure(Size = 0x18)]
                public class ShaderMap
                {
                    public TagInstance Bitmap;
                    public sbyte Unknown;
                    public sbyte BitmapIndex;
                    public sbyte Unknown2;
                    public byte BitmapFlags;
                    public sbyte UnknownBitmapIndexEnable;
                    public sbyte UvArgumentIndex;
                    public sbyte Unknown3;
                    public sbyte Unknown4;
                }

                [TagStructure(Size = 0x10)]
                public class Argument
                {
                    public float Arg1;
                    public float Arg2;
                    public float Arg3;
                    public float Arg4;
                }

                [TagStructure(Size = 0x4)]
                public class UnknownBlock
                {
                    public uint Unknown;
                }

                [TagStructure(Size = 0x2)]
                public class UnknownBlock2
                {
                    public short Unknown;
                }

                [TagStructure(Size = 0x6)]
                public class UnknownBlock3
                {
                    public uint Unknown;
                    public sbyte Unknown2;
                    public sbyte Unknown3;
                }

                [TagStructure(Size = 0x4)]
                public class UnknownBlock4
                {
                    public short Unknown;
                    public short Unknown2;
                }

                [TagStructure(Size = 0x24)]
                public class Function
                {
                    public int Unknown;
                    public StringID Name;
                    public uint Unknown2;
                    public uint Unknown3;
                    public byte[] Function2;
                }
            }
        }
    }
}
