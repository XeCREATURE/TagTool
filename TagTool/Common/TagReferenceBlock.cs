using TagTool.Serialization;

namespace TagTool.Common
{
    [TagStructure(Size = 0x10)]
    public class TagReferenceBlock
    {
        public TagInstance Reference;
    }
}
