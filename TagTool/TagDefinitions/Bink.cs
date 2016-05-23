using TagTool.Definitions;
using TagTool.Cache;
using TagTool.Serialization;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "bink", Class = "bink", Size = 0x18, MaxVersion = DefinitionSet.HaloOnline571627)]
    [TagStructure(Name = "bink", Class = "bink", Size = 0x14, MinVersion = DefinitionSet.HaloOnline700123)]
    public class Bink
    {
        public int FrameCount;
        public ResourceReference Resource;
        public int UselessPadding;
        public uint Unknown;
        public uint Unknown2;

        [MaxVersion(DefinitionSet.HaloOnline571627)]
        public uint Unknown3;
    }
}
