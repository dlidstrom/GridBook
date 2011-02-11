namespace GridBook.CommandLine.Programs
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using NDesk.Options;
	using GridBook.Domain.Importers;

	public class ParentChild : ProgramBase
	{
		public override void Run(string[] args)
		{
			var filename = string.Empty;
			var options = new OptionSet()
			{
				{ "f=|file=", "Extracts parent/child relationship from book.", f => filename = f }
			};

			options.Parse(args);

			if (string.IsNullOrWhiteSpace(filename))
			{
				throw new OptionSetException(options);
			}

			int written = 0;
			var importer = new NtestImporter(filename);
			foreach (var pos in from kvp in importer.Import()
								select kvp.Key.MinimalReflection())
			{
				foreach (var successor in pos.CalculateMinimalSuccessors())
				{
					Console.WriteLine("{0},{1}", successor.ToGuid(), pos.ToGuid());

					if (++written % 1000000 == 0)
					{
						Console.Error.WriteLine("Written {0} lines", written);
					}
				}
			}

			Console.Error.WriteLine("Wrote {0} lines", written);
		}

		public override string Description()
		{
			return "Creates parent/child relationships.";
		}

		public override string HelpMessage()
		{
			return "Parent/child relationships are written in CSV format.";
		}

		public override OptionSet Options
		{
			get
			{
				return new OptionSet()
				{
					{ "f=|file=", "Extracts parent/child relationship from book.", f => { } },
					{ "p|parent", "Writes parent relationship.", f => { } },
					{ "c|child", "Writes successor relationship.", f => { } }
				};
			}
		}
	}
}
