using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using static TagTool.Commands.ArgumentParser;

namespace TagTool.Commands.Tags
{
    class NullTagCommand : Command
    {
        public OpenTagCache Info { get; }

        public NullTagCommand(OpenTagCache info)
            : base(CommandFlags.None,
                  "nulltag",
                  "Nulls a tag in the current tag cache.",
                  "nulltag <tag index>",
                  "Nulls a tag in the current tag index. The tag's data will be removed from cache.")
        {
            Info = info;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var tag = ParseTagIndex(Info, args[0]);

            if (tag == null)
            {
                Console.WriteLine($"ERROR: invalid tag index specified: {args[0]}");
                return false;
            }

            Console.Write($"Nulling {Info.StringIDs.GetString(tag.Group.Name)} tag 0x{tag.Index:X4}...");
            
            using (var stream = Info.OpenCacheReadWrite())
            {
                Info.Cache.SetTagDataRaw(stream, tag, new byte[] { });
                Info.Cache.Tags[tag.Index] = null;

                using (var writer = new BinaryWriter(stream))
                    Info.Cache.UpdateTagOffsets(writer);
            }

            Console.WriteLine("done.");

            return true;
        }
    }
}
