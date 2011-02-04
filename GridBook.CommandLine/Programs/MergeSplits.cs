namespace GridBook.CommandLine.Programs
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.IO;

	public class MergeSplits : ProgramBase
	{
		public override void Run(string[] args)
		{
			var files = new List<string>(Directory.EnumerateFiles(@".\", "split*.bin"));
		}

		public override string Description()
		{
			return "Merge the split files in the current directory.";
		}

		public override string HelpMessage()
		{
			return "Will output csv from split files.";
		}
	}
}
