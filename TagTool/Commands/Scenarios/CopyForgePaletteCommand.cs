using System.Collections.Generic;
using System.IO;
using TagTool.Serialization;
using TagTool.TagStructures;

namespace TagTool.Commands.Scenarios
{
    class CopyForgePaletteCommand : Command
    {
        private OpenTagCache Info { get; }
        private Scenario Definition { get; }

        public CopyForgePaletteCommand(OpenTagCache info, Scenario definition)
            : base(CommandFlags.Inherit,
                 "CopyForgePalette",
                 "Copies the forge palette from the current scenario to another scenario.",
                 "CopyForgePalette <destination scenario>",
                 "Copies the forge palette from the current scenario to another scenario.")
        {
            Info = info;
            Definition = definition;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var destinationTag = ArgumentParser.ParseTagIndex(Info.Cache, args[0]);

            Scenario destinationScenario = null;

            using (var cacheStream = Info.CacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var scenarioContext = new TagSerializationContext(cacheStream, Info.Cache, Info.StringIDs, destinationTag);
                destinationScenario = Info.Deserializer.Deserialize<Scenario>(scenarioContext);
            }

            destinationScenario.SandboxBudget = Definition.SandboxBudget;
            destinationScenario.SandboxEquipment = Definition.SandboxEquipment;
            destinationScenario.SandboxGoalObjects = Definition.SandboxGoalObjects;
            destinationScenario.SandboxScenery = Definition.SandboxScenery;
            destinationScenario.SandboxSpawning = Definition.SandboxSpawning;
            destinationScenario.SandboxTeleporters = Definition.SandboxTeleporters;
            destinationScenario.SandboxVehicles = Definition.SandboxVehicles;
            destinationScenario.SandboxWeapons = Definition.SandboxWeapons;

            using (var cacheStream = Info.CacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var scenarioContext = new TagSerializationContext(cacheStream, Info.Cache, Info.StringIDs, destinationTag);
                Info.Serializer.Serialize(scenarioContext, destinationScenario);
            }

            return true;
        }
    }
}

