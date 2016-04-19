using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Geometry;
using TagTool.Serialization;
using TagTool.TagStructures;

namespace TagTool.Commands.Models
{
    class ExtractModelCommand : Command
    {
        private OpenTagCache Info { get; }
        private Model Definition { get; }

        public ExtractModelCommand(OpenTagCache info, Model model) : base(
            CommandFlags.Inherit,

            "extractmodel",
            "Extracts a render model from the current model definition.",

            "extractmodel <variant> <filetype> <filename>",

            "Extracts a variant of the render model to a file.\n" +
            "Use the \"listvariants\" command to list available variants.\n" +
            "If the model does not have any variants, just use \"default\"." +
            "\n" +
            "Supported file types: obj")
        {
            Info = info;
            Definition = model;
        }

        private bool ExtractAMF(Model.Variant variant, string fileName)
        {
            // Load resource caches
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

            // Deserialize the render model tag
            Console.WriteLine("Reading model data...");
            RenderModel renderModel;
            using (var cacheStream = Info.CacheFile.OpenRead())
            {
                var renderModelContext = new TagSerializationContext(cacheStream, Info.Cache, Info.StringIDs, Definition.RenderModel);
                renderModel = Info.Deserializer.Deserialize<RenderModel>(renderModelContext);
            }

            if (renderModel.Geometry.Resource == null)
            {
                Console.WriteLine("Render model does not have a resource associated with it");
                return true;
            }

            // Deserialize the resource definition
            var resourceContext = new ResourceSerializationContext(renderModel.Geometry.Resource);
            var definition = Info.Deserializer.Deserialize<RenderGeometryResourceDefinition>(resourceContext);

            using (var resourceStream = new MemoryStream())
            {
                // Extract the resource data
                resourceManager.Extract(renderModel.Geometry.Resource, resourceStream);

                var regionMeshes = new Dictionary<string, Mesh>();

                foreach (var region in variant.Regions)
                {
                    regionMeshes[Info.StringIDs.GetString(region.Name)] = renderModel.Geometry.Meshes[region.RenderModelRegionIndex];
                }

                var headerAddressList = new List<int>();
                var headerValueList = new List<int>();
                var markerAddressList = new List<int>();
                var markerValueList = new List<int>();
                var permAddressList = new List<int>();
                var permValueList = new List<int>();
                var vertAddressList = new List<int>();
                var indxAddressList = new List<int>();
                var meshAddressList = new List<int>();

                using (var bw = new BinaryWriter(File.Create(fileName)))
                {
                    #region Header
                    bw.Write("AMF!".ToCharArray());
                    bw.Write(2.0f); //format version
                    bw.Write((Info.StringIDs.GetString(renderModel.Name) + "\0").ToCharArray());

                    bw.Write(renderModel.Nodes.Count);
                    headerAddressList.Add((int)bw.BaseStream.Position);
                    bw.Write(0);

                    bw.Write(renderModel.MarkerGroups.Count);
                    headerAddressList.Add((int)bw.BaseStream.Position);
                    bw.Write(0);

                    bw.Write(regionMeshes.Count);
                    headerAddressList.Add((int)bw.BaseStream.Position);
                    bw.Write(0);

                    bw.Write(renderModel.Materials.Count);
                    headerAddressList.Add((int)bw.BaseStream.Position);
                    bw.Write(0);
                    #endregion
                    #region Nodes
                    headerValueList.Add((int)bw.BaseStream.Position);
                    foreach (var node in renderModel.Nodes)
                    {
                        bw.Write((Info.StringIDs.GetString(node.Name) + "\0").ToCharArray());
                        bw.Write((short)node.ParentNode);
                        bw.Write((short)node.FirstChildNode);
                        bw.Write((short)node.NextSiblingNode);
                        bw.Write(node.DefaultTranslation.X * 100);
                        bw.Write(node.DefaultTranslation.Y * 100);
                        bw.Write(node.DefaultTranslation.Z * 100);
                        bw.Write(node.DefaultRotation.X);
                        bw.Write(node.DefaultRotation.Y);
                        bw.Write(node.DefaultRotation.Z);
                        bw.Write(node.DefaultRotation.W);
                    }
                    #endregion
                    #region Marker Groups
                    headerValueList.Add((int)bw.BaseStream.Position);
                    foreach (var group in renderModel.MarkerGroups)
                    {
                        bw.Write((Info.StringIDs.GetString(group.Name) + "\0").ToCharArray());
                        bw.Write(group.Markers.Count);
                        markerAddressList.Add((int)bw.BaseStream.Position);
                        bw.Write(0);
                    }
                    #endregion
                    #region Markers
                    foreach (var group in renderModel.MarkerGroups)
                    {
                        markerValueList.Add((int)bw.BaseStream.Position);
                        foreach (var marker in group.Markers)
                        {
                            bw.Write((byte)marker.RegionIndex);
                            bw.Write((byte)marker.PermutationIndex);
                            bw.Write((short)marker.NodeIndex);
                            bw.Write(marker.Translation.X * 100);
                            bw.Write(marker.Translation.Y * 100);
                            bw.Write(marker.Translation.Z * 100);
                            bw.Write(marker.Rotation.X);
                            bw.Write(marker.Rotation.Y);
                            bw.Write(marker.Rotation.Z);
                            bw.Write(marker.Rotation.W);
                        }
                    }
                    #endregion
                    #region Regions
                    headerValueList.Add((int)bw.BaseStream.Position);
                    foreach (var region in renderModel.Regions)
                    {
                        bw.Write((Info.StringIDs.GetString(region.Name) + "\0").ToCharArray());
                        bw.Write(regionMeshes.Count);
                        permAddressList.Add((int)bw.BaseStream.Position);
                        bw.Write(0);
                    }
                    #endregion
                    #region Permutations
                    foreach (var part in regionMeshes)
                    {
                        permValueList.Add((int)bw.BaseStream.Position);
                        bw.Write((Info.StringIDs.GetString(variant.Name) + "\0").ToCharArray());

                        if (part.Value.Type == VertexType.Rigid)
                            bw.Write((byte)1);
                        else if (part.Value.Type == VertexType.Skinned)
                            bw.Write((byte)2);
                        else
                            throw new NotImplementedException();
                        
                        bw.Write((byte)part.Value.RigidNodeIndex);

                        bw.Write(definition.VertexBuffers[part.Value.VertexBuffers[0]].Definition.Count);
                        vertAddressList.Add((int)bw.BaseStream.Position);
                        bw.Write(0);

                        int count = 0;
                        foreach (var submesh in part.Value.SubParts)
                            count += submesh.IndexCount;

                        bw.Write(count);
                        indxAddressList.Add((int)bw.BaseStream.Position);
                        bw.Write(0);

                        bw.Write(part.Value.SubParts.Count);
                        meshAddressList.Add((int)bw.BaseStream.Position);
                        bw.Write(0);

                        bw.Write(float.NaN); //no transforms (render_models are pre-transformed)
                    }
                    #endregion
                }
            }

            return true;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count != 3)
                return false;

