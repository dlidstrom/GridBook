namespace GridBook.Domain.Importers
{
	using System.Collections.Generic;

	public interface IImporter
	{
		IEnumerable<Entry> Import();

		int Version
		{
			get;
		}

		int Positions
		{
			get;
		}
	}
}
