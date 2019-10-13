using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finix.Domain.Entity.Project
{
    public class RecipientModel
    {
        public string F_Id { get; set; }
        public string P_TypeId { get; set; }
        public string P_TypeName { get; set; }
        public string P_ItemId { get; set; }
        public string P_ItemName { get; set; }
        public int P_RecipientNum { get; set; }
        public string P_RecipientOwner { get; set; }
        public string P_Discription { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public string P_AssetModel { get; set; }
    }
}
