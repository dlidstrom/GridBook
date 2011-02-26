namespace GridBook.Mapping
{
	using FluentNHibernate.Mapping;
	using GridBook.Domain;

	public class EntryMap : ClassMap<Entry>
	{
		public EntryMap()
		{
			Id(e => e.Id);
			References(e => e.Board)
				.Access.ReadOnlyPropertyThroughLowerCaseField()
				.Not.Nullable()
				.Column("BoardId")
				.Cascade.All();
			Map(e => e.Depth)
				.Not.Nullable()
				.Access.ReadOnlyPropertyThroughLowerCaseField();
			Map(e => e.Percent)
				.Not.Nullable()
				.Access.ReadOnlyPropertyThroughLowerCaseField();
		}
	}
}
