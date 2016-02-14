using System;

namespace HaloOnlineTagTool.Serialization
{
    /// <summary>
    /// Attribute indicating the first engine version in which a tag element is present.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class MinVersionAttribute : Attribute
    {
        public MinVersionAttribute(EngineVersion version)
        {
            Version = version;
        }

        public EngineVersion Version { get; set; }
    }
}
