using HaloOnlineTagTool.Endian;
using bik = HaloOnlineTagTool.XboxCache.Definitions.bink;

namespace HaloOnlineTagTool.XboxCache.Definitions.Halo4Retail
{
    public class bink : bik
    {
        public bink(CacheBase Cache, int Address)
        {
            EndianReader Reader = Cache.Reader;
            Reader.SeekTo(Address);

            Reader.Skip(4);
            RawID = Reader.ReadInt32();
        }
    }
}
