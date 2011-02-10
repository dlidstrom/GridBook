namespace GridBook.Domain.Mapping
{
	using FluentNHibernate.Mapping;
	using GridBook.Mapping;

	public class BoardMap : ClassMap<Board>
	{
		public BoardMap()
		{
			Id(x => x.Id).GeneratedBy.Custom<CustomGuidGenerator>();
			//NaturalId().Property(x => x.Empty).Property(x => x.Mover);
			Map(x => x.Empty).Not.Nullable().Access.ReadOnlyPropertyThroughLowerCaseField();
			Map(x => x.Mover).Not.Nullable().Access.ReadOnlyPropertyThroughLowerCaseField();
			Map(x => x.Ply).Not.Nullable().Access.ReadOnlyPropertyThroughLowerCaseField();
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
				.Cascade.All()
			    .AsSet()
			    .Table("Parents");
		}
	}
}
