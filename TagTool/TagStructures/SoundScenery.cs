using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.TagStructures
{
    [TagStructure(Name = "sound_scenery", Class = "ssce", Size = 0x1C)]
    public class SoundScenery : GameObject
    {
        public float DistanceMin;
        public float DistanceMax;
        public Angle ConeAngleMin;
        public Angle ConeAngleMax;
        public uint Unknown6;
        public uint Unknown7;
        public uint Unknown8;
    }
}
