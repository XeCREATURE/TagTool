﻿using System;

namespace TagTool.Definitions.Halo2Vista
{
    public class CacheFile : Halo2Xbox.CacheFile
    {
        public CacheFile(string Filename, string Build)
            : base(Filename, Build)
        {
            Version = DefinitionSet.Halo2Vista;
        }

        public override byte[] GetRawFromID(int ID, int DataLength)
        {
            throw new NotSupportedException("no raw for h2v");
        }
    }
}
