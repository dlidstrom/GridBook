namespace GridBook.Console
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using GridBook.Test;

	class Program
	{
		static void Main(string[] args)
		{
			try
			{
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
			Console.WriteLine(importer.Entries[Board.Start.Play("D3")].HumanString());
		}
	}
}
