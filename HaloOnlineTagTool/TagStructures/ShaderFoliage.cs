using HaloOnlineTagTool.Serialization;

namespace HaloOnlineTagTool.TagStructures
{
    [TagStructure(Name = "shader_foliage", Class = "rmfl", Size = 0x4)]
    public class ShaderFoliage : RenderMethod
    {
        public StringId Material;
    }
}
