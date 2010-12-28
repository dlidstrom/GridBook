namespace GridBook.Data
{
	public interface IKeyedRepository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		TEntity FindBy(int id);
	}
}
