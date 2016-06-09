using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TagTool.Cache;
using TagTool.Common;
using TagTool.TagGroups;
using TagTool.TagDefinitions;

namespace TagTool.Commands
{
    static class ArgumentParser
    {
        public static Dictionary<string, string> Variables { get; } = new Dictionary<string, string>();

        public static List<string> ParseCommand(string command, out string redirectFile)
        {
            var results = new List<string>();
            var currentArg = new StringBuilder();
            var partStart = -1;
            var quoted = false;
            var redirectStart = -1;
            redirectFile = null;
            for (var i = 0; i < command.Length; i++)
            {
                switch (command[i])
                {
                    case '>':
                        if (quoted)
                            goto default; // Treat like a normal char when quoted
                        redirectStart = (partStart != -1) ? results.Count + 1 : results.Count;
                        goto case ' '; // Treat like a space
                    case ' ':
                        if (quoted)
                            goto default; // Treat like a normal char when quoted
                        if (partStart != -1)
                            currentArg.Append(command.Substring(partStart, i - partStart));
                        if (currentArg.Length > 0)
                        {
                            var arg = currentArg.ToString();
                            if (arg.StartsWith("$"))
                            {
                                var arg2 = arg.Substring(1);
                                if (arg2.Length != 0 && Variables.ContainsKey(arg2))
                                    arg = Variables[arg2];
                            }
                            results.Add(arg);
                        }
                        currentArg.Clear();
                        partStart = -1;
                        break;
                    case '"':
                        quoted = !quoted;
                        if (partStart != -1)
                            currentArg.Append(command.Substring(partStart, i - partStart));
                        partStart = -1;
                        break;
                    default:
                        if (partStart == -1)
                            partStart = i;
                        break;
                }
            }
            if (partStart != -1)
                currentArg.Append(command.Substring(partStart));
            if (currentArg.Length > 0)
            {
                var arg = currentArg.ToString();
                if (arg.StartsWith("$"))
                {
                    var arg2 = arg.Substring(1);
                    if (arg2.Length != 0 && Variables.ContainsKey(arg2))
                        arg = Variables[arg2];
                }
                results.Add(arg);
            }
            if (redirectStart >= 0 && redirectStart < results.Count)
            {
                redirectFile = string.Join(" ", results.Skip(redirectStart));
                results.RemoveRange(redirectStart, results.Count - redirectStart);
            }
            return results;
        }

        public static TagInstance ParseTagName(OpenTagCache info, string name)
        {
            if (name.Length == 0 || !char.IsLetter(name[0]) || !name.Contains('.'))
                throw new Exception($"Invalid tag name: {name}");

            var namePieces = name.Split('.');

            var groupTag = ParseGroupTag(info.StringIDs, namePieces[1]);
            if (groupTag == Tag.Null)
                throw new Exception($"Invalid tag name: {name}");

            var tagName = namePieces[0];
            
            foreach (var nameEntry in info.TagNames)
            {
                if (nameEntry.Value == tagName)
                {
                    var instance = info.Cache.Tags[nameEntry.Key];

                    if (instance.Group.Tag == groupTag)
                        return instance;
                }
            }

            Console.WriteLine($"Invalid tag name: {name}");
            return null;
        }

        public static TagInstance ParseTagIndex(OpenTagCache info, string arg)
        {
            if (!(arg == "*" || arg == "null" || char.IsLetter(arg[0]) || arg.StartsWith("0x")))
            {
                Console.WriteLine($"Invalid tag index specifier: {arg}");
                return null;
            }

            if (arg == "*")
                return info.Cache.Tags.Last();
            else if (arg == "null")
                return null;
            else if (char.IsLetter(arg[0]))
                return ParseTagName(info, arg);
            else if (arg.StartsWith("0x"))
                arg = arg.Substring(2);

            int tagIndex;
            if (!int.TryParse(arg, NumberStyles.HexNumber, null, out tagIndex))
                return null;

            if (!info.Cache.Tags.Contains(tagIndex))
            {
                Console.WriteLine("Unable to find tag {0:X8}.", tagIndex);
                return null;
            }

            return info.Cache.Tags[tagIndex];
        }

        public static Tag ParseGroupTag(StringIDCache stringIDs, string groupName)
        {
            if (TagStructureTypes.IsGroupTag(groupName))
                return new Tag(groupName);

            foreach (var pair in TagGroup.Instances)
            {
                if (groupName == stringIDs.GetString(pair.Value.Name))
                    return pair.Value.Tag;
            }

            return Tag.Null;
        }

        public static List<Tag> ParseGroupTags(StringIDCache stringIDs, IEnumerable<string> classNames)
        {
            var searchClasses = classNames.Select(a => ParseGroupTag(stringIDs, a)).ToList();

            return (searchClasses.Any(c => c.Value == -1)) ? null : searchClasses;
        }

        public static bool ParseLanguage(string name, out GameLanguage result) =>
            _languages.TryGetValue(name, out result);

        public static string Unescape(string str)
        {
            var result = new StringBuilder();
            var escape = false;
            foreach (var ch in str)
            {
                if (!escape)
                {
                    if (ch == '\\')
                        escape = true;
                    else
                        result.Append(ch);
                    continue;
                }
                escape = false;
                switch (ch)
                {
                    case 'n':
                        result.Append('\n');
                        break;
                    case 'r':
                        result.Append('\r');
                        break;
                    case 't':
                        result.Append('\t');
                        break;
                    case 'q':
                        result.Append('"');
                        break;
                    case '\\':
                        result.Append('\\');
                        break;
                    default:
                        result.Append(ch);
                        break;
                }
            }
            return result.ToString();
        }

        private static readonly Dictionary<string, GameLanguage> _languages = new Dictionary<string, GameLanguage>
        {
            {"english", GameLanguage.English},
            {"japanese", GameLanguage.Japanese},
            {"german", GameLanguage.German},
            {"french", GameLanguage.French},
            {"spanish", GameLanguage.Spanish},
            {"mexican", GameLanguage.Mexican},
            {"italian", GameLanguage.Italian},
            {"korean", GameLanguage.Korean},
            {"chinese-trad", GameLanguage.ChineseTraditional},
            {"chinese-simp", GameLanguage.ChineseSimplified},
            {"portuguese", GameLanguage.Portuguese},
            {"russian", GameLanguage.Russian}
        };
    }
}
