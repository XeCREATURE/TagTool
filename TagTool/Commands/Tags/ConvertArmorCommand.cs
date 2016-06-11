using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Definitions;
using TagTool.Serialization;
using static System.Console;
using static System.IO.Path;
using static TagTool.Cache.StringIDResolverFactory;
using static TagTool.Commands.ArgumentParser;
using static TagTool.Common.TagVersionMap;
using static TagTool.Definitions.Definition;
using static TagTool.Definitions.DefinitionSet;

namespace TagTool.Commands.Tags
{
    class ConvertArmorCommand : Command
    {
        private OpenTagCache Info { get; }

        public ConvertArmorCommand(OpenTagCache info)
            : base(CommandFlags.None,
                  "convertarmor",
                  "",
                  "convertarmor <tag index> <input csv> <output csv> <dest cache dir>",
                  "")
        {
            Info = info;
        }
        
        public override bool Execute(List<string> args)
        {
            if (args.Count != 4)
                return false;

            var sourceTag = ParseTagIndex(Info, args[0]);
            if (sourceTag == null || !sourceTag.IsInGroup(new Tag("armr")))
            {
                WriteLine("Source tag must be of group 'armr'.");
                return false;
            }

            var inputCsv = new FileInfo(args[1]);
            if (!inputCsv.Exists)
            {
                WriteLine($"Input csv file does not exist: {inputCsv.FullName}");
                return false;
            }
            
            var outputCsv = new FileInfo(args[2]);

            var destDir = new DirectoryInfo(args[3]);
            if (!destDir.Exists)
            {
                WriteLine($"Destination cache directory does not exist: {destDir.FullName}");
                return false;
            }

            var destTagsFile = new FileInfo(Combine(destDir.FullName, "tags.dat"));
            if (!destTagsFile.Exists)
            {
                WriteLine($"Destination tag cache file does not exist: {destTagsFile.FullName}");
                return false;
            }

            var destStringIDsFile = new FileInfo(Combine(destDir.FullName, "string_ids.dat"));
            if (!destStringIDsFile.Exists)
            {
                WriteLine($"Destination string id cache file does not exist: {destStringIDsFile.FullName}");
                return false;
            }

            var destResourcesFile = new FileInfo(Combine(destDir.FullName, "resources.dat"));
            if (!destResourcesFile.Exists)
            {
                WriteLine($"Destination resource cache file does not exist: {destResourcesFile.FullName}");
                return false;
            }

            var destTexturesFile = new FileInfo(Combine(destDir.FullName, "textures.dat"));
            if (!destTexturesFile.Exists)
            {
                WriteLine($"Destination texture cache file does not exist: {destTexturesFile.FullName}");
                return false;
            }

            var destTexturesBFile = new FileInfo(Combine(destDir.FullName, "textures_b.dat"));
            if (!destTexturesBFile.Exists)
            {
                WriteLine($"Destination texture cache file does not exist: {destTexturesBFile.FullName}");
                return false;
            }

            var destAudioFile = new FileInfo(Combine(destDir.FullName, "audio.dat"));
            if (!destAudioFile.Exists)
            {
                WriteLine($"Destination audio cache file does not exist: {destAudioFile.FullName}");
                return false;
            }

            TagVersionMap tagMap;
            using (var reader = new StreamReader(inputCsv.OpenRead()))
                tagMap = ParseTagVersionMap(reader);

            TagCache destTagCache;
            using (var stream = destTagsFile.OpenRead())
                destTagCache = new TagCache(stream);
            
            DefinitionSet guessedVersion;
            var destVersion = Detect(destTagCache, out guessedVersion);
            if (destVersion == Unknown)
            {
                WriteLine($"Unrecognized target version! (guessed {GetVersionString(guessedVersion)})");
                return true;
            }

            WriteLine($"Destination cache version: {GetVersionString(destVersion)}");

            StringIDCache destStringIDCache;
            using (var stream = destStringIDsFile.OpenRead())
                destStringIDCache = new StringIDCache(stream, Create(destVersion));

            var destResources = new ResourceDataManager();
            destResources.LoadCachesFromDirectory(destDir.FullName);

            var srcResources = new ResourceDataManager();
            srcResources.LoadCachesFromDirectory(Info.CacheFile.DirectoryName);

            var destSerializer = new TagSerializer(destVersion);
            var destDeserializer = new TagDeserializer(destVersion);

            return true;
        }
    }
}
