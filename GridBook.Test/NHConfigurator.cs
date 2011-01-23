namespace GridBook.Test
{
	using FluentNHibernate.Cfg;
	using FluentNHibernate.Cfg.Db;
	using GridBook.Domain.Mapping;
	using NHibernate;
	using NHibernate.Cfg;
	using NHibernate.Tool.hbm2ddl;

	public static class NHConfigurator
	{
		private static readonly Configuration configuration;
		private static readonly ISessionFactory sessionFactory;

		static NHConfigurator()
		{
			configuration = Fluently
				.Configure()
				.Database(SQLiteConfiguration.Standard.InMemory().Provider<TestConnectionProvider>().ShowSql)
				.Mappings(m => m.FluentMappings.AddFromAssemblyOf<BoardMap>())
				.BuildConfiguration();

			configuration.SetProperty(Environment.CurrentSessionContextClass, "thread_static");
			var props = configuration.Properties;
			if (props.ContainsKey(Environment.ConnectionStringName))
			{
				props.Remove(Environment.ConnectionStringName);
			}

			sessionFactory = configuration.BuildSessionFactory();
		}

		public static Configuration Configuration
		{
			get
			{
				return configuration;
			}
		}

		public static ISessionFactory SessionFactory
		{
			get
			{
				return sessionFactory;
			}
		}

#if true
		/// <summary>
		/// Creates SQLite session factory.
		/// </summary>
		/// <returns></returns>
		public static ISessionFactory CreateSessionFactory()
		{
			return
				Fluently
					.Configure()
					//.BuildConfiguration()
					.Database(SQLiteConfiguration.Standard.InMemory().ShowSql)
					.Mappings(m => m.FluentMappings.AddFromAssemblyOf<BoardMap>())
					.ExposeConfiguration((c) => SavedConfig = c)
					.BuildSessionFactory();
		}

		private static Configuration SavedConfig;

		public static void BuildSchema(ISession session)
		{
			var export = new SchemaExport(SavedConfig);
			export.Execute(true, true, false, session.Connection, null);
		}
#else
		/// <summary>
		/// Creates MySql session factory.
		/// </summary>
		/// <returns></returns>
		public static ISessionFactory CreateSessionFactory()
		{
			return
				Fluently
					.Configure()
					.Database(MySQLConfiguration.Standard.ConnectionString(c => c.FromConnectionStringWithKey("DbTest")).ShowSql())
					.ExposeConfiguration(c => new SchemaExport(c).Create(true, true))
					.Mappings(m => m.FluentMappings.AddFromAssemblyOf<BookMap>())
					.BuildSessionFactory();
		}

		public static void BuildSchema(ISession session)
		{
		}
#endif
	}
}
