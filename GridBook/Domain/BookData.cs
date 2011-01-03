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
			builder.Append("Height: " + Height);
			builder.Append("Prune: " + Prune);
			builder.Append("WLD?: " + (WLD ? "Yes" : "No"));
			return builder.ToString();
		}
	}
}
