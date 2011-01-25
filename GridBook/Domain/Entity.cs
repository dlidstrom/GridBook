namespace GridBook.Domain
{
	using System;

	public abstract class Entity<TId>
	{
		/// <summary>
		/// Database id.
		/// </summary>
		public virtual TId Id
		{
			get;
			protected set;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Entity<TId>);
		}

		private static bool IsTransient(Entity<TId> obj)
		{
			return obj != null && Equals(obj.Id, default(TId));
		}

		private Type GetUnproxiedType()
		{
			return GetType();
		}

		public virtual bool Equals(Entity<TId> other)
		{
			if (other == null)
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			if (!IsTransient(this) && !IsTransient(other) && Equals(Id, other.Id))
			{
				var otherType = other.GetUnproxiedType();
				var thisType = GetUnproxiedType();
				return otherType.IsAssignableFrom(thisType) || thisType.IsAssignableFrom(otherType);
			}

			return false;
		}

		public override int GetHashCode()
		{
			if (Equals(Id, default(TId)))
			{
				return base.GetHashCode();
			}

			return Id.GetHashCode();
		}
	}

	/// <summary>
	/// An entity with guid.comb id generator.
	/// </summary>
	public abstract class Entity : Entity<Guid>
	{
	}
}
