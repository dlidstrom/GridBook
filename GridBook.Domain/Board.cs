namespace GridBook.Test
{
	using System;
	using System.Text;
	using GridBook.Domain;

	public class Board
	{
		public Board(ulong empty, ulong mover)
		{
			this.Empty = empty;
			this.Mover = mover;
			if (((Empty | Mover) ^ Mover) != Empty)
			{
				throw new ArgumentException(string.Format("Empty and Mover overlap. Empty: 0x{0:X} Mover: 0x{1:X}", Empty, Mover));
			}
		}

		public static Board Start
		{
			get
			{
				return new Board(18446743970227683327, 34628173824);
			}
		}

		public Board Play(string move)
		{
			return Start;
		}

		public int Empties
		{
			get
			{
				return Bits.Count(Empty);
			}
		}

		private ulong Empty
		{
			get;
			set;
		}

		private ulong Mover
		{
			get;
			set;
		}

		public override int GetHashCode()
		{
			int hashCode = 0x61E04917;
			hashCode = (hashCode << 5) + Empty.GetHashCode();
			hashCode = (hashCode << 5) + Mover.GetHashCode();
			return hashCode;
		}

		public override bool Equals(object obj)
		{
			var board = obj as Board;
			if (board == null)
			{
				return false;
			}

			return board.Empty == Empty && board.Mover == Mover;
		}

		public override string ToString()
		{
			var builder = new StringBuilder();
			for (int i = 0; i < 64; i++)
			{
				ulong mask = (1UL << i);
				if ((Empty & mask) != 0)
				{
					builder.Append("-");
				}
				else if ((Mover & mask) != 0)
				{
					builder.Append("*");
				}
				else
				{
					builder.Append("O");
				}
			}

			builder.Append(" *");

			return builder.ToString();
		}
	}
}
