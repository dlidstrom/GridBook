namespace GridBook.CommandLine
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
					var programs = container.ResolveAll<ProgramBase>();
					if (args[0] == "help")
					{
						var program = container.Resolve<ProgramBase>(args[1]);
						program.Usage();
					}
					else
					{
						var programList = (from program in programs
										   where program.GetType().Name.StartsWith(args[0])
										   select program).ToList();
						if (programList.Count > 1)
						{
							Console.WriteLine("Command '{0}' is ambiguous:", args[0]);
							Console.WriteLine("    {0}", string.Join(" ", from p in programList
																		  select p.GetType().Name));
						}
						else
						{
							var subArgs = new List<string>(args);
							subArgs.RemoveAt(0);
							var program = programList.First();
							program.Run(subArgs.ToArray());
						}
					}
				}
				catch (OptionSetException)
				{
					Usage(container);
				}
				catch (ComponentNotFoundException)
				{
					Usage(container);
				}
				catch (IndexOutOfRangeException)
				{
					Usage(container);
				}
				catch (InvalidOperationException)
				{
					Usage(container);
				}
			}
		}

		private static void Usage(IWindsorContainer container)
		{
			var programs = container.ResolveAll<ProgramBase>();
			var q = from p in programs
					select p.GetType().Name;
			Console.Error.WriteLine("Usage: GridBook.CommandLine {0}", string.Join("|", q));
			Console.Error.WriteLine("For more help, try GridBook.CommandLine help <program>");
			Console.Error.WriteLine();
			Console.Error.WriteLine("Descriptions:");
			foreach (var program in programs)
			{
				Console.Error.WriteLine("   {0,-30} {1}", program.GetType().Name, program.Description());
			}
		}

		static void Main(string[] args)
		{
			try
			{
				new MainProgram().Run(args);
			}
			catch (Exception ex)
			{
				log.Error(ex.Message);
				Console.Error.WriteLine(ex);
			}
		}
	}
}
