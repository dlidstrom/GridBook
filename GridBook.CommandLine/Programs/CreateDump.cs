namespace GridBook.CommandLine.Programs
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using GridBook.Domain.Importers;
	using NDesk.Options;
	using GridBook.Domain;
	using GridBook.Service;

	public class CreateDump : ProgramBase
	{
		private string filename;
		private OptionSet options;

		public override void Run(string[] args)
		{
			options = new OptionSet()
					{
						{"f=|file=", "Creates a dump file from specified book.", v => filename = v}
					};
			options.Parse(args);
			if (filename == null)
			{
				throw new OptionSetException(options);
			}

			var importer = new NtestImporter(filename);
			var dumper = new Dumper(string.Empty);
			dumper.WriteHeading(Console.OpenStandardOutput());
			dumper.WriteBoardTable(Console.OpenStandardOutput());
			dumper.WritePositions(Console.OpenStandardOutput(), importer);
			dumper.WriteFooter(Console.OpenStandardOutput());
		}

		public override string HelpMessage()
		{
			return "Creates a MySql dump file from a book.\n"
				+ "You can import this file into a MySql database with the mysqlimport console application.";
		}

		public override string Description()
		{
			return "Creates a MySql dump file from a book.";
		}

		public override OptionSet Options
		{
			get
			{
				if (options == null)
				{
					options = new OptionSet()
					{
						{"f=|file=", "Creates a dump file from specified book.", v => filename = v}
					};
				}

				return options;
			}
		}
	}
}
