namespace GridBook.CommandLine.Programs
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using GridBook.Domain;

	public class MultiQueue
	{
		List<QueuedFile> queues;

		public MultiQueue(IEnumerable<string> files)
		{
			var q = from file in files
					select new QueuedFile(new FileStream(file, FileMode.Open, FileAccess.Read));
			queues = new List<QueuedFile>(q.ToArray());
		}

		public int Duplicates
		{
			get;
			private set;
		}

		public IEnumerable<Board> Merge()
		{
			Board last = null;
			while (queues.Count > 0)
			{
				var board = queues.First().Peek();
				var smallest = 0;

				for (int i = 1; i < queues.Count; i++)
				{
					var b = queues[i].Peek();
					if (b < board)
					{
						board = b;
						smallest = i;
					}
				}

				if (!board.Equals(last))
				{
					yield return board;
					last = board;
				}
				else
				{
					Duplicates++;
				}

				queues[smallest].Dequeue();
				if (queues[smallest].Count == 0)
				{
					queues[smallest].Close();
					queues.RemoveAt(smallest);
				}
			}
		}
	}
}
