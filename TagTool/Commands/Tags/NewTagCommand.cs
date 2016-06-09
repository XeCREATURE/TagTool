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
                  "newtag",
                  "Creates a new tag of the specified tag group in the current tag cache.",
                  "newtag <group tag>",
                  "Creates a new tag of the specified tag group in the current tag cache.")
        {
            Info = info;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var groupTag = ArgumentParser.ParseGroupTag(Info.StringIDs, args[0]);

            if (groupTag == null || !TagGroup.Instances.ContainsKey(groupTag))
                return false;

            TagInstance instance;

            using (var stream = Info.OpenCacheReadWrite())
            {
                instance = Info.Cache.AllocateTag(TagGroup.Instances[groupTag]);
                var context = new TagSerializationContext(stream, Info.Cache, Info.StringIDs, instance);
                var data = Activator.CreateInstance(TagStructureTypes.FindByGroupTag(groupTag));
                Info.Serializer.Serialize(context, data);
            }

            var tagName = Info.TagNames.ContainsKey(instance.Index) ?
                Info.TagNames[instance.Index] :
                $"0x{instance.Index:X4}";

            Console.WriteLine($"[Index: 0x{instance.Index:X4}, Offset: 0x{instance.HeaderOffset:X8}, Size: 0x{instance.TotalSize:X4}] {tagName}.{Info.StringIDs.GetString(instance.Group.Name)}");

            return true;
        }
    }
}
