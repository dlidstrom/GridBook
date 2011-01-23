namespace GridBook.Domain.Mapping
{
	using FluentNHibernate.Mapping;
	using FluentNHibernate.MappingModel.Collections;

	public class BoardMap : ClassMap<Board>
	{
		public BoardMap()
		{
			Id(x => x.Id);
			NaturalId().Property(x => x.Empty).Property(x => x.Mover);
			HasManyToMany(x => x.Successors)
				.Access.ReadOnlyPropertyThroughLowerCaseField()
				.ParentKeyColumn("ParentId")
				.ChildKeyColumn("ChildId")
				.Cascade.SaveUpdate()
				.AsSet(SortType.Unsorted)
				.Table("Successors");
			HasManyToMany(x => x.Parents)
				.Access.ReadOnlyPropertyThroughLowerCaseField()
				.ParentKeyColumn("ParentId")
				.ChildKeyColumn("ChildId")
				.Cascade.SaveUpdate()
				.AsSet(SortType.Unsorted)
				.Table("Parents");
		}
	}
}
