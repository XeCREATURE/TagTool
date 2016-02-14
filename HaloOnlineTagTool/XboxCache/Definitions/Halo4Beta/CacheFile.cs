namespace HaloOnlineTagTool.XboxCache.Definitions.Halo4Beta
{
    public class CacheFile : Halo3Retail.CacheFile
    {
        public CacheFile(string Filename, string Build)
            : base(Filename, Build)
        {
            Version = DefinitionSet.Halo4Beta;
        }
    }
}
