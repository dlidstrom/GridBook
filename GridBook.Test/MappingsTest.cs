namespace GridBook.Test
{
	using FluentNHibernate.Testing;
	using GridBook.Domain;
	using NHibernateLayer;
	using NUnit.Framework;

	[TestFixture, Database]
	public class MappingsTest : DatabaseTest
	{
		[Test]
		public void VerifyBoardMappings()
		{
			var uow = new UnitOfWork(CreateSession());

			new PersistenceSpecification<Board>(uow.Session)
				.CheckProperty(b => b.Id, 1)
				.CheckProperty(b => b.Empty, 10L)
				.CheckProperty(b => b.Mover, 20L)
				.VerifyTheMappings();
		}
	}
}
