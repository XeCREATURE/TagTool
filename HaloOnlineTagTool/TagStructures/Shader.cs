using HaloOnlineTagTool.Serialization;

namespace HaloOnlineTagTool.TagStructures
{
    [TagStructure(Name = "shader", Class = "rmsh", Size = 0x4)]
    public class Shader : RenderMethod
    {
        public StringId Material;
    }
}
