using TagTool.IO;
using unic = TagTool.Definitions.multilingual_unicode_string_list;

namespace TagTool.Definitions.ReachBeta
{
    public class multilingual_unicode_string_list : unic
    {
        public multilingual_unicode_string_list(CacheBase Cache, int Address)
        {
            EndianReader Reader = Cache.Reader;
            Reader.SeekTo(Address);

            Reader.SeekTo(Address + 44);
            for (int i = 0; i < 12; i++)
            {
                Indices.Add(Reader.ReadUInt16());
                Lengths.Add(Reader.ReadUInt16());
            }
        }
    }
}
