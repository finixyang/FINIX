using Finix.Data;
using Finix.Domain.Entity.Project;

namespace Finix.Domain.IRepository.Project
{
    public interface IFinancialRepository : IRepositoryBase<FinancialEntity>
    {
        void DeleteForm(string keyValue);
        int SubmitForm(FinancialEntity financialEntity, string keyValue);

        void CreateFlow(FinancialEntity financialEntity);
    }
}
