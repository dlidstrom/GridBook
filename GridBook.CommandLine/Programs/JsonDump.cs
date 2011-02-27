namespace GridBook.CommandLine.Programs
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using NDesk.Options;
	using GridBook.Domain.Importers;

	public class JsonDump : ProgramBase
	{
		public override void Run(string[] args)
		{
			var filename = string.Empty;
			var options = new OptionSet() { { "f=|file=", "Creates Json dump from specified book.", f => filename = f } };
			options.Parse(args);
			if (string.IsNullOrWhiteSpace(filename))
			{
				throw new OptionSetException(options);
			}

			var importer = new NtestImporter(filename);
			var written = 0;
			foreach (var pos in from kvp in importer.Import()
								select kvp.Board.MinimalReflection())
			{
				Console.Write("{{ _id : \"{0}\", \"successors\" : [ ", pos.ToGuid());
				var successors = pos.CalculateMinimalSuccessors();
				Console.Write("\"{0}\"", successors.First().ToGuid());

				// add rest of successors
				for (int i = 1; i < successors.Count; i++)
				{
					Console.Write(", \"{0}\"", successors[i].ToGuid());
					if (++written % 100000 == 0)
					{
						Console.Error.WriteLine("Written {0} lines", written);
					}
				}

				Console.WriteLine(" ] }");
			}

			Console.Error.WriteLine("Wrote {0} lines", written);
		}

		public override string Description()
		{
			return "Creates Json dump from book file.";
		}

		public override OptionSet Options
		{
			get
			{
				return new OptionSet() { { "f=|file=", "Creates Json dump from specified book.", f => { } } };
			}
		}
	}
}
