using System;
using System.Collections.Generic;
using TagTool.TagGroups;
using TagTool.TagDefinitions;

namespace TagTool.Commands.RenderModels
{
    class GetResourceCommand : Command
    {
        private OpenTagCache Info { get; }
        private TagInstance Tag { get; }
        private RenderModel Definition { get; }

        public GetResourceCommand(OpenTagCache info, TagInstance tag, RenderModel definition)
            : base(CommandFlags.None,
                  "getresource",
                  "",
                  "getresource",
                  "")
        {
            Info = info;
            Tag = tag;
            Definition = definition;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            Console.WriteLine();
            Console.WriteLine($"[Location: {Definition.Geometry.Resource.GetLocation()}, Index: {Definition.Geometry.Resource.Index}, Compressed Size: {Definition.Geometry.Resource.CompressedSize}]");

            return true;
        }
    }
}
