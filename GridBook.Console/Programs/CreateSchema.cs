namespace GridBook.Console.Programs
{
	using System;

	public class CreateSchema : IProgram
	{
		public void Run(string[] args)
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

		public void Help()
		{
			Console.WriteLine("Generates schema. This will drop data!");
		}

		public string Description()
		{
			return "Schema generator.";
		}
	}
}
