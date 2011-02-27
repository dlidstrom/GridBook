namespace GridBook.Domain
{
	using System;
	using System.Text;

	public class BookData
	{
		public int Height
		{
			get;
			set;
		}

		public int Prune
		{
			get;
			set;
		}

		public bool WLD
		{
			get;
			set;
		}

		public bool KnownSolve
		{
			get;
			set;
		}

		public short Cutoff
		{
			get;
			set;
		}

		public short HeuristicValue
		{
			get;
			set;
		}

		public short BlackValue
		{
			get;
			set;
		}

		public short WhiteValue
		{
			get;
			set;
		}

		public int[] Games
		{
			get;
			set;
		}

		public string HumanString()
		{
			var builder = new StringBuilder();
			builder.AppendFormat("Height: {0}", Height);
			builder.AppendFormat("Prune: {0}", Prune);
			builder.AppendFormat("WLD?: {0}", WLD ? "Yes" : "No");
			return builder.ToString();
		}
	}
}
