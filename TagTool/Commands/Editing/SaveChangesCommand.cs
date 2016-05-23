using System;
using System.Collections.Generic;
using TagTool.Serialization;
using TagTool.TagGroups;

namespace TagTool.Commands.Editing
{
    class SaveChangesCommand : Command
    {
        private OpenTagCache Info { get; }
        private TagInstance Tag { get; }
        private object Value { get; }

        public SaveChangesCommand(OpenTagCache info, TagInstance tag, object value)
            : base(CommandFlags.Inherit,
                  "save_changes",
                  $"Saves changes made to the current {info.StringIDs.GetString(tag.Group.Name)} definition.",
                  "save_changes",
                  $"Saves changes made to the current {info.StringIDs.GetString(tag.Group.Name)} definition.")
        {
            Info = info;
            Tag = tag;
            Value = value;
        }

        public override bool Execute(List<string> args)
        {
            using (var stream = Info.OpenCacheReadWrite())
            {
                var context = new TagSerializationContext(stream, Info.Cache, Info.StringIDs, Tag);
                Info.Serializer.Serialize(context, Value);
            }

            Console.WriteLine("Done!");

            return true;
        }
    }
}
