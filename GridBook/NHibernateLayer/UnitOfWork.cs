﻿namespace NHibernateLayer
{
	using NHibernate;
	using System.Data;
	using System;

	public class UnitOfWork : IUnitOfWork
	{
		private readonly ITransaction transaction;

		public UnitOfWork(ISession session)
		{
			Session = session;
			Session.FlushMode = FlushMode.Auto;
			transaction = Session.BeginTransaction(IsolationLevel.ReadCommitted);
		}

		public ISession Session
		{
			get;
			private set;
		}

		public void Commit()
		{
			if (!transaction.IsActive)
			{
				throw new InvalidOperationException("No active transaction.");
			}

			transaction.Commit();
		}

		public void Rollback()
		{
			if (transaction.IsActive)
			{
				transaction.Rollback();
			}
		}

		public void Dispose()
		{
			Session.Close();
		}
	}
}