using System.Collections.Generic;

namespace TagTool.Commands.Core
{
    class SetVarCommand : Command
    {
        private OpenTagCache Info { get; }

        public SetVarCommand(OpenTagCache info)
            : base(CommandFlags.Inherit,
                  "setvar",
                  "Assigns a value to a variable.",
                  "setvar <name> <value>",
                  "Assigns a value to a tag tool global variable, which can be accessed via $name")
        {
            Info = info;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 2)
                return false;

            string value;
            if (args[1].Contains(" "))
                value = $"\"{args[1]}\"";
            else
                value = args[1];

            if (value == "*")
                value = $"{ArgumentParser.ParseTagIndex(Info, value).Index:X4}";

            ArgumentParser.Variables[args[0]] = value;

            return true;
        }
    }
}
