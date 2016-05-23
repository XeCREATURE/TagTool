using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.TagGroups;

namespace TagTool.Commands.Tags
{
    class ListCommand : Command
    {
        private OpenTagCache Info { get; }

        public ListCommand(OpenTagCache info)
            : base(CommandFlags.Inherit,
                  "list",
                  "Lists tag instances that are of the specified tag group.",
                  "list <group tag 1> ... <group tag n>",
                  "Lists tag instances that are of the specified tag group." +
                  "Multiple group tags to list tags from can be specified.\n" +
                  "Tags of a group which inherit from the given group tags will also\n" +
                  "be printed. If no group tag is specified, all tags in the current\n" +
                  "tag cache file will be listed.")
        {
            Info = info;
        }

        public override bool Execute(List<string> args)
        {
            var searchClasses = ArgumentParser.ParseGroupTags(Info.StringIDs, args);

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
                var tagName = Info.TagNames.ContainsKey(tag.Index) ?
                    Info.TagNames[tag.Index] :
                    $"0x{tag.Index:X4}";

                Console.WriteLine($"[Index: 0x{tag.Index:X4}, Offset: 0x{tag.HeaderOffset:X8}, Size: 0x{tag.TotalSize:X4}] {tagName}.{Info.StringIDs.GetString(tag.Group.Name)}");
            }

            return true;
        }
    }
}
