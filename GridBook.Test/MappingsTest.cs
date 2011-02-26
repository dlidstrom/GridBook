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
					.CheckProperty(b => b.Empty, 10L, (b, e) => typeof(Board).GetField("empty", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(b, e))
					.CheckProperty(b => b.Mover, 20L, (b, m) => typeof(Board).GetField("mover", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(b, m))
					.CheckProperty(b => b.Ply, 24, (b, p) => typeof(Board).GetField("ply", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(b, p))
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
