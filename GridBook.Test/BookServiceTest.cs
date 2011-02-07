namespace GridBook.Test
{
	using System;
	using System.Linq;
	using GridBook.Domain;
	using GridBook.Service;
	using NHibernate.Linq;
	using NUnit.Framework;

	[TestFixture]
	public class BookServiceTest : NHibernateFixture
	{
		[Test]
		public void CanSaveSinglePosition()
		{
			// Arrange
			var pos = Board.Start.Play(Move.D3);

			using (var session = SessionFactory.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				session.Save(pos);
				tx.Commit();
			}

			// Act
			Board found = null;
			using (var session = SessionFactory.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				var q = from b in session.Query<Board>()
						where b.Empty == pos.Empty && b.Mover == pos.Mover
						select b;
				found = q.Single();
				tx.Commit();
			}

			// Assert
			Assert.AreEqual(pos, found);
		}

		[Test]
		public void CanSaveSuccessorWithParent()
		{
			// Arrange
			var start = Board.Start;
			using (var session = SessionFactory.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				var pos = start;
				var successor = pos.Play(Move.D3);
				pos.AddSuccessor(successor);
				successor.AddParent(pos);
				session.Save(pos);
				tx.Commit();
			}

			// Assert
			using (var session = SessionFactory.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				var q = from b in session.Query<Board>()
						where b.Empty == start.Empty && b.Mover == start.Mover
						select b;
				var found = q.Single();
				Assert.AreEqual(1, found.Successors.Count());
				var successor = found.Successors.First();
				Assert.AreEqual(1, successor.Parents.Count());
				tx.Commit();
			}
		}

		[Test]
		public void CanSaveTwoSuccessors()
		{
			// Arrange
			var start = Board.Start;
			var successors = new Board[] { Board.FromString("---------------------------O*------*O-------*O------------------ *"),
				Board.FromString("---------------------------O*-----OOO-----*O-------------------- *")
			};
			using (var session = SessionFactory.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				var pos = start;
				foreach (var successor in successors)
				{
					pos.AddSuccessor(successor);
					successor.AddParent(pos);
				}

				session.Save(pos);
				tx.Commit();
			}

			// Assert
			using (var session = SessionFactory.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				var q = from b in session.Query<Board>()
						where b.Empty == start.Empty && b.Mover == start.Mover
						select b;
				var found = q.Single();
				Assert.AreEqual(successors.Count(), found.Successors.Count());
				var successor = found.Successors.First();
				Assert.AreEqual(1, successor.Parents.Count());
				tx.Commit();
			}
		}

		[Test]
		public void AddSuccessorToStoredParent()
		{
			// Arrange
			Guid id = Guid.Empty;
			using (var session = SessionFactory.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				id = (Guid)session.Save(Board.Start);
				tx.Commit();
			}

			// Act
			using (var session = SessionFactory.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				var successor = Board.Start.Play(Move.D3);
				var pos = session.Get<Board>(id);
				pos.AddSuccessor(successor);
				successor.AddParent(pos);
				session.Update(pos);
				tx.Commit();
			}

			// Assert
			using (var session = SessionFactory.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				var found = session.Get<Board>(id);
				var successors = found.Successors;
				Assert.AreEqual(successors.Count(), found.Successors.Count());
				var successor = found.Successors.First();
				Assert.AreEqual(1, successor.Parents.Count());
				tx.Commit();
			}
		}

		[Test]
		public void VerifyHierarchy()
		{
			// Arrange (add successor, without parent)
			var start = Board.Start;
			var successor = start.Play(Move.D3);
			using (var session = SessionFactory.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				session.Save(successor);
				tx.Commit();
			}

			// Act
			Guid id = Guid.Empty;
			using (var session = SessionFactory.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				// grab successor from store
				var q = from b in session.Query<Board>()
						where b.Empty == successor.Empty && b.Mover == successor.Mover
						select b;
				Assert.AreEqual(1, q.Count());
				var persistentSuccessor = q.Single();
				start.AddSuccessor(persistentSuccessor);
				persistentSuccessor.AddParent(start);
				id = (Guid)session.Save(start);
				tx.Commit();
			}

			using (var session = SessionFactory.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				var found = session.Get<Board>(id);
				Assert.AreEqual(Board.Start, found);
				Assert.AreEqual(1, found.Successors.Count());
				Assert.AreEqual(successor, found.Successors.First());
				Assert.AreEqual(1, found.Successors.First().Parents.Count());
				tx.Commit();
			}
		}

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
			{
				var found = (from b in session.Query<Board>()
							 select b).ToList();
				Assert.AreEqual(2, found.Count());
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
			{
				var found = session.Get<Board>(pos.Id);
				Assert.IsNotNull(found);
				Assert.AreEqual(1, found.Successors.Count());
				Assert.That(found.Successors.First().Parents.Contains(found));
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
				Assert.AreEqual(Board.Start.MinimalReflection(), found);
			}
		}
	}
}
