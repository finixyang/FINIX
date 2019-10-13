using System;

namespace Finix.Domain.Entity.Project
{
    public class OrderpaymentEntity : IEntity<OrderpaymentEntity>, ICreationAudited
    {
        public string F_Id { get; set; }

        public string P_OrderId { get; set; }

        public decimal P_Amount { get; set; }

        public bool F_DeleteMark { get; set; }

        public bool F_EnabledMark { get; set; }

        public string F_Description { get; set; }

        public string F_CreatorUserId { get; set; }

        public DateTime? F_CreatorTime { get; set; }
    }
}
