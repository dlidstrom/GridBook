namespace NHibernateLayer
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using GridBook.Data;
	using NHibernate;
	using NHibernate.Linq;
	using GridBook.Domain;

	public class Repository<T, TId> : IKeyedRepository<T, TId> where T : Entity<TId>
	{
		private readonly ISession session;

		public Repository(ISession session)
		{
			this.session = session;
		}

		public T FindBy(TId id)
		{
			return Transact(() => session.Get<T>(id));
		}

		public TId Add(T entity)
		{
			return Transact(() => (TId)session.Save(entity));
		}

		public bool Add(IEnumerable<T> items)
		{
			return Transact(() =>
			{
				foreach (var item in items)
				{
					Add(item);
				}

				return true;
			});
		}

		public bool Update(T entity)
		{
			return Transact(() =>
			{
				session.Update(entity);

				return true;
			});
		}

		public bool Delete(T entity)
		{
			return Transact(() =>
			{
				session.Delete(entity);

				return true;
			});
		}

		public bool Delete(IEnumerable<T> entities)
		{
			return Transact(() =>
			{
				foreach (var item in entities)
				{
					Delete(item);
				}

				return true;
			});
		}

		public IQueryable<T> All()
		{
			return Transact(() => session.Query<T>());
		}

		public T FindBy(Expression<Func<T, bool>> expression)
		{
			return Transact(() => FilterBy(expression).Single());
		}

		public IQueryable<T> FilterBy(Expression<Func<T, bool>> expression)
		{
			return Transact(() => All().Where(expression));
		}

		private TResult Transact<TResult>(Func<TResult> func)
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

		private void Transact(Action action)
		{
			Transact<bool>(() =>
			{
				action.Invoke();
				return false;
			});
		}
	}

	public class Repository<T> : Repository<T, Guid> where T : Entity<Guid>
	{
		public Repository(ISession session)
			: base(session)
		{
		}
	}
}
