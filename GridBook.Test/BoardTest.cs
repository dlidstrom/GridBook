namespace GridBook.Test
{
	using System;
	using NUnit.Framework;
	using GridBook.Domain;

	[TestFixture]
	public class BoardTest
	{
		[Test]
		public void EmptiesAtStart()
		{
			Assert.AreEqual(60, Board.Start.Empties);
		}

		[Test]
		public void BoardString()
		{
			var board = Board.Start;
			Assert.AreEqual("--------" + "--------" + "--------" + "---O*---" + "---*O---" + "--------" + "--------" + "-------- *", board.ToString());
		}

		[Test]
		public void MakeMoveAtD3()
		{
			// Arrange
			var board = Board.Start;

			// Act
			var successor = board.Play(new Move("D3"));

			// Assert
			Assert.AreEqual("--------" + "--------" + "---*----" + "---**---" + "---*O---" + "--------" + "--------" + "-------- O", successor.ToString());
		}

		[Test]
		public void MakeMoveAtC4()
		{
			// Arrange
			var board = Board.Start;

			// Act
			var successor = board.Play(new Move("C4"));

			// Assert
			Assert.AreEqual("--------------------------***------*O--------------------------- O", successor.ToString());
		}

		[Test]
		public void MakeMoveAtF5()
		{
			// Arrange
			var board = Board.Start;

			// Act
			var successor = board.Play(new Move("F5"));

			// Assert
			Assert.AreEqual("---------------------------O*------***-------------------------- O", successor.ToString());
		}

		[Test]
		public void MakeMoveAtE6()
		{
			// Arrange
			var board = Board.Start;

			// Act
			var successor = board.Play(new Move("E6"));

			// Assert
			Assert.AreEqual("---------------------------O*------**-------*------------------- O", successor.ToString());
		}

		[Test]
		public void InvalidMoveThrows()
		{
			// Arrange
			var board = Board.Start;

			// Act
			try
			{
				board.Play(new Move("A1"));
				// Assert
				Assert.Fail("Expected invalid move to throw");
			}
			catch (ArgumentException)
			{
			}
		}

	}
}
