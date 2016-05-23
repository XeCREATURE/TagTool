using System.Collections.Generic;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.TagGroups;

namespace TagTool.TagDefinitions
{
    [TagStructure(Class = "item", Size = 0xB4)]
    public class Item : GameObject
    {
        public uint Flags2;
        public short OldMessageIndex;
        public short SortOrder;
        public float OldMultiplayerOnGroundScale;
        public float OldCampaignOnGroundScale;
        public StringID PickupMessage;
        public StringID SwapMessage;
        public StringID PickupOrDualWieldMessage;
        public StringID SwapOrDualWieldMessage;
        public StringID PickedUpMessage;
        public StringID SwitchToMessage;
        public StringID SwitchToFromAiMessage;
        public StringID AllWeaponsEmptyMessage;
        public TagInstance CollisionSound;
        public List<TagInstance> PredictedBitmaps;
        public TagInstance DetonationDamageEffect;
        public float DetonationDelayMin;
        public float DetonationDelayMax;
        public TagInstance DetonatingEffect;
        public TagInstance DetonationEffect;
        public float CampaignGroundScale;
        public float MultiplayerGroundScale;
        public float SmallHoldScale;
        public float SmallHolsterScale;
        public float MediumHoldScale;
        public float MediumHolsterScale;
        public float LargeHoldScale;
        public float LargeHolsterScale;
        public float HugeHoldScale;
        public float HugeHolsterScale;
        public float GroundedFrictionLength;
        public float GroundedFrictionUnknown;
    }
}
