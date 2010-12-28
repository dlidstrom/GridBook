namespace GridBook.Test
{
	using GridBook.Domain;
	using NHibernateLayer;
	using NUnit.Framework;

	[TestFixture, Database]
	public class RepositoryTest
	{
		[Test]
		public void CanAddBoards()
		{
			// Arrange
			NHibernateHelper helper = new NHibernateHelper("DbTest", true);
			using (var uow = new UnitOfWork(helper.SessionFactory))
			{
				// Act
				var repo = new Repository<Board>(uow.Session);
				repo.Add(Board.Start);
				uow.Commit();
			}

			// Assert
			using (var uow = new UnitOfWork(helper.SessionFactory))
			{
				var repo2 = new Repository<Board>(uow.Session);
				var board = repo2.FindBy(1);
				Assert.AreEqual(Board.Start, board);
			}
		}
	}
}
