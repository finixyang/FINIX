﻿using System;
using System.Collections.Generic;

namespace Finix.Domain.Entity.Project
{
    public class ConsumableList : IEntity<ConsumableList>, ICreationAudited, IModificationAudited, IDeleteAudited
    {
        public string F_Id { get; set; }
        public string F_ParentId { get; set; }
        public string P_TypeId { get; set; }
        public string P_AssetModel { get; set; }
        public string P_PayType { get; set; }
        public int P_AssetNumber { get; set; }
        public decimal P_AssetPrice { get; set; }

        public int P_AssetToaltNum { get; set; }
        public decimal P_AssetAmount { get; set; }
        public string F_Description { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
        public bool? F_DeleteMark { get; set; }
        public string P_AssetName { get; set; }
        public int P_RemNum { get; set; }
        public string P_Owner { get; set; }

        public List<ConsumableEntity> childList { get; set; }
    }
}
