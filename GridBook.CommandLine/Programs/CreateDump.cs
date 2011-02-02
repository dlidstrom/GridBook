namespace GridBook.CommandLine.Programs
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using GridBook.Domain.Importers;
	using NDesk.Options;
	using GridBook.Domain;

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
			// for each ply
			for (int ply = 0; ply < 60; ply++)
			{
				// for each position, add its successors to set
				var set = new HashSet<Guid>();
				var q = from kvp in importer.Import()
						where kvp.Key.Ply == ply
						select kvp.Key;
				foreach (var pos in q)
				{
					var bytes = new List<Byte>(BitConverter.GetBytes(pos.Empty));
					bytes.AddRange(BitConverter.GetBytes(pos.Mover));
					set.Add(new Guid(bytes.ToArray()));
				}

				// print sql-create statements
				if (set.Count > 0)
				{
					Console.WriteLine("INSERT INTO `board` VALUES ");
					int count = set.Count;
					foreach (var item in set)
					{
						var bytes = item.ToByteArray();
						var empty = BitConverter.ToInt64(bytes, 0);
						var mover = BitConverter.ToInt64(bytes, 8);
						Console.Write("('{0}',{1},{2},{3})", item, empty, mover, ply);
						if (--count > 0)
						{
							Console.WriteLine(",");
						}
					}

					Console.WriteLine(";");
				}
			}
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