            var variantName = args[0];
            var fileType = args[1];
            var fileName = args[2];

            if (fileType != "obj" && fileType != "amf")
                return false;

            // Find the variant to extract
            if (Definition.RenderModel == null)
            {
                Console.WriteLine("The model does not have a render model associated with it.");
                return true;
            }

            var variant = Definition.Variants.FirstOrDefault(v => (Info.StringIDs.GetString(v.Name) ?? v.Name.ToString()) == variantName);
            if (variant == null && Definition.Variants.Count > 0)
            {
                Console.WriteLine("Unable to find variant \"{0}\"", variantName);
                Console.WriteLine("Use \"listvariants\" to list available variants.");
                return true;
            }

            if (fileType == "amf")
                return ExtractAMF(variant, fileName);

            // Load resource caches
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

            // Deserialize the render model tag
            Console.WriteLine("Reading model data...");
            RenderModel renderModel;
            using (var cacheStream = Info.CacheFile.OpenRead())
            {
                var renderModelContext = new TagSerializationContext(cacheStream, Info.Cache, Info.StringIDs, Definition.RenderModel);
                renderModel = Info.Deserializer.Deserialize<RenderModel>(renderModelContext);
            }

            if (renderModel.Geometry.Resource == null)
            {
                Console.WriteLine("Render model does not have a resource associated with it");
                return true;
            }
            
            // Deserialize the resource definition
            var resourceContext = new ResourceSerializationContext(renderModel.Geometry.Resource);
            var definition = Info.Deserializer.Deserialize<RenderGeometryResourceDefinition>(resourceContext);

            using (var resourceStream = new MemoryStream())
            {
                // Extract the resource data
                resourceManager.Extract(renderModel.Geometry.Resource, resourceStream);

                using (var objFile = new StreamWriter(File.Open(fileName, FileMode.Create, FileAccess.Write)))
                {
                    var objExtractor = new ObjExtractor(objFile);
                    var vertexCompressor = new VertexCompressor(renderModel.Geometry.Compression[0]); // Create a (de)compressor from the first compression block

                    if (variant != null)
                    {
                        // Extract each region in the variant
                        foreach (var region in variant.Regions)
                        {
                            // Get the corresonding region in the render model tag
                            if (region.RenderModelRegionIndex >= renderModel.Regions.Count)
                                continue;

                            var renderModelRegion = renderModel.Regions[region.RenderModelRegionIndex];

                            // Get the corresponding permutation in the render model tag
                            // (Just extract the first permutation for now)
                            if (region.Permutations.Count == 0)
                                continue;

                            var permutation = region.Permutations[0];

                            if (permutation.RenderModelPermutationIndex < 0 ||
                                permutation.RenderModelPermutationIndex >= renderModelRegion.Permutations.Count)
                                continue;

                            var renderModelPermutation = renderModelRegion.Permutations[permutation.RenderModelPermutationIndex];

                            // Extract each mesh in the permutation
                            var meshIndex = renderModelPermutation.MeshIndex;
                            var meshCount = renderModelPermutation.MeshCount;
                            var regionName = Info.StringIDs.GetString(region.Name) ?? region.Name.ToString();
                            var permutationName = Info.StringIDs.GetString(permutation.Name) ?? permutation.Name.ToString();

                            Console.WriteLine("Extracting {0} mesh(es) for {1}:{2}...", meshCount, regionName, permutationName);

                            for (var i = 0; i < meshCount; i++)
                            {
                                // Create a MeshReader for the mesh and pass it to the obj extractor
                                var meshReader = new MeshReader(Info.Version, renderModel.Geometry.Meshes[meshIndex + i], definition);
                                objExtractor.ExtractMesh(meshReader, vertexCompressor, resourceStream);
                            }
                        }
                    }
                    else
                    {
                        // No variant - just extract every mesh
                        Console.WriteLine("Extracting {0} mesh(es)...", renderModel.Geometry.Meshes.Count);

                        foreach (var mesh in renderModel.Geometry.Meshes)
                        {
                            // Create a MeshReader for the mesh and pass it to the obj extractor
                            var meshReader = new MeshReader(Info.Version, mesh, definition);
                            objExtractor.ExtractMesh(meshReader, vertexCompressor, resourceStream);
                        }
                    }

                    objExtractor.Finish();
                }
            }
            Console.WriteLine("Done!");
            return true;
        }
    }
}
