using Finix.Data;
using Finix.Domain.Entity.Project;
using Finix.Domain.IRepository.Project;

namespace Finix.Repository.Project
{
    public class FinancialRepository : RepositoryBase<FinancialEntity>, IFinancialRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<FinancialEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public int SubmitForm(FinancialEntity financialEntity, string keyValue)
        {
            var result = 0;
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(financialEntity);
                }
                else
                {
                    db.Insert(financialEntity);
                }
                return db.Commit();
            }
        }
        public void CreateFlow(FinancialEntity financialEntity)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                FinancialFlowEntity financialFlowEntity = new FinancialFlowEntity
                {
                    P_FinancialId = financialEntity.F_Id,
                    P_Executor = financialEntity.F_CreatorUserId,
                    //F_LastModifyTime = orderEntity.F_CreatorTime,
                    //F_LastModifyUserId = orderEntity.P_OrderOwner,
                    P_FlowId = financialEntity.P_FinancialStatus,
                    P_FlowStatus = "0"
                };
                financialFlowEntity.Create();
                db.Insert(financialFlowEntity);
                db.Commit();
            }
        }
    }
}