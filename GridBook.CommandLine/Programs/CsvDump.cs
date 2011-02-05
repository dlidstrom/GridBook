namespace GridBook.CommandLine.Programs
{
	using System;
	using System.IO;
	using GridBook.Domain;
	using NDesk.Options;

	public class CsvDump : ProgramBase
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

			var written = 0;
			using (var stream = File.Open(filename, FileMode.Open, FileAccess.Read))
			using (var reader = new BinaryReader(stream))
			{
				var bytes = reader.ReadBytes(16);
				while (bytes.Length > 0)
				{
					var g = new Guid(bytes);
					var board = Board.FromGuid(g);
					Console.WriteLine("'{0}',{1},{2},{3}", g, board.Empty, board.Mover, board.Ply);
					bytes = reader.ReadBytes(16);
					if (++written % 1000000 == 0)
					{
						Console.Error.WriteLine("Written {0} lines", written);
					}
				}
			}
		}

		public override string Description()
		{
			return "Creates CSV dump from split file.";
		}

		public override string HelpMessage()
		{
			return "Creates CSV dump from split file.";
		}

		public override OptionSet Options
		{
			get
			{
				return new OptionSet() { { "f=|file=", "Creates CSV dump from specified book.", f => { } } };
			}
		}
	}
}
