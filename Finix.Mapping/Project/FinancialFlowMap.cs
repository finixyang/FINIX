using Finix.Domain.Entity.Project;
using System.Data.Entity.ModelConfiguration;

namespace Finix.Mapping.Project
{
    public class FinancialFlowMap : EntityTypeConfiguration<FinancialFlowEntity>
    {
        public FinancialFlowMap()
        {
            this.ToTable("FinancialFlow");
            this.HasKey(t => t.F_Id);
        }
    }
}