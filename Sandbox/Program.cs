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
			var board = Board.Start;
			var successors = board.CalculateSuccessors();
		}
	}
}
