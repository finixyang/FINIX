using System;

namespace Finix.Domain.Entity.Project
{
    public class FinancialEntity : IEntity<FinancialEntity>, ICreationAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string P_Description { get; set; }
        public decimal P_TotalAmount { get; set; }
        public string P_FinancialType { get; set; }
        public bool? F_DeleteMark { get; set; }
        public bool? F_EnabledMark { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
        public string P_FinancialStatus { get; set; }
    }
}
