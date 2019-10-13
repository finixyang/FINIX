using Finix.Domain.Entity.Project;
using System.Data.Entity.ModelConfiguration;

namespace Finix.Mapping.Project
{
    public class ConsumableMap : EntityTypeConfiguration<ConsumableEntity>
    {
        public ConsumableMap()
        {
            this.ToTable("ConsumableInfo");
            this.HasKey(t => t.F_Id);
        }
    }
}
