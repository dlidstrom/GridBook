namespace GridBook.Data
{
	public interface IKeyedRepository<TEntity, TId> : IRepository<TEntity> where TEntity : class
	{
		/// <summary>
		/// Add entity to database.
		/// </summary>
		/// <param name="entity">Entity to add.</param>
		/// <returns>Entity id after successful save.</returns>
		TId Add(TEntity entity);

		/// <summary>
		/// Find entity by its database id.
		/// </summary>
		/// <param name="id">Database id.</param>
		/// <returns>Entity with the given id.</returns>
		TEntity FindBy(TId id);
	}
}
