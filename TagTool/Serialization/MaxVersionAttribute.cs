using System;
using TagTool.Definitions;

namespace TagTool.Serialization
{
    /// <summary>
    /// Attribute indicating the last engine version in which a tag element is present.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class MaxVersionAttribute : Attribute
    {
        public MaxVersionAttribute(DefinitionSet version)
        {
            Version = version;
        }

        public DefinitionSet Version { get; set; }
    }
}
