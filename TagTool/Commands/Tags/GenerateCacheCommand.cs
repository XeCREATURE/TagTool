using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Commands.Tags
{
    class GenerateCacheCommand : Command
    {
        public GenerateCacheCommand()
            : base(CommandFlags.Inherit,
                  "generate_cache",
                  "Generates an empty set of cache files.",
                  "generate_cache <output directory>",
                  "Generates an empty set of cache files.")
        {
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var directory = args[0];

            if (!Directory.Exists(directory))
            {
                Console.Write("Destination directory does not exist. Create it? [y/n] ");
                var answer = Console.ReadLine().ToLower();

                if (answer.Length == 0 || !(answer.StartsWith("y") || answer.StartsWith("n")))
                    return false;

                if (answer.StartsWith("y"))
                    Directory.CreateDirectory(directory);
                else
                    return false;
            }

            Console.WriteLine($"Generating cache files in \"{directory}\"...");

            var tagCachePath = Path.Combine(directory, "tags.dat");

            Console.Write($"Generating {tagCachePath}...");
            using (var tagCacheStream = File.Create(tagCachePath))
            using (var writer = new BinaryWriter(tagCacheStream))
            {
                writer.Write((int)0); // padding
                writer.Write((int)0); // tag list offset
                writer.Write((int)0); // tag count
                writer.Write((int)0); // padding
                writer.Write((long)130713360239499012); // timestamp
                writer.Write((long)0); // padding
            }
            Console.WriteLine("done.");

            var stringIDCachePath = Path.Combine(directory, "string_ids.dat");

            Console.Write($"Generating {stringIDCachePath}...");
            using (var stringIDCacheStream = File.Create(stringIDCachePath))
            using (var writer = new BinaryWriter(stringIDCacheStream))
            {
                writer.Write((int)0); // string count
                writer.Write((int)0); // data size
            }
            Console.WriteLine("done.");

            var resourceCachePaths = new string[]
            {
                Path.Combine(directory, "audio.dat"),
                Path.Combine(directory, "resources.dat"),
                Path.Combine(directory, "textures.dat"),
                Path.Combine(directory, "textures_b.dat"),
                Path.Combine(directory, "video.dat")
            };

            foreach (var resourceCachePath in resourceCachePaths)
            {
                Console.Write($"Generating {resourceCachePath}...");
                using (var resourceCacheStream = File.Create(resourceCachePath))
                using (var writer = new BinaryWriter(resourceCacheStream))
                {
                    writer.Write((int)0); // padding
                    writer.Write((int)0); // table offset
                    writer.Write((int)0); // resource count
                    writer.Write((int)0); // padding
                }
                Console.WriteLine("done.");
            }

            Console.WriteLine($"Done generating cache files in \"{directory}\".");

            return true;
        }
    }
}
