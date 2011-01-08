namespace GridBook.Domain
{
	using System;
	using System.Text;

	public class Board
	{
		/// <summary>
		/// Gets a board representing the starting position.
		/// </summary>
		public static Board Start
		{
			get
			{
				return new Board(18446743970227683327, 34628173824, Color.Black);
			}
		}

		public Board()
			: this(18446743970227683327, 34628173824, Color.Black)
		{
		}

		public Board(ulong empty, ulong mover, Color color)
		{
			this.Empty = empty.ToInt64();
			this.Mover = mover.ToInt64();
			this.Color = color;
			if (((Empty | Mover) ^ Mover) != Empty)
			{
				throw new ArgumentException(string.Format("Empty and Mover overlap. Empty: 0x{0:X} Mover: 0x{1:X}", Empty, Mover));
			}
		}

		public virtual Board Play(Move move)
		{
			int pos = move.Pos;
			long opponent = ~(Empty | Mover);
			long cumulativeChange = 0;

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

			long newEmpty = Empty ^ (1L << move.Pos);
			long mover = opponent ^ cumulativeChange;

			return new Board(newEmpty.ToUInt64(), mover.ToUInt64(), this.Color.Opponent());
		}

		/// <summary>
		/// Gets the number of empty positions.
		/// </summary>
		public virtual int Empties
		{
			get
			{
				return Bits.Count(Empty);
			}
		}

		/// <summary>
		/// Gets a 64-bit long integer representing the empty squares.
		/// </summary>
		public virtual long Empty
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a 64-bit long integer representing the squares of the player to move.
		/// </summary>
		public virtual long Mover
		{
			get;
			private set;
		}

		public virtual int Id
		{
			get;
			private set;
		}

		public override int GetHashCode()
		{
			int seed = (int)(Empty >> 32);
			seed ^= (int)Empty + (seed << 6) + (seed >> 2);
			seed ^= (int)(Mover >> 32) + (seed << 6) + (seed >> 2);
			seed ^= (int)Mover + (seed << 6) + (seed >> 2);
			return seed;
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
				long mask = (1L << i);
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

		private long scanDirection(int pos, int dir, long opponent, Func<int, bool> cond)
		{
			long cumulativeChange = 0;
			pos += dir;
			if (((1L << pos) & opponent) != 0)
			{
				long change = 1L << pos;
				for (pos += dir; cond(pos) && ((1L << pos) & opponent) != 0; pos += dir)
				{
					change |= 1L << pos;
				}
				if (cond(pos) && (1L << pos & Mover) != 0)
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
	}
}
