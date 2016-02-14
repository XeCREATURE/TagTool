using System.IO;
using TagTool.Definitions;

namespace TagTool.Resources.Geometry
{
    public static class VertexStreamFactory
    {
        /// <summary>
        /// Creates a vertex stream for a given engine version.
        /// </summary>
        /// <param name="version">The engine version.</param>
        /// <param name="stream">The base stream.</param>
        /// <returns>The created vertex stream.</returns>
        public static IVertexStream Create(DefinitionSet version, Stream stream)
        {
            if (Definition.Compare(version, DefinitionSet.HaloOnline235640) >= 0)
                return new Definitions.HaloOnline235640.VertexStream(stream);
            return new Definitions.HaloOnline106708.VertexStream(stream);
        }
    }
}
