﻿namespace GridBook.Console
{
	using System;
	using System.IO;
	using NDesk.Options;

	public abstract class ProgramBase
	{
		public abstract void Run(string[] args);
		public abstract string HelpMessage();

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
				Console.WriteLine("Usage: GridBook.Console.exe {0} <options>", GetType().Name);
				Console.WriteLine("Options:");
				var writer = new StringWriter();
				Options.WriteOptionDescriptions(writer);
				Console.WriteLine(writer);
			}
			else
			{
				Console.WriteLine("Usage: GridBook.Console.exe {0}", GetType().Name);
			}
		}

		public abstract string Description();
	}
}
