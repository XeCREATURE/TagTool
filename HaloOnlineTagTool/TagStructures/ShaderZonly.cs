using HaloOnlineTagTool.Serialization;

namespace HaloOnlineTagTool.TagStructures
{
    [TagStructure(Name = "shader_zonly", Class = "rmzo", Size = 0x8)]
    public class ShaderZonly : RenderMethod
    {
        public uint Unknown8;
        public uint Unknown9;
    }
}
