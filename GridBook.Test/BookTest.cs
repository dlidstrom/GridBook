namespace GridBook.Test
{
	using System;
	using NUnit.Framework;
	using GridBook.Domain;

	[TestFixture, Unit]
	public class BookTest
	{
		[Test]
		public void CanAddStartPosition()
		{
			// Arrange
			var start = Board.Start;

			// Act
			var book = new Book();
			book.Add(start);

			// Assert
			Assert.That(book.Contains(start));
			Assert.That(book.Contains(start.Play(new Move("D3")).MinimalReflection()));
			Assert.That(book.Contains(start.Play(new Move("C4")).MinimalReflection()));
			Assert.That(book.Contains(start.Play(new Move("F5")).MinimalReflection()));
			Assert.That(book.Contains(start.Play(new Move("E6")).MinimalReflection()));
		}
	}
}
