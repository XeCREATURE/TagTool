using System.Collections.Generic;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.TagStructures
{
    [TagStructure(Name = "ai_mission_dialogue", Class = "mdlg", Size = 0xC)]
    public class AiMissionDialogue
    {
        public List<Line> Lines;

        [TagStructure(Size = 0x14)]
        public class Line
        {
            public StringID Name;
            public List<Variant> Variants;
            public StringID DefaultSoundEffect;

            [TagStructure(Size = 0x18)]
            public class Variant
            {
                public StringID Designation;
                public TagInstance Sound;
                public StringID SoundEffect;
            }
        }
    }
}
