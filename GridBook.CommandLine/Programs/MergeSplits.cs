namespace GridBook.CommandLine.Programs
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using NDesk.Options;

	public class MergeSplits : ProgramBase
	{
		const int Take = 2;

		public override void Run(string[] args)
		{
			var filenames = string.Empty;
			var options = new OptionSet() { { "f=|files=", "Merges the specified split files.", f => filenames = f } };
			options.Parse(args);
			if (string.IsNullOrWhiteSpace(filenames))
			{
				throw new OptionSetException(options);
			}

			var splitFiles = new HashSet<string>(Directory.EnumerateFiles(@".\", filenames));
			var workFiles = new List<string>(splitFiles.Take(splitFiles.Count - Take >= Take ? Take : splitFiles.Count));
			splitFiles.RemoveWhere(s => workFiles.Contains(s));
			int splitId = 0;
			bool firstRun = true;
			var createdFiles = new HashSet<string>();

			while (workFiles.Count > 1)
			{
				using (var stream = Utils.FindFirstSplitFile("merge", ref splitId))
				{
					createdFiles.Add(stream.Name);
					Console.WriteLine("Merging {0} into {1}", string.Join(",", from f in workFiles
																			   select Path.GetFileName(f)),
																			   Path.GetFileName(stream.Name));
					var multiQueue = new MultiQueue(workFiles);
					int written = 0;
					foreach (var item in multiQueue.Merge())
					{
						stream.Write(item.ToGuid().ToByteArray(), 0, 16);
						if (++written % 1000000 == 0)
						{
							Console.WriteLine("Written {0} lines", written);
						}
					}

					Console.WriteLine("Written {0} lines", written);
					Console.WriteLine("Excluded {0} duplicates.", multiQueue.Duplicates);
					if (!firstRun)
					{
						workFiles.ForEach(f => File.Delete(f));
					}

					workFiles = new List<string>(splitFiles.Take(splitFiles.Count - Take >= Take ? Take : splitFiles.Count));
					splitFiles.RemoveWhere(s => workFiles.Contains(s));
					if (workFiles.Count < Take)
					{
						splitFiles = createdFiles;
						createdFiles = new HashSet<string>();
						workFiles = new List<string>(splitFiles.Take(Take));
						splitFiles.RemoveWhere(s => workFiles.Contains(s));
						firstRun = false;
					}
				}
			}
		}

		public override string Description()
		{
			return "Merge the split files in the current directory.";
		}

		public override string HelpMessage()
		{
			return "Will output csv from split files.";
		}

		public override OptionSet Options
		{
			get
			{
				return new OptionSet() { { "f=|files=", "Merges the specified split files.", f => { } } };
			}
		}
	}
}
