using Finix.Data;
using Finix.Domain.Entity.Project;
using Finix.Domain.IRepository.Project;

namespace Finix.Repository.Project
{
    public class OrderFlowRepository : RepositoryBase<OrderFlowEntity>, IOrderFlowRepository
    {
        public int DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<OrderFlowEntity>(t => t.F_Id == keyValue);
                return db.Commit();
            }
        }
        public int SubmitForm(OrderFlowEntity orderFlowEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(orderFlowEntity);
                }
                else
                {
                    db.Insert(orderFlowEntity);
                }
                return db.Commit();
            }
        }
    }
}
