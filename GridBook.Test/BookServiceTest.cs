namespace GridBook.Test
{
	using System.Linq;
	using GridBook.Domain;
	using GridBook.Service;
	using NHibernate;
	using NHibernate.Linq;
	using NUnit.Framework;
	using System;

	[TestFixture]
	public class BookServiceTest : NHibernateFixture
	{
		[Test]
		public void CanQuery()
		{
			// Arrange
			Board found = null;
			using (var session = SessionFactory.OpenSession())
			{
				// Act
				found = new BookService(session).Find(Board.Start);
			}

			// Assert
			Assert.IsNull(found);
		}

		[Test]
		public void CanAddPosition()
		{
			// Arrange
			using (var session = SessionFactory.OpenSession())
			{
				var bs = new BookService(session);

				// Act
				bs.Add(Board.Start);
			}

			// Assert
			using (var session = SessionFactory.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				var found = (from b in session.Query<Board>()
							 select b).ToList();
				Assert.AreEqual(2, found.Count());
				tx.Commit();
			}
		}

		[Test]
		public void CanHaveSuccessor()
		{
			// Arrange
			var pos = Board.Start;
			var successor = pos.Play(new Move("D3"));

			// Act
			pos.AddSuccessor(successor);
			successor.AddParent(pos);

			// Assert
			Assert.That(pos.Successors.Contains(successor));
			Assert.That(successor.Parents.Contains(pos));

			using (var session = SessionFactory.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				session.Save(pos);
				tx.Commit();
			}

			using (var session = SessionFactory.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				var found = session.Get<Board>(pos.Id);
				Assert.IsNotNull(found);
				Assert.AreEqual(1, found.Successors.Count());
				Assert.That(found.Successors.First().Parents.Contains(found));
				tx.Commit();
			}
		}

		[Test]
		public void CanAddStartPosition()
		{
			// Arrange
			var start = Board.Start;

			// Act
			using (var session = SessionFactory.OpenSession())
			{
				var book = new BookService(session);
				book.Add(start);
			}

			// Assert
			using (var session = SessionFactory.OpenSession())
			{
				var book = new BookService(session);
				Assert.That(book.Contains(start));
				Assert.That(book.Contains(start.Play(new Move("D3")).MinimalReflection()));
				Assert.That(book.Contains(start.Play(new Move("C4")).MinimalReflection()));
				Assert.That(book.Contains(start.Play(new Move("F5")).MinimalReflection()));
				Assert.That(book.Contains(start.Play(new Move("E6")).MinimalReflection()));
			}
		}

		[Test]
		public void BookCanFindPositions()
		{
			// Arrange
			using (var session = SessionFactory.OpenSession())
			{
				var book = new BookService(session);
				book.Add(Board.Start);
			}

			using (var session = SessionFactory.OpenSession())
			{
				// Act
				var book = new BookService(session);
				var found = book.Find(Board.Start);

				// Assert
				Assert.IsNotNull(found);
				Assert.AreEqual(Board.Start, found);
			}
		}

		//[Test]
		//public void AddingGameShouldAddAllParentsAndChildren()
		//{
		//    // Arrange
		//    var game = "D3C3C4";

		//    // Act
		//    using (var session = SessionFactory.OpenSession())
		//    {
		//        var book = new BookService(session);
		//        var positions = new List<Board>();
		//        var currentPosition = new Board();
		//        for (int i = 0; i < game.Length - 1; i += 2)
		//        {
		//            book.Add(currentPosition);
		//            positions.Add(currentPosition);
		//            var move = new Move(game.Substring(i, 2));
		//            currentPosition = currentPosition.Play(move);
		//        }
		//    }

		//    // Assert
		//    foreach (var position in positions)
		//    {
		//        Assert.That(book.Contains(position));
		//        var storedPosition = book.Find(position);
		//    }
		//}

		//[Test]
		//public void BookStoresMinimalReflection()
		//{
		//    // Arrange
		//    var book = new BookService(CurrentSession());
		//    var pos = Board.Start.Play(new Move("D3"));

		//    // Act
		//    book.Add(pos);
		//    pos = pos.Play(new Move("C3"));
		//    book.Add(pos);

		//    // Assert
		//    // book should now contain all successors of pos
		//    foreach (var successor in pos.Successors)
		//    {
		//        Assert.That(book.Contains(successor));
		//    }
		//}
	}
}
