using TagTool.Serialization;

namespace TagTool.TagStructures
{
    [TagStructure(Name = "sound_dialogue_constants", Class = "spk!", Size = 0x30)]
    public class SoundDialogueConstants
    {
        public float AlmostNever;
        public float Rarely;
        public float Somewhat;
        public float Often;
        public uint Unknown;
        public uint Unknown2;
        public uint Unknown3;
        public uint Unknown4;
        public uint Unknown5;
        public uint Unknown6;
        public uint Unknown7;
        public uint Unknown8;
    }
}
