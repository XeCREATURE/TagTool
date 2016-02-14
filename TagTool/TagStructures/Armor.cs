using TagTool.Definitions;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.TagStructures
{
    [TagStructure(Name = "armor", Class = "armr", Size = 0x28, MinVersion = DefinitionSet.HaloOnline430475)]
    public class Armor : GameObject
    {
        public TagInstance ParentModel;
        public TagInstance FirstPersonModel;
        public TagInstance ThirdPersonModel;
        public uint Unused1;
    }
}
