namespace GridBook.Test
{
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
					//.CheckProperty(b => b.Id, Guid.Empty)
					.CheckProperty(b => b.Empty, 10L)
					.CheckProperty(b => b.Mover, 20L)
					.VerifyTheMappings();
			}
		}
	}
}
