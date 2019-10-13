using System;

namespace Finix.Domain.Entity.Project
{
    public class ProductEntity : IEntity<ProductEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }

        public string P_ProductName { get; set; }

        public string P_ProductDrawings { get; set; }

        public string F_CreatorUserId { get; set; }

        public DateTime? F_CreatorTime { get; set; }

        public bool? F_DeleteMark { get; set; }

        public string F_DeleteUserId { get; set; }

        public DateTime? F_DeleteTime { get; set; }

        public string F_LastModifyUserId { get; set; }

        public DateTime? F_LastModifyTime { get; set; }

        public bool P_ProductStatus { get; set; }

        public string P_DrawingNoOrModelNo { get; set; }

        public int P_InventoryQuantity { get; set; }
    }
}
