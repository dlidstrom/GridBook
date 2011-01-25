namespace GridBook.Test
{
	using System.Collections.Generic;
	using FluentNHibernate.Testing;
	using GridBook.Domain;
	using NUnit.Framework;

	[TestFixture]
	public class MappingsTest : NHibernateFixture
	{
		[Test]
		public void VerifyBoardMappings()
		{
			using (var session = SessionFactory.OpenSession())
			{
				new PersistenceSpecification<Board>(session)
					.CheckProperty(b => b.Empty, 10L)
					.CheckProperty(b => b.Mover, 20L)
					.CheckList(b => b.Successors,
						new HashSet<Board>() { Board.Start.Play(Move.D3) },
						(board, successor) => board.AddSuccessor(successor))
					.CheckList(b => b.Parents,
						new HashSet<Board>() { Board.Start }, // can only check one, the order is random it seems
						(board, parent) => board.AddParent(parent))
					.VerifyTheMappings();
			}
		}
	}
}
