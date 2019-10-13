using Finix.Domain.Entity.Project;
using System.Data.Entity.ModelConfiguration;

namespace Finix.Mapping.Project
{
    public class OrderProductMap : EntityTypeConfiguration<OrderProductEntity>
    {
        public OrderProductMap()
        {
            this.ToTable("OrderProduct");
            this.HasKey(t => t.F_Id);
        }
    }
}
