using System;
using System.Runtime.InteropServices;

namespace TagTool.Animations
{
    [StructLayout(LayoutKind.Sequential)]
    public class Animation
    {
        Int32 unknown;
        Int32 production_checksum;
        Int16 frame_count;
        Int16 node_count;
        sbyte unknown2;
        sbyte unknown3;
        Int32 frame_info_size;
        Int32 default_data_size;
        Int16 unknown4;
        Int32 frame_data_size;
        Int32 anim_size;
        Int32 unknown5;
        Int32 unknown6;
        Int32 fixup;
        Int32 unknown7;
    }

        /*
        public List<Part> Parts;
        public List<SubPart> SubParts;
        [TagField(Count = 10)] public ushort[] VertexBuffers;
        public sbyte RigidNodeIndex;
        //public MeshFlags Flags;
        public List<TagBlock20> Unknown34;
        public List<short> Unknown40;
        [TagStructure(Size = 0x10)]
        public class Part
        {
            public short FirstSubPartIndex;
            public byte UnknownD;
        }
        [TagStructure(Size = 0x10)]
        public class SubPart
        {
            public ushort FirstIndex;
        }
        [TagStructure(Size = 0x10)]
        public class TagBlock20
        {
            public int Unknown0;
        }*/
}
