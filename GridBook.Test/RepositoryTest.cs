namespace GridBook.Test
{
	using System;
	using GridBook.Domain;
	using NHibernate;
	using NHibernateLayer;
	using NUnit.Framework;

	[TestFixture, Database]
	public class RepositoryTest : DatabaseTest
	{
		[Test]
		public void CanAddBoards()
		{
			// Arrange
			ISession session = CurrentSession();

			// Act
			var repo = new Repository<Board>(session);
			var id = repo.Add(Board.Start);

			session.Clear();

			// Assert
			Assert.AreNotEqual(Guid.Empty, id);
			var board = repo.FindBy(id);
			Assert.AreEqual(Board.Start, board);
		}
	}
}
