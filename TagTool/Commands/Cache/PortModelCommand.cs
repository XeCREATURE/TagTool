using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands.Tags;
using TagTool.Common;
using TagTool.Definitions;
using TagTool.Geometry;
using TagTool.TagGroups;

namespace TagTool.Commands.Cache
{
    class PortModelCommand : Command
    {
        private OpenTagCache Info { get; }
        private CacheBase BlamCache { get; }

        public PortModelCommand(OpenTagCache info, CacheBase blamCache)
            : base(CommandFlags.Inherit,
                  "portmodel",
                  "",
                  "portmodel [new] <blam tag path> <eldorado tag index>",
                  "")
        {
            Info = info;
            BlamCache = blamCache;
        }

        public override bool Execute(List<string> args)
        {
            var initialStringIDCount = Info.StringIDs.Strings.Count;

            bool isNew = false;
            if (args.Count == 3)
            {
                if (args[0] != "new")
                    return false;
                isNew = true;
                args.Remove("new");
            }

            if (args.Count != 2)
                return false;

            //
            // Verify the Blam render_model tag
            //

            var renderModelName = args[0];

            CacheBase.IndexItem item = null;

            Console.WriteLine("Verifying Blam tag...");

            foreach (var tag in BlamCache.IndexItems)
            {
                if ((tag.ClassCode == "mode" || tag.ClassCode == "sbsp") && tag.Filename == renderModelName)
                {
                    item = tag;
                    break;
                }
            }

            if (item == null)
            {
                Console.WriteLine("Blam tag does not exist: " + args[0]);
                return false;
            }

            //
            // Verify the ED render_model tag
            //

            Console.WriteLine("Verifying ED tag index...");

            int edRenderModelIndex;

            if (!int.TryParse(args[1], NumberStyles.HexNumber, null, out edRenderModelIndex) ||
                (edRenderModelIndex >= Info.Cache.Tags.Count))
            {
                Console.WriteLine("Invalid tag index: " + args[1]);
                return false;
            }

            var edTag = Info.Cache.Tags[edRenderModelIndex];

            if (edTag.Group.Name != Info.StringIDs.GetStringID("render_model"))
            {
                Console.WriteLine("Specified tag index is not a render_model: " + args[1]);
                return false;
            }

            //
            // Deserialize the selected render_model
            //

            Console.WriteLine("Loading ED render_model tag...");

            TagDefinitions.RenderModel renderModel;

            using (var cacheStream = Info.CacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                try
                {
                    var context = new Serialization.TagSerializationContext(cacheStream, Info.Cache, Info.StringIDs, Info.Cache.Tags[(int)edRenderModelIndex]);
                    renderModel = Info.Deserializer.Deserialize<TagDefinitions.RenderModel>(context);
                }
                catch
                {
                    Console.WriteLine("Failed to deserialize selected render_model tag: " + edRenderModelIndex);
                    return true;
                }
            }

            //
            // Load the Blam render_model tag raw
            //

            var isBSP = item.ClassCode == "sbsp";

            scenario_structure_bsp sbsp = null;
            render_model mode = null;

            if (isBSP)
            {
                sbsp = DefinitionsManager.sbsp(BlamCache, item);
                sbsp.LoadRaw();
            }
            else
            {
                mode = DefinitionsManager.mode(BlamCache, item);
                mode.LoadRaw();
            }

            //
            // Duplicate the render_model tag we're injecting over
            //

            TagInstance newTag;

            if (isNew)
            {
                Console.WriteLine("Duplicating selected render_model tag...");

                if (!new DuplicateTagCommand(Info).Execute(new List<string> { edRenderModelIndex.ToString("X8") }))
                {
                    Console.WriteLine("Failed to duplicate render_model tag: " + edRenderModelIndex);
                    return false;
                }

                newTag = Info.Cache.Tags[Info.Cache.Tags.Count - 1];
            }
            else
            {
                newTag = edTag;
            }

            //
            // Start porting the model
            //

            RenderModelBuilder builder = new RenderModelBuilder(DefinitionSet.HaloOnline106708);

            var blamNodes = isBSP ?
                new List<render_model.Node>
                {
                    new render_model.Node
                    {
                        Name = "default",
                        ParentIndex = -1,
                        FirstChildIndex = -1,
                        NextSiblingIndex = -1,
                        Position = new Vector(),
                        Rotation = new Vector(0, 0, 0, -1),
                        TransformScale = 1,
                        TransformMatrix = new Matrix4x3(
                            1, 0, 0,
                            0, 1, 0,
                            0, 0, 1,
                            0, 0, 0),
                        DistanceFromParent = 0
                    }
                } : mode.Nodes;

            foreach (var node in blamNodes)
            {
                var nodeNameId = Info.StringIDs.GetStringID(node.Name);

                builder.AddNode(
                    new TagDefinitions.RenderModel.Node
                    {
                        Name = nodeNameId.Index == 0 && node.Name != "" ? nodeNameId = Info.StringIDs.Add(node.Name) : nodeNameId,
                        ParentNode = (short)node.ParentIndex,
                        FirstChildNode = (short)node.FirstChildIndex,
                        NextSiblingNode = (short)node.NextSiblingIndex,
                        ImportNode = 0,
                        DefaultTranslation = new Common.Vector3(node.Position.X, node.Position.Y, node.Position.Z),
                        DefaultRotation = new Common.Vector4(node.Rotation.X, node.Rotation.Y, node.Rotation.Z, node.Rotation.W),
                        DefaultScale = node.TransformScale,
                        InverseForward = new Common.Vector3(node.TransformMatrix.m11, node.TransformMatrix.m12, node.TransformMatrix.m13),
                        InverseLeft = new Common.Vector3(node.TransformMatrix.m21, node.TransformMatrix.m22, node.TransformMatrix.m23),
                        InverseUp = new Common.Vector3(node.TransformMatrix.m31, node.TransformMatrix.m32, node.TransformMatrix.m33),
                        InversePosition = new Common.Vector3(node.TransformMatrix.m41, node.TransformMatrix.m42, node.TransformMatrix.m43),
                        DistanceFromParent = node.DistanceFromParent
                    });
            }

            //
            // Create empty materials for now...
            //

            var blamShaders = isBSP ? sbsp.Shaders : mode.Shaders;

            foreach (var shader in blamShaders)
                builder.AddMaterial(
                    new RenderMaterial
                    {
                        RenderMethod = Info.Cache.Tags[0x101F]
                    });

            //
            // Build the model regions
            //

            if (isBSP)
            {
                builder.BeginRegion(Info.StringIDs.GetStringID("default"));
                builder.BeginPermutation(Info.StringIDs.GetStringID("default"));

                foreach (var section in sbsp.ModelSections)
                {
                    if (section.Submeshes.Count == 0)
                        continue;

                    var rigidVertices = new List<RigidVertex>();

                    VertexValue v;
                    if (section.Vertices != null)
                    {
                        foreach (var vertex in section.Vertices)
                        {
                            vertex.TryGetValue("position", 0, out v);
                            var position = new Common.Vector4(v.Data.X, v.Data.Y, v.Data.Z, 1);

                            vertex.TryGetValue("normal", 0, out v);
                            var normal = new Common.Vector3(v.Data.I, v.Data.J, v.Data.K);

                            vertex.TryGetValue("texcoords", 0, out v);
                            var texcoord = new Common.Vector2(v.Data.X, v.Data.Y);

                            vertex.TryGetValue("tangent", 0, out v);
                            var tangent = new Common.Vector4(v.Data.X, v.Data.Y, v.Data.Z, 1);

                            vertex.TryGetValue("binormal", 0, out v);
                            var binormal = new Common.Vector3(v.Data.X, v.Data.Y, v.Data.Z);

                            rigidVertices.Add(
                                new RigidVertex
                                {
                                    Position = position,
                                    Normal = normal,
                                    Texcoord = texcoord,
                                    Tangent = tangent,
                                    Binormal = binormal
                                });
                        }
                    }

                    // Build the section's subparts

                    builder.BeginMesh();

                    var indices = new List<ushort>();

                    foreach (var submesh in section.Submeshes)
                    {
                        builder.BeginPart((short)submesh.ShaderIndex, (ushort)submesh.FaceIndex, (ushort)submesh.FaceCount, (ushort)submesh.VertexCount);
                        for (var j = 0; j < submesh.SubsetCount; j++)
                        {
                            var subpart = section.Subsets[submesh.SubsetIndex + j];
                            builder.DefineSubPart((ushort)subpart.FaceIndex, (ushort)subpart.FaceCount, (ushort)subpart.VertexCount);
                        }
                        builder.EndPart();
                    }

                    builder.BindRigidVertexBuffer(rigidVertices, 0);
                    builder.BindIndexBuffer(section.Indices.Select(index => (ushort)index), PrimitiveType.TriangleList);

                    builder.EndMesh();
                }

                builder.EndPermutation();
                builder.EndRegion();

                foreach (var instance in sbsp.GeomInstances)
                {
                    var mesh = builder.Meshes[instance.SectionIndex];

                    if (mesh.VertexFormat == VertexBufferFormat.Rigid)
                        foreach (var i in mesh.RigidVertices)
                                i.Position = new Common.Vector4(
                                    i.Position.X + instance.TransformMatrix.m41,
                                    i.Position.Y + instance.TransformMatrix.m42,
                                    i.Position.Z + instance.TransformMatrix.m43,
                                    i.Position.W);

                    else if (mesh.VertexFormat == VertexBufferFormat.World)
                        foreach (var i in mesh.WorldVertices)
                            i.Position = new Common.Vector4(
                                i.Position.X + instance.TransformMatrix.m41,
                                i.Position.Y + instance.TransformMatrix.m42,
                                i.Position.Z + instance.TransformMatrix.m43,
                                i.Position.W);

                    else if (mesh.VertexFormat == VertexBufferFormat.Skinned)
                        foreach (var i in mesh.SkinnedVertices)
                            i.Position = new Common.Vector4(
                                i.Position.X + instance.TransformMatrix.m41,
                                i.Position.Y + instance.TransformMatrix.m42,
                                i.Position.Z + instance.TransformMatrix.m43,
                                i.Position.W);
                }
            }

            else

            foreach (var region in mode.Regions)
            {
                var regionNameId = Info.StringIDs.GetStringID(region.Name);

                builder.BeginRegion(regionNameId.Index == 0 && region.Name != "" ? regionNameId = Info.StringIDs.Add(region.Name) : regionNameId);

                foreach (var permutation in region.Permutations)
                {
                    if (permutation.PieceCount <= 0 || permutation.PieceIndex == -1)
                        continue;

                    var permutationNameId = Info.StringIDs.GetStringID(permutation.Name);

                    builder.BeginPermutation(permutationNameId.Index == 0 && permutation.Name != "" ? permutationNameId = Info.StringIDs.Add(permutation.Name) : permutationNameId);

                    for (var i = permutation.PieceIndex; i < permutation.PieceIndex + permutation.PieceCount; i++)
                    {
                        var section = mode.ModelSections[i];

                        if (section.Submeshes.Count == 0 || section.Vertices == null)
                            continue;

                        //
                        // Collect the section's vertices
                        //

                        var skinnedVertices = new List<SkinnedVertex>();
                        var rigidVertices = new List<RigidVertex>();

                        VertexValue v;
                        bool isSkinned = section.Vertices[0].TryGetValue("blendindices", 0, out v) && section.NodeIndex == 255;
                        bool isBoned = section.Vertices[0].FormatName.Contains("rigid_boned");

                        foreach (var vertex in section.Vertices)
                        {
                            vertex.TryGetValue("position", 0, out v);
                            var position = new Common.Vector4(v.Data.X, v.Data.Y, v.Data.Z, 1);

                            vertex.TryGetValue("normal", 0, out v);
                            var normal = new Common.Vector3(v.Data.I, v.Data.J, v.Data.K);

                            vertex.TryGetValue("texcoords", 0, out v);
                            var texcoord = new Common.Vector2(v.Data.X, v.Data.Y);

                            vertex.TryGetValue("tangent", 0, out v);
                            var tangent = new Common.Vector4(v.Data.X, v.Data.Y, v.Data.Z, 1);

                            vertex.TryGetValue("binormal", 0, out v);
                            var binormal = new Common.Vector3(v.Data.X, v.Data.Y, v.Data.Z);

                            rigidVertices.Add(
                                new RigidVertex
                                {
                                    Position = position,
                                    Normal = normal,
                                    Texcoord = texcoord,
                                    Tangent = tangent,
                                    Binormal = binormal
                                });

                            if (isBoned)
                            {
                                var blendIndices = new List<byte>();

                                vertex.TryGetValue("blendindices", 0, out v);

                                blendIndices.Add((byte)v.Data.A);
                                blendIndices.Add((byte)v.Data.B);
                                blendIndices.Add((byte)v.Data.C);
                                blendIndices.Add((byte)v.Data.D);

                                skinnedVertices.Add(new SkinnedVertex
                                {
                                    Position = position,
                                    Normal = normal,
                                    Texcoord = texcoord,
                                    Tangent = tangent,
                                    Binormal = binormal,
                                    BlendIndices = blendIndices.ToArray(),
                                    BlendWeights = new[] { 1.0f, 0.0f, 0.0f, 0.0f }
                                });
                            }
                            else if (isSkinned)
                            {
                                var blendIndices = new List<byte>();
                                var blendWeights = new List<float>();

                                vertex.TryGetValue("blendindices", 0, out v);

                                blendIndices.Add((byte)v.Data.A);
                                blendIndices.Add((byte)v.Data.B);
                                blendIndices.Add((byte)v.Data.C);
                                blendIndices.Add((byte)v.Data.D);

                                vertex.TryGetValue("blendweight", 0, out v);

                                blendWeights.Add(v.Data.A);
                                blendWeights.Add(v.Data.B);
                                blendWeights.Add(v.Data.C);
                                blendWeights.Add(v.Data.D);

                                skinnedVertices.Add(new SkinnedVertex
                                {
                                    Position = position,
                                    Normal = normal,
                                    Texcoord = texcoord,
                                    Tangent = tangent,
                                    Binormal = binormal,
                                    BlendIndices = blendIndices.ToArray(),
                                    BlendWeights = blendWeights.ToArray()
                                });
                            }
                        }

                        bool isRigid = false;

                        if (skinnedVertices.Count == 0)
                            isRigid = rigidVertices.Count != 0;

                        //
                        // Build the section's submeshes
                        //

                        builder.BeginMesh();

                        var indices = new List<ushort>();

                        foreach (var submesh in section.Submeshes)
                        {
                            builder.BeginPart((short)submesh.ShaderIndex, (ushort)submesh.FaceIndex, (ushort)submesh.FaceCount, (ushort)submesh.VertexCount);
                            for (var j = 0; j < submesh.SubsetCount; j++)
                            {
                                var subpart = section.Subsets[submesh.SubsetIndex + j];
                                builder.DefineSubPart((ushort)subpart.FaceIndex, (ushort)subpart.FaceCount, (ushort)subpart.VertexCount);
                            }
                            builder.EndPart();
                        }

                        if (isRigid)
                            builder.BindRigidVertexBuffer(rigidVertices, (sbyte)section.NodeIndex);
                        else if (isSkinned || isBoned)
                            builder.BindSkinnedVertexBuffer(skinnedVertices);
                        builder.BindIndexBuffer(section.Indices.Select(index => (ushort)index), PrimitiveType.TriangleStrip);

                        builder.EndMesh();
                    }

                    builder.EndPermutation();
                }

                builder.EndRegion();
            }

            //
            // Finalize the new render_model tag
            //

            var resourceStream = new MemoryStream();
            var newRenderModel = builder.Build(Info.Serializer, resourceStream);

            var renderModelNameStringID = Info.StringIDs.GetStringID(isBSP ? "default" : mode.Name);

            newRenderModel.Name = renderModelNameStringID.Index == -1 ?
                renderModelNameStringID = Info.StringIDs.Add(isBSP ? "default" : mode.Name) :
                renderModelNameStringID;

            //
            // Add the markers to the new render_model
            //

            newRenderModel.MarkerGroups = new List<TagDefinitions.RenderModel.MarkerGroup>();

            var blamMarkerGroups = isBSP ? new List<render_model.MarkerGroup>() : mode.MarkerGroups;

            foreach (var markerGroup in blamMarkerGroups)
            {
                var markerGroupNameId = Info.StringIDs.GetStringID(markerGroup.Name);

                if (markerGroupNameId.Index == -1)
                    markerGroupNameId = Info.StringIDs.Add(markerGroup.Name);

                newRenderModel.MarkerGroups.Add(
                    new TagDefinitions.RenderModel.MarkerGroup
                    {
                        Name = markerGroupNameId,
                        Markers = markerGroup.Markers.Select(marker =>
                            new TagDefinitions.RenderModel.MarkerGroup.Marker
                            {
                                RegionIndex = (sbyte)marker.RegionIndex,
                                PermutationIndex = (sbyte)marker.PermutationIndex,
                                NodeIndex = (sbyte)marker.NodeIndex,
                                Unknown3 = 0,
                                Translation = new Common.Vector3(marker.Position.X, marker.Position.Y, marker.Position.Z),
                                Rotation = new Common.Vector4(marker.Rotation.X, marker.Rotation.Y, marker.Rotation.Z, marker.Rotation.W),
                                Scale = marker.Scale
                            }).ToList()
                    });
            }

            //
            // Disable rigid nodes on skinned meshes
            //

            foreach (var mesh in newRenderModel.Geometry.Meshes)
                if (mesh.Type == VertexType.Skinned)
                    mesh.RigidNodeIndex = -1;

            //
            // Add a new resource for the model data
            //

            Console.WriteLine("Writing resource data...");

            var resources = new ResourceDataManager();
            resources.LoadCachesFromDirectory(Info.CacheFile.DirectoryName);
            resourceStream.Position = 0;
            resources.Add(newRenderModel.Geometry.Resource, ResourceLocation.Resources, resourceStream);

            using (var cacheStream = Info.CacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                Console.WriteLine("Writing tag data...");

                newRenderModel.Geometry.Resource.Owner = newTag;

                var context = new Serialization.TagSerializationContext(cacheStream, Info.Cache, Info.StringIDs, newTag);
                Info.Serializer.Serialize(context, newRenderModel);
            }

            resourceStream.Close();

            //
            // Save new string_ids
            //

            if (Info.StringIDs.Strings.Count != initialStringIDCount)
            {
                Console.WriteLine("Saving string_ids...");

                using (var stringIdStream = Info.StringIDsFile.Open(FileMode.Open, FileAccess.ReadWrite))
                    Info.StringIDs.Save(stringIdStream);
            }

            //
            // Done!
            //

            Console.WriteLine("Ported render_model \"" + renderModelName + "\" successfully!");

            return true;
        }
    }
}
