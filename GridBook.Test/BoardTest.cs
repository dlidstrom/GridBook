﻿namespace GridBook.Test
{
	using System;
	using NUnit.Framework;
	using GridBook.Domain;

	[TestFixture, Unit]
	public class BoardTest
	{
		[Test]
		public void EmptiesAtStart()
		{
			Assert.AreEqual(60, Board.Start.GetEmpties());
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

		[Test]
		public void FromStringFactoryMethod()
		{
			// Act
			var board = Board.FromString("---------------------------O*------*O--------------------------- *");

			// Assert
			Assert.AreEqual(Board.Start, board);
		}

		[Test]
		public void FromStringAfterC4()
		{
			// Arrange
			var afterC4 = Board.Start.Play(new Move("C4"));

			// Act
			var board = Board.FromString("--------------------------***------*O--------------------------- O");

			// Assert
			Assert.AreEqual(afterC4, board);
		}

		[Test]
		public void RoundTripString()
		{
			// Arrange
			var afterC4 = Board.Start.Play(new Move("C4"));

			// Act
			var stringRepresentation = afterC4.ToString();
			var roundTripped = Board.FromString(stringRepresentation);

			// Assert
			Assert.AreEqual(afterC4, roundTripped);
		}
	}
}
