namespace GridBook.Domain
{
	using System;
	using System.Text;
	using System.Collections.Generic;

	public class Board : Entity
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

		/// <summary>
		/// Initializes a new instance of the Board class.
		/// </summary>
		public Board()
			: this(18446743970227683327, 34628173824, Color.Black)
		{
		}

		/// <summary>
		/// Initializes a new instance of the Board class.
		/// </summary>
		/// <param name="empty">Bits representing the empty squares.</param>
		/// <param name="mover">Bits representing the squares of the mover.</param>
		/// <param name="color">Color of the player to move.</param>
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

		/// <summary>
		/// Initializes a new instance of the Board class.
		/// </summary>
		/// <param name="empty">Bits representing the empty squares.</param>
		/// <param name="mover">Bits representing the squares of the mover.</param>
		/// <param name="color">Color of the player to move.</param>
		public Board(long empty, long mover, Color color)
			: this(empty.ToUInt64(), mover.ToUInt64(), color)
		{
		}

		/// <summary>
		/// Play a move.
		/// </summary>
		/// <param name="move">Move to play.</param>
		/// <returns>Board result after move has been played.</returns>
		/// <exception cref="ArgumentException">If move is invalid.</exception>
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
		public virtual int GetEmpties()
		{
			return Bits.Count(Empty);
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

		/// <summary>
		/// Gets or sets the player to move.
		/// </summary>
		private Color Color
		{
			get;
			set;
		}

		/// <summary>
		/// Calculates a minimal reflection.
		/// </summary>
		/// <returns>Minimal reflection of this board state.</returns>
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

		/// <summary>
		/// Construct a board from a string representation.
		/// </summary>
		/// <param name="s">String representation.</param>
		/// <returns>Board instance.</returns>
		/// <exception cref="ArgumentException">If string content is invalid.</exception>
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

		/// <summary>
		/// Less-than operator.
		/// </summary>
		/// <param name="left">Left side.</param>
		/// <param name="right">Right side.</param>
		/// <returns>True if left &lt; right, otherwise false.</returns>
		public static bool operator <(Board left, Board right)
		{
			if (left.Mover == right.Mover)
			{
				return left.Empty < right.Empty;
			}

			return left.Mover < right.Mover;
		}

		/// <summary>
		/// Greater-than operator.
		/// </summary>
		/// <param name="left">Left side.</param>
		/// <param name="right">Right side.</param>
		/// <returns>True if left &gt; right, otherwise false.</returns>
		public static bool operator >(Board left, Board right)
		{
			if (left.Mover == right.Mover)
			{
				return left.Empty > right.Empty;
			}

			return left.Mover > right.Mover;
		}

		/// <summary>
		/// Flip board on the A8-H1 diagonal.
		/// </summary>
		/// <returns>Flipped board.</returns>
		private Board FlipDiagonal()
		{
			return new Board(this.Empty.ToUInt64().FlipDiagonal(), this.Mover.ToUInt64().FlipDiagonal(), this.Color);
		}

		/// <summary>
		/// Flip board horizontally.
		/// </summary>
		/// <returns>Flipped board.</returns>
		private Board FlipHorizontal()
		{
			return new Board(this.Empty.ToUInt64().FlipHorizontal(), this.Mover.ToUInt64().FlipHorizontal(), this.Color);
		}

		/// <summary>
		/// Flip board vertically.
		/// </summary>
		/// <returns>Flipped board.</returns>
		private Board FlipVertical()
		{
			return new Board(this.Empty.ToUInt64().FlipVertical(), this.Mover.ToUInt64().FlipVertical(), this.Color);
		}

		/// <summary>
		/// Calculate a hash code for the board. This allows boards to be put inside hashtable-based dictionaries.
		/// </summary>
		/// <returns>Board hash code.</returns>
		public override int GetHashCode()
		{
			int seed = (int)(Empty >> 32);
			seed ^= (int)Empty + (seed << 6) + (seed >> 2);
			seed ^= (int)(Mover >> 32) + (seed << 6) + (seed >> 2);
			seed ^= (int)Mover + (seed << 6) + (seed >> 2);
			return seed;
		}

		/// <summary>
		/// Determines whether this board is equal to another board.
		/// </summary>
		/// <param name="obj">Other board.</param>
		/// <returns>True if boards are equal, false otherwise.</returns>
		public override bool Equals(object obj)
		{
			var board = obj as Board;
			if (board == null)
			{
				return false;
			}

			return board.Empty == Empty && board.Mover == Mover;
		}

		/// <summary>
		/// Returns a string representation of this board.
		/// </summary>
		/// <returns>String representation.</returns>
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

		/// <summary>
		/// Scan along a direction.
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="dir"></param>
		/// <param name="opponent"></param>
		/// <param name="cond"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Get the successors of this position.
		/// </summary>
		/// <returns>Successors of this position.</returns>
		public virtual IList<Board> Successors()
		{
			ulong empty = this.Empty.ToUInt64();
			var successors = new List<Board>();
			while (empty != 0)
			{
				try
				{
					successors.Add(Play(Move.FromPos(empty.LSB())));
				}
				catch (ArgumentException)
				{
				}

				empty = empty & (empty - 1);
			}

			return successors;
		}
	}
}
