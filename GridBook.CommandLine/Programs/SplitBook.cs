namespace GridBook.CommandLine.Programs
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using NDesk.Options;
	using GridBook.Domain.Importers;
	using System.IO;

	public class SplitBook : ProgramBase
	{
		public override void Run(string[] args)
		{
			var filename = string.Empty;
			var options = new OptionSet() { { "f=|file=", "Creates split files from specified book.", f => filename = f } };
			options.Parse(args);
			if (string.IsNullOrWhiteSpace(filename))
			{
				throw new OptionSetException(options);
			}

			var importer = new NtestImporter(filename);
			// find first available split file
			int splitId = 0;
			FileStream stream = findFirstSplitFile(ref splitId);
			Console.WriteLine("Creating {0}", stream.Name);

			var list = new List<Guid>();
			foreach (var pos in from kvp in importer.Import()
								select kvp.Key.MinimalReflection())
			{
				// add position
				var bytes = new List<Byte>(BitConverter.GetBytes(pos.Empty));
				bytes.AddRange(BitConverter.GetBytes(pos.Mover));
				list.Add(new Guid(bytes.ToArray()));

				// add successors
				foreach (var successor in pos.CalculateMinimalSuccessors())
				{
					var b = new List<Byte>(BitConverter.GetBytes(successor.Empty));
					b.AddRange(BitConverter.GetBytes(successor.Mover));
					list.Add(new Guid(b.ToArray()));
				}

				if (list.Count > 1000000)
				{
					list.Sort();
					list.ForEach(g => stream.Write(g.ToByteArray(), 0, 16));
					stream.Close();
					stream = findFirstSplitFile(ref splitId);
					Console.WriteLine("Creating {0}", stream.Name);
					list = new List<Guid>();
				}
			}

			list.Sort();
			list.ForEach(g => stream.Write(g.ToByteArray(), 0, 16));
			stream.Close();
		}

		public override string Description()
		{
			return "Splits book into smaller, sorted pieces, ready for CreateDump.";
		}

		public override string HelpMessage()
		{
			return "Use this command to split a book before you run CreateDump.\n"
				+ "This will create a number of split files with sorted subsets of\n"
				+ "the positions, and their successors, found in the input.";
		}

		public override OptionSet Options
		{
			get
			{
				return new OptionSet() { { "f=|file=", "Creates split files from specified book.", f => { } } };
			}
		}

		private FileStream findFirstSplitFile(ref int splitId)
		{
			// try 1000 at most
			for (int i = 0; i < 1000; i++)
			{
				try
				{
					return File.Open(string.Format("split_{0}.bin", splitId++), FileMode.CreateNew, FileAccess.Write);
				}
				catch (IOException)
				{
				}
			}

			throw new Exception("Could not find any available split file slot.");
		}
	}
}
