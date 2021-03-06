using System.Collections.Generic;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "formation", Class = "form", Size = 0x18)]
    public class Formation
    {
        public StringID Name;
        public List<UnknownBlock> Unknown;
        public uint Unknown2;
        public uint Unknown3;

        [TagStructure(Size = 0x24)]
        public class UnknownBlock
        {
            public short Unknown;
            public short Unknown2;
            public short Unknown3;
            public short Unknown4;
            public float Unknown5;
            public float Unknown6;
            public float Unknown7;
            public float Unknown8;
            public List<UnknownBlock2> Unknown9;

            [TagStructure(Size = 0x8)]
            public class UnknownBlock2
            {
                public Angle Unknown;
                public float Unknown2;
            }
        }
    }
}
