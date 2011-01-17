namespace GridBook.Test
{
	using System.Linq;
	using GridBook.Domain;
	using NHibernateLayer;
	using NUnit.Framework;
	using NHibernate;

	[TestFixture, Database]
	public class RepositoryTest : DatabaseTest
	{
		[Test]
		public void CanAddBoards()
		{
			// Arrange
			ISession session = CreateSession();

			// Act
			var repo = new Repository<Board>(session);
			var id = repo.Add(Board.Start);

			session.Clear();

			// Assert
			var repo2 = new Repository<Board>(session);
			var board = repo2.FindBy(id);
			Assert.AreEqual(Board.Start, board);
		}
	}
}
