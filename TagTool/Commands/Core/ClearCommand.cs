using System;
using System.Collections.Generic;

namespace TagTool.Commands.Core
{
    class ClearCommand : Command
    {
        public ClearCommand()
            : base(CommandFlags.Inherit,
                  "clear",
                  "Clears the screen of all output",
                  "clear",
                  "Clears the screen of all output.")
        {
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count > 0)
                return false;

            Console.Clear();

            return true;
        }
    }
}