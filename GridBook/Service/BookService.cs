namespace GridBook.Service
{
	using System.Data;
	using GridBook.Domain;
	using NHibernate;

	public class BookService
	{
		private ISession session;

		public BookService(ISession session)
		{
			// TODO: Complete member initialization
			this.session = session;
		}

		public void Add(Board board)
		{
			using (var transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
			{
				session.Save(board);
				transaction.Commit();
			}
		}
	}
}
