using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Scripting
{
    public enum ScriptModelStateValue : short
    {
        Standard = 0,
        MinorDamage = 1,
        MediumDamage = 2,
        MajorDamage = 3,
        Destroyed = 4
    }
}
