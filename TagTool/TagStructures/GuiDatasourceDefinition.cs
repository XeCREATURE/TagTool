using System.Collections.Generic;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.TagStructures
{
    [TagStructure(Name = "gui_datasource_definition", Class = "dsrc", Size = 0x20)]
    public class GuiDatasourceDefinition
    {
        public StringID Name;
        public uint Unknown;
        public uint Unknown2;
        public uint Unknown3;
        public List<Datum> Data;
        public uint Unknown4;

        [TagStructure(Size = 0x28)]
        public class Datum
        {
            public List<IntegerValue> IntegerValues;
            public List<StringValue> StringValues;
            public List<StringidValue> StringidValues;
            public StringID Unknown;

            [TagStructure(Size = 0x8)]
            public class IntegerValue
            {
                public StringID DataType;
                public int Value;
            }

            [TagStructure(Size = 0x24)]
            public class StringValue
            {
                public StringID DataType;
                [TagField(Length = 20)] public string Value;
            }

            [TagStructure(Size = 0x8)]
            public class StringidValue
            {
                public StringID DataType;
                public StringID Value;
            }
        }
    }
}
