namespace GridBook.Test
{
	using System;
	using NUnit.Framework;
	using GridBook.Domain;

	[TestFixture]
	public class BitsTest
	{
		[Test]
		public void BitCount()
		{
			Assert.AreEqual(0, Bits.Count(0));
			Assert.AreEqual(8, Bits.Count(255));
			Assert.AreEqual(32, Bits.Count(0xFFFFFFFF));
			Assert.AreEqual(64, Bits.Count(0xFFFFFFFFFFFFFFFF));
		}
	}
}
