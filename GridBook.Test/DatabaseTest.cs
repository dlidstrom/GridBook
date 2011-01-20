namespace GridBook.Test
{
	using FluentNHibernate.Cfg;
	using FluentNHibernate.Cfg.Db;
	using GridBook.Domain.Mapping;
	using NHibernate;
	using NHibernate.Cfg;
	using NHibernate.Tool.hbm2ddl;
	using System;
	using NUnit.Framework;

	public static class SessionFactory
	{
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

	public class DatabaseTest
	{
		private ISession session;

		public ISession CurrentSession()
		{
			if (session == null)
			{
				var factory = SessionFactory.CreateSessionFactory();
				session = factory.OpenSession();
				SessionFactory.BuildSchema(session);
			}

			return session;
		}

		protected TResult Transact<TResult>(Func<TResult> func)
		{
			if (session.Transaction.IsActive)
			{
				return func.Invoke();
			}

			TResult result = default(TResult);
			using (var tx = session.BeginTransaction())
			{
				result = func.Invoke();
				tx.Commit();
			}

			return result;
		}

		protected void Transact(Action action)
		{
			Transact<bool>(() =>
			{
				action.Invoke();
				return false;
			});
		}
	}
}
