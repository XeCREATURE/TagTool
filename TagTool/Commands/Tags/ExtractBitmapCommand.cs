using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Bitmaps;
using TagTool.Cache;
using TagTool.Serialization;

namespace TagTool.Commands.Tags
{
    class ExtractBitmapCommand : Command
    {
        private OpenTagCache Info { get; }

        public ExtractBitmapCommand(OpenTagCache info)
            : base(CommandFlags.None,
                  "extractbitmap",
                  "Extracts a bitmap to a file.",
                  "extractbitmap <tag index> <output directory>",
                  "Extracts a bitmap to a file.")
        {
            Info = info;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 2)
                return false;

            var tag = ArgumentParser.ParseTagIndex(Info, args[0]);

            if (tag == null)
                return false;

            var directory = args[1];

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

            Console.Write("Loading resource caches...");
            var resourceManager = new ResourceDataManager();
            try
            {
                resourceManager.LoadCachesFromDirectory(Info.CacheFile.DirectoryName);
            }
            catch
            {
                Console.WriteLine("Unable to load the resource .dat files.");
                Console.WriteLine("Make sure that they all exist and are valid.");
                return true;
            }
            Console.WriteLine("done.");

            var extractor = new BitmapDdsExtractor(resourceManager);

            using (var tagsStream = Info.OpenCacheRead())
            {
                try
                {
                    var tagContext = new TagSerializationContext(tagsStream, Info.Cache, Info.StringIDs, tag);
                    var bitmap = Info.Deserializer.Deserialize<TagDefinitions.Bitmap>(tagContext);
                    var ddsOutDir = directory;
                    if (bitmap.Images.Count > 1)
                    {
                        ddsOutDir = Path.Combine(directory, tag.Index.ToString("X8"));
                        Directory.CreateDirectory(ddsOutDir);
                    }
                    for (var i = 0; i < bitmap.Images.Count; i++)
                    {
                        var outPath = Path.Combine(ddsOutDir,
                            ((bitmap.Images.Count > 1) ? i.ToString() : tag.Index.ToString("X8")) + ".dds");
                        using (var outStream = File.Open(outPath, FileMode.Create, FileAccess.Write))
                        {
                            extractor.ExtractDds(Info.Deserializer, bitmap, i, outStream);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR: Failed to extract bitmap: " + ex.Message);
                }
            }

            var tagName = Info.TagNames.ContainsKey(tag.Index) ?
                Info.TagNames[tag.Index] :
                $"0x{tag.Index:X4}";

            Console.WriteLine($"Extracted [Index: 0x{tag.Index:X4}, Offset: 0x{tag.HeaderOffset:X8}, Size: 0x{tag.TotalSize:X4}] {tagName}.{Info.StringIDs.GetString(tag.Group.Name)} to '{directory}'");

            return true;
        }
    }
}
