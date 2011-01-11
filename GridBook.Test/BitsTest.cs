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

		[Test]
		public void FlipVertical()
		{
			// Arrange
			ulong x = 0xFF00000000000000;

			// Act
			x = x.FlipVertical();

			// Assert
			Assert.AreEqual(0x00000000000000FF, x);
		}

		[Test]
		public void FlipHorizontal()
		{
			// Arrange
			ulong x = 0x1010101010101010;

			// Act
			x = x.FlipHorizontal();

			// Assert
			Assert.AreEqual(0x0808080808080808, x);
		}

		[Test]
		public void FlipDiagonalA8H1()
		{
			// Arrange
			ulong x = 0x8080808080808080;

			// Act
			x = x.FlipDiagonal();

			// Assert
			Assert.AreEqual(0x00000000000000FF, x);
		}
	}
}
