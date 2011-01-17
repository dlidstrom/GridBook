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
			this.session = session;
		}

		public void Add(Board board)
		{
			session.Save(board);
		}
	}
}
