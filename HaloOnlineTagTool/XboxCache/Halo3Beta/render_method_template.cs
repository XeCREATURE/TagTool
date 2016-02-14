using HaloOnlineTagTool.Endian;
using rmt2 = HaloOnlineTagTool.XboxCache.render_method_template;

namespace HaloOnlineTagTool.XboxCache.Halo3Beta
{
    public class render_method_template : rmt2
    {
        public render_method_template(CacheBase Cache, int Address)
        {
            EndianReader Reader = Cache.Reader;
            Reader.SeekTo(Address);

            #region Usage Blocks
            Reader.SeekTo(Address + 72);
            int iCount = Reader.ReadInt32();
            int iOffset = Reader.ReadInt32() - Cache.Magic;
            for (int i = 0; i < iCount; i++)
                ArgumentBlocks.Add(new ArgumentBlock(Cache, iOffset + 4 * i));
            #endregion

            #region Usage Blocks
            Reader.SeekTo(Address + 108);
            iCount = Reader.ReadInt32();
            iOffset = Reader.ReadInt32() - Cache.Magic;
            for (int i = 0; i < iCount; i++)
                UsageBlocks.Add(new UsageBlock(Cache, iOffset + 4 * i));
            #endregion
        }

        new public class ArgumentBlock : rmt2.ArgumentBlock
        {
            public ArgumentBlock(CacheBase Cache, int Address)
            {
                EndianReader Reader = Cache.Reader;
                Reader.SeekTo(Address);

                Argument = Cache.Strings.GetItemByID(Reader.ReadInt32());
            }
        }

        new public class UsageBlock : rmt2.UsageBlock
        {
            public UsageBlock(CacheBase Cache, int Address)
            {
                EndianReader Reader = Cache.Reader;
                Reader.SeekTo(Address);

                Usage = Cache.Strings.GetItemByID(Reader.ReadInt32());
            }
        }
    }
}
