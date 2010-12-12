namespace GridBook.Domain
{
	using System;
	using System.Text;

	public enum Color
	{
		Black,
		White
	}

	public static class ColorExtensions
	{
		public static Color Opponent(this Color color)
		{
			if (color == Color.White)
			{
				return Color.Black;
			}

			return Color.White;
		}

		public static string Char(this Color color)
		{
			if (color == Color.White)
			{
				return "O";
			}

			return "*";
		}
	}

	public class Board
	{
		public Board(ulong empty, ulong mover, Color color)
		{
			this.Empty = empty;
			this.Mover = mover;
			this.Color = color;
			if (((Empty | Mover) ^ Mover) != Empty)
			{
				throw new ArgumentException(string.Format("Empty and Mover overlap. Empty: 0x{0:X} Mover: 0x{1:X}", Empty, Mover));
			}
		}

		public static Board Start
		{
			get
			{
				return new Board(18446743970227683327, 34628173824, Color.Black);
			}
		}

		public Board Play(Move move)
		{
			int pos = move.Pos;
			ulong opponent = ~(Empty | Mover);
			ulong cumulativeChange = 0;

			// up
			cumulativeChange |= scanDirection(pos, 8, opponent, p => p <= 63);

			// down
			cumulativeChange |= scanDirection(pos, -8, opponent, p => p >= 0);

			// left
			cumulativeChange |= scanDirection(pos, 1, opponent, p => p <= 63 && (p % 8) != 0);

			// right
			cumulativeChange |= scanDirection(pos, -1, opponent, p => p >= 0 && ((p + 1) % 8) != 0);

			// up-left
			cumulativeChange |= scanDirection(pos, 9, opponent, p => p <= 63 && (p % 8) != 0);

			// up-right
			cumulativeChange |= scanDirection(pos, 7, opponent, p => p <= 63 && ((p + 1) % 8 != 0));

			// down-right
			cumulativeChange |= scanDirection(pos, 8, opponent, p => p >= 0 && ((p + 1) % 8) != 0);

			// down-left
			cumulativeChange |= scanDirection(pos, 8, opponent, p => p >= 0 && (p % 8) != 0);

			if (cumulativeChange == 0)
			{
				throw new ArgumentException("Invalid move", "move");
			}

			ulong newEmpty = Empty ^ (1UL << move.Pos);
			ulong mover = opponent ^ cumulativeChange;

			return new Board(newEmpty, mover, this.Color.Opponent());
		}

		public int Empties
		{
			get
			{
				return Bits.Count(Empty);
			}
		}

		private ulong scanDirection(int pos, int dir, ulong opponent, Func<int, bool> cond)
		{
			ulong cumulativeChange = 0;
			pos += dir;
			if (((1UL << pos) & opponent) != 0)
			{
				ulong change = 1UL << pos;
				for (pos += dir; cond(pos) && ((1UL << pos) & opponent) != 0; pos += dir)
				{
					change |= 1UL << pos;
				}
				if (cond(pos) && (1UL << pos & Mover) != 0)
				{
					cumulativeChange = change;
				}
			}

			return cumulativeChange;
		}

		private Color Color
		{
			get;
			set;
		}

		public ulong Empty
		{
			get;
			private set;
		}

		public ulong Mover
		{
			get;
			private set;
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
			for (int i = 63; i >= 0; i--)
			{
				ulong mask = (1UL << i);
				if ((Empty & mask) != 0)
				{
					builder.Append("-");
				}
				else if ((Mover & mask) != 0)
				{
					builder.Append(Color.Char());
				}
				else
				{
					builder.Append(Color.Opponent().Char());
				}
			}

			builder.Append(" " + Color.Char());

			return builder.ToString();
		}
	}
}
