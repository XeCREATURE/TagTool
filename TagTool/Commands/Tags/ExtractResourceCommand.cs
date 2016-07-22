using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.TagDefinitions;
using TagTool.Serialization;

namespace TagTool.Commands.Tags
{
    class ExtractResourceCommand : Command
    {
        public OpenTagCache Info { get; }

        public ExtractResourceCommand(OpenTagCache info) : base(
            CommandFlags.None,

            "extractresource",
            "Extract the raw resource referenced by a bitm, jmad, or mode tag.",

            "extractresource <cache file> <tag> <output file> <permutation index (bitm or jmad only)>\n",

            "Example: extractresource .\resources.dat 0x3317 street_cone.raw 0\n")
        {
            Info = info;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 3 && args.Count != 4)
                return false;

            var cachePath = args[0];
            var tagIndex = args[1];
            var dataPath = args[2];
            int permutationIndex = Int32.Parse(args[3]);
            var tag = ArgumentParser.ParseTagIndex(Info, tagIndex);
            if (tag.IsInGroup("bitm") && cachePath.Contains("textures.dat")) { ExtractBitmapResource(tagIndex, cachePath, dataPath, permutationIndex); }
            else if (tag.IsInGroup("mode") && cachePath.Contains("resources.dat")) { ExtractModelResource(tagIndex, cachePath, dataPath); }
            else if (tag.IsInGroup("jmad") && cachePath.Contains("resources.dat")) { ExtractAnimationResource(tagIndex, cachePath, dataPath, permutationIndex); }
            else return false;
            return true;
        }

        private void ExtractBitmapResource(string tagIndex, string cachePath, string dataPath, int permutationIndex)
        {
            var tag = ArgumentParser.ParseTagIndex(Info, tagIndex);
            int resourceIndex;
            Bitmap bitmap;
            using (var tagsStream = Info.OpenCacheRead())
            {
                var tagContext = new TagSerializationContext(tagsStream, Info.Cache, Info.StringIDs, tag);
                bitmap = Info.Deserializer.Deserialize<Bitmap>(tagContext);
            }
            try
            {
                resourceIndex = bitmap.Resources[permutationIndex].Resource.Index;
                try
                {
                    using (var stream = File.OpenRead(Info.CacheFile.DirectoryName + "\\" + cachePath))
                    {
                        var cache = new ResourceCache(stream);
                        using (var outStream = File.Open(dataPath, FileMode.Create, FileAccess.Write))
                        {
                            cache.Decompress(stream, resourceIndex, 0xFFFFFFFF, outStream);
                            Console.WriteLine("Wrote 0x{0:X} bytes to {1}. Resource index: {2}, 0x{3}.", outStream.Position, dataPath, resourceIndex, resourceIndex.ToString("X"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to extract resource: {0}", ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to extract resource: {0}", ex.Message);
            }
        }

        private void ExtractModelResource(string tagIndex, string cachePath, string dataPath)
        {
            var tag = ArgumentParser.ParseTagIndex(Info, tagIndex);
            int resourceIndex;
            RenderModel model;
            using (var tagsStream = Info.OpenCacheRead())
            {
                var tagContext = new TagSerializationContext(tagsStream, Info.Cache, Info.StringIDs, tag);
                model = Info.Deserializer.Deserialize<RenderModel>(tagContext);
            }
            resourceIndex = model.Geometry.Resource.Index;

            try
            {
                using (var stream = File.OpenRead(Info.CacheFile.DirectoryName + "\\" + cachePath))
                {
                    var cache = new ResourceCache(stream);
                    using (var outStream = File.Open(dataPath, FileMode.Create, FileAccess.Write))
                    {
                        cache.Decompress(stream, resourceIndex, 0xFFFFFFFF, outStream);
                        Console.WriteLine("Wrote 0x{0:X} bytes to {1}. Resource index: {2}, 0x{3}.", outStream.Position, dataPath, resourceIndex, resourceIndex.ToString("X"));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to extract resource: {0}", ex.Message);
            }
        }

        private void ExtractAnimationResource(string tagIndex, string cachePath, string dataPath, int permutationIndex)
        {
            var tag = ArgumentParser.ParseTagIndex(Info, tagIndex);
            int resourceIndex;
            ModelAnimationGraph animation;

            using (var tagsStream = Info.OpenCacheRead())
            {
                var tagContext = new TagSerializationContext(tagsStream, Info.Cache, Info.StringIDs, tag);
                animation = Info.Deserializer.Deserialize<ModelAnimationGraph>(tagContext);
            }

            try
            {
                using (var stream = File.OpenRead(Info.CacheFile.DirectoryName + "\\" + cachePath))
                {
                    var cache = new ResourceCache(stream);
                    using (var outStream = File.Open(dataPath, FileMode.Create, FileAccess.Write))
                    {
                        try
                        {
                            resourceIndex = animation.ResourceGroups[permutationIndex].Resource.Index;
                            cache.Decompress(stream, resourceIndex, 0xFFFFFFFF, outStream);
                            Console.WriteLine("Wrote 0x{0:X} bytes to {1}. Resource index: {2}, 0x{3}.", outStream.Position, dataPath, resourceIndex, resourceIndex.ToString("X"));

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Failed to extract resource: {0}", ex.Message);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to extract resource: {0}", ex.Message);
            }
        }
    }
}
