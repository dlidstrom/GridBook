namespace Sandbox
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using GridBook.Domain;
	using System.Diagnostics;

	public class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Position:");
			Console.WriteLine(Board.FromGuid(new Guid("01000001-c303-ffff-fe1f-3e1c80300000")).MinimalReflection().ToGuid());
			Console.WriteLine(Board.FromGuid(new Guid("80000080-c3c0-ffff-7ff8-7c38010c0000")).MinimalReflection().ToGuid());
		}
	}
}
