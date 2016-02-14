using TagTool.Definitions;

namespace TagTool.Cache
{
    public static class StringIDResolverFactory
    {
        /// <summary>
        /// Creates a stringID resolver for a given engine version.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns>The resolver.</returns>
        public static StringIDResolverBase Create(DefinitionSet version)
        {
            if (Definition.Compare(version, DefinitionSet.HaloOnline700123) >= 0)
                return new Definitions.HaloOnline700123.StringIDResolver();
            if (Definition.Compare(version, DefinitionSet.HaloOnline498295) >= 0)
                return new Definitions.HaloOnline498295.StringIDResolver();
            return new Definitions.HaloOnline106708.StringIDResolver();
        }
    }
}
