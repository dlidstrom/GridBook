namespace Sandbox
{
	using System;
	using GridBook.Domain;
	using System.Collections.Generic;

	public class Program
	{
		static void Main(string[] args)
		{
			var pos = Board.FromString("O*OOOOO*OOOOOOO*OOOOOO**O*OOOO*-O******OO****-----*-------*----- *");

			// Act
			var successors = pos.CalculateSuccessors();
		}
	}
}
