using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Definitions;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.TagDefinitions;
using TagTool.TagGroups;
using static System.Console;
using static System.IO.Path;
using static TagTool.Cache.StringIDResolverFactory;
using static TagTool.Commands.ArgumentParser;
using static TagTool.Common.TagVersionMap;
using static TagTool.Definitions.Definition;
using static TagTool.Definitions.DefinitionSet;

namespace TagTool.Commands.RenderModels
{
    class ReplaceCommand : Command
    {
        private OpenTagCache Info { get; }
        private TagInstance Tag { get; }
        private RenderModel Definition { get; }

        public ReplaceCommand(OpenTagCache info, TagInstance tag, RenderModel definition)
            : base(CommandFlags.None,
                  "replace",
                  "",
                  "replace <dest tag index> <dest cache dir>",
                  "")
        {
            Info = info;
            Tag = tag;
            Definition = definition;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 2)
                return false;

            var destDir = new DirectoryInfo(args[1]);
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

            var destTag = ParseTagIndex(destInfo, args[0]);
            if (destTag == null || !destTag.IsInGroup(new Tag("mode")))
            {
                WriteLine("Destination tag must be of group 'mode'.");
                return false;
            }

            RenderModel destDefinition;
            using (var destStream = destInfo.OpenCacheRead())
            {
                var context = new TagSerializationContext(destStream, destInfo.Cache, destInfo.StringIDs, destTag);
                destDefinition = destInfo.Deserializer.Deserialize<RenderModel>(context);
            }

            using (MemoryStream inStream = new MemoryStream(), outStream = new MemoryStream())
            {
                // First extract the model data
                srcResources.Extract(Definition.Geometry.Resource, inStream);

                // Now open source and destination vertex streams
                inStream.Position = 0;
                var inVertexStream = VertexStreamFactory.Create(Info.Version, inStream);
                var outVertexStream = VertexStreamFactory.Create(destInfo.Version, outStream);

                // Deserialize the definition data
                var resourceContext = new ResourceSerializationContext(Definition.Geometry.Resource);
                var definition = Info.Deserializer.Deserialize<RenderGeometryResourceDefinition>(resourceContext);

                // Convert each vertex buffer
                foreach (var buffer in definition.VertexBuffers)
                    TagConverter.ConvertVertexBuffer(buffer.Definition, inStream, inVertexStream, outStream, outVertexStream);

                // Copy each index buffer over
                foreach (var buffer in definition.IndexBuffers)
                {
                    if (buffer.Definition.Data.Size == 0)
                        continue;
                    inStream.Position = buffer.Definition.Data.Address.Offset;
                    buffer.Definition.Data.Address = new ResourceAddress(ResourceAddressType.Resource, (int)outStream.Position);
                    var bufferData = new byte[buffer.Definition.Data.Size];
                    inStream.Read(bufferData, 0, bufferData.Length);
                    outStream.Write(bufferData, 0, bufferData.Length);
                    StreamUtil.Align(outStream, 4);
                }

                destInfo.Serializer.Serialize(resourceContext, definition);

                outStream.Position = 0;
                destResources.Replace(destDefinition.Geometry.Resource, outStream);
            }

            return true;
        }
    }
}
