namespace GridBook.Data
{
	using System.Collections.Generic;

	public interface IRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
	{
		bool Add(TEntity entity);
		bool Add(IEnumerable<TEntity> items);
		bool Update(TEntity entity);
		bool Delete(TEntity entity);
		bool Delete(IEnumerable<TEntity> entities);
	}
}
