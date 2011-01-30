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
			Map(x => x.Ply).Not.Nullable();
			HasManyToMany(x => x.Successors)
				.Access.ReadOnlyPropertyThroughLowerCaseField()
				.ParentKeyColumn("ParentId")
				.ChildKeyColumn("ChildId")
				.Cascade.All()
				.AsSet()
				.Table("Successors");
			HasManyToMany(x => x.Parents)
			    .Access.ReadOnlyPropertyThroughLowerCaseField()
			    .ParentKeyColumn("ChildId")
			    .ChildKeyColumn("ParentId")
				.Cascade.All() // test
			    .AsSet()
			    .Table("Parents");
		}
	}
}
