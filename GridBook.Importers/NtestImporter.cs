namespace GridBook.Test
{
	using System.IO;
	using System.Linq;
	using System.Collections.Generic;
	using GridBook.Domain;

	public class NtestImporter
	{
		public NtestImporter(string filename)
		{
			Entries = new Dictionary<Board, BookData>();

			using (var reader = new BinaryReader(File.OpenRead(filename)))
			{
				Version = reader.ReadInt32();
				Positions = reader.ReadInt32();
				foreach (var i in Enumerable.Range(0, Positions))
				{
					var empty = reader.ReadUInt64();
					var mover = reader.ReadUInt64();
					var board = new Board(empty, mover);
					var height = reader.ReadInt32();
					var prune = reader.ReadInt32();
					var wld = reader.ReadBoolean();
					var knownSolve = reader.ReadBoolean();
					var fillOut = reader.ReadInt16();
					var cutoff = reader.ReadInt16();
					var heuristic = reader.ReadInt16();
					var black = reader.ReadInt16();
					var white = reader.ReadInt16();
					var set = reader.ReadBoolean();
					var assigned = reader.ReadBoolean();
					var wldSolved = reader.ReadBoolean();
					var fill2 = reader.ReadByte();
					var games = new int[] { reader.ReadInt32(), reader.ReadInt32() };
					var root = reader.ReadBoolean();
					var fill3 = reader.ReadBytes(3);
					Entries[board] = new BookData()
					{
						Height = height,
						Prune = prune,
						WLD = wld,
						KnownSolve = knownSolve,
						Cutoff = cutoff,
						HeuristicValue = heuristic,
						BlackValue = black,
						WhiteValue = white,
						Games = games
					};
				}
			}
		}

		public int Version
		{
			get;
			private set;
		}

		public int Positions
		{
			get;
			private set;
		}

		public IDictionary<Board, BookData> Entries
		{
			get;
			private set;
		}
	}
}
