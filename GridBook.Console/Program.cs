namespace GridBook.Console
{
	using System;
	using System.IO;
	using FluentNHibernate.Cfg;
	using FluentNHibernate.Cfg.Db;
	using GridBook.Domain;
	using GridBook.Domain.Importers;
	using GridBook.Domain.Mapping;
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
		static void Main(string[] args)
		{
			try
			{
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
				Console.WriteLine(ex);
			}
		}

		void Run(string file, bool createSchema)
		{
			var sessionFactory = CreateSessionFactory("DbConnection", createSchema);

			using (var session = sessionFactory.OpenSession())
			{
				using (var transaction = session.BeginTransaction())
				{
					var name = Path.GetFileNameWithoutExtension(file);
					var importer = new NtestImporter(file);
					var book = new Book()
					{
						Name = name
					};

					foreach (var item in importer.Import())
					{
						var board = item.Key;
						book.Positions.Add(board);
					}

					session.Save(book);
					transaction.Commit();
				}
			}
		}

		private static ISessionFactory CreateSessionFactory(string connectionString, bool createSchema)
		{
			var cfg = new Configuration();
			cfg.Properties.Add("show_sql", "true");
			var builder = Fluently.Configure(cfg)
				.Database(MySQLConfiguration.Standard.ConnectionString(c => c.FromConnectionStringWithKey(connectionString)))
				.Mappings(m => m.FluentMappings.AddFromAssemblyOf<BookMap>());
			//.ExposeConfiguration(c => new SchemaExport(c).Create(true, true))
			if (createSchema)
			{
				return builder.ExposeConfiguration(c => new SchemaExport(c).Create(true, true)).BuildSessionFactory();
			}

			return builder.BuildSessionFactory();
		}
	}
}
