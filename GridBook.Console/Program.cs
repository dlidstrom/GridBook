﻿namespace GridBook.Console
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using GridBook.Domain.Importers;
	using GridBook.Domain;
	using Infrastructure.Data.Repository;
	using Infrastructure.Data.EntityFramework;
	using System.Data.Entity;
	using System.Data.Entity.ModelConfiguration;
	using NHibernate;
	using NHibernate.Cfg;
	using FluentNHibernate.Cfg;
	using FluentNHibernate.Cfg.Db;
	using System.IO;
	using GridBook.Domain.Mapping;
	using NHibernate.Tool.hbm2ddl;
	using NDesk.Options;

	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				string file = string.Empty;
				var p = new OptionSet() { { "i=|import=", v => file = v } };
				p.Parse(args);
				if (string.IsNullOrWhiteSpace(file))
				{
					throw new ArgumentException("Usage: GridBook.Console.exe i|import <file>");
				}

				new Program().Run(file);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		void Run(string file)
		{
			var sessionFactory = CreateSessionFactory("DbConnection");

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
						session.SaveOrUpdate(board);
						book.Positions.Add(board);
						//Console.WriteLine("Added {0}", board);
					}

					session.SaveOrUpdate(book);

					transaction.Commit();
				}
			}
		}

		private static ISessionFactory CreateSessionFactory(string connectionString)
		{
			var cfg = new Configuration();
			cfg.Properties.Add("show_sql", "true");
			return Fluently.Configure(cfg)
				.Database(MySQLConfiguration.Standard.ConnectionString(c => c.FromConnectionStringWithKey(connectionString)))
				.Mappings(m => m.FluentMappings.AddFromAssemblyOf<BookMap>())
				.ExposeConfiguration(c => new SchemaExport(c).Create(true, true))
				.BuildSessionFactory();
		}
	}
}
