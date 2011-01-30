namespace GridBook.Console
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Castle.MicroKernel;
	using Castle.Windsor;
	using Castle.Windsor.Installer;
	using Common.Logging;

	public class MainProgram
	{
		private static ILog log = LogManager.GetCurrentClassLogger();

		void Run(string[] args)
		{
			using (var container = new WindsorContainer())
			{
				try
				{
					container.Install(FromAssembly.This());
					if (args[0] == "help")
					{
						var program = container.Resolve<IProgram>(args[1]);
						program.Help();
					}
					else
					{
						var program = container.Resolve<IProgram>(args[0]);
						var subArgs = new List<string>(args);
						subArgs.RemoveAt(0);
						program.Run(subArgs.ToArray());
					}
				}
				catch (ComponentNotFoundException ex)
				{
					Usage(container, ex);
				}
				catch (IndexOutOfRangeException ex)
				{
					Usage(container, ex);
				}
			}
		}

		void Usage(IWindsorContainer container, Exception ex)
		{
			var programs = container.ResolveAll<IProgram>();
			var q = from p in programs
					select p.GetType().Name;
			Console.WriteLine("Usage: GridBook.Console {0}", string.Join("|", q));
			Console.WriteLine("For more help, try GridBook.Console help <program>");
			Console.WriteLine();
			Console.WriteLine("Descriptions:");
			foreach (var program in programs)
			{
				Console.WriteLine("   {0,-30} {1}", program.GetType().Name, program.Description());
			}
		}

		static void Main(string[] args)
		{
			try
			{
				new MainProgram().Run(args);
				//    // begin
				//    string file = string.Empty;
				//    bool createSchema = false;
				//    var p = new OptionSet()
				//    {
				//        { "i=|import=", "Imports a book file into GridBook database.", v => file = v },
				//        { "create-schema", "Create database schema. This will drop any existing data!", v => createSchema = true }
				//    };
				//    p.Parse(args);
				//    if (string.IsNullOrWhiteSpace(file))
				//    {
				//        throw new OptionSetException(p);
				//    }

				//    new MainProgram().Run(file, createSchema);
				//}
				//catch (OptionSetException ex)
				//{
				//    var writer = new StringWriter();
				//    ex.OptionSet.WriteOptionDescriptions(writer);
				//    Console.WriteLine("Usage: GridBook.Console.exe <option>");
				//    Console.WriteLine("Options:");
				//    Console.WriteLine(writer);
				//}
			}
			catch (Exception ex)
			{
				log.Error(ex.Message);
			}
		}
	}
}
