﻿namespace GridBook.Domain
{
	using System;

	/// <summary>
	/// Represents a move.
	/// </summary>
	public class Move
	{
		public static readonly Move D3 = new Move("D3");
		public static readonly Move C4 = new Move("C4");
		public static readonly Move F5 = new Move("F5");
		public static readonly Move E6 = new Move("E6");

		/// <summary>
		/// Initializes a new instance of the Move class.
		/// </summary>
		/// <param name="move">Move as a string format.</param>
		/// <exception cref="ArgumentNullException">If move is null.</exception>
		/// <exception cref="ArgumentException">Invalid move string.</exception>
		public Move(string move)
		{
			if (move == null)
			{
				throw new ArgumentNullException("move");
			}

			Pos = (7 - (move[0] - 'A')) + (7 - (move[1] - '1')) * 8;
			if (Pos < 0 || Pos > 63)
			{
				throw new ArgumentException("Invalid move", "move");
			}
		}

		/// <summary>
		/// Gets or sets the position of the move.
		/// </summary>
		public int Pos
		{
			get;
			private set;
		}

		/// <summary>
		/// Creates a move when given a position index.
		/// </summary>
		/// <param name="pos">Position index.</param>
		/// <returns>Move instance.</returns>
		/// <exception cref="ArgumentException">pos out of range.</exception>
		public static Move FromPos(int pos)
		{
			if (pos < 0 || pos > 63)
			{
				throw new ArgumentException("Invalid move");
			}

			int col = (63 - pos) % 8;
			int row = (63 - pos) / 8;

			return new Move(string.Format("{0}{1}", (char)('A' + col), (char)('1' + row)));
		}

		public override string ToString()
		{
			int col = (63 - Pos) % 8;
			int row = (63 - Pos) / 8;

			return string.Format("{0}{1}", (char)('A' + col), (char)('1' + row));
		}
	}
}
