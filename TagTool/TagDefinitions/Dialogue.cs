using System.Collections.Generic;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.TagGroups;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "dialogue", Class = "udlg", Size = 0x30)]
    public class Dialogue
    {
        public TagInstance GlobalDialogueInfo;
        public uint Flags;
        public List<Vocalization> Vocalizations;
        public StringID MissionDialogueDesignator;
        public uint Unknown;
        public uint Unknown2;
        public uint Unknown3;

        [TagStructure(Size = 0x18)]
        public class Vocalization
        {
            public ushort Flags;
            public short Unknown;
            public StringID Vocalization2;
            public TagInstance Sound;
        }
    }
}
