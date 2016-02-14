using System.Collections.Generic;
using CacheH1P = TagTool.Definitions.Halo1PC.CacheFile;

namespace TagTool.Definitions.Halo1AE
{
    public class CacheFile : CacheBase
    {
        public CacheFile(string Filename, string Build)
            : base(Filename, Build)
        {
            Version = DefinitionSet.Halo1AE;

            Header = new CacheH1P.CacheHeader(this);
            IndexHeader = new CacheH1P.CacheIndexHeader(this);
            IndexItems = new CacheH1P.IndexTable(this);
            Strings = new StringTable(this);

            LocaleTables = new List<LocaleTable>();
        }
    }
}
