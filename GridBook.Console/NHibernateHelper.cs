﻿namespace GridBook.Console
{
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

		public static ISessionFactory CreateSessionFactory(string connectionString, bool createSchema)
		{
			log.DebugFormat("Creating session");

			var builder = Fluently.Configure()
				.Database(MySQLConfiguration.Standard.ConnectionString(c => c.FromConnectionStringWithKey(connectionString)))
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

			return builder.BuildSessionFactory();
		}
	}
}
