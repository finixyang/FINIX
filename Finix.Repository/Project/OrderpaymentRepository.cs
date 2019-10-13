using Finix.Data;
using Finix.Domain.Entity.Project;
using Finix.Domain.IRepository.Project;

namespace Finix.Repository.Project
{
    public class OrderpaymentRepository : RepositoryBase<OrderpaymentEntity>, IOrderpaymentRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<OrderpaymentEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public int SubmitForm(OrderpaymentEntity orderpaymentEntity, string keyValue)
        {
            var result = 0;
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(orderpaymentEntity);
                }
                else
                {
                    db.Insert(orderpaymentEntity);
                }
                return db.Commit();
            }
        }

        public void CreateOrderpayment(OrderpaymentEntity orderpaymentEntity)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                orderpaymentEntity.Create();
                db.Insert(orderpaymentEntity);
                db.Commit();
            }
        }
    }
}
