namespace GridBook.Domain.Importers
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using GridBook.Domain;

	public class NtestImporter : IImporter
	{
		private string filename;

		public NtestImporter(string filename)
		{
			this.filename = filename;
			using (var reader = new BinaryReader(File.OpenRead(filename)))
			{
				// 4
				Version = reader.ReadInt32();
				// 4
				Positions = reader.ReadInt32();
			}
		}

		public IEnumerable<KeyValuePair<Board, BookData>> Import()
		{
			using (var reader = new BinaryReader(File.OpenRead(filename)))
			{
				// 4
				Version = reader.ReadInt32();
				// 4
				Positions = reader.ReadInt32();
				foreach (var i in Enumerable.Range(0, Positions))
				{
					// 16
					var board = new Board(reader.ReadUInt64(), reader.ReadUInt64(), Color.Black);
					// 20
					var height = reader.ReadInt32();
					// 24
					var prune = reader.ReadInt32();
					// 25
					var wld = reader.ReadBoolean();
					// 26
					var knownSolve = reader.ReadBoolean();
					// 28
					var fillOut = reader.ReadInt16();
					// 30
					var cutoff = reader.ReadInt16();
					// 32
					var heuristic = reader.ReadInt16();
					// 34
					var black = reader.ReadInt16();
					// 36
					var white = reader.ReadInt16();
					// 37
					var set = reader.ReadBoolean();
					// 38
					var assigned = reader.ReadBoolean();
					// 39
					var wldSolved = reader.ReadBoolean();
					// 40
					var fill2 = reader.ReadByte();
					// 48
					var games = new int[] { reader.ReadInt32(), reader.ReadInt32() };
					// 49
					var root = reader.ReadBoolean();
					// 52
					var fill3 = reader.ReadBytes(3);
					yield return new KeyValuePair<Board, BookData>(board, new BookData()
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
					});
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
	}
}
