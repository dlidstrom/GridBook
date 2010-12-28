namespace NHibernateLayer.Test
{
	using System;
	using NUnit.Framework;
	using GridBook.Domain;
	using FluentNHibernate.Testing;

	[TestFixture]
	public class RepositoryTest
	{
		[Test]
		public void CountBoardsInDB()
		{
			// Arrange
			NHibernateHelper helper = new NHibernateHelper("DbTest", true);
			var uow = new UnitOfWork(helper.SessionFactory);

			// Act
			new PersistenceSpecification<Board>(uow.Session)
				.CheckProperty(b => b.Id, 1)
				.CheckProperty(b => b.Empty, 10UL)
				.CheckProperty(b => b.Mover, 20UL)
				.VerifyTheMappings();

			// Assert
		}
	}
}
