namespace GridBook.Test
{
	using System;
	using NHibernate;
	using NHibernate.Context;
	using NHibernate.Tool.hbm2ddl;

	public class NHibernateFixture : BaseFixture
	{
		protected ISessionFactory SessionFactory
		{
			get
			{
				return NHConfigurator.SessionFactory;
			}
		}

		protected ISession Session
		{
			get
			{
				return SessionFactory.GetCurrentSession();
			}
		}

		protected override void OnSetup()
		{
			SetupNHibernateSession();
			base.OnSetup();
		}

		protected void SetupNHibernateSession()
		{
			TestConnectionProvider.CloseDatabase();
			SetupContextualSession();
			BuildSchema();
		}

		protected void TearDownNHibernateSession()
		{
			TearDownContextualSession();
			TestConnectionProvider.CloseDatabase();
		}

		private void SetupContextualSession()
		{
			var session = SessionFactory.OpenSession();
			CurrentSessionContext.Bind(session);
		}

		private void TearDownContextualSession()
		{
			var sessionFactory = NHConfigurator.SessionFactory;
			var session = CurrentSessionContext.Unbind(sessionFactory);
			session.Close();
		}

		private void BuildSchema()
		{
			var cfg = NHConfigurator.Configuration;
			var schemaExport = new SchemaExport(cfg);
			schemaExport.Create(true, true);
		}
	}
}
