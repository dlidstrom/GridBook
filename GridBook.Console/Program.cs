namespace GridBook.Console
{
	using System;
	using System.IO;
	using Common.Logging;
	using FluentNHibernate.Cfg;
	using FluentNHibernate.Cfg.Db;
	using GridBook.Domain.Importers;
	using GridBook.Domain.Mapping;
	using GridBook.Service;
	using NDesk.Options;
	using NHibernate;
	using NHibernate.Cfg;
	using NHibernate.Tool.hbm2ddl;

	class OptionSetException : Exception
	{
		public OptionSetException(OptionSet set)
		{
			this.OptionSet = set;
		}

		public OptionSet OptionSet
		{
			get;
			set;
		}
	}

	class Program
	{
		private static ILog log = LogManager.GetCurrentClassLogger();

		static void Main(string[] args)
		{
			try
			{
				// begin
				string file = string.Empty;
				bool createSchema = false;
				var p = new OptionSet()
				{
					{ "i=|import=", "Imports a book file into GridBook database.", v => file = v },
					{ "create-schema", "Create database schema. This will drop any existing data!", v => createSchema = true }
				};
				p.Parse(args);
				if (string.IsNullOrWhiteSpace(file))
				{
					throw new OptionSetException(p);
				}

				new Program().Run(file, createSchema);
			}
			catch (OptionSetException ex)
			{
				var writer = new StringWriter();
				ex.OptionSet.WriteOptionDescriptions(writer);
				Console.WriteLine("Usage: GridBook.Console.exe <option>");
				Console.WriteLine("Options:");
				Console.WriteLine(writer);
			}
			catch (Exception ex)
			{
				log.Error(ex.Message);
			}
		}

		void Run(string file, bool createSchema)
		{
			var sessionFactory = CreateSessionFactory("DbConnection", createSchema);

			using (var session = sessionFactory.OpenSession())
			{
				var name = Path.GetFileNameWithoutExtension(file);
				var bookService = new BookService(session);
				//bookService.AddRangeStateless(new NtestImporter(file), new ProgressBar());
				bookService.AddRangeStatefull(new NtestImporter(file), new ProgressBar());
				Console.WriteLine();
			}
		}

		private static ISessionFactory CreateSessionFactory(string connectionString, bool createSchema)
		{
			var cfg = new Configuration();
			cfg.Properties.Add("show_sql", "true");
			var builder = Fluently.Configure(cfg)
				.Database(MySQLConfiguration.Standard.ConnectionString(c => c.FromConnectionStringWithKey(connectionString)))
				.Mappings(m => m.FluentMappings.AddFromAssemblyOf<BoardMap>());
			if (createSchema)
			{
				builder.ExposeConfiguration(c => new SchemaExport(c).Create(true, true));
			}

			return builder.BuildSessionFactory();
		}
	}
}
