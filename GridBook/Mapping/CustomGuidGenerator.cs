namespace GridBook.Mapping
{
	using System;
	using System.Collections.Generic;
	using GridBook.Domain;
	using NHibernate.Engine;
	using NHibernate.Id;

	/// <summary>
	/// A custom Id generator. This generator uses the Empty and Mover
	/// properties of Board to generate a Guid.
	/// </summary>
	public class CustomGuidGenerator : IIdentifierGenerator
	{
		/// <summary>
		/// Generate a new identifier.
		/// </summary>
		/// <param name="session">The ISessionImplementor this id is being generated in.</param>
		/// <param name="obj">The entity for which the id is being generated.</param>
		/// <returns>The new identifier.</returns>
		public object Generate(ISessionImplementor session, object obj)
		{
			// can only be used with Board
			var board = obj as Board;
			if (board == null)
			{
				throw new InvalidOperationException("CustomGuidGenerator can only be used with Board");
			}

			var bytes = new List<byte>(BitConverter.GetBytes(board.Empty));
			bytes.AddRange(BitConverter.GetBytes(board.Mover));
			return new Guid(bytes.ToArray());
		}
	}
}
