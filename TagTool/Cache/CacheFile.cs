using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Cache
{
    public abstract class CacheFile
    {
        public FileInfo Info { get; }

        public CacheFile(FileInfo info)
        {
            Info = info;
        }

        public abstract void Load();

        public virtual FileStream Open(FileMode mode) =>
            Info.Open(mode);

        public virtual FileStream Open(FileMode mode, FileAccess access) =>
            Info.Open(mode, access);

        public virtual FileStream Open(FileMode mode, FileAccess access, FileShare share) =>
            Info.Open(mode, access, share);

        public virtual FileStream OpenRead() =>
            Info.OpenRead();

        public virtual FileStream OpenWrite() =>
            Info.OpenWrite();

        public virtual FileStream OpenReadWrite() =>
            Info.Open(FileMode.Open, FileAccess.ReadWrite);
    }
}
