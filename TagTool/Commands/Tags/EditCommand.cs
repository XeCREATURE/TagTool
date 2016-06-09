using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands.Editing;
using TagTool.TagGroups;

namespace TagTool.Commands.Tags
{
    class EditCommand : Command
    {
        private readonly CommandContextStack _stack;
        private readonly TagCache _cache;
        private readonly OpenTagCache _info;

        public EditCommand(CommandContextStack stack, OpenTagCache info) : base(
            CommandFlags.None,

            "edit",
            "Edit tag-specific data",

            "edit <tag index>",

            "If the tag contains data which is supported by this program,\n" +
            "this command will make special tag-specific commands available\n" +
            "which can be used to edit or view the data in the tag.\n" +
            "\n" +
            "Currently-supported tag types: bitm, hlmt, unic, vfsl")
        {
            _stack = stack;
            _cache = info.Cache;
            _info = info;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;
            
            var tag = ArgumentParser.ParseTagIndex(_info, args[0]);

            if (tag == null)
                return false;

            var oldContext = _stack.Context;

            _stack.Push(EditTagContextFactory.Create(_stack, _info, tag));

            var groupName = _info.StringIDs.GetString(tag.Group.Name);
            var tagName = $"0x{tag.Index:X4}";

            if (_info.TagNames.ContainsKey(tag.Index))
            {
                tagName = _info.TagNames[tag.Index];
                tagName = $"(0x{tag.Index:X4}) {tagName.Substring(tagName.LastIndexOf('\\') + 1)}";
            }

            Console.WriteLine($"Tag {tagName}.{groupName} has been opened for editing.");
            Console.WriteLine("New commands are now available. Enter \"help\" to view them.");
            Console.WriteLine("Use \"exit\" to return to {0}.", oldContext.Name);

            return true;
        }

    }
}
