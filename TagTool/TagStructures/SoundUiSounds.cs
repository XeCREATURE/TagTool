using System.Collections.Generic;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.TagStructures
{
    [TagStructure(Name = "sound_ui_sounds", Class = "sus!", Size = 0x10)]
    public class SoundUiSounds
    {
        public List<UiSound> UiSounds;
        public uint Unknown;

        [TagStructure(Size = 0x10)]
        public class UiSound
        {
            public TagInstance Sound;
        }
    }
}
