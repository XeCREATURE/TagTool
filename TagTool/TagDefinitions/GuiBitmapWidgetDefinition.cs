using TagTool.Common;
using TagTool.Serialization;
using TagTool.TagGroups;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "gui_bitmap_widget_definition", Class = "bmp3", Size = 0x60)]
    public class GuiBitmapWidgetDefinition
    {
        public uint Flags;
        public StringID Name;
        public short Unknown;
        public short Layer;
        public short WidescreenYBoundsMin;
        public short WidescreenXBoundsMin;
        public short WidescreenYBoundsMax;
        public short WidescreenXBoundsMax;
        public short StandardYBoundsMin;
        public short StandardXBoundsMin;
        public short StandardYBoundsMax;
        public short StandardXBoundsMax;
        public TagInstance Animation;
        public TagInstance Bitmap;
        public TagInstance Unknown2;
        public BlendMethodValue BlendMethod;
        public short Unknown3;
        public short SpriteIndex;
        public short Unknown4;
        public StringID DataSourceName;
        public StringID SpriteDataSourceName;
        public uint Unknown5;

        public enum BlendMethodValue : short
        {
            Standard,
            Unknown,
            Unknown2,
            Alpha,
            Overlay,
            Unknown3,
            LighterColor,
            Unknown4,
            Unknown5,
            Unknown6,
            InvertedAlpha,
            Unknown7,
            Unknown8,
            Unknown9,
        }
    }
}
