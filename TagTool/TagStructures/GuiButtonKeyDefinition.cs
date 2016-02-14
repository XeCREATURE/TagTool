using System.Collections.Generic;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.TagStructures
{
    [TagStructure(Name = "gui_button_key_definition", Class = "bkey", Size = 0x54)]
    public class GuiButtonKeyDefinition
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
        public TagInstance Strings;
        public List<TextWidget> TextWidgets;
        public List<BitmapWidget> BitmapWidgets;

        [TagStructure(Size = 0x4C)]
        public class TextWidget
        {
            public TagInstance Parent;
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
            public StringID DataSourceName;
            public StringID TextString;
            public StringID TextColor;
            public short TextFont;
            public short Unknown2;
        }

        [TagStructure(Size = 0x6C)]
        public class BitmapWidget
        {
            public TagInstance Parent;
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
}
