﻿namespace GridBook.Console
{
	using System;
	using System.Configuration;
	using System.IO;
	using Common.Logging;
	using FluentNHibernate.Cfg;
	using FluentNHibernate.Cfg.Db;
	using GridBook.Domain.Importers;
	using GridBook.Domain.Mapping;
	using GridBook.Service;
	using NDesk.Options;
	using NHibernate;
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
				var importer = new NtestImporter(file);
				log.DebugFormat("Importing {0} positions", importer.Positions);
				bookService.AddRange(importer, 1000);
				Console.WriteLine();
			}
		}

		private static ISessionFactory CreateSessionFactory(string connectionString, bool createSchema)
		{
			log.DebugFormat("Creating session");

			var builder = Fluently.Configure()
				.Database(MySQLConfiguration.Standard.ConnectionString(c => c.FromConnectionStringWithKey(connectionString)))
				//.Database(SQLiteConfiguration.Standard.UsingFile("positions.db"))
				.Mappings(m => m.FluentMappings.AddFromAssemblyOf<BoardMap>())
				.ExposeConfiguration(c =>
				{
					if (createSchema)
					{
						if (File.Exists("positions.db"))
							File.Delete("positions.db");
						new SchemaExport(c).Create(true, true);
					}
				});

			return builder.BuildSessionFactory();
		}
	}
}
