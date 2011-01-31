namespace GridBook.Console.Programs
{
	using System;

	public class CreateSchema : ProgramBase
	{
		public override void Run(string[] args)
		{
			Console.WriteLine("This will drop schema! Are you sure (Y/[n])?");
			char c = (char)Console.Read();
			if (c == 'Y')
			{
				NHibernateHelper.CreateSessionFactory("DbConnection", true);
			}
			else
			{
				Console.WriteLine("No action taken.");
			}
		}

		public override string HelpMessage()
		{
			return "Generates schema. This will drop data!";
		}

		public override string Description()
		{
			return "Schema generator.";
		}
	}
}
