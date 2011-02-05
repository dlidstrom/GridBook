namespace GridBook.CommandLine.Programs
{
	using System;
	using System.IO;

	public static class Utils
	{
		public static FileStream FindFirstSplitFile(string prefix, ref int splitId)
		{
			// try 1000 at most
			for (int i = 0; i < 1000; i++)
			{
				try
				{
					return File.Open(string.Format("{0}_{1}.bin", prefix, splitId++), FileMode.CreateNew, FileAccess.Write);
				}
				catch (IOException)
				{
				}
			}

			throw new Exception("Could not find any available split file slot.");
		}
	}
}
