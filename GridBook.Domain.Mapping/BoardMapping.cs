namespace GridBook.Domain.Mapping
{
	using System;
	using System.Data.Entity.ModelConfiguration;

	public class BoardMapping : EntityTypeConfiguration<Board>
	{
		public BoardMapping()
		{
			HasKey(b => b.Id);
			//Property(b => b.Empty).HasColumnName("empty");
			//Property(b => b.Mover).HasColumnName("mover");
		}
	}
}
