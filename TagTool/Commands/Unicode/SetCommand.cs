using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.TagStructures;

namespace TagTool.Commands.Unicode
{
    class SetCommand : Command
    {
        private OpenTagCache Info { get; }
        private TagInstance Tag { get; }
        private MultilingualUnicodeStringList Definition { get; }

        public SetCommand(OpenTagCache info, TagInstance tag, MultilingualUnicodeStringList unic) : base(
            CommandFlags.None,

            "set",
            "Set the value of a string",

            "set <language> <stringid> <value>",

            "Sets the string associated with a stringID in a language.\n" +
            "Remember to put the string value in quotes if it contains spaces.\n" +
            "If the string does not exist, it will be added.")
        {
            Info = info;
            Tag = tag;
            Definition = unic;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 3)
                return false;

            GameLanguage language;
            if (!ArgumentParser.ParseLanguage(args[0], out language))
                return false;

            // Look up the stringID that was passed in
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
            var newValue = ArgumentParser.Unescape(args[2]);

            // Look up or create a localized string entry
            var localizedStr = Definition.Strings.FirstOrDefault(s => s.StringID == stringId);
            var added = false;
            if (localizedStr == null)
            {
                // Add a new string
                localizedStr = new LocalizedString { StringID = stringId, StringIDStr = stringIdStr };
                Definition.Strings.Add(localizedStr);
                added = true;
            }

            // Save the tag data
            Definition.SetString(localizedStr, language, newValue);
            using (var stream = Info.OpenCacheReadWrite())
                Info.Serializer.Serialize(new TagSerializationContext(stream, Info.Cache, Info.StringIDs, Tag), Definition);

            if (added)
                Console.WriteLine("String added successfully.");
            else
                Console.WriteLine("String changed successfully.");
            return true;
        }
    }
}
