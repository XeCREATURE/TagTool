using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using TagTool.Tags;
using TagTool.TagStructures;

namespace TagTool.Commands.Unicode
{
    class GetCommand : Command
    {
        private OpenTagCache Info { get; }
        private TagInstance Tag { get; }
        private MultilingualUnicodeStringList Definition { get; }

        public GetCommand(OpenTagCache info, TagInstance tag, MultilingualUnicodeStringList unic)
            : base(CommandFlags.Inherit, "get", "", "get <language> <string_id>", "")
        {
            Info = info;
            Tag = tag;
            Definition = unic;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 2)
                return false;

            GameLanguage language;

            if (!ArgumentParser.ParseLanguage(args[0], out language))
                return false;

            var stringIdStr = args[1];
            var stringIdIndex = Info.StringIDs.Strings.IndexOf(stringIdStr);
            if (stringIdIndex < 0)
            {
                Console.WriteLine("Unable to find stringID \"{0}\".", stringIdStr);
                return true;
            }

            var stringId = Info.StringIDs.GetStringID(stringIdIndex);
            if (stringId == StringID.Null)
            {
                Console.WriteLine("Failed to resolve the stringID.");
                return true;
            }

            var localizedStr = Definition.Strings.FirstOrDefault(s => s.StringID == stringId);
            if (localizedStr == null)
            {
                Console.WriteLine("Unable to find unicode string \"{0}\"", stringIdStr);
                return true;
            }

            Console.WriteLine(Definition.GetString(localizedStr, language));

            return true;
        }
    }
}
