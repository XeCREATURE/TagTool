using TagTool.Serialization;
using TagTool.TagGroups;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "sound_global_propagation", Class = "sgp!", Size = 0x50)]
    public class SoundGlobalPropagation
    {
        public TagInstance UnderwaterEnvironment;
        public TagInstance UnderwaterLoop;
        public uint Unknown;
        public uint Unknown2;
        public TagInstance EnterUnderater;
        public TagInstance ExitUnderwater;
        public uint Unknown3;
        public uint Unknown4;
    }
}
