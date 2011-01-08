namespace GridBook.Test
{
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
			repo.Add(Board.Start);

			session.Clear();

			// Assert
			var repo2 = new Repository<Board>(session);
			var board = repo2.FindBy(1);
			Assert.AreEqual(Board.Start, board);
		}
	}
}
