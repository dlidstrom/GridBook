namespace GridBook.Domain
{
	using System;
	using System.Linq;
	using System.Text;
	using Iesi.Collections.Generic;

	public class Board : Entity
	{
		private readonly ISet<Board> successors = new HashedSet<Board>();
		private readonly ISet<Board> parents = new HashedSet<Board>();
		private readonly long empty;
		private readonly long mover;
		private readonly int ply;
		private readonly Color color;

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
		/// <exception cref="ArgumentException">If empty and mover bitboards overlap.</exception>
		public Board(ulong empty, ulong mover, Color color)
		{
			this.empty = empty.ToInt64();
			this.mover = mover.ToInt64();
			this.color = color;
			this.ply = 60 - Bits.Count(this.empty);
			if (((this.empty | this.mover) ^ this.mover) != this.empty)
			{
				throw new ArgumentException(string.Format("Empty and Mover overlap. Empty: 0x{0:X} Mover: 0x{1:X}", this.empty, this.mover));
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
		/// Gets the number of empty positions.
		/// </summary>
		public virtual int GetEmpties()
		{
			return Bits.Count(this.empty);
		}

		/// <summary>
		/// Gets a 64-bit long integer representing the empty squares.
		/// </summary>
		public virtual long Empty
		{
			get
			{
				return empty;
			}
		}

		/// <summary>
		/// Gets a 64-bit long integer representing the squares of the player to move.
		/// </summary>
		public virtual long Mover
		{
			get
			{
				return mover;
			}
		}

		/// <summary>
		/// Gets the player to move.
		/// </summary>
		public virtual Color Color
		{
			get
			{
				return color;
			}
		}

		/// <summary>
		/// Gets the ply of this position.
		/// </summary>
		public virtual int Ply
		{
			get
			{
				return ply;
			}
		}

		/// <summary>
		/// Gets the parents of this position.
		/// </summary>
		public virtual System.Collections.Generic.IEnumerable<Board> Parents
		{
			get
			{
				return parents;
			}
		}

		/// <summary>
		/// Adds a parent to this position.
		/// </summary>
		/// <param name="parent"></param>
		public virtual void AddParent(Board parent)
		{
			parents.Add(parent);
		}

		/// <summary>
		/// Gets the successors of this position.
		/// </summary>
		public virtual System.Collections.Generic.IEnumerable<Board> Successors
		{
			get
			{
				return successors;
			}
		}

		/// <summary>
		/// Adds a successor to this position.
		/// </summary>
		/// <param name="successor"></param>
		public virtual void AddSuccessor(Board successor)
		{
			successors.Add(successor);
		}

		/// <summary>
		/// Calculates the successors of this position.
		/// If a pass is required then the position after pass will be returned.
		/// </summary>
		/// <returns>Successors of this position.</returns>
		public virtual System.Collections.Generic.IList<Board> CalculateSuccessors()
		{
			var successors = calculateSuccessors();
			if (successors.Count == 0)
			{
				var passed = pass();
				if (passed.calculateSuccessors().Count > 0)
				{
					successors.Add(passed);
				}
			}

			return successors;
		}

		/// <summary>
		/// Calculates the minimal successors of this position.
		/// </summary>
		/// <returns>Minimal successors of this position.</returns>
		public virtual System.Collections.Generic.IList<Board> CalculateMinimalSuccessors()
		{
			return new System.Collections.Generic.HashSet<Board>(from b in CalculateSuccessors()
																 select b.MinimalReflection()).ToList();
		}

		/// <summary>
		/// Calculates a minimal reflection.
		/// </summary>
		/// <returns>Minimal reflection of this board state.</returns>
		public virtual Board MinimalReflection()
		{
			Board temp = new Board(this.empty.ToUInt64(), this.mover.ToUInt64(), this.color);
			Board result = new Board(this.empty.ToUInt64(), this.mover.ToUInt64(), this.color);

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
		/// Play a move.
		/// </summary>
		/// <param name="move">Move to play.</param>
		/// <returns>Board result after move has been played.</returns>
		/// <exception cref="ArgumentNullException">If move is null.</exception>
		/// <exception cref="ArgumentException">If move is invalid.</exception>
		public virtual Board Play(Move move)
		{
			if (move == null)
			{
				throw new ArgumentNullException("move");
			}

			int pos = move.Pos;
			long opponent = ~(this.empty | this.mover);
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
			cumulativeChange |= scanDirection(pos, -9, opponent, p => p >= 0 && ((p + 1) % 8) != 0);

			// down-left
			cumulativeChange |= scanDirection(pos, -7, opponent, p => p >= 0 && (p % 8) != 0);

			if (cumulativeChange == 0)
			{
				throw new ArgumentException("Invalid move", "move");
			}

			long newEmpty = this.empty ^ (1L << move.Pos);
			long mover = opponent ^ cumulativeChange;

			return new Board(newEmpty.ToUInt64(), mover.ToUInt64(), this.color.Opponent());
		}

		/// <summary>
		/// Construct a board from a string representation.
		/// </summary>
		/// <param name="s">String representation.</param>
		/// <returns>Board instance.</returns>
		/// <exception cref="ArgumentNullException">If s is null.</exception>
		/// <exception cref="ArgumentException">If string content is invalid.</exception>
		public static Board FromString(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}

			if (s.Length != 66 || (s[65] != '*' && s[65] != 'O'))
			{
				throw new ArgumentException("invalid content", "s");
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
		/// <exception cref="ArgumentNullException">If left is null.</exception>
		/// <exception cref="ArgumentNullException">If right is null.</exception>
		public static bool operator <(Board left, Board right)
		{
			if (left == null)
			{
				throw new ArgumentNullException("left");
			}
			if (right == null)
			{
				throw new ArgumentNullException("right");
			}

			return left.ToGuid().CompareTo(right.ToGuid()) < 0;
		}

		/// <summary>
		/// Greater-than operator.
		/// </summary>
		/// <param name="left">Left side.</param>
		/// <param name="right">Right side.</param>
		/// <returns>True if left &gt; right, otherwise false.</returns>
		/// <exception cref="ArgumentNullException">If left is null.</exception>
		/// <exception cref="ArgumentNullException">If right is null.</exception>
		public static bool operator >(Board left, Board right)
		{
			if (left == null)
			{
				throw new ArgumentNullException("left");
			}
			if (right == null)
			{
				throw new ArgumentNullException("right");
			}

			return left.ToGuid().CompareTo(right.ToGuid()) > 0;
		}

		/// <summary>
		/// Calculate successors to this position. Does not handle passing.
		/// </summary>
		/// <returns>Successors of this position.</returns>
		private System.Collections.Generic.IList<Board> calculateSuccessors()
		{
			var successors = new System.Collections.Generic.List<Board>();
			ulong empty = moves();
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

		/// <summary>
		/// Get legal moves.
		/// </summary>
		/// <returns>Legal moves.</returns>
		private ulong moves()
		{
			ulong P = this.mover.ToUInt64();
			ulong O = ~(this.mover | this.empty).ToUInt64();
			return (get_some_moves(P, O & 0x7E7E7E7E7E7E7E7E, 1) // horizontal
				| get_some_moves(P, O & 0x00FFFFFFFFFFFF00, 8)   // vertical
				| get_some_moves(P, O & 0x007E7E7E7E7E7E00, 7)   // diagonals
				| get_some_moves(P, O & 0x007E7E7E7E7E7E00, 9))
				& ~(P | O); // mask with empties
		}

		/// <summary>
		/// Get moves along a direction.
		/// </summary>
		/// <param name="P">Player.</param>
		/// <param name="mask">Bitmask.</param>
		/// <param name="dir">Direction.</param>
		/// <returns>Moves along direction.</returns>
		private static ulong get_some_moves(ulong P, ulong mask, int dir)
		{
			// sequential algorithm
			// 7 << + 7 >> + 6 & + 12 |
			ulong flip = (((P << dir) | (P >> dir)) & mask);
			flip |= (((flip << dir) | (flip >> dir)) & mask);
			flip |= (((flip << dir) | (flip >> dir)) & mask);
			flip |= (((flip << dir) | (flip >> dir)) & mask);
			flip |= (((flip << dir) | (flip >> dir)) & mask);
			flip |= (((flip << dir) | (flip >> dir)) & mask);
			return (flip << dir) | (flip >> dir);
		}

		/// <summary>
		/// Pass to the other player.
		/// </summary>
		private Board pass()
		{
			long opponent = ~(this.empty | this.mover);
			return new Board(this.empty, opponent, this.color.Opponent());
		}

		/// <summary>
		/// Flip board on the A8-H1 diagonal.
		/// </summary>
		/// <returns>Flipped board.</returns>
		private Board FlipDiagonal()
		{
			return new Board(this.empty.ToUInt64().FlipDiagonal(), this.mover.ToUInt64().FlipDiagonal(), this.color);
		}

		/// <summary>
		/// Flip board horizontally.
		/// </summary>
		/// <returns>Flipped board.</returns>
		private Board FlipHorizontal()
		{
			return new Board(this.empty.ToUInt64().FlipHorizontal(), this.mover.ToUInt64().FlipHorizontal(), this.color);
		}

		/// <summary>
		/// Flip board vertically.
		/// </summary>
		/// <returns>Flipped board.</returns>
		private Board FlipVertical()
		{
			return new Board(this.empty.ToUInt64().FlipVertical(), this.mover.ToUInt64().FlipVertical(), this.color);
		}

		/// <summary>
		/// Convert into Guid.
		/// </summary>
		/// <returns>Guid instance.</returns>
		public virtual Guid ToGuid()
		{
			var bytes = new System.Collections.Generic.List<byte>(BitConverter.GetBytes(this.empty));
			bytes.AddRange(BitConverter.GetBytes(this.mover));
			return new Guid(bytes.ToArray());
		}

		/// <summary>
		/// Create Board from Guid.
		/// </summary>
		/// <param name="guid">Guid representing board.</param>
		/// <returns>Board instance.</returns>
		public static Board FromGuid(Guid guid)
		{
			var bytes = guid.ToByteArray();
			var empty = BitConverter.ToInt64(bytes, 0);
			var mover = BitConverter.ToInt64(bytes, 8);
			return new Board(empty, mover, Color.Black);
		}

		/// <summary>
		/// Calculate a hash code for the board. This allows boards to be put inside hashtable-based dictionaries.
		/// </summary>
		/// <returns>Board hash code.</returns>
		public override int GetHashCode()
		{
			int seed = (int)(this.empty >> 32);
			seed ^= (int)this.empty + (seed << 6) + (seed >> 2);
			seed ^= (int)(this.mover >> 32) + (seed << 6) + (seed >> 2);
			seed ^= (int)this.mover + (seed << 6) + (seed >> 2);
			return seed;
		}

		/// <summary>
		/// Determines whether this board is equal to another board.
		/// </summary>
		/// <param name="obj">Other board.</param>
		/// <returns>True if boards are equal, false otherwise.</returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as Board);
		}

		/// <summary>
		/// Determines whether this board is equal to another board.
		/// </summary>
		/// <param name="obj">Other board.</param>
		/// <returns>True if boards are equal, false otherwise.</returns>
		public virtual bool Equals(Board board)
		{
			if (board == null)
			{
				return false;
			}

			return board.Empty == this.empty && board.Mover == this.mover;
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
				if ((this.empty & mask) != 0)
				{
					builder.Append("-");
				}
				else if ((this.mover & mask) != 0)
				{
					builder.Append(this.color.Char());
				}
				else
				{
					builder.Append(this.color.Opponent().Char());
				}
			}

			builder.Append(" " + this.color.Char());

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
			long change = 0;
			for (pos += dir; cond(pos) && ((1L << pos) & opponent) != 0; pos += dir)
			{
				change |= 1L << pos;
			}

			if (cond(pos) && (1L << pos & this.mover) != 0)
			{
				cumulativeChange = change;
			}

			return cumulativeChange;
		}
	}
}
