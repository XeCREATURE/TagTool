using TagTool.Serialization;

namespace TagTool.TagStructures
{
    [TagStructure(Name = "point_physics", Class = "pphy", Size = 0x40)]
    public class PointPhysics
    {
        public uint Flags;
        public float Unknown;
        public float Unknown2;
        public float Unknown3;
        public uint Unknown4;
        public uint Unknown5;
        public uint Unknown6;
        public uint Unknown7;
        public float Density;
        public float AirFriction;
        public float WaterFriction;
        public float SurfaceFriction;
        public float Elasticity;
        public uint Unknown8;
        public uint Unknown9;
        public uint Unknown10;
    }
}
