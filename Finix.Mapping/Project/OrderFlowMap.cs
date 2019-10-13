using Finix.Domain.Entity.Project;
using System.Data.Entity.ModelConfiguration;

namespace Finix.Mapping.Project
{
    public class OrderFlowMap : EntityTypeConfiguration<OrderFlowEntity>
    {
        public OrderFlowMap()
        {
            this.ToTable("OrderFlow");
            this.HasKey(t => t.F_Id);
        }
    }
}