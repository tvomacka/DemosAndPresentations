using System.Drawing;

namespace ConvexHull
{
    public class ConvexHull
    {
        public static bool OrientedClockwise(PointF p1, PointF p2, PointF p3)
        {
            return p1.X * p2.Y + p3.X * p1.Y + p2.X * p3.Y - p3.X * p2.Y - p2.X * p1.Y - p1.X * p3.Y < 0;
        }
    }
}
