using Finix.Domain.Entity.Project;
using System.Data.Entity.ModelConfiguration;


namespace Finix.Mapping.Project
{
    public class CustomerMap : EntityTypeConfiguration<CustomerEntity>
    {
        public CustomerMap()
        {
            this.ToTable("CustomerInfo");
            this.HasKey(t => t.F_Id);
        }
    }
}
