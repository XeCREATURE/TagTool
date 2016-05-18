using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Bitmaps;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Definitions;
using TagTool.Serialization;

namespace TagTool.Commands.Cache
{
    class ListBitmapsCommand : Command
    {
        private readonly OpenTagCache _info;
        private readonly CacheBase _blamCache;

        public ListBitmapsCommand(OpenTagCache info, CacheBase blamCache)
            : base(CommandFlags.None, "listBitmaps", "", "listBitmaps <blam tag path>", "")
        {
            _info = info;
            _blamCache = blamCache;
        }

        public override bool Execute(List<string> args)
        {

            if (args.Count != 1)
                return false;

            CacheBase.IndexItem item = null;

            Console.WriteLine("Verifying blam shader tag...");

            var shaderName = args[0];

            foreach (var tag in _blamCache.IndexItems)
            {
                if ((tag.ParentClass == "rm") && tag.Filename == shaderName)
                {
                    item = tag;
                    break;
                }
            }

            if (item == null)
            {
                Console.WriteLine("Blam shader tag does not exist: " + shaderName);
                return false;
            }

            var renderMethod = DefinitionsManager.rmsh(_blamCache, item);

            var templateItem = _blamCache.IndexItems.Find(i =>
                i.ID == renderMethod.Properties[0].TemplateTagID);

            var template = DefinitionsManager.rmt2(_blamCache, templateItem);

            for (var i = 0; i < template.UsageBlocks.Count; i++)
            {
                var bitmItem = _blamCache.IndexItems.Find(j =>
                j.ID == renderMethod.Properties[0].ShaderMaps[i].BitmapTagID);
                Console.WriteLine(bitmItem);
            }

            return true;
        }
    }
}
