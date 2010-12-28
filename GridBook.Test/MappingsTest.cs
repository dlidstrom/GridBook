namespace GridBook.Test
{
	using FluentNHibernate.Testing;
	using GridBook.Domain;
	using NHibernateLayer;
	using NUnit.Framework;

	[TestFixture, Database]
	public class MappingsTest
	{
		[Test]
		public void VerifyBoardMappings()
		{
			NHibernateHelper helper = new NHibernateHelper("DbTest", true);
			var uow = new UnitOfWork(helper.SessionFactory);

			new PersistenceSpecification<Board>(uow.Session)
				.CheckProperty(b => b.Id, 1)
				.CheckProperty(b => b.Empty, 10UL)
				.CheckProperty(b => b.Mover, 20UL)
				.VerifyTheMappings();
		}
	}
}
