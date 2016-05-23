using System.Collections.Generic;
using TagTool.Serialization;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "camera_track", Class = "trak", Size = 0x14)]
    public class CameraTrack
    {
        public uint Unknown;
        public List<CameraPoint> CameraPoints;
        public uint Unknown2;

        [TagStructure(Size = 0x1C)]
        public class CameraPoint
        {
            public float PositionI;
            public float PositionJ;
            public float PositionK;
            public float OrientationI;
            public float OrientationJ;
            public float OrientationK;
            public float OrientationW;
        }
    }
}
