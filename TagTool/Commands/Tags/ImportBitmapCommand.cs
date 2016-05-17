using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Common;
using TagTool.Cache;
using TagTool.Bitmaps;
using TagTool.Serialization;

namespace TagTool.Commands.Tags
{
    class ImportBitmapCommand : Command
    {
        private readonly OpenTagCache _info;
        private readonly TagCache _cache;

        public ImportBitmapCommand(OpenTagCache info) : base(
            CommandFlags.Inherit,

            "importbitmap",
            "Create a new bitmap tag from a DDS file",

            "importbitmap <tag> <dds file>",

            "The DDS file will be imported into textures.dat as a new resource.\n" +
            "Make sure to add the new bitmap tag as a dependency if you edit a shader!")
        {
            _cache = cache;
            _info = info;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 2)
                return false;
                
            var tag = ArgumentParser.ParseTagIndex(_cache, args[0]);
            var imagePath = args[1];

            Console.WriteLine("Loading textures.dat...");
            var resourceManager = new ResourceDataManager();
            resourceManager.LoadCacheFromDirectory(_info.CacheFile.DirectoryName, ResourceLocation.Textures);

            Console.WriteLine("Importing image...");
            var bitmap = new TagStructures.Bitmap
            {
                Flags = TagStructures.Bitmap.RuntimeFlags.UseResource,
                Sequences = new List<TagStructures.Bitmap.Sequence>
                {
                    new TagStructures.Bitmap.Sequence
                    {
                        FirstBitmapIndex = 0,
                        BitmapCount = 1
                    }
                },
                Images = new List<TagStructures.Bitmap.Image>
                {
                    new TagStructures.Bitmap.Image
                    {
                        Signature = new Tag("bitm").Value,
                        Unknown28 = -1,
                    }
                },
                Resources = new List<TagStructures.Bitmap.BitmapResource>
                {
                    new TagStructures.Bitmap.BitmapResource()
                }
            };
            using (var imageStream = File.OpenRead(imagePath))
            {
                var injector = new BitmapDdsInjector(resourceManager);
                injector.InjectDds(_info.Serializer, _info.Deserializer, bitmap, 0, imageStream);
            }

            Console.WriteLine("Creating a new tag...");

            using (var tagsStream = _info.OpenCacheReadWrite())
            {
                var tagContext = new TagSerializationContext(tagsStream, _info.Cache, _info.StringIDs, tag);
                _info.Serializer.Serialize(tagContext, bitmap);
            }

            Console.WriteLine();
            Console.WriteLine("All done! The new bitmap tag is:");
            TagPrinter.PrintTagShort(tag);
            return true;
        }
    }
}
