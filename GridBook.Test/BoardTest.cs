namespace GridBook.Test
{
	using System;
	using NUnit.Framework;

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
			Assert.AreEqual("--------"+"--------"+"--------"+"---O*---"+"---*O---"+"--------"+"--------"+"-------- *", board.ToString());
		}
	}
}
