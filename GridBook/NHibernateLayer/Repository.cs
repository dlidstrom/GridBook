namespace NHibernateLayer
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using GridBook.Data;
	using NHibernate;
	using NHibernate.Linq;

	public class Repository<T> : IKeyedRepository<T> where T : class
	{
		private readonly ISession session;

		public Repository(ISession session)
		{
			this.session = session;
		}

		public T FindBy(int id)
		{
			return session.Get<T>(id);
		}

		public bool Add(T entity)
		{
			session.Save(entity);
			return true;
		}

		public bool Add(IEnumerable<T> items)
		{
			foreach (var item in items)
			{
				Add(item);
			}

			return true;
		}

		public bool Update(T entity)
		{
			session.Update(entity);

			return true;
		}

		public bool Delete(T entity)
		{
			session.Delete(entity);

			return true;
		}

		public bool Delete(IEnumerable<T> entities)
		{
			foreach (var item in entities)
			{
				Delete(item);
			}

			return true;
		}

		public IQueryable<T> All()
		{
			return session.Query<T>();
		}

		public T FindBy(Expression<Func<T, bool>> expression)
		{
			return FilterBy(expression).Single();
		}

		public IQueryable<T> FilterBy(Expression<Func<T, bool>> expression)
		{
			return All().Where(expression);
		}
	}
}
