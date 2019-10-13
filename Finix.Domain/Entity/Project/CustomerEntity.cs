using System;

namespace Finix.Domain.Entity.Project
{
    public class CustomerEntity : IEntity<ProductEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }

        public string P_CustomerName { get; set; }

        public int P_CustomerLevel { get; set; }

        public string P_CustomerCode { get; set; }

        public string P_CustomerMark { get; set; }

        public bool P_CustomerStatus { get; set; }

        public string F_CreatorUserId { get; set; }

        public DateTime? F_CreatorTime { get; set; }

        public bool? F_DeleteMark { get; set; }

        public string F_DeleteUserId { get; set; }

        public DateTime? F_DeleteTime { get; set; }

        public string F_LastModifyUserId { get; set; }

        public DateTime? F_LastModifyTime { get; set; }
    }
}
