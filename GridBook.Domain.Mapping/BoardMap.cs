namespace GridBook.Domain.Mapping
{
	using System;
	using FluentNHibernate.Mapping;

	public class BoardMap : ClassMap<Board>
	{
		public BoardMap()
		{
			Id(x => x.Id);
			Map(x => x.Empty);
			Map(x => x.Mover);
		}
	}
}
