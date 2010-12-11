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
		}
	}
}
