using System;
using System.Collections.Generic;
using TagTool.Common;
using TagTool.Definitions;
using TagTool.Serialization;
using TagTool.TagGroups;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "chud_definition", Class = "chdt", Size = 0x18)]
    public class ChudDefinition
    {
        public List<HudWidget> HudWidgets;
        public int LowClipCutoff;
        public int LowAmmoCutoff;
        public int AgeCutoff;

        [TagStructure(Size = 0x60)]
        public class HudWidget
        {
            public StringID Name;
            public SpecialHudTypeValue SpecialHudType;
            public byte Unknown;
            public byte Unknown2;
            public List<StateDatum> StateData;
            public List<PlacementDatum> PlacementData;
            public List<AnimationDatum> AnimationData;
            public List<RenderDatum> RenderData;
            public TagInstance ParallaxData;
            public List<BitmapWidget> BitmapWidgets;
            public List<TextWidget> TextWidgets;

            public enum SpecialHudTypeValue : short
            {
                Unspecial,
                Ammo,
                CrosshairAndScope,
                UnitShieldMeter,
                Grenades,
                Gametype,
                MotionSensor,
                SpikeGrenade,
                FirebombGrenade,
                Compass,
                Stamina,
                EnergyMeter,
                Consumable
            }

            [TagStructure(Size = 0x44, MaxVersion = DefinitionSet.HaloOnline571627)]
            [TagStructure(Size = 0x48, MinVersion = DefinitionSet.HaloOnline700123)]
            public class StateDatum
            {
                public Flags1EngineValue Flags1Engine;
                public Flags2Value Flags2;
                public Flags3Value Flags3;
                public Flags4ResolutionValue Flags4Resolution;
                public Flags5ScoreboardValue Flags5Scoreboard;
                public Flags6ScoreboardBValue Flags6ScoreboardB;
                public Flags7Value Flags7;
                public Flags7BValue Flags7B;
                public Flags7EditorValue Flags7Editor;
                public Flags9Value Flags9;
                public Flags10SkullsValue Flags10Skulls;
                public Flags11Value Flags11;
                public Flags12Value Flags12;
                public Flags13Value Flags13;
                public Flags14Value Flags14;
                public Flags15Value Flags15;
                public Flags16Value Flags16;
                public Flags17Value Flags17;
                public Flags18Value Flags18;
                public Flags19Value Flags19;
                public Flags20Value Flags20;
                public ushort Flags21;
                public ushort Flags22;
                public ushort Flags23;
                public ushort Flags24;
                public ushort Flags25;
                public ushort Flags26;
                public ushort Flags27;
                public ushort Flags28;
                public ushort Flags29;
                public ushort Flags30;
                public ushort Flags31;
                public ushort Flags32;
                public ushort Flags33;
                [MinVersion(DefinitionSet.HaloOnline700123)] public ushort Flags34;
                [MinVersion(DefinitionSet.HaloOnline700123)] public ushort Flags35;

                [Flags]
                public enum Flags1EngineValue : ushort
                {
                    None,
                    Bit0 = 1 << 0,
                    Bit1 = 1 << 1,
                    Survival = 1 << 2,
                    Editor = 1 << 3,
                    Theater = 1 << 4,
                    CTF = 1 << 5,
                    Slayer = 1 << 6,
                    Oddball = 1 << 7,
                    KOTH = 1 << 8,
                    Juggernaut = 1 << 9,
                    Territories = 1 << 10,
                    Assault = 1 << 11,
                    VIP = 1 << 12,
                    Infection = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15
                }

                [Flags]
                public enum Flags2Value : ushort
                {
                    None,
                    Biped1 = 1 << 0,
                    Biped2 = 1 << 1,
                    Biped3 = 1 << 2,
                    Bit3 = 1 << 3,
                    Bit4 = 1 << 4,
                    Bit5 = 1 << 5,
                    Bit6 = 1 << 6,
                    Bit7 = 1 << 7,
                    Bit8 = 1 << 8,
                    Bit9 = 1 << 9,
                    Bit10 = 1 << 10,
                    Bit11 = 1 << 11,
                    Bit12 = 1 << 12,
                    Bit13 = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15
                }

                [Flags]
                public enum Flags3Value : ushort
                {
                    None,
                    Offense = 1 << 0,
                    Defense = 1 << 1,
                    FreeForAll = 1 << 2,
                    Bit3 = 1 << 3,
                    Bit4 = 1 << 4,
                    Bit5 = 1 << 5,
                    TalkingDisabled = 1 << 6,
                    TapToTalk = 1 << 7,
                    TalkingEnabled = 1 << 8,
                    NotTalking = 1 << 9,
                    Talking = 1 << 10,
                    Bit11 = 1 << 11,
                    Bit12 = 1 << 12,
                    Bit13 = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15
                }

                [Flags]
                public enum Flags4ResolutionValue : ushort
                {
                    None,
                    Bit0 = 1 << 0,
                    Bit1 = 1 << 1,
                    Bit2 = 1 << 2,
                    Bit3 = 1 << 3,
                    Bit4 = 1 << 4,
                    Bit5 = 1 << 5,
                    Bit6 = 1 << 6,
                    Bit7 = 1 << 7,
                    Bit8 = 1 << 8,
                    Bit9 = 1 << 9,
                    Bit10 = 1 << 10,
                    Bit11 = 1 << 11,
                    Bit12 = 1 << 12,
                    Bit13 = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15
                }

                [Flags]
                public enum Flags5ScoreboardValue : ushort
                {
                    None,
                    HasFriends = 1 << 0,
                    HasEnemies = 1 << 1,
                    HasVariantName = 1 << 2,
                    SomeoneIsTalking = 1 << 3,
                    IsArming = 1 << 4,
                    TimeEnabled = 1 << 5,
                    Bit6 = 1 << 6,
                    Bit7 = 1 << 7,
                    FriendlyAction = 1 << 8,
                    EnemyAction = 1 << 9,
                    XIsDown = 1 << 10,
                    AttackerBombDropped = 1 << 11,
                    AttackedBombPickedUp = 1 << 12,
                    DefenderTeamIsDead = 1 << 13,
                    AttackerTeamIsDead = 1 << 14,
                    Bit15 = 1 << 15
                }

                [Flags]
                public enum Flags6ScoreboardBValue : ushort
                {
                    None,
                    SummaryEnabled = 1 << 0,
                    NetDebug = 1 << 1,
                    Bit2 = 1 << 2,
                    Bit3 = 1 << 3,
                    Bit4 = 1 << 4,
                    Bit5 = 1 << 5,
                    Bit6 = 1 << 6,
                    Bit7 = 1 << 7,
                    Bit8 = 1 << 8,
                    Bit9 = 1 << 9,
                    Bit10 = 1 << 10,
                    Bit11 = 1 << 11,
                    Bit12 = 1 << 12,
                    Bit13 = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15
                }

                [Flags]
                public enum Flags7Value : ushort
                {
                    None,
                    Bit0 = 1 << 0,
                    Autoaim = 1 << 1,
                    Bit2 = 1 << 2,
                    Bit3 = 1 << 3,
                    TrainingPrompt = 1 << 4,
                    ObjectivePrompt = 1 << 5,
                    SurvivalState = 1 << 6,
                    Bit7 = 1 << 7,
                    Bit8 = 1 << 8,
                    Bit9 = 1 << 9,
                    Bit10 = 1 << 10,
                    Bit11 = 1 << 11,
                    Bit12 = 1 << 12,
                    Bit13 = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15
                }

                [Flags]
                public enum Flags7BValue : ushort
                {
                    None,
                    Bit0 = 1 << 0,
                    Bit1 = 1 << 1,
                    Bit2 = 1 << 2,
                    Bit3 = 1 << 3,
                    Bit4 = 1 << 4,
                    Bit5 = 1 << 5,
                    Bit6 = 1 << 6,
                    Bit7 = 1 << 7,
                    Bit8 = 1 << 8,
                    Bit9 = 1 << 9,
                    Bit10 = 1 << 10,
                    Bit11 = 1 << 11,
                    Bit12 = 1 << 12,
                    Bit13 = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15
                }

                [Flags]
                public enum Flags7EditorValue : ushort
                {
                    None,
                    EditorInactive = 1 << 0,
                    EditorAcitve = 1 << 1,
                    EditorHolding = 1 << 2,
                    EditorNotAllowed = 1 << 3,
                    IsEditorBiped = 1 << 4,
                    Bit5 = 1 << 5,
                    Bit6 = 1 << 6,
                    Bit7 = 1 << 7,
                    Bit8 = 1 << 8,
                    Bit9 = 1 << 9,
                    Bit10 = 1 << 10,
                    Bit11 = 1 << 11,
                    Bit12 = 1 << 12,
                    Bit13 = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15
                }

                [Flags]
                public enum Flags9Value : ushort
                {
                    None,
                    MotionTracker10M = 1 << 0,
                    MotionTracker25M = 1 << 1,
                    MotionTracker75M = 1 << 2,
                    MotionTracker150M = 1 << 3,
                    Bit4 = 1 << 4,
                    MetagamePlayer2Exists = 1 << 5,
                    Bit6 = 1 << 6,
                    MetagamePlayer3Exists = 1 << 7,
                    Bit8 = 1 << 8,
                    MetagamePlayer4Exists = 1 << 9,
                    Bit10 = 1 << 10,
                    MetagameScoreAdded = 1 << 11,
                    Bit12 = 1 << 12,
                    MetagameScoreRemoved = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15
                }

                [Flags]
                public enum Flags10SkullsValue : ushort
                {
                    None,
                    IronSkullEnabled = 1 << 0,
                    BlackEyeSkullEnabled = 1 << 1,
                    ToughLuckSkullEnabled = 1 << 2,
                    CatchSkullEnabled = 1 << 3,
                    CloudSkullEnabled = 1 << 4,
                    FamineSkullEnabled = 1 << 5,
                    ThunderstormSkullEnabled = 1 << 6,
                    TiltSkullEnabled = 1 << 7,
                    MythicSkullEnabled = 1 << 8,
                    AssassinsSkullEnabled = 1 << 9,
                    BlindSkullEnabled = 1 << 10,
                    CowbellSkullEnabled = 1 << 11,
                    GruntBirthdayPartySkullEnabled = 1 << 12,
                    IWHBYDSkullEnabled = 1 << 13,
                    ThirdPersonSkullEnabled = 1 << 14,
                    DirectorsCutSkullEnabled = 1 << 15
                }

                [Flags]
                public enum Flags11Value : ushort
                {
                    None,
                    Wave1Background = 1 << 0,
                    Wave2Background = 1 << 1,
                    Wave3Background = 1 << 2,
                    Wave4Background = 1 << 3,
                    Bit4 = 1 << 4,
                    Bit5 = 1 << 5,
                    BonusRound = 1 << 6,
                    Bit7 = 1 << 7,
                    Bit8 = 1 << 8,
                    Bit9 = 1 << 9,
                    Bit10 = 1 << 10,
                    Bit11 = 1 << 11,
                    Bit12 = 1 << 12,
                    Bit13 = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15
                }

                [Flags]
                public enum Flags12Value : ushort
                {
                    None,
                    Bit0 = 1 << 0,
                    Wave1 = 1 << 1,
                    Wave2 = 1 << 2,
                    Wave3 = 1 << 3,
                    Wave4 = 1 << 4,
                    Wave5 = 1 << 5,
                    Bit6 = 1 << 6,
                    Bit7 = 1 << 7,
                    Bit8 = 1 << 8,
                    Bit9 = 1 << 9,
                    Bit10 = 1 << 10,
                    Bit11 = 1 << 11,
                    Bit12 = 1 << 12,
                    Bit13 = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15
                }

                [Flags]
                public enum Flags13Value : ushort
                {
                    None,
                    Bit0 = 1 << 0,
                    Bit1 = 1 << 1,
                    Bit2 = 1 << 2,
                    Bit3 = 1 << 3,
                    Bit4 = 1 << 4,
                    Bit5 = 1 << 5,
                    Bit6 = 1 << 6,
                    Bit7 = 1 << 7,
                    Bit8 = 1 << 8,
                    Bit9 = 1 << 9,
                    Bit10 = 1 << 10,
                    Bit11 = 1 << 11,
                    Bit12 = 1 << 12,
                    Bit13 = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15
                }

                [Flags]
                public enum Flags14Value : ushort
                {
                    None,
                    Bit0 = 1 << 0,
                    Bit1 = 1 << 1,
                    Bit2 = 1 << 2,
                    Bit3 = 1 << 3,
                    Bit4 = 1 << 4,
                    Bit5 = 1 << 5,
                    Bit6 = 1 << 6,
                    Bit7 = 1 << 7,
                    Bit8 = 1 << 8,
                    Bit9 = 1 << 9,
                    Bit10 = 1 << 10,
                    Bit11 = 1 << 11,
                    Bit12 = 1 << 12,
                    Bit13 = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15
                }

                [Flags]
                public enum Flags15Value : ushort
                {
                    None,
                    Bit0 = 1 << 0,
                    Bit1 = 1 << 1,
                    Bit2 = 1 << 2,
                    Bit3 = 1 << 3,
                    Bit4 = 1 << 4,
                    Bit5 = 1 << 5,
                    Bit6 = 1 << 6,
                    Bit7 = 1 << 7,
                    Bit8 = 1 << 8,
                    Bit9 = 1 << 9,
                    Bit10 = 1 << 10,
                    Bit11 = 1 << 11,
                    Bit12 = 1 << 12,
                    Bit13 = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15
                }

                [Flags]
                public enum Flags16Value : ushort
                {
                    None,
                    PickupGrenades = 1 << 0,
                    Bit1 = 1 << 1,
                    Bit2 = 1 << 2,
                    Bit3 = 1 << 3,
                    Bit4 = 1 << 4,
                    LivesAdded = 1 << 5,
                    Consumable1Unknown = 1 << 6,
                    Consumable2Unknown = 1 << 7,
                    Consumable3Unknown = 1 << 8,
                    Consumable4Unknown = 1 << 9,
                    Bit10 = 1 << 10,
                    Bit11 = 1 << 11,
                    HitMarkerLow = 1 << 12,
                    HitMarkerMedium = 1 << 13,
                    HitMarkerHigh = 1 << 14,
                    Bit15 = 1 << 15
                }

                [Flags]
                public enum Flags17Value : ushort
                {
                    None,
                    BinocularsEnabled = 1 << 0,
                    UnitIsZoomedLevel1 = 1 << 1,
                    UnitIsZoomedLevel2 = 1 << 2,
                    Bit3 = 1 << 3,
                    Bit4 = 1 << 4,
                    Bit5 = 1 << 5,
                    Bit6 = 1 << 6,
                    Bit7 = 1 << 7,
                    Bit8 = 1 << 8,
                    Bit9 = 1 << 9,
                    Bit10 = 1 << 10,
                    Bit11 = 1 << 11,
                    Bit12 = 1 << 12,
                    Bit13 = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15
                }

                [Flags]
                public enum Flags18Value : ushort
                {
                    None,
                    PrimaryWeapon = 1 << 0,
                    SecondaryWeapon = 1 << 1,
                    Bit2 = 1 << 2,
                    Bit3 = 1 << 3,
                    Bit4 = 1 << 4,
                    Bit5 = 1 << 5,
                    Bit6 = 1 << 6,
                    Bit7 = 1 << 7,
                    Bit8 = 1 << 8,
                    Bit9 = 1 << 9,
                    Bit10 = 1 << 10,
                    Bit11 = 1 << 11,
                    Bit12 = 1 << 12,
                    Bit13 = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15
                }

                [Flags]
                public enum Flags19Value : ushort
                {
                    None,
                    Bit0 = 1 << 0,
                    Bit1 = 1 << 1,
                    Bit2 = 1 << 2,
                    Bit3 = 1 << 3,
                    Bit4 = 1 << 4,
                    Bit5 = 1 << 5,
                    Bit6 = 1 << 6,
                    Bit7 = 1 << 7,
                    Bit8 = 1 << 8,
                    Bit9 = 1 << 9,
                    Bit10 = 1 << 10,
                    Bit11 = 1 << 11,
                    Bit12 = 1 << 12,
                    Bit13 = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15
                }

                [Flags]
                public enum Flags20Value : ushort
                {
                    None,
                    MotionTrackerEnabled = 1 << 0,
                    Bit1 = 1 << 1,
                    SelectedFragGrenades = 1 << 2,
                    SelectedPlasmaGrenades = 1 << 3,
                    SelectedSpikeGrenades = 1 << 4,
                    SelectedFireGrenades = 1 << 5,
                    Bit6 = 1 << 6,
                    VisionWarning = 1 << 7,
                    Bit8 = 1 << 8,
                    Bit9 = 1 << 9,
                    Bit10 = 1 << 10,
                    Bit11 = 1 << 11,
                    HasOvershieldLevel1 = 1 << 12,
                    HasOvershieldLevel2 = 1 << 13,
                    HasOvershieldLevel3 = 1 << 14,
                    HasShields = 1 << 15
                }
            }

            [TagStructure(Size = 0x1C)]
            public class PlacementDatum
            {
                public AnchorValue Anchor;
                public short Unknown;
                public Vector2 MirrorOffset;
                public Vector2 Offset;
                public Vector2 Scale;

                public enum AnchorValue : short
                {
                    TopLeft,
                    TopRight,
                    BottomRight,
                    BottomLeft,
                    Center,
                    TopEdge,
                    GrenadeA,
                    GrenadeB,
                    GrenadeC,
                    GrenadeD,
                    ScoreboardFriendly,
                    ScoreboardEnemy,
                    HealthAndShield,
                    BottomEdge,
                    Unknown,
                    Equipment,
                    Unknown2,
                    Depreciated,
                    Depreciated2,
                    Depreciated3,
                    Depreciated4,
                    Depreciated5,
                    Unknown3,
                    Gametype,
                    Unknown4,
                    StateRight,
                    StateLeft,
                    StateCenter,
                    Unknown5,
                    GametypeFriendly,
                    GametypeEnemy,
                    MetagameTop,
                    MetagamePlayer1,
                    MetagamePlayer2,
                    MetagamePlayer3,
                    MetagamePlayer4,
                    Theater
                }
            }

            [TagStructure(Size = 0x90)]
            public class AnimationDatum
            {
                public uint Unknown;
                public TagInstance Animation1;
                public uint Unknown2;
                public uint Unknown3;
                public TagInstance Animation2;
                public uint Unknown4;
                public uint Unknown5;
                public TagInstance Animation3;
                public uint Unknown6;
                public uint Unknown7;
                public TagInstance Animation4;
                public uint Unknown8;
                public uint Unknown9;
                public TagInstance Animation5;
                public uint Unknown10;
                public uint Unknown11;
                public TagInstance Animation6;
                public uint Unknown12;
            }

            [TagStructure(Size = 0x4)]
            public class RgbaColor
            {
                public byte R;
                public byte G;
                public byte B;
                public byte A;
            }

            [TagStructure(Size = 0x48)]
            public class RenderDatum
            {
                public ShaderIndexValue ShaderIndex;
                public short Unknown;
                public InputValue Input;
                public InputValue RangeInput;
                [TagField(Count = 0x4)] public RgbaColor[] Colors;
                public float LocalScalarA;
                public float LocalScalarB;
                public float LocalScalarC;
                public float LocalScalarD;
                public OutputColorValue OutputColorA;
                public OutputColorValue OutputColorB;
                public OutputColorValue OutputColorC;
                public OutputColorValue OutputColorD;
                public OutputColorValue OutputColorE;
                public OutputColorValue OutputColorF;
                public OutputScalarValue OutputScalarA;
                public OutputScalarValue OutputScalarB;
                public OutputScalarValue OutputScalarC;
                public OutputScalarValue OutputScalarD;
                public OutputScalarValue OutputScalarE;
                public OutputScalarValue OutputScalarF;
                public short Unknown2;
                public short Unknown3;
                public short Unknown4;
                public short Unknown5;

                public enum ShaderIndexValue : short
                {
                    Simple,
                    Meter,
                    TextSimple,
                    MeterShield,
                    MeterGradient,
                    Crosshair,
                    DirectionalDamage,
                    Solid,
                    Sensor,
                    MeterSingleColor,
                    Navpoint,
                    Medal,
                    TextureCam,
                    CortanaScreen,
                    CortanaCamera,
                    CortanaOffscreen,
                    CortanaScreenFinal,
                    MeterChapter,
                    MeterDoubleGradient,
                    MeterRadialGradient,
                    Turbulence,
                    Emblem,
                    CortanaComposite,
                    DirectionalDamageApply,
                    ReallySimple,
                    Unknown
                }

                public enum InputValue : short
                {
                    Zero,
                    One,
                    Time,
                    Fade,
                    UnitHealthCurrent,
                    UnitHealth,
                    UnitShieldCurrent,
                    UnitShield,
                    ClipAmmoFraction,
                    TotalAmmoFraction,
                    WeaponVersionNumber,
                    HeatFraction,
                    BatteryFraction,
                    WeaponErrorCurrent1,
                    WeaponErrorCurrent2,
                    Pickup,
                    UnitAutoaimed,
                    Grenade,
                    GrenadeFraction,
                    ChargeFraction,
                    FriendlyScore,
                    EnemyScore,
                    ScoreToWin,
                    ArmingFraction,
                    UnknownX18,
                    Unit1xOvershieldCurrent,
                    Unit1xOvershield,
                    Unit2xOvershieldCurrent,
                    Unit2xOvershield,
                    Unit3xOvershieldCurrent,
                    Unit3xOvershield,
                    AimYaw,
                    AimPitch,
                    TargetDistance,
                    TargetElevation,
                    EditorBudget,
                    EditorBudgetCost,
                    FilmTotalTime,
                    FilmCurrentTime,
                    UnknownX27,
                    FilmTimelineFraction1,
                    FilmTimelineFraction2,
                    UnknownX2a,
                    UnknownX2b,
                    MetagameTime,
                    MetagameScoreTransient,
                    MetagameScorePlayer1,
                    MetagameScorePlayer2,
                    MetagameScorePlayer3,
                    MetagameScorePlayer4,
                    MetagameModifier,
                    MetagameSkullModifier,
                    SensorRange,
                    NetdebugLatency,
                    NetdebugLatencyQuality,
                    NetdebugHostQuality,
                    NetdebugLocalQuality,
                    MetagameScoreNegative,
                    SurvivalCurrentSet,
                    UnknownX3b,
                    UnknownX3c,
                    SurvivalCurrentLives,
                    SurvivalBonusTime,
                    SurvivalBonusScore,
                    SurvivalMultiplier,
                    UnknownX41,
                    UnknownX42,
                    UnknownX43,
                    UnknownX44,
                    UnknownX45,
                    UnknownX46,
                    UnknownX47,
                    UnknownX48,
                    UnknownX49,
                    UnknownX4a,
                    UnknownX4b,
                    UnknownX4c,
                    UnknownX4d,
                    Consumable1Icon,
                    Consumable2Icon,
                    UnknownX50,
                    UnknownX51,
                    UnknownX52,
                    Consumable3Icon,
                    Consumable4Icon,
                    ConsumableName,
                    UnknownX56,
                    UnknownX57,
                    UnknownX58,
                    ConsumableCooldownText,
                    ConsumableCooldownMeter,
                    UnknownX5b,
                    UnknownX5c,
                    UnknownX5d,
                    UnknownX5e,
                    Consumable1Charge,
                    Consumable2Charge,
                    Consumable3Charge,
                    Consumable4Charge,
                    UnknownX63,
                    UnknownX64,
                    EnergyMeter1,
                    EnergyMeter2,
                    EnergyMeter3,
                    EnergyMeter4,
                    EnergyMeter5,
                    Consumable1Cost,
                    Consumable2Cost,
                    Consumable3Cost,
                    Consumable4Cost,
                    UnitStaminaCurrent
                }
                
                public enum OutputColorValue : short
                {
                    LocalA,
                    LocalB,
                    LocalC,
                    LocalD,
                    Unknown4,
                    Unknown5,
                    ScoreboardFriendly,
                    ScoreboardEnemy,
                    ArmingTeam,
                    MetagamePlayer1,
                    MetagamePlayer2,
                    MetagamePlayer3,
                    MetagamePlayer4,
                    Unknown13,
                    Unknown14,
                    GlobalDynamic0,
                    GlobalDynamic1,
                    GlobalDynamic2,
                    GlobalDynamic3,
                    GlobalDynamic4,
                    GlobalDynamic5,
                    GlobalDynamic6,
                    GlobalDynamic7,
                    GlobalDynamic8,
                    GlobalDynamic9,
                    GlobalDynamic10,
                    GlobalDynamic11,
                    GlobalDynamic12,
                    GlobalDynamic13,
                    GlobalDynamic14,
                    GlobalDynamic15,
                    GlobalDynamic16,
                    GlobalDynamic17,
                    GlobalDynamic18,
                    GlobalDynamic19,
                    GlobalDynamic20,
                    GlobalDynamic21,
                    GlobalDynamic22,
                    GlobalDynamic23,
                    GlobalDynamic24,
                    GlobalDynamic25,
                    GlobalDynamic26,
                    GlobalDynamic27,
                    GlobalDynamic28,
                    GlobalDynamic29,
                    GlobalDynamic30,
                    GlobalDynamic31,
                    GlobalDynamic32,
                    GlobalDynamic33,
                    GlobalDynamic34,
                    GlobalDynamic35,
                    GlobalDynamic36
                }
                
                public enum OutputScalarValue : short
                {
                    Input,
                    RangeInput,
                    LocalA,
                    LocalB,
                    LocalC,
                    LocalD,
                    Unknown6,
                    Unknown7
                }
            }

            [TagStructure(Size = 0x54)]
            public class BitmapWidget
            {
                public StringID Name;
                public SpecialHudTypeValue SpecialHudType;
                public byte Unknown;
                public byte Unknown2;
                public List<StateDatum> StateData;
                public List<PlacementDatum> PlacementData;
                public List<AnimationDatum> AnimationData;
                public List<RenderDatum> RenderData;
                public int WidgetIndex;
                public FlagsValue Flags;
                public short Unknown3;
                public TagInstance Bitmap;
                public byte BitmapSpriteIndex;
                public byte Unknown4;
                public byte Unknown5;
                public byte Unknown6;

                [Flags]
                public enum FlagsValue : ushort
                {
                    None,
                    MirrorHorizontally = 1 << 0,
                    MirrorVertically = 1 << 1,
                    StretchEdges = 1 << 2,
                    EnableTextureCam = 1 << 3,
                    Looping = 1 << 4,
                    Bit5 = 1 << 5,
                    Player1Emblem = 1 << 6,
                    Player2Emblem = 1 << 7,
                    Player3Emblem = 1 << 8,
                    Player4Emblem = 1 << 9,
                    Bit10 = 1 << 10,
                    Bit11 = 1 << 11,
                    Bit12 = 1 << 12,
                    Bit13 = 1 << 13,
                    InputControlsConsumable = 1 << 14,
                    InputControlsWeapon = 1 << 15
                }
            }

            [TagStructure(Size = 0x48)]
            public class TextWidget
            {
                public StringID Name;
                public SpecialHudTypeValue SpecialHudType;
                public byte Unknown1;
                public byte Unknown2;
                public List<StateDatum> StateData;
                public List<PlacementDatum> PlacementData;
                public List<AnimationDatum> AnimationData;
                public List<RenderDatum> RenderData;
                public int WidgetIndex;
                public FlagsValue Flags;
                public FontValue Font;
                public short Unknown3;
                public StringID String;
                
                [Flags]
                public enum FlagsValue : int
                {
                    None,
                    StringIsANumber = 1 << 0,
                    Force2Digit = 1 << 1,
                    Force3Digit = 1 << 2,
                    PlusSuffix = 1 << 3,
                    MSuffix = 1 << 4,
                    TenthsDecimal = 1 << 5,
                    HundredthsDecimal = 1 << 6,
                    ThousandthsDecimal = 1 << 7,
                    HundredThousandthsDecimal = 1 << 8,
                    OnlyANumber = 1 << 9,
                    XSuffix = 1 << 10,
                    InBrackets = 1 << 11,
                    TimeFormat_S_MS = 1 << 12,
                    TimeFormat_H_M_S = 1 << 13,
                    MoneyFormat = 1 << 14,
                    MinusPrefix = 1 << 15,
                    Bit16 = 1 << 16,
                    Bit17 = 1 << 17,
                    Bit18 = 1 << 18,
                    Bit19 = 1 << 19,
                    Bit20 = 1 << 20,
                    Bit21 = 1 << 21,
                    Bit22 = 1 << 22,
                    Bit23 = 1 << 23,
                    Bit24 = 1 << 24,
                    Bit25 = 1 << 25,
                    Bit26 = 1 << 26,
                    Bit27 = 1 << 27,
                    Bit28 = 1 << 28,
                    Bit29 = 1 << 29,
                    Bit30 = 1 << 30,
                    Bit31 = 1 << 31
                }

                public enum FontValue : short
                {
                    Conduit18,
                    Agency16,
                    Fixedsys9,
                    Conduit14,
                    Conduit32,
                    Agency32,
                    Conduit23,
                    Agency18,
                    Conduit18_2,
                    Conduit16,
                    Agency23
                }
            }
        }
    }
}
