using FluentNHibernate.Mapping;

namespace dao_library.Sistema
{
	public class PriorityMap : ClassMap<entity_library.Priority>
	{
		public PriorityMap()
		{
			Table("priority");
			Id(x => x.Id)
				.Column("id_priority")
				.GeneratedBy.Increment();

			Map(x => x.Description)
				.Column("description");

		}
	}
}