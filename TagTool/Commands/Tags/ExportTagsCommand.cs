using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Serialization;

namespace TagTool.Commands.Tags
{
    class ExportTagsCommand : Command
    {
        public OpenTagCache Info { get; }

        public ExportTagsCommand(OpenTagCache info)
            : base(CommandFlags.None,
                  "export_tags",
                  "Exports all tags in the current tag cache to a specific directory.",
                  "export_tags <output directory>",
                  "Exports all tags in the current tag cache to a specific directory.")
        {
            Info = info;
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

            using (var cacheStream = Info.OpenCacheRead())
            {
                foreach (var instance in Info.Cache.Tags)
                {
                    if (instance == null)
                        continue;

                    var tagName = Info.TagNames[instance.Index] + "." + Info.StringIDs.GetString(instance.Group.Name);
                    var tagPath = Path.Combine(directory, tagName);
                    var tagDirectory = Path.GetDirectoryName(tagPath);

                    if (!Directory.Exists(tagDirectory))
                        Directory.CreateDirectory(tagDirectory);

                    using (var tagStream = File.Create(tagPath))
                    using (var writer = new BinaryWriter(tagStream))
                    {
                        writer.Write(Info.Cache.ExtractTagRaw(cacheStream, instance));
                    }

                    Console.WriteLine($"Exported {tagName}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Done!");

            return true;
        }
    }
}
