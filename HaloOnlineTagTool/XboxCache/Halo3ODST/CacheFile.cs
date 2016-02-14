namespace HaloOnlineTagTool.XboxCache.Halo3ODST
{
    public class CacheFile : Halo3Retail.CacheFile
    {
        public CacheFile(string Filename, string Build)
            : base(Filename, Build)
        {
            Version = DefinitionSet.Halo3ODST;
        }
    }
}
