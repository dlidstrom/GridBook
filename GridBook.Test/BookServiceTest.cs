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
	}
}
