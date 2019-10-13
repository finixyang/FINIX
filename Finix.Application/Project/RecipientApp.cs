using Finix.Code;
using Finix.Domain.Entity.Project;
using Finix.Domain.IRepository.Project;
using Finix.Repository.Project;
using System.Collections.Generic;
using System.Linq;
using System;
using Finix.Application.SystemManage;
using Finix.Domain.Entity.SystemManage;

namespace Finix.Application.Project
{
    public class RecipientApp
    {
        private IConsumableRepository consumableService = new ConsumableRepository();
        private IFixedassetsRepository fixedassetsService = new FixedassetsRepository();
        private IRecipientRepository service = new RecipientRepository();
        public List<RecipientEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<RecipientEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.P_ItemId.Contains(keyword));
            }
            return service.FindList(expression, pagination);
        }

        public List<RecipientModel> SetRecipientInfo(List<RecipientEntity> recipientInfo)
        {
            List<RecipientModel> list = new List<RecipientModel>();
            var assetsInfo = this.GetAssetsInfo();
            foreach (var item in recipientInfo)
            {
                var recipient = new RecipientModel();
                assetsInfo.TryGetValue(item.P_ItemId, out recipient);
                if (recipient != null)
                {
                    list.Add(new RecipientModel()
                    {
                        F_Id = item.F_Id,
                        F_CreatorTime = item.F_CreatorTime,
                        F_CreatorUserId = item.F_CreatorUserId,
                        P_Discription = item.P_Discription,
                        P_ItemId = item.P_ItemId,
                        P_ItemName = recipient.P_ItemName,
                        P_RecipientNum = item.P_RecipientNum,
                        P_RecipientOwner = item.P_RecipientOwner,
                        P_TypeId = item.P_TypeId,
                        P_TypeName = recipient.P_TypeName,
                        P_AssetModel = item.P_AssetModel
                    });
                }
            }
            return list;
        }

        private Dictionary<string, RecipientModel> GetAssetsInfo()
        {
            Dictionary<string, RecipientModel> dic = new Dictionary<string, RecipientModel>();
            var fixedAssetsInfo = fixedassetsService.IQueryable().Where(f => f.F_DeleteMark != true).ToList();
            var consumableInfo = consumableService.IQueryable().Where(f => f.F_DeleteMark != true).ToList();
            var itemsInfo = new ItemsDetailApp().GetList();
            foreach (var fixedItem in fixedAssetsInfo)
            {
                dic.Add(fixedItem.F_Id, new RecipientModel()
                {
                    P_TypeId = fixedItem.F_Id,
                    P_TypeName = itemsInfo.Where(x => x.F_Id == fixedItem.P_TypeId).SingleOrDefault().F_ItemName,
                    P_ItemName = fixedItem.P_AssetName
                });
            }
            foreach (var consumableItem in consumableInfo)
            {
                dic.Add(consumableItem.F_Id, new RecipientModel()
                {
                    P_TypeId = consumableItem.F_Id,
                    P_TypeName = itemsInfo.Where(x => x.F_Id == consumableItem.P_TypeId).SingleOrDefault().F_ItemName,
                    P_ItemName = consumableItem.P_AssetName
                });
            }
            return dic;
        }

        public void DeleteForm(string keyValue)
        {
            //删除的同时归还借用数量
            service.DeleteForm(keyValue);
        }

        public Object GetRecipientInfo(string keyword, string flag)
        {
            if (flag == "Fixedassets")
            {
                var fixedAssetsInfo = fixedassetsService.IQueryable().Where(f => f.F_Id == keyword).SingleOrDefault();
                var itemInfo = new ItemsDetailApp().GetForm(fixedAssetsInfo.P_TypeId);
                var recipientInfo = new RecipientModel();
                recipientInfo.P_ItemId = fixedAssetsInfo.F_Id;
                recipientInfo.P_ItemName = fixedAssetsInfo.P_AssetName;
                recipientInfo.P_TypeId = itemInfo.F_Id;
                recipientInfo.P_TypeName = itemInfo.F_ItemName;
                recipientInfo.P_AssetModel = fixedAssetsInfo.P_AssetModel;
                return recipientInfo;
            }
            if (flag == "Consumable")
            {
                var consumableInfo = consumableService.IQueryable().Where(f => f.F_Id == keyword).SingleOrDefault();
                var itemInfo = new ItemsDetailApp().GetForm(consumableInfo.P_TypeId);
                var recipientInfo = new RecipientModel();
                recipientInfo.P_ItemId = consumableInfo.F_Id;
                recipientInfo.P_ItemName = consumableInfo.P_AssetName;
                recipientInfo.P_TypeId = itemInfo.F_Id;
                recipientInfo.P_TypeName = itemInfo.F_ItemName;
                recipientInfo.P_AssetModel = consumableInfo.P_AssetModel;
                return recipientInfo;
            }
            return new Object() { };
        }

        public bool CheckNum(RecipientModel recipientModel)
        {
            var fixedassetsNum = fixedassetsService.IQueryable().Where(f => f.F_Id == recipientModel.P_ItemId).ToList().Sum( f => f.P_RemNum);
            if (fixedassetsNum >= recipientModel.P_RecipientNum)
                return true;
            var consumableNum = consumableService.IQueryable().Where(f => f.F_Id == recipientModel.P_ItemId).ToList().Sum(f => f.P_RemNum);
            if (consumableNum >= recipientModel.P_RecipientNum)
                return true;
            return false;
        }

        public List<ItemsDetailEntity> GetRecipientInfo(string flag)
        {
            var enCode = "";
            if (flag == "Fixedassets")
            {
                enCode = "fixedAssetTypes";
            }
            if (flag == "Consumable")
            {
                enCode = "consumableTypes";
            }
            var data = new ItemsApp().GetList();
            var dataId = data.Where(x => x.F_EnCode == enCode).SingleOrDefault();
            var itemdata = new ItemsDetailApp().GetList();
            return itemdata.FindAll(t => t.F_ItemId == dataId.F_Id);
        }

        public void SubmitForm(RecipientModel recipientModel)
        {
            var recipientEntity = new RecipientEntity();
            recipientEntity.P_ItemId = recipientModel.P_ItemId;
            recipientEntity.P_TypeId = recipientModel.P_TypeId;
            recipientEntity.P_RecipientNum = recipientModel.P_RecipientNum;
            recipientEntity.P_RecipientOwner = recipientModel.P_RecipientOwner;
            recipientEntity.P_Discription = recipientModel.P_Discription;
            recipientEntity.P_AssetModel = recipientModel.P_AssetModel;
            recipientEntity.Create();
            var result = service.SubmitForm(recipientEntity, "");
            if (result > 0)
            {
                var fixedassets = fixedassetsService.IQueryable().Where(f => f.F_Id == recipientEntity.P_ItemId).SingleOrDefault();
                if (fixedassets !=null && !string.IsNullOrEmpty(fixedassets.F_Id))
                {
                    fixedassets.P_RemNum = fixedassets.P_RemNum - recipientEntity.P_RecipientNum;
                    fixedassetsService.SubmitForm(fixedassets, fixedassets.F_Id);
                }
                else
                {
                    var consumable = consumableService.IQueryable().Where(f => f.F_Id == recipientEntity.P_ItemId).SingleOrDefault();
                    if (consumable != null && !string.IsNullOrEmpty(consumable.F_Id))
                    {
                        consumable.P_RemNum = consumable.P_RemNum - recipientEntity.P_RecipientNum;
                        consumableService.SubmitForm(consumable, consumable.F_Id);
                    }
                }
            }
        }
    }
}
