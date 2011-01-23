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

		[Test]
		public void A1FromPos()
		{
			// Arrange
			var move = Move.FromPos(63);

			// Assert
			Assert.AreEqual(63, move.Pos);
		}

		[Test]
		public void A8FromPos()
		{
			// Arrange
			var move = Move.FromPos(7);

			// Assert
			Assert.AreEqual(7, move.Pos);
		}

		[Test]
		public void H1FromPos()
		{
			// Arrange
			var move = Move.FromPos(56);

			// Assert
			Assert.AreEqual(56, move.Pos);
		}

		[Test]
		public void H8FromPos()
		{
			// Arrange
			var move = Move.FromPos(0);

			// Assert
			Assert.AreEqual(0, move.Pos);
		}

		[Test]
		public void OutsideBelowFails()
		{
			try
			{
				// Act
				var move = Move.FromPos(-1);

				// Assert
				Assert.Fail("Should not accept pos < 0");
			}
			catch (ArgumentException)
			{
			}
		}

		[Test]
		public void OutsideAboveFails()
		{
			try
			{
				// Act
				var move = Move.FromPos(64);

				// Assert
				Assert.Fail("Should not accept pos < 0");
			}
			catch (ArgumentException)
			{
			}
		}

		[Test]
		public void A1ToString()
		{
			// Arrange
			var move = new Move("A1");

			// Assert
			Assert.AreEqual("A1", move.ToString());
		}

		[Test]
		public void A8ToString()
		{
			// Arrange
			var move = new Move("A8");

			// Assert
			Assert.AreEqual("A8", move.ToString());
		}

		[Test]
		public void H1ToString()
		{
			// Arrange
			var move = new Move("H1");

			// Assert
			Assert.AreEqual("H1", move.ToString());
		}

		[Test]
		public void H8ToString()
		{
			// Arrange
			var move = new Move("H8");

			// Assert
			Assert.AreEqual("H8", move.ToString());
		}
	}
}
