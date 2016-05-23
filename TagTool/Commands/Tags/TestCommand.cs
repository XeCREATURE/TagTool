using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using TagTool.TagGroups;

namespace TagTool.Commands.Tags
{
    class TestCommand : Command
    {
        public OpenTagCache Info { get; }

        public TestCommand(OpenTagCache info)
            : base(CommandFlags.Inherit, "test", "", "test", "")
        {
            Info = info;
        }
        
        public override bool Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            var groups = new Dictionary<Tag, TagGroup>();

            using (var stream = File.Create("groups.txt"))
            using (var writer = new StreamWriter(stream))
            {
                foreach (var t in Info.Cache.Tags)
                {
                    if (t == null)
                        continue;

                    if (!groups.ContainsKey(t.Group.Tag))
                        groups[t.Group.Tag] = t.Group;
                }

                foreach (var entry in groups)
                {
                    var group = entry.Value;

                    writer.WriteLine($"{group.Tag} | {group.ParentTag} | {group.GrandparentTag}");
                }
            }

            return true;
        }
    }
}
