using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Scripting
{
    public enum ScriptEventValue : short
    {
        Verbose = 0,
        Status = 1,
        Message = 2,
        Warning = 3,
        Error = 4,
        Critical = 5,
        None = 6
    }
}
