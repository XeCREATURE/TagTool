using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Commands.Tags
{
    class ListCommand : Command
    {
        private OpenTagCache Info { get; }

        public ListCommand(OpenTagCache info) : base(
            CommandFlags.Inherit,

            "list",
            "List tags",
            
            "list [class...]",
            
            "class is a 4-character string identifying the tag class, e.g. \"proj\".\n" +
            "Multiple classes to list tags from can be specified.\n" +
            "Tags which inherit from the given classes will also be printed.\n" +
            "If no class is specified, all tags in the file will be listed.")
        {
            Info = info;
        }

        public override bool Execute(List<string> args)
        {
            var searchClasses = ArgumentParser.ParseTagClasses(Info.Cache, args);
            if (searchClasses == null)
                return false;

            TagInstance[] tags;
            if (args.Count > 0)
                tags = Info.Cache.Tags.FindAllInGroups(searchClasses).ToArray();
            else
                tags = Info.Cache.Tags.NonNull().ToArray();

            if (tags.Length == 0)
            {
                Console.Error.WriteLine("No tags found.");
                return true;
            }

            foreach (var tag in tags)
            {
                var tagName = $"0x{tag.Index:X4}";

                if (Info.TagNames.ContainsKey(tag.Index))
                {
                    tagName = Info.TagNames[tag.Index];
                    tagName = $"(0x{tag.Index:X4}) {tagName}";
                }

                Console.WriteLine("{0} {1} [Offset = 0x{2:X}, Size = 0x{3:X}]", tag.Group.Tag, tagName, tag.HeaderOffset, tag.TotalSize);
            }

            return true;
        }
    }
}
