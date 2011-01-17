namespace GridBook.Domain.Mapping
{
	using FluentNHibernate.Mapping;

	public class BoardMap : ClassMap<Board>
	{
		public BoardMap()
		{
			Id(x => x.Id);
			//Map(x => x.Empty);//.Index("idx_board_empty");
			//Map(x => x.Mover);//.Index("idx_board_mover");
			NaturalId().Property(x => x.Empty).Property(x => x.Mover);
		}
	}
}
