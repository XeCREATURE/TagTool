using TagTool.Serialization;

namespace TagTool.TagStructures
{
    [TagStructure(Name = "device_machine", Class = "mach", Size = 0x18)]
    public class DeviceMachine : Device
    {
        public TypeValue Type;
        public ushort Flags3;
        public float DoorOpenTime;
        public float OcclusionBoundsMin;
        public float OcclusionBoundsMax;
        public CollisionResponseValue CollisionResponse;
        public short ElevatorNode;
        public PathfindingPolicyValue PathfindingPolicy;
        public short Unknown6;

        public enum TypeValue : short
        {
            Door,
            Platform,
            Gear,
        }

        public enum CollisionResponseValue : short
        {
            PauseUntilCrushed,
            ReverseDirections,
            Discs,
        }

        public enum PathfindingPolicyValue : short
        {
            Discs,
            Sectors,
            CutOut,
            None,
        }
    }
}
