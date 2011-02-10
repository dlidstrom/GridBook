namespace GridBook.CommandLine
{
	using System;
	using NDesk.Options;

	public abstract class ProgramBase
	{
		public abstract void Run(string[] args);
		public abstract string Description();
		public virtual string HelpMessage()
		{
			return Description();
		}

		public virtual OptionSet Options
		{
			get
			{
				return null;
			}
		}

		public virtual void Usage()
		{
			if (Options != null)
			{
				Console.WriteLine("Usage: GridBook.CommandLine.exe {0} <options>", GetType().Name);
				Console.WriteLine(HelpMessage());
				Console.WriteLine("Options:");
				Options.WriteOptionDescriptions(Console.Out);
			}
			else
			{
				Console.WriteLine("Usage: GridBook.CommandLine.exe {0}", GetType().Name);
			}
		}
	}
}
