using Finix.Domain.Entity.Project;
using System.Data.Entity.ModelConfiguration;

namespace Finix.Mapping.Project
{
    public class FinancialMap : EntityTypeConfiguration<FinancialEntity>
    {
        public FinancialMap()
        {
            this.ToTable("FinancialInfo");
            this.HasKey(t => t.F_Id);
        }
    }
}