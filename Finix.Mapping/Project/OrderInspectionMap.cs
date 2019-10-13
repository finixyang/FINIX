using Finix.Domain.Entity.Project;
using System.Data.Entity.ModelConfiguration;

namespace Finix.Mapping.Project
{
    public class OrderInspectionMap : EntityTypeConfiguration<OrderInspectionEntity>
    {
        public OrderInspectionMap()
        {
            this.ToTable("OrderInspection");
            this.HasKey(t => t.F_Id);
        }
    }
}
