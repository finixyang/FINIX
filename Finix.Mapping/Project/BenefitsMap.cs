using Finix.Domain.Entity.Project;
using System.Data.Entity.ModelConfiguration;

namespace Finix.Mapping.Project
{
    public class BenefitsMap : EntityTypeConfiguration<BenefitsEntity>
    {
        public BenefitsMap()
        {
            this.ToTable("BenefitsInfo");
            this.HasKey(t => t.F_Id);
        }
    }
}
