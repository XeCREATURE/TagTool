using System.Collections.Generic;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "achievements", Class = "achi", Size = 0x18)]
    public class Achievements
    {
        public List<AchievementInformationBlock> AchievementInformation;
        public uint Unknown;
        public uint Unknown2;
        public uint Unknown3;

        [TagStructure(Size = 0x18)]
        public class AchievementInformationBlock
        {
            public int Unknown;
            public int Unknown2;
            public StringID LevelName;
            public int Unknown3;
            public int Unknown4;
            public int Unknown5;
        }
    }
}
