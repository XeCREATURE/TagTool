using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.TagDefinitions;
using TagTool.Serialization;
using TagTool.TagGroups;

namespace TagTool.Commands.Tags
{
    class ImportResourceCommand : Command
    {
        private readonly OpenTagCache _info;
        private readonly TagCache _cache;
        private readonly FileInfo _fileInfo;
        private readonly StringIDCache _stringIds;

        public ImportResourceCommand(OpenTagCache info) : base(
            CommandFlags.Inherit,

            "importresource",
            "Import as a new resource for a specified tag.",

            "importresource <cache file> <tag> <input file>\n",

            "For bitm, it only works with permutations. The path must end with a folder, and not a file.\n")
        {
            _info = info;
            _cache = info.Cache;
            _fileInfo = info.CacheFile;
            _stringIds = info.StringIDs;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 3)
                return false;
            TagInstance tagIndex = _cache.Tags[0x3317];
            string cachePath = args[0];
            tagIndex = ArgumentParser.ParseTagIndex(_info, args[1]);
            string dataPath = args[2];
            // int flag = Int32.Parse(args[3]); // jmad location flag

            if (tagIndex.IsInGroup("mode") && cachePath.Contains("resources.dat")) { ImportModelResource(tagIndex, cachePath, dataPath); }
            else if (tagIndex.IsInGroup("bitm") && cachePath.Contains("textures.dat")) { ImportBitmapResource(tagIndex, cachePath, dataPath); }
            else if (tagIndex.IsInGroup("snd!") && cachePath.Contains("audio.dat")) { ImportSoundResource(tagIndex, cachePath, dataPath); }
            else if (tagIndex.IsInGroup("jmad") && cachePath.Contains("resources.dat")) { ImportAnimationResource(tagIndex, cachePath, dataPath); }
            else return false;

            return true;
        }

        private bool ImportModelResource(TagInstance tagIndex, string cachePath, string dataPath)
        {
            RenderModel renderModel;
            ResourceCache resourceCache;
            uint compressedSize = 0;
            var data = File.ReadAllBytes(dataPath);

            using (var cacheStream = _info.OpenCacheReadWrite())
            {
                var tagContext = new TagSerializationContext(cacheStream, _info.Cache, _info.StringIDs, tagIndex);
                renderModel = _info.Deserializer.Deserialize<RenderModel>(tagContext);
            }
            using (var stream = File.Open(_info.CacheFile.DirectoryName + "\\" + cachePath, FileMode.Open, FileAccess.ReadWrite))
            {
                resourceCache = new ResourceCache(stream);
                renderModel.Geometry.Resource.Index = resourceCache.Add(stream, data, out compressedSize);
                renderModel.Geometry.Resource.CompressedSize = compressedSize;
            }
            using (var cacheStream = _fileInfo.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var context = new TagSerializationContext(cacheStream, _cache, _stringIds, tagIndex);
                _info.Serializer.Serialize(context, renderModel);
            }
            Console.WriteLine("{1}: Imported 0x{0}.", compressedSize, tagIndex);
            return true;
        }

        private bool ImportAnimationResource(TagInstance tagIndex, string cachePath, string dataPath)
        {
            ModelAnimationGraph animation;
            ResourceCache resourceCache;
            uint compressedSize = 0;
            var data = File.ReadAllBytes(dataPath);

            using (var cacheStream = _info.OpenCacheReadWrite())
            {
                var tagContext = new TagSerializationContext(cacheStream, _info.Cache, _info.StringIDs, tagIndex);
                animation = _info.Deserializer.Deserialize<ModelAnimationGraph>(tagContext);
            }
            using (var stream = File.Open(_info.CacheFile.DirectoryName + "\\" + cachePath, FileMode.Open, FileAccess.ReadWrite))
            {
                resourceCache = new ResourceCache(stream);
                animation.ResourceGroups[0].Resource.Index = resourceCache.Add(stream, data, out compressedSize);
                animation.ResourceGroups[0].Resource.CompressedSize = compressedSize;
                animation.ResourceGroups[0].Resource.OldLocationFlags = (OldResourceLocationFlags)2;
            }
            using (var cacheStream = _fileInfo.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var context = new TagSerializationContext(cacheStream, _cache, _stringIds, tagIndex);
                _info.Serializer.Serialize(context, animation);
            }
            Console.WriteLine("{1}: Imported 0x{0}.", compressedSize, tagIndex);
            return true;
        }

        private bool ImportBitmapResource(TagInstance tagIndex, string cachePath, string dataPath)
        {
            Bitmap bitmap;
            ResourceCache resourceCache;
            uint compressedSize = 0;

            using (var cacheStream = _info.OpenCacheRead())
            {
                var tagContext = new TagSerializationContext(cacheStream, _info.Cache, _info.StringIDs, tagIndex);
                bitmap = _info.Deserializer.Deserialize<Bitmap>(tagContext);
            }
            using (var stream = File.Open(_info.CacheFile.DirectoryName + "\\" + cachePath, FileMode.Open, FileAccess.ReadWrite))
            {
                int imageIndex = 0;
                foreach (string file in Directory.EnumerateFiles(dataPath, "*.bitm"))
                {
                    byte[] inBitmapData = File.ReadAllBytes(file);
                    resourceCache = new ResourceCache(stream);
                    bitmap.Resources[imageIndex].Resource.Index = resourceCache.Add(stream, inBitmapData, out compressedSize);
                    bitmap.Resources[imageIndex].Resource.CompressedSize = compressedSize;
                    imageIndex++;
                }
            }
            using (var cacheStream = _fileInfo.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var context = new TagSerializationContext(cacheStream, _cache, _stringIds, tagIndex);
                _info.Serializer.Serialize(context, bitmap);
            }
            Console.WriteLine("{1}: Imported 0x{0}.", compressedSize, tagIndex);
            return true;
        }

        private bool ImportSoundResource(TagInstance tagIndex, string cachePath, string dataPath)
        {
            Sound sound;
            ResourceCache resourceCache;
            uint compressedSize = 0;
            var data = File.ReadAllBytes(dataPath);

            using (var cacheStream = _info.OpenCacheReadWrite())
            {
                var tagContext = new TagSerializationContext(cacheStream, _info.Cache, _info.StringIDs, tagIndex);
                sound = _info.Deserializer.Deserialize<Sound>(tagContext);
            }
            using (var stream = File.Open(_info.CacheFile.DirectoryName + "\\" + cachePath, FileMode.Open, FileAccess.ReadWrite))
            {
                resourceCache = new ResourceCache(stream);
                sound.Resource.Index = resourceCache.Add(stream, data, out compressedSize);
                sound.Resource.CompressedSize = compressedSize;
                sound.Resource.OldLocationFlags = (OldResourceLocationFlags)16;
            }
            using (var cacheStream = _fileInfo.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var context = new TagSerializationContext(cacheStream, _cache, _stringIds, tagIndex);
                _info.Serializer.Serialize(context, sound);
            }
            Console.WriteLine("{1}: Imported 0x{0}.", compressedSize, tagIndex);
            return true;
        }
    }
}
