using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Commands.Tags
{
    class ListNullTagsCommand : Command
    {
        public OpenTagCache Info { get; }

        public ListNullTagsCommand(OpenTagCache info)
            : base(CommandFlags.None,
                  "listnulltags",
                  "Lists all null tag indices in the current tag cache",
                  "listnulltags",
                  "Lists all null tag indices in the current tag cache")
        {
            Info = info;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            for (var i = 0; i < Info.Cache.Tags.Count; i++)
            {
                if (Info.Cache.Tags[i] == null)
                    Console.WriteLine($"0x{i:X4}");
            }

            return true;
        }
    }
}
