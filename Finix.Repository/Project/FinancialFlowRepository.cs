using Finix.Data;
using Finix.Domain.Entity.Project;
using Finix.Domain.IRepository.Project;

namespace Finix.Repository.Project
{
    public class FinancialFlowRepository : RepositoryBase<FinancialFlowEntity>, IFinancialFlowRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<FinancialFlowEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public int SubmitForm(FinancialFlowEntity financialFlowEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(financialFlowEntity);
                }
                else
                {
                    db.Insert(financialFlowEntity);
                }
                return db.Commit();
            }
        }
    }
}