using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Bitmaps;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Definitions;
using TagTool.Serialization;

namespace TagTool.Commands.Cache
{
    class PortShaderCommand : Command
    {
        private readonly OpenTagCache _info;
        private readonly CacheBase _blamCache;

        public PortShaderCommand(OpenTagCache info, CacheBase blamCache)
            : base(CommandFlags.None, "portshader", "", "portshader <blam tag path>", "")
        {
            _info = info;
            _blamCache = blamCache;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            //
            // Verify and load the blam shader
            //

            var shaderName = args[0];

            CacheBase.IndexItem item = null;

            Console.WriteLine("Verifying blam shader tag...");

            foreach (var tag in _blamCache.IndexItems)
            {
                if ((tag.ParentClass == "rm") && tag.Filename == shaderName)
                {
                    item = tag;
                    break;
                }
            }

            if (item == null)
            {
                Console.WriteLine("Blam shader tag does not exist: " + shaderName);
                return false;
            }

            var renderMethod = DefinitionsManager.rmsh(_blamCache, item);

            var templateItem = _blamCache.IndexItems.Find(i =>
                i.ID == renderMethod.Properties[0].TemplateTagID);

            var template = DefinitionsManager.rmt2(_blamCache, templateItem);

            //
            // Determine the blam shader's base bitmap
            //

            var bitmapIndex = -1;
            var bitmapArgName = "";

            for (var i = 0; i < template.UsageBlocks.Count; i++)
            {
                var entry = template.UsageBlocks[i];

                if (entry.Usage.StartsWith("base_map") ||
                    entry.Usage.StartsWith("diffuse_map") ||
                    entry.Usage == "foam_texture")
                {
                    bitmapIndex = i;
                    bitmapArgName = entry.Usage;
                    break;
                }
            }

            //
            // Load and decode the blam shader's base bitmap
            //

            var bitmItem = _blamCache.IndexItems.Find(i =>
                i.ID == renderMethod.Properties[0].ShaderMaps[bitmapIndex].BitmapTagID);

            var bitm = DefinitionsManager.bitm(_blamCache, bitmItem);
            var submap = bitm.Bitmaps[0];

            byte[] raw;

            if (_blamCache.Version <= DefinitionSet.Halo2Vista)
                raw = _blamCache.GetRawFromID(submap.PixelsOffset, submap.RawSize);
            else
            {
                if (bitm.RawChunkBs.Count > 0)
                {
                    int rawID = bitm.RawChunkBs[submap.InterleavedIndex].RawID;
                    byte[] buffer = _blamCache.GetRawFromID(rawID);
                    raw = new byte[submap.RawSize];
                    Array.Copy(buffer, submap.Index2 * submap.RawSize, raw, 0, submap.RawSize);
                }
                else
                {
                    int rawID = bitm.RawChunkAs[0].RawID;
                    raw = _blamCache.GetRawFromID(rawID, submap.RawSize);
                }
            }

            var vHeight = submap.VirtualHeight;
            var vWidth = submap.VirtualWidth;

            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);

            if (submap.Flags.Values[3])
                raw = DXTDecoder.ConvertToLinearTexture(raw, vWidth, vHeight, submap.Format);

            if (submap.Format != BitmapFormat.A8R8G8B8)
                for (int i = 0; i < raw.Length; i += 2)
                    Array.Reverse(raw, i, 2);
            else
                for (int i = 0; i < (raw.Length); i += 4)
                    Array.Reverse(raw, i, 4);

            new DDS(submap).Write(bw);
            bw.Write(raw);

            raw = ms.ToArray();

            bw.Close();
            bw.Dispose();

            //
            // ElDorado Serialization
            //

            using (var cacheStream = _info.CacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                //
                // Create the new eldorado bitmap
                //

                var resourceManager = new ResourceDataManager();
                resourceManager.LoadCachesFromDirectory(_info.CacheFile.DirectoryName);

                var newBitm = _info.Cache.DuplicateTag(cacheStream, _info.Cache.Tags[0x101F]);

                var bitmap = new TagStructures.Bitmap
                {
                    Flags = TagStructures.Bitmap.RuntimeFlags.UseResource,
                    Sequences = new List<TagStructures.Bitmap.Sequence>
                    {
                        new TagStructures.Bitmap.Sequence
                        {
                            Name = "",
                            FirstBitmapIndex = 0,
                            BitmapCount = 1
                        }
                    },
                    Images = new List<TagStructures.Bitmap.Image>
                    {
                        new TagStructures.Bitmap.Image
                        {
                            Signature = new Tag("bitm").Value,
                            Unknown28 = -1
                        }
                    },
                    Resources = new List<TagStructures.Bitmap.BitmapResource>
                    {
                        new TagStructures.Bitmap.BitmapResource()
                    }
                };

                using (var imageStream = new MemoryStream(raw))
                {
                    var injector = new BitmapDdsInjector(resourceManager);
                    imageStream.Seek(0, SeekOrigin.Begin);
                    injector.InjectDds(_info.Serializer, _info.Deserializer, bitmap, 0, imageStream);
                }

                var context = new TagSerializationContext(cacheStream, _info.Cache, _info.StringIDs, newBitm);
                _info.Serializer.Serialize(context, bitmap);

                //
                // Create the new eldorado shader
                //

                var newRmsh = _info.Cache.DuplicateTag(cacheStream, _info.Cache.Tags[0x331A]);
                context = new TagSerializationContext(cacheStream, _info.Cache, _info.StringIDs, newRmsh);
                var shader = _info.Deserializer.Deserialize<TagStructures.Shader>(context);

                shader.ShaderProperties[0].ShaderMaps[0].Bitmap = newBitm;

                _info.Serializer.Serialize(context, shader);

                Console.WriteLine("Done! New shader tag is 0x" + newRmsh.Index.ToString("X8"));
            }

            return true;
        }
    }
}
