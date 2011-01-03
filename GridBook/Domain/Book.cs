namespace GridBook.Domain
{
	using System;
	using System.Collections.Generic;

	public class Book
	{
		public Book()
		{
			Positions = new List<Board>();
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

		public virtual IList<Board> Positions
		{
			get;
			set;
		}
	}
}
