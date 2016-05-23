using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "camo", Class = "cmoe", Size = 0x40)]
    public class Camo
    {
        public short Unknown;
        public short Unknown2;
        public StringID Unknown3;
        public uint Unknown4;
        public byte[] Unknown5;
        public StringID Unknown6;
        public uint Unknown7;
        public byte[] Unknown8;
    }
}
