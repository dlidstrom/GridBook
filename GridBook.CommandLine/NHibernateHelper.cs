namespace GridBook.CommandLine
{
	using System;
	using System.Configuration;
	using System.IO;
	using Common.Logging;
	using FluentNHibernate.Cfg;
	using FluentNHibernate.Cfg.Db;
	using GridBook.Domain.Mapping;
	using NHibernate;
	using NHibernate.Tool.hbm2ddl;

	public static class NHibernateHelper
	{
		private static ILog log = LogManager.GetCurrentClassLogger();
		private static ISessionFactory sessionFactory;

		public static ISessionFactory SessionFactory
		{
			get
			{
				if (sessionFactory == null)
				{
					throw new InvalidOperationException("Session factory has not been created. Call CreateSessionFactory.");
				}

				return sessionFactory;
			}
		}

		public static void CreateSessionFactory(string password, bool createSchema)
		{
			var connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
			log.DebugFormat("Creating session [{0}]", string.Format(connectionString, new string('*', 8)));

			var builder = Fluently.Configure()
				//.Database(MySQLConfiguration.Standard.ConnectionString(c => c.FromConnectionStringWithKey(connectionString)))
				.Database(PostgreSQLConfiguration.PostgreSQL82.ConnectionString(c => c.FromConnectionStringWithKey(connectionString)))
				//.Database(SQLiteConfiguration.Standard.UsingFile("positions.db"))
				.Mappings(m => m.FluentMappings.AddFromAssemblyOf<BoardMap>())
				.ExposeConfiguration(c =>
				{
					if (createSchema)
					{
						if (File.Exists("positions.db"))
							File.Delete("positions.db");
						new SchemaExport(c).Create(true, true);
					}
				});

			sessionFactory = builder.BuildSessionFactory();
		}
	}
}
