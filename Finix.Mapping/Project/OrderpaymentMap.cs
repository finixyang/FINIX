using Finix.Domain.Entity.Project;
using System.Data.Entity.ModelConfiguration;

namespace Finix.Mapping.Project
{
    public class OrderpaymentMap : EntityTypeConfiguration<OrderpaymentEntity>
    {
        public OrderpaymentMap()
        {
            this.ToTable("Orderpayment");
            this.HasKey(t => t.F_Id);
        }
    }
}
