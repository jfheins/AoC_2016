using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Day_16.Test
{
	[TestClass]
	public class UnitTest1
	{
		private void TestSeedResult(string seed, string expectedResult)
		{
			var result = StringEnlarger.Enlarge(seed);
			Assert.AreEqual(result, expectedResult);
		}

		[TestMethod]
		public void TestEnlarger_1()
		{
			TestSeedResult("1", "100");
			TestSeedResult("0", "001");
		}

		[TestMethod]
		public void TestEnlarger_2()
		{
			TestSeedResult("11111", "11111000000");
		}

		[TestMethod]
		public void TestEnlarger_3()
		{
			TestSeedResult("111100001010", "1111000010100101011110000");
		}

		[TestMethod]
		public void TestEnlargerLength_1()
		{
			var result = StringEnlarger.EnlargeToLength("1", 8);
			Assert.AreEqual(result, "10001100");
		}

		[TestMethod]
		public void TestEnlargerLength_2()
		{
			var result = StringEnlarger.EnlargeToLength("111100001010", 13);
			Assert.AreEqual(result, "1111000010100");
		}

		[TestMethod]
		public void TestChecksum()
		{
			var diskContent = StringEnlarger.EnlargeToLength("10011111011011001", 272);
			var checksum = StringEnlarger.CalculateChecksum(diskContent);
			Assert.AreEqual(checksum, "10111110010110110");
		}

		[TestMethod]
		public void TestChecksum2()
		{
			var diskContent = StringEnlarger.EnlargeToLength("10011111011011001", 272);
			var checksum = StringEnlarger.CalculateChecksum(diskContent, 272);
			Assert.AreEqual(checksum, "10111110010110110");
		}
	}
}