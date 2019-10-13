using System;

namespace Finix.Domain.Entity.Project
{
    public class OrderFlowModel
    {
        public string F_Id { get; set; }
        public string P_OrderId { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string P_FlowStatus { get; set; }
        public string P_Executor { get; set; }
        public string P_FlowId { get; set; }
        public string P_OrderCode { get; set; }
        public string P_ProductId { get; set; }
        public DateTime? P_OrderDate { get; set; }
        public DateTime? P_DeliveryDate { get; set; }
        public int P_OrdreNumber { get; set; }
        public string P_OrderOwner { get; set; }
        public string P_OrderStatus { get; set; }
        public string P_OrderOwner_next { get; set; }
        public string P_OrderStatus_next { get; set; }
        public string P_CustomerId { get; set; }
        public int P_FlowCount { get; set; }

        /// <summary>
        /// 外观检验结果
        /// </summary>
        public string P_AppearanceIinspection { get; set; }
        /// <summary>
        /// 尺寸检验
        /// </summary>
        public string P_DimensionalIinspection { get; set; }
        /// <summary>
        /// 检验数量
        /// </summary>
        public decimal P_AmountOfIinspection { get; set; }
        /// <summary>
        /// 检验比例
        /// </summary>
        public string P_InspectionProportion { get; set; }
        /// <summary>
        /// 检验结果
        /// </summary>
        public string P_InspectionResult { get; set; }
    }
}
