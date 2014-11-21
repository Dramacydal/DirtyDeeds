using System.Drawing;

namespace DD.Extensions
{
    public static class PointExtensions
    {
        public static int X(this Size sz)
        {
            return sz.Width;
        }

        public static int Y(this Size sz)
        {
            return sz.Height;
        }

        public static Point Clone(this Point p)
        {
            return new Point(p.X, p.Y);
        }
    }
}
