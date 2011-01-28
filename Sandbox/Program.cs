namespace Sandbox
{
	using System;
	using GridBook.Domain;

	public class Program
	{
		static void Main(string[] args)
		{
			var board = new Board(-4711394422865021, 4588872671356, Color.Black);
			Console.WriteLine(board);
		}
	}
}
