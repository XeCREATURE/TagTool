using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Common;
using TagTool.Definitions;
using TagTool.Cache;
using TagTool.Serialization;
using TagTool.TagGroups;
using static TagTool.Definitions.TagConverter;

namespace TagTool.Commands.Tags
{
    class ConvertCommand : Command
    {
        private OpenTagCache Info { get; }
        private bool IsDecalShader { get; set; } = false;

        public ConvertCommand(OpenTagCache info) : base(
            CommandFlags.None,

            "convert",
            "Convert a tag and its dependencies to another engine version",

            "convert [replace] <tag index> <tag map csv> <output csv> <target directory>",

            "The tag map CSV should be generated using the \"matchtags\" command.\n" +
            "If a tag is listed in the CSV file, it will not be converted.\n" +
            "The output CSV file is used for converting multiple maps.\n" +
            "Subsequent convert commands should use the new CSV.\n" +
            "The target directory should be the maps folder for the target engine.")
        {
            Info = info;
        }

        public override bool Execute(List<string> args)
        {
            if (!(args.Count == 4 || args.Count == 5))
                return false;
            
            var srcTag = ArgumentParser.ParseTagIndex(Info, args[0]);

            if (srcTag == null)
                return false;

            var csvPath = args[1];
            var csvOutPath = args[2];
            var targetDir = args[3];

            // Load the CSV
            Console.WriteLine("Reading {0}...", csvPath);
            TagCacheMap tagMap;
            using (var reader = new StreamReader(File.OpenRead(csvPath)))
                tagMap = TagCacheMap.ParseCsv(reader);
            
            // Load destination files
            Console.WriteLine("Loading the target tags.dat...");
            var destCachePath = Path.Combine(targetDir, "tags.dat");
            var destInfo = new OpenTagCache { CacheFile = new FileInfo(destCachePath) };
            using (var stream = destInfo.OpenCacheRead())
                destInfo.Cache = new TagCache(stream);

            // Do version detection
            DefinitionSet guessedVersion;
            destInfo.Version = Definition.Detect(destInfo.Cache, out guessedVersion);
            if (destInfo.Version == DefinitionSet.Unknown)
            {
                Console.WriteLine("Unrecognized target version!");
                return true;
            }
            Console.WriteLine("- Detected version {0}", Definition.GetVersionString(destInfo.Version));

            if (Info.Version != DefinitionSet.HaloOnline498295 && destInfo.Version != DefinitionSet.HaloOnline106708)
            {
                Console.Error.WriteLine("Conversion is only supported from 11.1.498295 Live to 1.106708 cert_ms23.");
                return true;
            }

            // Set up version-specific objects
            destInfo.Serializer = new TagSerializer(destInfo.Version);
            destInfo.Deserializer = new TagDeserializer(destInfo.Version);

            // Load stringIDs
            Console.WriteLine("Loading the target string_ids.dat...");

            var resolver = StringIDResolverFactory.Create(destInfo.Version);

            var destStringIDsPath = Path.Combine(targetDir, "string_ids.dat");
            destInfo.StringIDsFile = new FileInfo(destStringIDsPath);

            using (var stream = destInfo.StringIDsFile.OpenRead())
                destInfo.StringIDs = new StringIDCache(stream, resolver);

            // Load resources for the target build
            Console.WriteLine("Loading target resources...");
            var destResources = new ResourceDataManager();
            destResources.LoadCachesFromDirectory(destInfo.CacheFile.DirectoryName);

            // Load resources for our build
            Console.WriteLine("Loading source resources...");
            var srcResources = new ResourceDataManager();
            srcResources.LoadCachesFromDirectory(Info.CacheFile.DirectoryName);

            Console.WriteLine();
            Console.WriteLine("CONVERTING FROM VERSION {0} TO {1}", Definition.GetVersionString(Info.Version), Definition.GetVersionString(destInfo.Version));
            Console.WriteLine();

            TagInstance resultTag;
            using (Stream srcStream = Info.OpenCacheRead(), destStream = destInfo.OpenCacheReadWrite())
                resultTag = ConvertTag(srcTag, Info, srcStream, srcResources, destInfo, destStream, destResources, tagMap);

            Console.WriteLine();
            Console.WriteLine("Repairing decal systems...");
            FixDecalSystems(destInfo, resultTag.Index);

            Console.WriteLine();
            Console.WriteLine("Saving stringIDs...");
            using (var stream = destInfo.StringIDsFile.Open(FileMode.Open, FileAccess.ReadWrite))
                destInfo.StringIDs.Save(stream);

            Console.WriteLine("Writing {0}...", csvOutPath);
            using (var stream = new StreamWriter(File.Open(csvOutPath, FileMode.Create, FileAccess.ReadWrite)))
                tagMap.WriteCsv(stream);
            
            Console.WriteLine();
            Console.WriteLine("All done! The converted tag is:");
            TagPrinter.PrintTagShort(resultTag);
            return true;
        }
    }
}
