using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Scripting
{
    public enum ScriptValueType : short
    {
        Unparsed = 0x00,
        SpecialForm = 0x01,
        FunctionName = 0x02,
        Passthrough = 0x03,
        Void = 0x04,
        Boolean = 0x05,
        Real = 0x06,
        Short = 0x07,
        Long = 0x08,
        String = 0x09,
        Script = 0x0A,
        StringID = 0x0B,
        UnitSeatMapping = 0x0C,
        TriggerVolume = 0x0D,
        CutsceneFlag = 0x0E,
        CutsceneCameraPoint = 0x0F,
        CutsceneTitle = 0x10,
        CutsceneRecording = 0x11,
        DeviceGroup = 0x12,
        AI = 0x13,
        AICommandList = 0x14,
        AICommandScript = 0x15,
        AIBehavior = 0x16,
        AIOrders = 0x17,
        AILine = 0x18,
        StartingProfile = 0x19,
        Conversation = 0x1A,
        ZoneSet = 0x1B,
        DesignerZone = 0x1C,
        PointReference = 0x1D,
        Style = 0x1E,
        ObjectList = 0x1F,
        Folder = 0x20,
        Sound = 0x21,
        Effect = 0x22,
        Damage = 0x23,
        LoopingSound = 0x24,
        AnimationGraph = 0x25,
        DamageEffect = 0x26,
        ObjectDefinition = 0x27,
        Bitmap = 0x28,
        Shader = 0x29,
        RenderModel = 0x2A,
        StructureDefinition = 0x2B,
        LightmapDefinition = 0x2C,
        CinematicDefinition = 0x2D,
        CinematicSceneDefinition = 0x2E,
        BinkDefinition = 0x2F,
        AnyTag = 0x30,
        AnyTagNotResolving = 0x31,
        GameDifficulty = 0x32, // enum ScriptGameDifficultyValue
        Team = 0x33, // enum ScriptTeamValue
        MultiplayerTeam = 0x34, // enum ScriptMultiplayerTeamValue
        Controller = 0x35, // enum ScriptControllerValue
        ButtonPreset = 0x36, // enum ScriptButtonPresetValue
        JoystickPreset = 0x37, // enum ScriptButtonPresetValue
        PlayerCharacterType = 0x38, // enum ScriptPlayerCharacterTypeValue
        VoiceOutputSetting = 0x39, // enum ScriptVoiceOutputSettingValue
        VoiceMask = 0x3A, // enum ScriptVoiceMaskValue
        SubtitleSetting = 0x3B, // enum ScriptSubtitleSettingValue
        ActorType = 0x3C, // enum ScriptActorTypeValue
        ModelState = 0x3D, // enum ScriptModelStateValue
        Event = 0x3E, // enum ScriptEventValue
        CharacterPhysics = 0x3F, // enum ScriptCharacterPhysicsValue
        PrimarySkull = 0x40, // enum ScriptPrimarySkullValue
        SecondarySkull = 0x41, // enum ScriptSecondarySkullValue
        Object = 0x42,
        Unit = 0x43,
        Vehicle = 0x44,
        Weapon = 0x45,
        Device = 0x46,
        Scenery = 0x47,
        EffectScenery = 0x48,
        ObjectName = 0x49,
        UnitName = 0x4A,
        VehicleName = 0x4B,
        WeaponName = 0x4C,
        DeviceName = 0x4D,
        SceneryName = 0x4E,
        EffectSceneryName = 0x4F,
        CinematicLightprobe = 0x50,
        AnimationBudgetReference = 0x51,
        LoopingSoundBudgetReference = 0x52,
        SoundBudgetReference = 0x53
    }
}
