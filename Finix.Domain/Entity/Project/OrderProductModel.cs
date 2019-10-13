using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finix.Domain.Entity.Project
{
    public class OrderProductModel
    {
        public string F_Id { get; set; }

        public string P_CustomerId { get; set; }

        public string F_ParentId { get; set; }

        public string P_OrderCode { get; set; }

        public DateTime? P_OrderDate { get; set; }

        public DateTime? P_DeliveryDate { get; set; }

        public string P_OrderOwner { get; set; }

        public string P_OrderStatus { get; set; }

        public string F_CreatorUserId { get; set; }

        public DateTime? F_CreatorTime { get; set; }

        public bool? F_DeleteMark { get; set; }

        public string F_DeleteUserId { get; set; }

        public DateTime? F_DeleteTime { get; set; }

        public string F_LastModifyUserId { get; set; }

        public DateTime? F_LastModifyTime { get; set; }

        public string P_IsOutsourcing { get; set; }

        public string P_Status { get; set; }

        public Dictionary<string, List<Object>> OrderProductList { get; set; }
    }
}
