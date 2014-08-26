using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itchy
{
    public class FrameInfo
    {
        public int stat;
        public int frames;
    }

    public static class UnitFrames
    {
        public static FrameInfo[] FasterBlockRate = new FrameInfo[]
        {
            new FrameInfo{stat=0, frames=17},
            //new FrameInfo{stat=4,frames=
        };
    }
}
