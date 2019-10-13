using Finix.Data;
using Finix.Domain.Entity.Project;

namespace Finix.Domain.IRepository.Project
{
    public interface IBenefitsRepository : IRepositoryBase<BenefitsEntity>
    {
        void DeleteForm(string keyValue);
        int SubmitForm(BenefitsEntity benefitsEntity, string keyValue);
    }
}
