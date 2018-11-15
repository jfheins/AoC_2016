using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Day_16.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestEnlargeSeed_1()
        {
            var seed = "1";
            var result = StringEnlarger.Enlarge(seed);
            Assert.AreEqual(result, "100");
        }
    }
}
