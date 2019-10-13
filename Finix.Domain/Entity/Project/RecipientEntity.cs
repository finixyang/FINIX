using System;

namespace Finix.Domain.Entity.Project
{
    public class RecipientEntity : IEntity<RecipientEntity>, ICreationAudited
    {
        public string F_Id { get; set; }
        public string P_TypeId { get; set; }
        public string P_ItemId { get; set; }
        public int P_RecipientNum { get; set; }
        public string P_RecipientOwner { get; set; }
        public string P_Discription { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public string P_AssetModel { get; set; }
    }
}
