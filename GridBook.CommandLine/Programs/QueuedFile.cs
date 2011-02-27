namespace GridBook.CommandLine.Programs
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using GridBook.Domain;

	/// <summary>
	/// Represents a file as a queue.
	/// </summary>
	public class QueuedFile : IDisposable
	{
		private const int QueueCapacity = 1000;
		private BinaryReader reader;
		private Queue<Board> queue = new Queue<Board>(QueueCapacity);

		/// <summary>
		/// Initializes a new instance of the FileQueue class.
		/// </summary>
		/// <param name="stream">Stream to read from.</param>
		public QueuedFile(Stream stream)
		{
			reader = new BinaryReader(stream);
			loadQueue();
		}

		/// <summary>
		/// Gets the number of items in the queue.
		/// </summary>
		public int Count
		{
			get
			{
				return queue.Count;
			}
		}

		/// <summary>
		/// Returns the next item in the queue.
		/// </summary>
		/// <returns>Next item in queue.</returns>
		public Board Peek()
		{
			return queue.Peek();
		}

		/// <summary>
		/// Removes the next item from the queue.
		/// </summary>
		public void Dequeue()
		{
			try
			{
				queue.Dequeue();
				if (queue.Count == 0)
				{
					loadQueue();
				}
			}
			catch (InvalidOperationException)
			{
			}
		}

		/// <summary>
		/// Closes the queue and the underlying stream.
		/// </summary>
		public void Close()
		{
			reader.Close();
		}

		private void loadQueue()
		{
			for (int i = 0; i < QueueCapacity; i++)
			{
				var bytes = reader.ReadBytes(16);
				if (bytes.Length == 0)
				{
					break;
				}

				var empty = BitConverter.ToInt64(bytes, 0);
				var mover = BitConverter.ToInt64(bytes, 8);
				queue.Enqueue(new Board(empty, mover, Color.Black));
			}
		}

		public void Dispose()
		{
			reader.Dispose();
		}
	}
}
