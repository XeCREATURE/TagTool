﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Analysis;
using TagTool.Cache;
using TagTool.Layouts;
using TagTool.TagGroups;

namespace TagTool.Commands.Tags
{
    class GenerateLayoutsCommand : Command
    {
        private readonly TagCache _cache;
        private readonly OpenTagCache _info;

        public GenerateLayoutsCommand(OpenTagCache info) : base(
            CommandFlags.Inherit,

            "genlayouts",
            "Generate tag layouts",

            "genlayouts <tag index> <type> <output dir>",

            "Scans tags by index group in the file to guess tag layouts.\n" +
            "Layouts will be written to the output directory in the chosen format.\n" +
            "\n" +
            "Supported types: csharp, cpp")
        {
            _cache = info.Cache;
            _info = info;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 3)
                return false;
            var entry = args[0];
            var type = args[1];
            var outDir = args[2];
            TagLayoutWriter writer;
            switch (type)
            {
                case "csharp":
                    writer = new CSharpLayoutWriter();
                    break;
                case "cpp":
                    writer = new CppLayoutWriter();
                    break;
                default:
                    return false;
            }
            Directory.CreateDirectory(outDir);
            var count = 0;
            using (var stream = _info.OpenCacheRead())
            {
                //foreach (var groupTag in _cache.Tags.NonNull().Select(t => t.Group.Tag).Distinct())
                var groupTag = ArgumentParser.ParseTagIndex(_info, entry).Group.Tag;
                {
                    TagLayoutGuess layout = null;
                    TagInstance lastTag = null;
                    foreach (var tag in _cache.Tags.FindAllInGroup(groupTag))
                    {
                        Console.Write("Analyzing ");
                        TagPrinter.PrintTagShort(tag);

                        lastTag = tag;
                        var analyzer = new TagAnalyzer(_cache);
                        var data = _cache.ExtractTag(stream, tag);
                        var tagLayout = analyzer.Analyze(data);
                        if (layout != null)
                            layout.Merge(tagLayout);
                        else
                            layout = tagLayout;
                    }
                    if (layout != null && lastTag != null)
                    {
                        Console.WriteLine("Writing {0} layout", groupTag);
                        var name = _info.StringIDs.GetString(lastTag.Group.Name);
                        var tagLayout = LayoutGuessFinalizer.MakeLayout(layout, name, groupTag);
                        var path = Path.Combine(outDir, writer.GetSuggestedFileName(tagLayout));
                        writer.WriteLayout(tagLayout, path);
                        count++;
                    }
                }
            }
            Console.WriteLine("Successfully generated {0} layouts!", count);
            return true;
        }
    }
}
