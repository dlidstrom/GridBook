namespace GridBook.Test
{
	using System;
	using NUnit.Framework;
	using GridBook.Domain;
	using System.Collections.Generic;

	[TestFixture]
	public class MinimalReflectionTest
	{
		[Test]
		public void ShouldHaveOneAfterStart()
		{
			// Arrange
			var b1 = Board.Start.Play(new Move("D3"));
			var b2 = Board.Start.Play(new Move("E6"));
			var b3 = Board.Start.Play(new Move("F5"));
			var b4 = Board.Start.Play(new Move("C4"));

			// Act
			var h = new HashSet<Board>() { b1.MinimalReflection(), b2.MinimalReflection(), b3.MinimalReflection(), b4.MinimalReflection() };

			// Assert
			Assert.AreEqual(1, h.Count);
		}

		[Test]
		public void ShouldBeOneAndSame()
		{
			// Arrange
			var b1 = Board.FromString("----------O------****-----***O-----**------O*------------------- O");
			var b2 = Board.FromString("-------------*------**O---O***----****------O------------------- O");
			var b3 = Board.FromString("-------------------*O------**-----O***-----****------O---------- O");
			var b4 = Board.FromString("-------------------O------****----***O---O**------*------------- O");
			var b5 = Board.FromString("-------------O-----****---O***-----**------*O------------------- O");
			var b6 = Board.FromString("--------------------O-----****----O***------**O------*---------- O");
			var b7 = Board.FromString("-------------------O*------**-----***O---****-----O------------- O");
			var b8 = Board.FromString("----------*------O**------***O----****-----O-------------------- O");

			// Act
			var h = new HashSet<Board>()
			{
				b1.MinimalReflection(),
				b2.MinimalReflection(),
				b3.MinimalReflection(),
				b4.MinimalReflection(),
				b5.MinimalReflection(),
				b6.MinimalReflection(),
				b7.MinimalReflection(),
				b8.MinimalReflection()
			};

			// Assert
			Assert.AreEqual(1, h.Count);
		}
	}
}
