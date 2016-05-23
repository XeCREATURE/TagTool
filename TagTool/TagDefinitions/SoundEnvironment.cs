using TagTool.Serialization;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "sound_environment", Class = "snde", Size = 0x50)]
    public class SoundEnvironment
    {
        public uint Unknown;
        public short Priority;
        public short Unknown2;
        public float RoomIntensity;
        public float RoomIntensityHighFrequency;
        public float RoomRolloff;
        public float DecayTime;
        public float DecayHighFrequencyRatio;
        public float ReflectionsIntensity;
        public float ReflectionsDelay;
        public float ReverbIntensity;
        public float ReverbDelay;
        public float Diffusion;
        public float Density;
        public float HighFrequencyRefrence;
        public uint Unknown3;
        public uint Unknown4;
        public uint Unknown5;
        public uint Unknown6;
        public uint Unknown7;
        public uint Unknown8;
    }
}
