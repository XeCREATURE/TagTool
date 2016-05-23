using TagTool.Serialization;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "shader_halogram", Class = "rmhg", Size = 0x4)]
    public class ShaderHalogram : RenderMethod
    {
        public uint Unknown8;
    }
}
