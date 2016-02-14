using System;
using TagTool.Definitions;

namespace TagTool.Serialization
{
    /// <summary>
    /// Attribute for a serializable structure in a tag.
    /// If a structure has more than one of these attributes, then all attributes with version restrictions will be checked first.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class TagStructureAttribute : Attribute
    {
        public TagStructureAttribute()
        {
            MinVersion = DefinitionSet.Unknown;
            MaxVersion = DefinitionSet.Unknown;
        }

        /// <summary>
        /// Gets or sets the internal name of the structure.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the tag class that the structure applies to.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets the size of the structure in bytes, NOT including parent structures.
        /// </summary>
        public uint Size { get; set; }

        /// <summary>
        /// Gets or sets the minimum engine version which the structure applies to.
        /// Can be <see cref="DefinitionSet.Unknown"/> (default) if unbounded.
        /// </summary>
        public DefinitionSet MinVersion { get; set; }

        /// <summary>
        /// Gets or sets the maximum engine version which the structure applies to.
        /// Can be <see cref="DefinitionSet.Unknown"/> (default) if unbounded.
        /// </summary>
        public DefinitionSet MaxVersion { get; set; }

        /// <summary>
        /// Gets or sets the power of two to align the block to.
        /// Can be 0 if not set.
        /// </summary>
        /// <remarks>
        /// Note that this value is only a guide for the serializer, and a
        /// different alignment may actually be used if necessary.
        /// </remarks>
        public uint Align { get; set; }
    }
}
