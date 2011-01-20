namespace GridBook.Domain
{
	using System.Collections.Generic;

	public class Book
	{
		public Book()
		{
			Positions = new HashSet<Board>();
		}

		public virtual int Id
		{
			get;
			private set;
		}

		public virtual string Name
		{
			get;
			set;
		}

		public virtual ISet<Board> Positions
		{
			get;
			set;
		}
	}
}
