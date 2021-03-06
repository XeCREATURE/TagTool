﻿using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;

namespace TagTool.Commands.Tags
{
    class ExtractCommand : Command
    {
        private OpenTagCache Info { get; }

        public ExtractCommand(OpenTagCache info) : base(
            CommandFlags.Inherit,

            "extract",
            "Extract a tag to a file",
            
            "extract <tag index> <filename>",
            
            "Use the \"import\" command to re-import an extracted tag.")
        {
            Info = info;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 2 && args.Count != 3)
                return false; // 3 arguments are allowed to prevent breaking old scripts that use "full"
            var tag = ArgumentParser.ParseTagIndex(Info, args[0]);
            if (tag == null)
                return false;
            var file = args[1];

            byte[] data;
            using (var stream = Info.OpenCacheRead())
                data = Info.Cache.ExtractTagRaw(stream, tag);

            using (var outStream = File.Open(file, FileMode.Create, FileAccess.Write))
            {
                outStream.Write(data, 0, data.Length);
                Console.WriteLine("Wrote 0x{0:X} bytes to {1}.", outStream.Position, file);
                Console.WriteLine("The tag's main struct will be at offset 0x{0:X}.", tag.MainStructOffset);
            }
            return true;
        }
    }
}
