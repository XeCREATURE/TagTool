using TagTool.IO;

namespace TagTool.Resources.Sounds
{
    public class SoundPackInfo
    {
        public string Name { get; set; }

        public EndianReader Reader { get; set; }

        public SoundPack Pack { get; set; }

        public override string ToString() => Name;
    }

    public class SoundFileInfo
    {
        public EndianReader Reader { get; set; }

        public int Offset { get; set; }

        public int Size { get; set; }

        public uint ID { get; set; }

        public SoundFormat Format { get; set; }
    }
}
