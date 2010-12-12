namespace GridBook.Console
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using GridBook.Domain.Importers;
	using GridBook.Domain;

	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				var board = Board.Start;

				// Act
				var successor = board.Play(new Move("D3"));
				Console.WriteLine(successor.ToString());
				return;
				new Program().Run(args);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		void Run(string[] args)
		{
			var importer = new NtestImporter(args[0]);
		}
	}
}
