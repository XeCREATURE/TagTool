using HaloOnlineTagTool.Serialization;

namespace HaloOnlineTagTool.TagStructures
{
    [TagStructure(Name = "collision_damage", Class = "cddf", Size = 0x30)]
    public class CollisionDamage
    {
        public float ApplyDamageScale;
        public float ApplyRecoilDamageScale;
        public float DamageAccelerationMin;
        public float DamageAccelerationMax;
        public float DamageScaleMin;
        public float DamageScaleMin2;
        public float Unknown;
        public float Unknown2;
        public float RecoilDamageAccelerationMin;
        public float RecoilDamageAccelerationMax;
        public float RecoilDamageScaleMin;
        public float RecoilDamageScaleMax;
    }
}
