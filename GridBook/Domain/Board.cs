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

		public virtual Board MinimalReflection()
		{
			Board temp = new Board(this.Empty.ToUInt64(), this.Mover.ToUInt64(), this.Color);
			Board result = new Board(this.Empty.ToUInt64(), this.Mover.ToUInt64(), this.Color);

			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					for (int k = 0; k < 2; k++)
					{
						if (temp < result)
							result = temp;

						temp = temp.FlipVertical();
					}

					temp = temp.FlipHorizontal();
				}

				temp = temp.FlipDiagonal();
			}

			return result;
		}

		public static Board FromString(string s)
		{
			if (s.Length != 66 || (s[65] != '*' && s[65] != 'O'))
			{
				throw new ArgumentException("Invalid board");
			}

			ulong empty = 0;
			ulong mover = 0;

			// assume black is mover
			for (int i = 0; i < 64; i++)
			{
				ulong mask = 1UL << (63 - i);

				if (s[i] == '*')
				{
					mover |= mask;
				}
				else if (s[i] == '-')
				{
					empty |= mask;
				}
				else if (s[i] != 'O')
				{
					throw new ArgumentException("Invalid board");
				}
			}

			Color color = s[65] == '*' ? Color.Black : Color.White;

			return color == Color.Black ? new Board(empty, mover, color) : new Board(empty, ~(empty | mover), color);
		}

		public static bool operator <(Board left, Board right)
		{
			if (left.Mover == right.Mover)
			{
				return left.Empty < right.Empty;
			}

			return left.Mover < right.Mover;
		}

		public static bool operator >(Board left, Board right)
		{
			if (left.Mover == right.Mover)
			{
				return left.Empty > right.Empty;
			}

			return left.Mover > right.Mover;
		}

		private Board FlipDiagonal()
		{
			return new Board(this.Empty.FlipDiagonal(), this.Mover.FlipDiagonal(), this.Color);
		}

		private Board FlipHorizontal()
		{
			return new Board(this.Empty.FlipHorizontal(), this.Mover.FlipHorizontal(), this.Color);
		}

		private Board FlipVertical()
		{
			return new Board(this.Empty.FlipVertical(), this.Mover.FlipVertical(), this.Color);
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
