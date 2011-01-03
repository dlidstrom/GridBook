namespace NHibernateLayer
{
	using FluentNHibernate.Cfg;
	using FluentNHibernate.Cfg.Db;
	using NHibernate;
	using NHibernate.Tool.hbm2ddl;

	public class NHibernateHelper
	{
		private readonly string connectionString;
		private readonly bool recreateDatabase;
		private ISessionFactory sessionFactory;

		public NHibernateHelper(string connectionString, bool recreateDatabase)
		{
			this.connectionString = connectionString;
			this.recreateDatabase = recreateDatabase;
		}

		public ISessionFactory SessionFactory
		{
			get
			{
				return sessionFactory ?? (sessionFactory = CreateSessionFactory());
			}
		}

		private ISessionFactory CreateSessionFactory()
		{
			var builder = Fluently.Configure()
				.Database(MySQLConfiguration.Standard.ConnectionString(c => c.FromConnectionStringWithKey(connectionString)))
				.Mappings(m => m.FluentMappings.AddFromAssemblyOf<GridBook.Domain.Mapping.BoardMap>());
			if (recreateDatabase)
			{
				return builder.ExposeConfiguration(c => new SchemaExport(c).Create(true, true)).BuildSessionFactory();
			}

			return builder.BuildSessionFactory();
		}
	}
}
