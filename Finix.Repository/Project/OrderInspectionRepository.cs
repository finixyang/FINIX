using Finix.Data;
using Finix.Domain.Entity.Project;
using Finix.Domain.IRepository.Project;

namespace Finix.Repository.Project
{
    public class OrderInspectionRepository : RepositoryBase<OrderInspectionEntity>, IOrderInspectionRepository
    {
        public int SubmitForm(OrderInspectionEntity orderInspectionEntity, string keyValue)
        {
            var result = 0;
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(orderInspectionEntity);
                }
                else
                {
                    db.Insert(orderInspectionEntity);
                }
                return db.Commit();
            }
        }

        public void CreateFlow(OrderInspectionEntity orderEntity)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                OrderFlowEntity orderFlowEntity = new OrderFlowEntity
                {
                    P_OrderId = orderEntity.F_Id,
                    //P_Executor = orderEntity.P_OrderOwner,
                    //F_LastModifyTime = orderEntity.F_CreatorTime,
                    //F_LastModifyUserId = orderEntity.P_OrderOwner,
                    //P_FlowId = orderEntity.P_OrderStatus,
                    P_FlowStatus = "0"
                };
                orderFlowEntity.Create();
                db.Insert(orderFlowEntity);
                db.Commit();
            }
        }
    }
}
