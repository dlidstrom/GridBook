namespace GridBook.Domain
{
	using System;

	public class Entity<TId>
	{
		/// <summary>
		/// Database id.
		/// </summary>
		public virtual TId Id
		{
			get;
			protected set;
		}
	}

	public class Entity : Entity<Guid>
	{
	}
}
