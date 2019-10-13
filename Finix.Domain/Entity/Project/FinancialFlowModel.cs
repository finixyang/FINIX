using System;

namespace Finix.Domain.Entity.Project
{
    public class FinancialFlowModel
    {
        public string F_Id { get; set; }
        public string P_FinancialId { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string P_FlowStatus { get; set; }
        public string P_Executor { get; set; }
        public string P_FlowId { get; set; }
        public string P_FinancialStatus { get; set; }
        public string P_Description { get; set; }
        public decimal P_TotalAmount { get; set; }
        public string P_FinancialType { get; set; }
        public string P_FinancialStatus_next { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }



    }
}
