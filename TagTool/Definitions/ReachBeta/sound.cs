using TagTool.IO;
using TagTool.Common;
using snd_ = TagTool.Definitions.sound;
using TagTool.Sounds;

namespace TagTool.Definitions.ReachBeta
{
    public class sound : snd_
    {
        public sound(CacheBase Cache, int Address)
        {
            EndianReader Reader = Cache.Reader;
            Reader.SeekTo(Address);

            Flags = new Bitmask(Reader.ReadInt16());
            SoundClass = Reader.ReadByte();
            SampleRate = (SampleRate)Reader.ReadByte();
            Encoding = Reader.ReadByte();
            CodecIndex = Reader.ReadByte();
            PlaybackIndex = Reader.ReadInt16();
            DialogueUnknown = Reader.ReadInt16();

            Reader.SeekTo(Address + 28);
            RawID = Reader.ReadInt32();
        }
    }
}
