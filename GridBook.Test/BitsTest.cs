namespace GridBook.Test
{
	using System;
	using NUnit.Framework;
	using GridBook.Domain;

	[TestFixture, Unit]
	public class BitsTest
	{
		[Test]
		public void BitCount()
		{
			Assert.AreEqual(0, Bits.Count(0));
			Assert.AreEqual(8, Bits.Count(255));
			Assert.AreEqual(32, Bits.Count(0xFFFFFFFF));
			unchecked
			{
				Assert.AreEqual(64, Bits.Count((long)(0xFFFFFFFFFFFFFFFF)));
			}
		}
	}
}
