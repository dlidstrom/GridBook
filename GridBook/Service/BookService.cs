namespace GridBook.Service
{
	using System;
	using GridBook.Domain;
	using NHibernate;

	public class BookService
	{
		private ISession session;

		public BookService(ISession session)
		{
			this.session = session;
		}

		public void Add(Board position)
		{
			var minimal = position.MinimalReflection();
			foreach (var successor in position.Successors)
			{
				successor.AddParent(minimal);
			}

			session.Save(minimal);
		}

		public bool Contains(Board position)
		{
			throw new NotImplementedException();
		}

		public Board Find(Board position)
		{
			throw new NotImplementedException();
		}
	}
}
