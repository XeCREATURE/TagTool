using System;
using System.Collections.Generic;
using TagTool.IO;

namespace TagTool.Commands.Core
{
    class DumpLogCommand : Command
    {
        public DumpLogCommand()
            : base(CommandFlags.Inherit,
                  "dump_log",
                  "Dumps the current log into the logs directory.",
                  "dump_log [name = hott_*_crash.log]",
                  "Dumps the current log into the logs directory.")
        {
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count > 1)
                return false;

            string path = args.Count == 0 ? null : args[0];
            var result = ConsoleHistory.Dump(path);

            Console.WriteLine("Successfully dumped log to '{0}'.", result);

            return true;
        }
    }
}
