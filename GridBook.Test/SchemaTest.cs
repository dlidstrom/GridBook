namespace GridBook.Test
{
	using FluentNHibernate.Cfg;
	using FluentNHibernate.Cfg.Db;
	using GridBook.Domain.Mapping;
	using NHibernate.Cfg;
	using NHibernate.Tool.hbm2ddl;
	using NUnit.Framework;

	[TestFixture]
	public class SchemaTest
	{
		[Test]
		public void CanGenerateSchema()
		{
			var cfg = new Configuration();
			cfg.Properties.Add("show_sql", "true");
			var builder = Fluently.Configure(cfg)
				.Database(MySQLConfiguration.Standard.ConnectionString(c => c.FromConnectionStringWithKey("DbTest")))
				.Mappings(m => m.FluentMappings.AddFromAssemblyOf<BookMap>())
				.ExposeConfiguration(c => new SchemaExport(c).Create(true, true));
			builder.ExposeConfiguration(c => new SchemaExport(c).Create(true, true)).BuildSessionFactory();
		}
	}
}
