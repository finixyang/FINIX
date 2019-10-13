using System;

namespace Finix.Domain.Entity.Project
{
    public class OrderEntity: IEntity<OrderEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }

        public string P_CustomerId { get; set; }

        public string P_OrderCode { get; set; }

        public string P_ProductId { get; set; }

        public DateTime? P_OrderDate { get; set; }

        public DateTime? P_DeliveryDate { get; set; }

        public int P_OrdreNumber { get; set; }

        public string P_OrderOwner { get; set; }

        public string P_OrderStatus { get; set; }

        public string F_CreatorUserId { get; set; }

        public DateTime? F_CreatorTime { get; set; }

        public bool? F_DeleteMark { get; set; }

        public string F_DeleteUserId { get; set; }

        public DateTime? F_DeleteTime { get; set; }

        public string F_LastModifyUserId { get; set; }

        public DateTime? F_LastModifyTime { get; set; }
        public decimal P_OrderPrice { get; set; }

        public string P_IsOutsourcing { get; set; }

        public decimal P_TotalAmount { get; set; }
        public string P_OrderUnit { get; set; }
    }
}
