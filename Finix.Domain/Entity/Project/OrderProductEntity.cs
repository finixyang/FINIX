using System;

namespace Finix.Domain.Entity.Project
{
    public class OrderProductEntity : IEntity<OrderProductEntity>, ICreationAudited
    {
        public string F_Id { get; set; }
        public string P_OrderId { get; set; }
        public string P_ProductId { get; set; }
        public int P_OrdreNumber { get; set; }
        public decimal P_OrderPrice { get; set; }
        public string P_OrderUnit { get; set; }
        public decimal P_TotalAmount { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
    }
}
