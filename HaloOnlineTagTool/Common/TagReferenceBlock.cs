using HaloOnlineTagTool.Serialization;

namespace HaloOnlineTagTool.Common
{
    [TagStructure(Size = 0x10)]
    public class TagReferenceBlock
    {
        public TagInstance Reference;
    }
}
