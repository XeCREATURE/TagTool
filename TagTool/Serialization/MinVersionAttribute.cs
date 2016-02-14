using System;
using TagTool.Definitions;

namespace TagTool.Serialization
{
    /// <summary>
    /// Attribute indicating the first engine version in which a tag element is present.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class MinVersionAttribute : Attribute
    {
        public MinVersionAttribute(DefinitionSet version)
        {
            Version = version;
        }

        public DefinitionSet Version { get; set; }
    }
}
