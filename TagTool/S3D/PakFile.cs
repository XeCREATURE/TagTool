﻿using System.Collections.Generic;
using System.IO;
using TagTool.IO;

namespace TagTool.S3D
{
    public class PakFile
    {
        #region Declarations
        public string Filename;
        public string FilePath
        {
            get { return Directory.GetParent(Filename).FullName; }
        }
        public EndianReader Reader;
        public PakTable PakItems;

        public SceneData SceneData;
        public SceneCDT SceneCDT;
        #endregion

        public PakFile(string Filename)
        {
            this.Filename = Filename;

            FileStream fs = new FileStream(Filename, FileMode.Open, FileAccess.Read);
            Reader = new EndianReader((Stream)fs, EndianFormat.Little);

            PakItems = new PakTable(this);

            foreach (var item in PakItems)
            {
                if (item.Class == TagType.SceneData) SceneData = new SceneData(this, item);
                if (item.Class == TagType.SceneCDT) SceneCDT = new SceneCDT(this, item);
            }
        }

        #region Classes
        public class PakTag
        {
            public int Offset;
            public int Size;
            public string Name;
            public TagType Class;
            public int unk1, unk2;

            public override string ToString()
            {
                return Name;
            }
        }

        public class PakTable : List<PakTag>
        {
            public PakTable(PakFile Pak)
            {
                var reader = Pak.Reader;

                reader.SeekTo(0);
                var fCount = reader.ReadInt32();

                for (int i = 0; i < fCount; i++)
                {
                    var item = new PakTag();

                    item.Offset = reader.ReadInt32();
                    item.Size = reader.ReadInt32();
                    var len = reader.ReadInt32();
                    item.Name = reader.ReadString(len);
                    item.Class = (TagType)reader.ReadInt32();
                    item.unk1 = reader.ReadInt32();
                    item.unk2 = reader.ReadInt32();

                    this.Add(item);
                }
            }
        }
        #endregion

        #region Methods
        public PakTag GetItemByName(string Name)
        {
            foreach (var item in PakItems)
                if (item.Name == Name) return item;

            return null;
        }

        public void Close()
        {
            Reader.Close();
            Reader.Dispose();
            PakItems.Clear();
        }
        #endregion
    }
}
