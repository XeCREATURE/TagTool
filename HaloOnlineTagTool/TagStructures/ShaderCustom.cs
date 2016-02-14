using HaloOnlineTagTool.Serialization;

namespace HaloOnlineTagTool.TagStructures
{
    [TagStructure(Name = "shader_custom", Class = "rmcs", Size = 0x4)]
    public class ShaderCustom : RenderMethod
    {
        public StringId Material;
    }
}
