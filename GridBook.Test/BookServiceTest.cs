namespace GridBook.Test
{
	using System.Linq;
	using GridBook.Domain;
	using GridBook.Service;
	using NHibernate;
	using NHibernate.Linq;
	using NUnit.Framework;

	[TestFixture, Unit]
	public class BookServiceTest : DatabaseTest
	{
		[Test]
		public void CanAddPosition()
		{
			// Arrange
			ISession session = CreateSession();
			var bs = new BookService(session);

			// Act
			var successor = Board.Start.Play(new Move("D3"));
			bs.Add(successor);
			session.Clear();

			// Assert
			var found = (from b in session.Query<Board>() /*where b == successor*/
						 select b).Single();
			Assert.AreEqual(successor, found);
		}
	}
}
