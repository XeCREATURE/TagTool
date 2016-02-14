using HaloOnlineTagTool.Endian;
using unic = HaloOnlineTagTool.XboxCache.Definitions.multilingual_unicode_string_list;

namespace HaloOnlineTagTool.XboxCache.Definitions.Halo3Beta
{
    public class multilingual_unicode_string_list : unic
    {
        public multilingual_unicode_string_list(CacheBase Cache, int Address)
        {
            EndianReader Reader = Cache.Reader;
            Reader.SeekTo(Address);

            Reader.SeekTo(Address + 32);
            for (int i = 0; i < 12; i++)
            {
                Indices.Add(Reader.ReadUInt16());
                Lengths.Add(Reader.ReadUInt16());
            }
        }
    }
}
