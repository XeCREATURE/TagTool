using System.Collections.Generic;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.TagGroups;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "podium_settings", Class = "pdm!", Size = 0x3C)]
    public class PodiumSettings
    {
        public uint Unknown;
        public uint Unknown2;
        public uint Unknown3;
        public Angle Unknown4;
        public Angle Unknown5;
        public uint Unknown6;
        public Angle Unknown7;
        public int Unknown8;
        public TagInstance Unknown9;
        public List<UnknownBlock> Unknown10;

        [TagStructure(Size = 0x2C)]
        public class UnknownBlock
        {
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
            public Angle Unknown4;
            public uint Unknown5;
            public uint Unknown6;
            public int Unknown7;
            public TagInstance Effect;
        }
    }
}
