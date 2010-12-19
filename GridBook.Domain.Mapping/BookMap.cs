namespace GridBook.Domain.Mapping
{
	using System;
	using FluentNHibernate.Mapping;

	public class BookMap : ClassMap<Book>
	{
		public BookMap()
		{
			Id(x => x.Id);
			Map(x => x.Name);
			HasMany(x => x.Positions)
				//.Cascade.All()
				.Table("BookPositions")
				;
		}
	}
}
