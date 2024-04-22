namespace Triangulation.Test
{
    [TestClass]
    public class SideTests
    {
        [TestMethod]
        public void Side_HasCorrectLength()
        {
            var s = new Side(0, 0, 3, 4);
            const double expectedLength = 5;
            Assert.AreEqual(expectedLength, s.Length, 1e-3);
        }
    }
}