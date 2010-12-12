namespace GridBook.Test
{
	using System;
	using NUnit.Framework;
	using GridBook.Domain;

	[TestFixture]
	public class MoveTest
	{
		[Test]
		public void A1ToPos()
		{
			// Arrange
			var move = new Move("A1");

			// Act
			int pos = move.Pos;

			// Assert
			Assert.AreEqual(63, pos);
		}

		[Test]
		public void A9ToPos()
		{
			// Arrange
			var move = new Move("A8");

			// Act
			int pos = move.Pos;

			// Assert
			Assert.AreEqual(7, pos);
		}

		[Test]
		public void H1ToPos()
		{
			// Arrange
			var move = new Move("H1");

			// Act
			int pos = move.Pos;

			// Assert
			Assert.AreEqual(56, pos);
		}

		[Test]
		public void H8ToPos()
		{
			// Arrange
			var move = new Move("H8");

			// Act
			int pos = move.Pos;

			// Assert
			Assert.AreEqual(0, pos);
		}
	}
}
