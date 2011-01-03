namespace GridBook.Domain
{
	using System;

	public class Move
	{
		public Move(string move)
		{
			Pos = (7 - (move[0] - 'A')) + (7 - (move[1] - '1')) * 8;
		}

		public int Pos
		{
			get;
			set;
		}
	}
}
