using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.Common
{
    [TagStructure(Size = 0x10)]
    public class TagReferenceBlock
    {
        public TagInstance Reference;
    }
}
