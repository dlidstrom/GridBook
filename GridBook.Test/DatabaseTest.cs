namespace GridBook.Test
{
	using System;
	using NHibernate;

	class DatabaseTest
	{
		private ISession session;

		public ISession CurrentSession()
		{
			if (session == null)
			{
				var factory = NHConfigurator.CreateSessionFactory();
				session = factory.OpenSession();
				NHConfigurator.BuildSchema(session);
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
