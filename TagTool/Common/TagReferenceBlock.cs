using TagTool.Serialization;
using TagTool.TagGroups;

namespace TagTool.Common
{
    [TagStructure(Size = 0x10)]
    public class TagReferenceBlock
    {
        public TagInstance Reference;
    }
}
