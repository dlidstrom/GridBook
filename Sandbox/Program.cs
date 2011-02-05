namespace Sandbox
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using GridBook.Domain;
	using System.Diagnostics;

	class BoardComparer : Comparer<Board>
	{
		public override int Compare(Board x, Board y)
		{
			if (x < y)
			{
				return -1;
			}
			else if (x == y)
			{
				return 0;
			}
			else
				return 1;
		}
	}

	public class Program
	{
		static void Main(string[] args)
		{
			// Arrange
			var random = new Random();
			var guids = new List<Guid>(Enumerable.Range(0, 6).Select(i =>
			{
				var bytes = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
				random.NextBytes(bytes);
				var empty = BitConverter.ToUInt64(bytes, 0);
				var mover = 0xFFFFFFFFFFFFFFFF ^ empty;
				var list = new List<byte>(bytes);
				list.AddRange(BitConverter.GetBytes(mover));
				return new Guid(list.ToArray());
			}));
			var boards = new List<Board>(guids.Select(g => Board.FromGuid(g)));

			// Act
			guids.Sort();
			boards.Sort(new BoardComparer());

			// Assert
			for (int i = 0; i < guids.Count; i++)
			{
				var g = guids[i];
				var b = boards[i];
				if (b.ToGuid() != g)
					throw new InvalidOperationException("Not same");
			}
		}
	}
}
