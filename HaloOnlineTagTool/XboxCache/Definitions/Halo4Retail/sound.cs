﻿using HaloOnlineTagTool.Endian;
using snd_ = HaloOnlineTagTool.XboxCache.Definitions.sound;

namespace HaloOnlineTagTool.XboxCache.Definitions.Halo4Retail
{
    public class sound : snd_
    {
        public uint SoundAddress1;
        public uint SoundAddress2;
        public int SoundBankTagID;

        public sound(CacheBase Cache, int Address)
        {
            EndianReader Reader = Cache.Reader;
            Reader.SeekTo(Address);

            Reader.SeekTo(Address + 12);
            SoundAddress1 = Reader.ReadUInt32();
            SoundAddress2 = Reader.ReadUInt32();

            Reader.SeekTo(Address + 52);
            SoundBankTagID = Reader.ReadInt32();
        }
    }
}
