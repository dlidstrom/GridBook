namespace GridBook.Test
{
	using System;
	using GridBook.Domain;
	using NHibernate;
	using NHibernateLayer;
	using NUnit.Framework;

	[TestFixture]
	public class RepositoryTest : NHibernateFixture
	{
		[Test]
		public void CanAddBoards()
		{
			// Arrange
			Guid id = default(Guid);
			using (var session = SessionFactory.OpenSession())
			{
				// Act
				var repo = new Repository<Board>(session);
				id = repo.Add(Board.Start);
			}

			// Assert
			Assert.AreNotEqual(Guid.Empty, id);
			using (var session = SessionFactory.OpenSession())
			{
				var repo = new Repository<Board>(session);
				var board = repo.FindBy(id);
				Assert.AreEqual(Board.Start, board);
			}
		}
	}
}
