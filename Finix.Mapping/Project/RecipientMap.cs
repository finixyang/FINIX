using Finix.Domain.Entity.Project;
using System.Data.Entity.ModelConfiguration;

namespace Finix.Mapping.Project
{
    public class RecipientMap : EntityTypeConfiguration<RecipientEntity>
    {
        public RecipientMap()
        {
            this.ToTable("RecipientInfo");
            this.HasKey(t => t.F_Id);
        }
    }
}
