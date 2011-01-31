namespace GridBook.Console.Programs
{
	using NDesk.Options;

	public class CreateDump : ProgramBase
	{
		private string filename;

		public override void Run(string[] args)
		{
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
				return new OptionSet()
				{
					{"f=|file=", "Creates a dump file from specified book.", v => filename = v}
				};
			}
		}
	}
}
