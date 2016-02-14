using System.Collections.Generic;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.TagStructures
{
    [TagStructure(Name = "user_interface_sounds_definition", Class = "uise", Size = 0x150)]
    public class UserInterfaceSoundsDefinition
    {
        public TagInstance Error;
        public TagInstance VerticalNavigation;
        public TagInstance HorizontalNavigation;
        public TagInstance AButton;
        public TagInstance BButton;
        public TagInstance XButton;
        public TagInstance YButton;
        public TagInstance StartButton;
        public TagInstance BackButton;
        public TagInstance LeftBumper;
        public TagInstance RightBumper;
        public TagInstance LeftTrigger;
        public TagInstance RightTrigger;
        public TagInstance TimerSound;
        public TagInstance TimerSoundZero;
        public TagInstance AltTimerSound;
        public TagInstance SecondAltTimerSound;
        public TagInstance MatchmakingAdvanceSound;
        public TagInstance RankUp;
        public TagInstance MatchmakingPartyUpSound;
        public List<AtlasSound> AtlasSounds;
        public uint Unknown;

        [TagStructure(Size = 0x14)]
        public class AtlasSound
        {
            public StringID Name;
            public TagInstance Sound;
        }
    }
}
