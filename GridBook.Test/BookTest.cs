﻿namespace GridBook.Test
{
	using System;
	using NUnit.Framework;
	using GridBook.Domain;
	using System.Collections.Generic;
	using GridBook.Service;

	[TestFixture, Database]
	public class BookTest : DatabaseTest
	{
		[Test]
		public void CanAddStartPosition()
		{
			// Arrange
			var start = Board.Start;

			// Act
			var book = new BookService(CurrentSession());
			book.Add(start);

			// Assert
			Assert.That(book.Contains(start));
			Assert.That(book.Contains(start.Play(new Move("D3")).MinimalReflection()));
			Assert.That(book.Contains(start.Play(new Move("C4")).MinimalReflection()));
			Assert.That(book.Contains(start.Play(new Move("F5")).MinimalReflection()));
			Assert.That(book.Contains(start.Play(new Move("E6")).MinimalReflection()));
		}

		[Test]
		public void BookCanFindPositions()
		{
			// Arrange
			var book = new BookService(CurrentSession());
			book.Add(Board.Start);

			// Act
			var found = book.Find(Board.Start);

			// Assert
			Assert.IsNotNull(found);
			Assert.AreEqual(Board.Start, found);
		}

		[Test]
		public void AddingGameShouldAddAllParentsAndChildren()
		{
			// Arrange
			var game = "D3C3C4";

			// Act
			var book = new BookService(CurrentSession());
			var positions = new List<Board>();
			var currentPosition = new Board();
			for (int i = 0; i < game.Length - 1; i += 2)
			{
				book.Add(currentPosition);
				positions.Add(currentPosition);
				var move = new Move(game.Substring(i, 2));
				currentPosition = currentPosition.Play(move);
			}

			// Assert
			foreach (var position in positions)
			{
				Assert.That(book.Contains(position));
				var storedPosition = book.Find(position);
			}
		}

		[Test]
		public void BookStoresMinimalReflection()
		{
			// Arrange
			var book = new BookService(CurrentSession());
			var pos = Board.Start.Play(new Move("D3"));

			// Act
			book.Add(pos);
			pos = pos.Play(new Move("C3"));
			book.Add(pos);

			// Assert
			// book should now contain all successors of pos
			foreach (var successor in pos.Successors)
			{
				Assert.That(book.Contains(successor));
			}
		}
	}
}
