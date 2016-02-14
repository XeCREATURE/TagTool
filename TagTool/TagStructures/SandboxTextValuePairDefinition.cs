using System.Collections.Generic;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.TagStructures
{
    [TagStructure(Name = "sandbox_text_value_pair_definition", Class = "jmrq", Size = 0xC)]
    public class SandboxTextValuePairDefinition
    {
        public List<SandboxTextValuePair> SandboxTextValuePairs;

        [TagStructure(Size = 0x10)]
        public class SandboxTextValuePair
        {
            public StringID ParameterName;
            public List<TextValuePari> TextValueParis;

            [TagStructure(Size = 0x14)]
            public class TextValuePari
            {
                public byte Flags;
                public ExpectedValueTypeValue ExpectedValueType;
                public short Unknown;
                public int IntValue;
                public StringID RefName;
                public StringID Name;
                public StringID Description;

                public enum ExpectedValueTypeValue : sbyte
                {
                    IntegerIndex,
                    StringidReference,
                    Incremental,
                }
            }
        }
    }
}
