using System.Collections.Generic;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.TagStructures
{
    [TagStructure(Name = "cache_file_global_tags", Class = "cfgt", Size = 0x10)]
    public class CacheFileGlobalTags
    {
        public List<TagInstance> GlobalTags;
        public uint Unknown;
    }
}
