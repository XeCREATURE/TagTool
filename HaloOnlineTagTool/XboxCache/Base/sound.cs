using HaloOnlineTagTool.Common;
using HaloOnlineTagTool.Resources.Sounds;

namespace HaloOnlineTagTool.XboxCache
{
    public abstract class sound
    {
        public Bitmask Flags;
        public int SoundClass;
        public SampleRate SampleRate;
        public int Encoding;
        public int CodecIndex;
        public int PlaybackIndex;
        public int DialogueUnknown;
        public int Unknown0;
        public int PitchRangeIndex1;
        public int PitchRangeIndex2;
        public int ScaleIndex;
        public int PromotionIndex;
        public int CustomPlaybackIndex;
        public int ExtraInfoIndex;
        public int Unknown1;
        public int RawID;
        public int MaxPlaytime;
    }
}
