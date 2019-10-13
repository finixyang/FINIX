using Finix.Domain.Entity.Project;
using System.Data.Entity.ModelConfiguration;

namespace Finix.Mapping.Project
{
    public class FixedassetsMap : EntityTypeConfiguration<FixedassetsEntity>
    {
        public FixedassetsMap()
        {
            this.ToTable("AssetsInfo");
            this.HasKey(t => t.F_Id);
        }
    }
}
