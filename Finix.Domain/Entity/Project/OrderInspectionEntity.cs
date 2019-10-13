using System;

namespace Finix.Domain.Entity.Project
{
    public class OrderInspectionEntity : IEntity<OrderInspectionEntity>, ICreationAudited, IModificationAudited
    {
        public string F_Id { get; set; }

        /// <summary>
        /// 订单产品关系ID
        /// </summary>
        public string F_OPId { get; set; }
        public string P_FlowId { get; set; }
        public string P_OrderId { get; set; }
        public string P_ProductId { get; set; }
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
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
    }
}
