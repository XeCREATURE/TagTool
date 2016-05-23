using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Serialization;
using TagTool.TagDefinitions;

namespace TagTool.Commands.Scenarios
{
    class CopyForgePaletteCommand : Command
    {
        private OpenTagCache Info { get; }
        private Scenario Definition { get; }

        public CopyForgePaletteCommand(OpenTagCache info, Scenario definition)
            : base(CommandFlags.Inherit,
                 "copy_forge_palette",
                 "Copies the forge palette from the current scenario to another scenario",
                 "copy_forge_palette [palette = all] <destination scenario>",
                 "Copies the forge palette from the current scenario to another scenario")
        {
            Info = info;
            Definition = definition;
        }

        private List<string> ValidPalettes = new List<string>
        {
            "all",
            "equipment",
            "goal_objects",
            "scenery",
            "spawning",
            "teleporters",
            "vehicles",
            "weapons"
        };

        public override bool Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 2)
                return false;

            string palette = "all";

            if (args.Count == 2)
            {
                palette = args[0].ToLower();
                args.RemoveAt(0);
            }

            if (ValidPalettes.Contains(palette))
            {
                Console.WriteLine($"ERROR: invalid forge palette specified: {palette}");
                return false;
            }

            var destinationTag = ArgumentParser.ParseTagIndex(Info.Cache, args[0]);

            if (destinationTag == null || destinationTag.Group.Tag.ToString() != "scnr")
            {
                Console.WriteLine($"ERROR: invalid destination scenario index: {args[0]}");
                return false;
            }

            Console.Write("Loading destination scenario...");

            Scenario destinationScenario = null;
            
            using (var cacheStream = Info.CacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var scenarioContext = new TagSerializationContext(cacheStream, Info.Cache, Info.StringIDs, destinationTag);
                destinationScenario = Info.Deserializer.Deserialize<Scenario>(scenarioContext);
            }

            Console.WriteLine("done.");

            Console.Write("Copying specified forge palettes...");

            destinationScenario.SandboxBudget = Definition.SandboxBudget;

            if (palette == "all" || palette == "equipment")
                destinationScenario.SandboxEquipment = Definition.SandboxEquipment;

            if (palette == "all" || palette == "goal_objects")
                destinationScenario.SandboxGoalObjects = Definition.SandboxGoalObjects;

            if (palette == "all" || palette == "scenery")
                destinationScenario.SandboxScenery = Definition.SandboxScenery;

            if (palette == "all" || palette == "spawning")
                destinationScenario.SandboxSpawning = Definition.SandboxSpawning;

            if (palette == "all" || palette == "teleporters")
                destinationScenario.SandboxTeleporters = Definition.SandboxTeleporters;

            if (palette == "all" || palette == "vehicles")
                destinationScenario.SandboxVehicles = Definition.SandboxVehicles;

            if (palette == "all" || palette == "weapons")
                destinationScenario.SandboxWeapons = Definition.SandboxWeapons;

            Console.WriteLine("done.");

            Console.Write("Serializing destination scenario...");

            using (var cacheStream = Info.CacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var scenarioContext = new TagSerializationContext(cacheStream, Info.Cache, Info.StringIDs, destinationTag);
                Info.Serializer.Serialize(scenarioContext, destinationScenario);
            }

            Console.WriteLine("done.");

            return true;
        }
    }
}

