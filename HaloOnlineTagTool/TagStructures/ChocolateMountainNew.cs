using System.Collections.Generic;
using HaloOnlineTagTool.Serialization;

namespace HaloOnlineTagTool.TagStructures
{
    [TagStructure(Name = "chocolate_mountain_new", Class = "chmt", Size = 0xC)]
    public class ChocolateMountainNew
    {
        public List<LightingVariable> LightingVariables;

        [TagStructure(Size = 0x4)]
        public class LightingVariable
        {
            public float LightmapBrightnessOffset;
        }
    }
}
