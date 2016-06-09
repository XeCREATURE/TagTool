using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Commands.Tags
{
    class ListUnreferencedTagsCommand : Command
    {
        public OpenTagCache Info { get; }

        public ListUnreferencedTagsCommand(OpenTagCache info)
            : base(CommandFlags.None,
                  "listunreferencedtags",
                  "Lists all unreferenced tags in the current tag cache",
                  "listunreferencedtags",
                  "Lists all unreferenced tags in the current tag cache")
        {
            Info = info;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            foreach (var tag in Info.Cache.Tags)
            {
                if (tag == null)
                    continue;

                var dependsOn = Info.Cache.Tags.NonNull().Where(t => t.Dependencies.Contains(tag.Index));

                if (dependsOn.Count() == 0)
                {
                    Console.Write($"{Info.TagNames[tag.Index]} ");
                    TagPrinter.PrintTagShort(tag);
                }
            }

            return true;
        }
    }
}
