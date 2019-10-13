using Finix.Data;
using Finix.Domain.Entity.Project;

namespace Finix.Domain.IRepository.Project
{
    public interface IFinancialFlowRepository: IRepositoryBase<FinancialFlowEntity>
    {
        void DeleteForm(string keyValue);
        int SubmitForm(FinancialFlowEntity financialEntity, string keyValue);
    }
}
