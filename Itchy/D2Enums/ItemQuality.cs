using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itchy.D2Enums
{
    public enum ItemQuality : uint
    {
        Any = 0,    // custom value
        Inferior = 1,
        Normal = 2,
        Superior = 3,
        Magic = 4,
        Set = 5,
        Rare = 6,
        Unique = 7,
        Craft = 8,
    }
}
