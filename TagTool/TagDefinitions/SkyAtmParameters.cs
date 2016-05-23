using System.Collections.Generic;
using TagTool.Common;
using TagTool.Definitions;
using TagTool.Serialization;
using TagTool.TagGroups;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "sky_atm_parameters", Class = "skya", Size = 0x4C, MaxVersion = DefinitionSet.HaloOnline449175)]
    [TagStructure(Name = "sky_atm_parameters", Class = "skya", Size = 0x54, MinVersion = DefinitionSet.HaloOnline498295)]
    public class SkyAtmParameters
    {
        public int Unknown;
        public TagInstance FogBitmap;
        public float Unknown2;
        public float Unknown3;
        public float Unknown4;
        public float Unknown5;
        public float Unknown6;
        public float Unknown7;
        public float Unknown8;
        public int Unknown9;
        [MinVersion(DefinitionSet.HaloOnline498295)] public float Unknown10;
        [MinVersion(DefinitionSet.HaloOnline498295)] public int Unknown11;
        public List<AtmosphereProperty> AtmosphereProperties;
        public List<UnderwaterBlock> Underwater;

        [TagStructure(Size = 0xA4)]
        public class AtmosphereProperty
        {
            public short Unknown;
            public short Unknown2;
            public StringID Name;
            public float LightSourceY;
            public float LightSourceX;
            public float FogColorR;
            public float FogColorG;
            public float FogColorB;
            public float Brightness;
            public float FogGradientThreshold;
            public float LightIntensity;
            public float SkyInvisiblilityThroughFog;
            public float Unknown3;
            public float Unknown4;
            public float LightSourceSpread;
            public uint Unknown5;
            public float FogIntensity;
            public float Unknown6;
            public float TintCyan;
            public float TintMagenta;
            public float TintYellow;
            public float FogIntensityCyan;
            public float FogIntensityMagenta;
            public float FogIntensityYellow;
            public float BackgroundColorRed;
            public float BackgroundColorGreen;
            public float BackgroundColorBlue;
            public float TintRed;
            public float Tint2Green;
            public float Tint2Blue;
            public float FogIntensity2;
            public float StartDistance;
            public float EndDistance;
            public float FogVelocityX;
            public float FogVelocityY;
            public float FogVelocityZ;
            public TagInstance WeatherEffect;
            public uint Unknown7;
            public uint Unknown8;
        }

        [TagStructure(Size = 0x14)]
        public class UnderwaterBlock
        {
            public StringID Name;
            public float ColorA;
            public float ColorR;
            public float ColorG;
            public float ColorB;
        }
    }
}
