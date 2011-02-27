namespace GridBook.CommandLine
{
	using System;
	using NDesk.Options;
using System.Runtime.Serialization;

	public class OptionSetException : Exception
	{
		protected OptionSetException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public OptionSetException()
		{
		}

		public OptionSetException(string message)
			: base(message)
		{
		}

		public OptionSetException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public OptionSetException(OptionSet set)
		{
			this.OptionSet = set;
		}

		public OptionSet OptionSet
		{
			get;
			private set;
		}
	}
}
