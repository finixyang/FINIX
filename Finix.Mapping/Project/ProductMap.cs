using Finix.Domain.Entity.Project;
using System.Data.Entity.ModelConfiguration;


namespace Finix.Mapping.Project
{
    public class ProductMap : EntityTypeConfiguration<ProductEntity>
    {
        public ProductMap()
        {
            this.ToTable("ProductInfo");
            this.HasKey(t => t.F_Id);
        }
    }
}
