using System.Drawing;

namespace ConvexHull.Test
{
    [TestClass]
    public class ConvexHullTests
    {
        [TestMethod]
        public void PointsOrientation_CCW_ReturnsFalse()
        {
            var p1 = new PointF(0, 0);
            var p2 = new PointF(1, 0);
            var p3 = new PointF(0, 1);

            Assert.IsFalse(ConvexHull.OrientedClockwise(p1, p2, p3));
        }

        [TestMethod]
        public void PointsOrientation_CW_ReturnsTrue()
        {
            var p1 = new PointF(0, 0);
            var p2 = new PointF(0, 1);
            var p3 = new PointF(1, 0);

            Assert.IsTrue(ConvexHull.OrientedClockwise(p1, p2, p3));
        }
    }
}