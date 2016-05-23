using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TagTool.Cache;
using TagTool.Bitmaps;
using TagTool.Serialization;
using TagTool.TagDefinitions;
using TagTool.TagGroups;

namespace TagTool.Commands.Bitmaps
{
    class ImportCommand : Command
    {
        private OpenTagCache Info { get; }
        private TagInstance Tag { get; }
        private Bitmap Bitmap { get; }

        public ImportCommand(OpenTagCache info, TagInstance tag, Bitmap bitmap)
            : base(CommandFlags.None,
                  "import",
                  "Imports an image from a DDS file.",
                  "import <image index> <path>",
                  "The image index must be in hexadecimal.\n" +
                  "No conversion will be done on the data in the DDS file.\n" +
                  "The pixel format must be supported by the game.")
        {
            Info = info;
            Tag = tag;
            Bitmap = bitmap;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 2)
                return false;
            int imageIndex;
            if (!int.TryParse(args[0], NumberStyles.HexNumber, null, out imageIndex))
                return false;
            if (imageIndex < 0 || imageIndex >= Bitmap.Images.Count)
            {
                Console.Error.WriteLine("Invalid image index.");
                return true;
            }
            var imagePath = args[1];

            Console.WriteLine("Loading resource caches...");
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
            Console.WriteLine("Importing image data...");
            try
            {
                using (var imageStream = File.OpenRead(imagePath))
                {
                    var injector = new BitmapDdsInjector(resourceManager);
                    injector.InjectDds(Info.Serializer, Info.Deserializer, Bitmap, imageIndex, imageStream);
                }
                using (var tagsStream = Info.OpenCacheReadWrite())
                {
                    var tagContext = new TagSerializationContext(tagsStream, Info.Cache, Info.StringIDs, Tag);
                    Info.Serializer.Serialize(tagContext, Bitmap);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Importing image data failed: " + ex.Message);
                return true;
            }
            Console.WriteLine("Done!");
            return true;
        }
    }
}
