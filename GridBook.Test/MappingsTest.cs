namespace GridBook.Test
{
	using FluentNHibernate.Testing;
	using GridBook.Domain;
	using NHibernateLayer;
	using NUnit.Framework;
	using System;

	[TestFixture, Database]
	public class MappingsTest : DatabaseTest
	{
		[Test]
		public void VerifyBoardMappings()
		{
			var uow = CreateSession();

			new PersistenceSpecification<Board>(uow)
				//.CheckProperty(b => b.Id, Guid.Empty)
				.CheckProperty(b => b.Empty, 10L)
				.CheckProperty(b => b.Mover, 20L)
				.VerifyTheMappings();
		}
	}
}
