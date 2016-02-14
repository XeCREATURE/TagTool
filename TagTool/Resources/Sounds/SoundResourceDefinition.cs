using TagTool.Serialization;

namespace TagTool.Resources.Sounds
{
    /// <summary>
    /// Resource definition data for sounds.
    /// </summary>
    [TagStructure(Size = 0x14)]
    public class SoundResourceDefinition
    {
        /// <summary>
        /// Gets or sets the reference to the sound data.
        /// </summary>
        public ResourceDataReference Data;
    }
}
