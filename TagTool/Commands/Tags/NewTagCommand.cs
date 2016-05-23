using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.TagDefinitions;
using TagTool.TagGroups;

namespace TagTool.Commands.Tags
{
    class NewTagCommand : Command
    {
        public OpenTagCache Info { get; }

        public NewTagCommand(OpenTagCache info)
            : base(CommandFlags.Inherit,
                  "new_tag",
                  "Creates a new tag of the specified tag group in the current tag cache.",
                  "new_tag <group tag> <group name>",
                  "Creates a new tag of the specified tag group in the current tag cache.")
        {
            Info = info;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 2)
                return false;

            var groupTag = ArgumentParser.ParseGroupTag(Info.StringIDs, args[0]);

            if (groupTag == null)
                return false;

            var groupName = Info.StringIDs.GetStringID(args[1]);

            var group = new TagGroup(groupTag, Tag.Null, Tag.Null, groupName);

            using (var stream = Info.OpenCacheReadWrite())
            {
                var instance = Info.Cache.AllocateTag(group);
                var context = new TagSerializationContext(stream, Info.Cache, Info.StringIDs, instance);
                var data = Activator.CreateInstance(TagStructureTypes.FindByGroupTag(groupTag));
                Info.Serializer.Serialize(context, data);
            }

            return true;
        }
    }
}
