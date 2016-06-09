using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assimp;
using TagTool.Common;
using TagTool.Cache;
using TagTool.Geometry;
using TagTool.Serialization;
using TagTool.TagDefinitions;
using PrimitiveType = TagTool.Geometry.PrimitiveType;
using TagTool.TagGroups;

namespace TagTool.Commands.Tags
{
    class ModelTestCommand : Command
    {
        private OpenTagCache Info { get; }

        public ModelTestCommand(OpenTagCache info) : base(
            CommandFlags.Inherit,

            "modeltest",
            "Model injection test",

            "modeltest [tag index] <model file>",

            "Injects the model over the traffic cone.\n" +
            "The model must only have a single material and no nodes.")
        {
            Info = info;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 2)
                return false;

            TagInstance destination = Info.Cache.Tags[0x3317];

            if (args.Count == 2)
            {
                destination = ArgumentParser.ParseTagIndex(Info, args[0]);

                if (!destination.IsInGroup("mode"))
                {
                    Console.WriteLine("Specified tag is not a render_model: " + args[0]);
                    return false;
                }

                args = args.Skip(1).ToList();
            }

            var builder = new RenderModelBuilder(Info.Version);

            // Add a root node
            var node = builder.AddNode(new RenderModel.Node
            {
                Name = Info.StringIDs.GetStringID("street_cone"),
                ParentNode = -1,
                FirstChildNode = -1,
                NextSiblingNode = -1,
                DefaultRotation = new Vector4(0, 0, 0, -1),
                DefaultScale = 1,
                InverseForward = new Vector3(1, 0, 0),
                InverseLeft = new Vector3(0, 1, 0),
                InverseUp = new Vector3(0, 0, 1),
            });

            // Begin building the default region and permutation
            builder.BeginRegion(Info.StringIDs.GetStringID("default"));
            builder.BeginPermutation(Info.StringIDs.GetStringID("default"));

            using (var importer = new AssimpContext())
            {
                Scene model;
                using (var logStream = new LogStream((msg, userData) => Console.WriteLine(msg)))
                {
                    logStream.Attach();
                    model = importer.ImportFile(args[0],
                        PostProcessSteps.CalculateTangentSpace |
                        PostProcessSteps.GenerateNormals |
                        PostProcessSteps.JoinIdenticalVertices |
                        PostProcessSteps.SortByPrimitiveType |
                        PostProcessSteps.PreTransformVertices |
                        PostProcessSteps.Triangulate);
                    logStream.Detach();
                }

                Console.WriteLine("Assembling vertices...");

                // Build a multipart mesh from the model data,
                // with each model mesh mapping to a part of one large mesh and having its own material
                builder.BeginMesh();
                ushort partStartVertex = 0;
                ushort partStartIndex = 0;
                var vertices = new List<RigidVertex>();
                var indices = new List<ushort>();
                foreach (var mesh in model.Meshes)
                {
                    for (var i = 0; i < mesh.VertexCount; i++)
                    {
                        var position = mesh.Vertices[i];
                        var normal = mesh.Normals[i];
                        var uv = mesh.TextureCoordinateChannels[0][i];
                        var tangent = mesh.Tangents[i];
                        var bitangent = mesh.BiTangents[i];
                        vertices.Add(new RigidVertex
                        {
                            Position = new Vector4(position.X, position.Y, position.Z, 1),
                            Normal = new Vector3(normal.X, normal.Y, normal.Z),
                            Texcoord = new Vector2(uv.X, uv.Y),
                            Tangent = new Vector4(tangent.X, tangent.Y, tangent.Z, 1),
                            Binormal = new Vector3(bitangent.X, bitangent.Y, bitangent.Z),
                        });
                    }

                    // Build the index buffer
                    var meshIndices = mesh.GetIndices();
                    indices.AddRange(meshIndices.Select(i => (ushort)(i + partStartVertex)));

                    // Define a material and part for this mesh
                    var material = builder.AddMaterial(new RenderMaterial
                    {
                        RenderMethod = Info.Cache.Tags[0x101F],
                    });


                    builder.BeginPart(material, partStartIndex, (ushort)meshIndices.Length, (ushort)mesh.VertexCount);
                    builder.DefineSubPart(partStartIndex, (ushort)meshIndices.Length, (ushort)mesh.VertexCount);
                    builder.EndPart();
                    
                    // Move to the next part
                    partStartVertex += (ushort)mesh.VertexCount;
                    partStartIndex += (ushort)meshIndices.Length;
                }

                // Bind the vertex and index buffers
                builder.BindRigidVertexBuffer(vertices, node);
                builder.BindIndexBuffer(indices, PrimitiveType.TriangleList);
                builder.EndMesh();
            }

            builder.EndPermutation();
            builder.EndRegion();

            Console.WriteLine("Building Blam mesh data...");

            var resourceStream = new MemoryStream();
            var renderModel = builder.Build(Info.Serializer, resourceStream);

            Console.WriteLine("Writing resource data...");

            // Add a new resource for the model data
            var resources = new ResourceDataManager();
            resources.LoadCachesFromDirectory(Info.CacheFile.DirectoryName);
            resourceStream.Position = 0;
            resources.Add(renderModel.Geometry.Resource, ResourceLocation.Resources, resourceStream);

            Console.WriteLine("Writing tag data...");

            using (var cacheStream = Info.OpenCacheReadWrite())
            {
                var tag = destination;
                var context = new TagSerializationContext(cacheStream, Info.Cache, Info.StringIDs, tag);
                Info.Serializer.Serialize(context, renderModel);
            }

            Console.WriteLine("Model imported successfully!");

            return true;
        }
    }
}
