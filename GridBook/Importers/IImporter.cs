namespace GridBook.Domain.Importers
{
	using System.Collections.Generic;

	public interface IImporter
	{
		IEnumerable<KeyValuePair<Board, BookData>> Import();

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
