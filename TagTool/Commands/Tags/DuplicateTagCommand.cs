using System;
using System.Collections.Generic;
using TagTool.TagGroups;

namespace TagTool.Commands.Tags
{
    class DuplicateTagCommand : Command
    {
        private readonly OpenTagCache _info;

        public DuplicateTagCommand(OpenTagCache info) : base(
            CommandFlags.None,

            "duplicate",
            "Create a copy of a tag",

            "duplicate <tag index>",

            "All of the tag's data, including tag blocks, will be copied into a new tag.\n" +
            "The new tag can then be edited independently of the old tag.")
        {
            _info = info;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;
            var tag = ArgumentParser.ParseTagIndex(_info, args[0]);
            if (tag == null)
                return false;

            TagInstance newTag;
            using (var stream = _info.OpenCacheReadWrite())
                newTag = _info.Cache.DuplicateTag(stream, tag);

            Console.WriteLine("Tag duplicated successfully!");
            Console.Write("New tag: ");
            TagPrinter.PrintTagShort(newTag);
            return true;
        }
    }
}
