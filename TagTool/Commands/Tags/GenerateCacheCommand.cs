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
using TagTool.TagGroups;
using static System.Console;
using static System.IO.Directory;
using static System.IO.Path;
using static TagTool.Cache.StringIDResolverFactory;
using static TagTool.Definitions.Definition;
using static TagTool.Definitions.DefinitionSet;

namespace TagTool.Commands.Tags
{
    class GenerateCacheCommand : Command
    {
        private OpenTagCache Info { get; }

        public GenerateCacheCommand(OpenTagCache info)
            : base(CommandFlags.Inherit,
                  "generatecache",
                  "Generates an empty set of cache files.",
                  "generatecache <output directory>",
                  "Generates an empty set of cache files.")
        {
            Info = info;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var destDir = new DirectoryInfo(args[0]);
            if (!destDir.Exists)
            {
                Write("Destination directory does not exist. Create it? [y/n] ");
                var answer = ReadLine().ToLower();

                if (answer.Length == 0 || !(answer.StartsWith("y") || answer.StartsWith("n")))
                    return false;

                if (answer.StartsWith("y"))
                    destDir.Create();
                else
                    return false;
            }

            WriteLine($"Generating cache files in \"{destDir.FullName}\"...");

            var destTagsFile = new FileInfo(Combine(destDir.FullName, "tags.dat"));

            Write($"Generating {destTagsFile.FullName}...");
            using (var tagCacheStream = destTagsFile.Create())
            using (var writer = new BinaryWriter(tagCacheStream))
            {
                writer.Write((int)0); // padding
                writer.Write((int)0); // tag list offset
                writer.Write((int)0); // tag count
                writer.Write((int)0); // padding
                writer.Write((long)130713360239499012); // timestamp
                writer.Write((long)0); // padding
            }
            WriteLine("done.");

            var destStringIDsFile = new FileInfo(Combine(destDir.FullName, "string_ids.dat"));

            Write($"Generating {destStringIDsFile.FullName}...");
            using (var stringIDCacheStream = destStringIDsFile.Create())
            using (var writer = new BinaryWriter(stringIDCacheStream))
            {
                writer.Write((int)0); // string count
                writer.Write((int)0); // data size
            }
            WriteLine("done.");

            var resourceCachePaths = new string[]
            {
                Combine(destDir.FullName, "audio.dat"),
                Combine(destDir.FullName, "resources.dat"),
                Combine(destDir.FullName, "textures.dat"),
                Combine(destDir.FullName, "textures_b.dat"),
                Combine(destDir.FullName, "video.dat")
            };

            foreach (var resourceCachePath in resourceCachePaths)
            {
                Write($"Generating {resourceCachePath}...");
                using (var resourceCacheStream = File.Create(resourceCachePath))
                using (var writer = new BinaryWriter(resourceCacheStream))
                {
                    writer.Write((int)0); // padding
                    writer.Write((int)0); // table offset
                    writer.Write((int)0); // resource count
                    writer.Write((int)0); // padding
                }
                WriteLine("done.");
            }

            var dependencies = new Dictionary<int, TagInstance>();
            LoadTagDependencies(0, ref dependencies);
            LoadTagDependencies(0x16, ref dependencies);
            LoadTagDependencies(0x27D7, ref dependencies);
            
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

            var destInfo = new OpenTagCache
            {
                Cache = destTagCache,
                CacheFile = destTagsFile,
                StringIDs = destStringIDCache,
                StringIDsFile = destStringIDsFile,
                Version = destVersion,
                Serializer = destSerializer,
                Deserializer = destDeserializer
            };

            using (Stream srcStream = Info.OpenCacheRead(), destStream = destInfo.OpenCacheReadWrite())
            {
                var maxDependency = dependencies.Keys.Max();
                for (var i = 0; i <= maxDependency; i++)
                {
                    var srcTag = Info.Cache.Tags[i];
                    if (srcTag == null)
                    {
                        destInfo.Cache.AllocateTag();
                        continue;
                    }

                    var srcData = Info.Cache.ExtractTagRaw(srcStream, srcTag);

                    var destTag = destInfo.Cache.AllocateTag(srcTag.Group);
                    destInfo.Cache.SetTagDataRaw(destStream, destTag, srcData);

                    srcData = new byte[0];
                }
            }
            
            WriteLine($"Done generating cache files in \"{destDir.FullName}\".");

            return true;
        }

        private void LoadTagDependencies(int index, ref Dictionary<int, TagInstance> tags)
        {
            var queue = new List<int> { index };

            while (queue.Count != 0)
            {
                var nextQueue = new List<int>();

                foreach (var entry in queue)
                {
                    if (!tags.ContainsKey(entry))
                    {
                        tags[entry] = Info.Cache.Tags[entry];
                        foreach (var dependency in tags[entry].Dependencies)
                            if (!nextQueue.Contains(dependency))
                                nextQueue.Add(dependency);
                    }
                }

                queue = nextQueue;
            }
        }
    }
}
