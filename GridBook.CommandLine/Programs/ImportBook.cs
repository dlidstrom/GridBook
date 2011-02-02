namespace GridBook.CommandLine.Programs
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using GridBook.Domain.Importers;
	using NDesk.Options;

	public class ImportBook : ProgramBase
	{
		public override void Run(string[] args)
		{
			string filename = null;
			var options = new OptionSet() { { "f=|file=", "Imports file to database", v => filename = v } };
			options.Parse(args);
			if (filename == null)
			{
				throw new OptionSetException(options);
			}

			var importer = new NtestImporter(filename);
			var q = from kvp in importer.Import()
					select kvp.Key;

			using (var session = NHibernateHelper.SessionFactory.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				foreach (var pos in q)
				{
					session.Save(pos);
				}

				tx.Commit();
			}
		}

		public override string HelpMessage()
		{
			return "This tool can be used to import some data into the database.\n"
				+ "Note, however, that since parent/child relations are not set up\n"
				+ "you can really only use this program for testing purposes.";
		}

		public override string Description()
		{
			return "Imports a book into database. Does not set up parent/child relations.";
		}

		public override OptionSet Options
		{
			get
			{
				return new OptionSet() { { "f=|file=", "Imports file to database", v => { } } };
			}
		}
	}
}
