using System;

namespace HaloOnlineTagTool.Serialization
{
    /// <summary>
    /// Attribute indicating the last engine version in which a tag element is present.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class MaxVersionAttribute : Attribute
    {
        public MaxVersionAttribute(EngineVersion version)
        {
            Version = version;
        }

        public EngineVersion Version { get; set; }
    }
}
