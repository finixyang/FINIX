using Finix.Data;
using Finix.Domain.Entity.Project;
using Finix.Domain.IRepository.Project;

namespace Finix.Repository.Project
{
    public class OrderRepository : RepositoryBase<OrderEntity>, IOrderRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<OrderEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public int SubmitForm(OrderEntity orderEntity, string keyValue)
        {
            var result = 0;
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(orderEntity);
                }
                else
                {
                    db.Insert(orderEntity);
                }
                return db.Commit();
            }
        }

        public void CreateFlow(OrderEntity orderEntity)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                OrderFlowEntity orderFlowEntity = new OrderFlowEntity
                {
                    P_OrderId = orderEntity.F_Id,
                    P_Executor = orderEntity.P_OrderOwner,
                    F_LastModifyTime = orderEntity.F_CreatorTime,
                    F_LastModifyUserId = orderEntity.P_OrderOwner,
                    P_FlowId = orderEntity.P_OrderStatus,
                    P_FlowCount = orderEntity.P_OrdreNumber,
                    P_FlowStatus = "0"
                };
                orderFlowEntity.Create();
                db.Insert(orderFlowEntity);
                db.Commit();
            }
        }
    }
}
