using TagTool.Serialization;

namespace TagTool.TagStructures
{
    [TagStructure(Name = "gui_widget_animation_definition", Class = "wgan", Size = 0x80)]
    public class GuiWidgetAnimationDefinition
    {
        public uint Unknown;
        public uint Unknown2;
        public TagInstance WidgetColor;
        public TagInstance WidgetPosition;
        public TagInstance WidgetRotation;
        public TagInstance WidgetScale;
        public TagInstance WidgetTextureCoordinate;
        public TagInstance WidgetSprite;
        public TagInstance WidgetFont;
        public uint Unknown3;
        public uint Unknown4;
    }
}
