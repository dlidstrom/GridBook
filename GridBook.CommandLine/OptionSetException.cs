namespace GridBook.CommandLine
{
	using System;
	using NDesk.Options;

	public class OptionSetException : Exception
	{
		public OptionSetException(OptionSet set)
		{
			this.OptionSet = set;
		}

		public OptionSet OptionSet
		{
			get;
			set;
		}
	}
}
