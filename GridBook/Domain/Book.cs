namespace GridBook.Domain
{
	using System;
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

		public virtual void Add(Board position)
		{
			Positions.Add(position.MinimalReflection());
			foreach (var successor in position.Successors())
			{
				Positions.Add(successor.MinimalReflection());
			}
		}

		public virtual bool Contains(Board position)
		{
			return Positions.Contains(position);
		}
	}
}
