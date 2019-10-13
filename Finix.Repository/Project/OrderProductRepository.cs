using Finix.Data;
using Finix.Domain.Entity.Project;
using Finix.Domain.IRepository.Project;

namespace Finix.Repository.Project
{
    public class OrderProductRepository : RepositoryBase<OrderProductEntity>, IOrderProductRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<OrderProductEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public int SubmitForm(OrderProductEntity orderProductEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(orderProductEntity);
                }
                else
                {
                    db.Insert(orderProductEntity);
                }
                return db.Commit();
            }
        }
    }
}
