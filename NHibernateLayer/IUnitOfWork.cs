namespace NHibernateLayer
{
	using System;
	using NHibernate;

	public interface IUnitOfWork : IDisposable
	{
		ISession Session
		{
			get;
		}

		void Commit();

		void Rollback();
	}
}
