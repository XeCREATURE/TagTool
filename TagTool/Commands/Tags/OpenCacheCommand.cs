using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Commands.Cache;
using TagTool.Definitions;

namespace TagTool.Commands.Tags
{
    class OpenCacheCommand : Command
    {
        private CommandContextStack Stack { get; }
        private OpenTagCache Info { get; }

        public OpenCacheCommand(CommandContextStack stack, OpenTagCache info)
            : base(CommandFlags.Inherit,
                  "opencache",
                  "Opens a cache file format comparison and porting",
                  "opencache <cache file>",
                  "Opens a cache file format comparison and porting")
        {
            Stack = stack;
            Info = info;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var mapFilePath = args[0];

            if (!File.Exists(mapFilePath))
            {
                Console.WriteLine("Map file does not exist: " + mapFilePath);
                return false;
            }

            Stack.Push(CacheContextFactory.Create(Stack.Context, Info, CacheManager.GetCache(mapFilePath)));

            return true;
        }
    }
}
