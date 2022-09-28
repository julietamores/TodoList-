using FluentNHibernate.Mapping;

namespace dao_library.Sistema
{
	public class CategoryMap : ClassMap<entity_library.Category>
	{
		public CategoryMap()
		{
			Table("category");
			Id(x => x.Id)
				.Column("id_category")
				.GeneratedBy.Increment();

			Map(x => x.Description)
				.Column("description");

			Map(x => x.Color)
				.Column("color");

		}
	}
}