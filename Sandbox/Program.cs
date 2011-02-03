namespace Sandbox
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class Comparer : IEqualityComparer<Guid>
	{
		public bool Equals(Guid x, Guid y)
		{
			return x.Equals(y);
		}

		public int GetHashCode(Guid obj)
		{
			return obj.GetHashCode();
		}
	}

	public class Program
	{
		static void Main(string[] args)
		{
			{
				//int n_positions = 1 << num;
				Console.ReadKey();
				int n_positions = 29 * 1000 * 1000;
				Console.WriteLine("Creating {0} positions...", n_positions);
				var set = new C5.HashSet<Guid>(n_positions, new Comparer());
				foreach (var i in Enumerable.Range(0, n_positions))
				{
					set.Add(Guid.NewGuid());
				}

				Console.WriteLine("Creation complete. Count = {0}", set.Count);
				Console.WriteLine("Moving to list...");
				var list = new List<Guid>();
				while (set.Count > 0)
				{
					var first = set.First();
					set.Remove(first);
					list.Add(first);
				}
				Console.WriteLine("Done. Sorting...");
				list.Sort();
				Console.WriteLine("Sorting done.");
				Console.ReadKey();
			}
		}
	}
}
