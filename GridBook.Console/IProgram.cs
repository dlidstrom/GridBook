namespace GridBook.Console
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public interface IProgram
	{
		void Run(string[] args);
		void Help();
		string Description();
	}
}
