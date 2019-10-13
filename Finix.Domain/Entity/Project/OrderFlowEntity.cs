using System;

namespace Finix.Domain.Entity.Project
{
    public class OrderFlowEntity : IEntity<OrderEntity>, ICreationAudited, IModificationAudited
    {
        public string F_Id { get; set; }
         public string P_OrderId { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string P_FlowStatus { get; set; }
        public string P_Executor { get; set; }
        public string P_FlowId { get; set; }

        public string F_LastModifyUserId { get; set; }

        public DateTime? F_LastModifyTime { get; set; }
        public int P_FlowCount { get; set; }
    }
}
