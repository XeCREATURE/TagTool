using System.Collections.Generic;
using System.IO;
using static TagTool.Commands.ArgumentParser;

namespace TagTool.Commands.Tags
{
    class NullTagCommand : Command
    {
        public OpenTagCache Info { get; }

        public NullTagCommand(OpenTagCache info)
            : base(CommandFlags.None,
                  "null_tag",
                  "Nulls a tag in the current tag cache.",
                  "null_tag <tag index>",
                  "Nulls a tag in the current tag index. The tag's data will be removed from cache.")
        {
            Info = info;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var tag = ParseTagIndex(Info.Cache, args[0]);

            if (tag == null)
                return false;

            Info.Cache.Tags[tag.Index] = null;

            using (var stream = Info.OpenCacheReadWrite())
            using (var writer = new BinaryWriter(stream))
            {
                Info.Cache.UpdateTagOffsets(writer);
            }

            return true;
        }
    }
}
