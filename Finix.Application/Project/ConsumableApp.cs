using Finix.Application.SystemManage;
using Finix.Code;
using Finix.Domain.Entity.Project;
using Finix.Domain.Entity.SystemManage;
using Finix.Domain.IRepository.Project;
using Finix.Repository.Project;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Finix.Application.Project
{
    public class ConsumableApp
    {
        private IConsumableRepository service = new ConsumableRepository();

        public List<ConsumableEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<ConsumableEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.P_AssetModel.Contains(keyword));
            }
            expression = expression.And(t => t.F_DeleteMark != true);
            SplitOrders();
            return service.FindList(expression, pagination);
        }

        public List<ConsumableEntity> GetList()
        {
            return service.IQueryable().OrderBy(t => t.F_CreatorTime).ToList();
        }

        private void SplitOrders()
        {
            var lists = service.IQueryable().Where(t => t.F_DeleteMark != true).ToList();
            Dictionary<string, string> dicIds = new Dictionary<string, string>();
            List<ParentClass> objList = new List<ParentClass>();
            foreach (var item in lists)
            {
                var parentClass = new ParentClass();
                //var key = item.P_TypeId + "-" + item.P_AssetName.Replace(" ","") + "-" + item.P_AssetModel.Replace(" ", "");
                var key = item.P_TypeId + "-" + item.P_AssetName.Replace(" ", "");
                var id = "";
                var itemId = item.F_Id;
                dicIds.TryGetValue(key, out id);
                var newParent = new ParentClass();
                if (id == null || id == "")
                {
                    id = CreateParentData(item);
                    dicIds.Add(key, id);
                }
                newParent.F_Id = itemId;
                newParent.F_ParentId = id;
                objList.Add(newParent);
            }
            foreach (var item in objList)
            {
                var consumableEntity = service.IQueryable().Where(t => t.F_DeleteMark != true && t.F_Id == item.F_Id).SingleOrDefault();
                if (consumableEntity != null)
                {
                    consumableEntity.F_ParentId = item.F_ParentId;
                    SubmitForm(consumableEntity, item.F_Id);
                }
            }
        }

        private string CreateParentData(ConsumableEntity con)
        {
            ConsumableEntity conNew = con;
            conNew.F_Id = "";
            conNew.F_ParentId = "0";
            conNew.Create();
            service.SubmitForm(conNew, "");
            return conNew.F_Id;
        }

        public ConsumableEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            var cusList = service.IQueryable().Where(t => t.F_ParentId == keyValue).ToList();
            foreach (var item in cusList)
            {
                service.DeleteForm(item.F_Id);
            }
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(ConsumableEntity consumableEntity, string keyValue)
        {
            var userInfo = OperatorProvider.Provider.GetCurrent();
            if (!string.IsNullOrEmpty(keyValue))
            {
                consumableEntity.F_LastModifyUserId = userInfo.UserId;
                consumableEntity.Modify(keyValue);
            }
            else
            {
                consumableEntity.Create();
                consumableEntity.P_RemNum = consumableEntity.P_AssetNumber;
                consumableEntity.F_ParentId = "0";
                consumableEntity.F_CreatorUserId = userInfo.UserId;
            }
            consumableEntity.P_AssetAmount = consumableEntity.P_AssetNumber * consumableEntity.P_AssetPrice;
            service.SubmitForm(consumableEntity, keyValue);
        }

        public void UpdateForm(ConsumableEntity consumableEntity)
        {
            service.Update(consumableEntity);
        }

        public List<ItemsDetailEntity> GetAssetsTypes()
        {
            var data = new ItemsApp().GetList();
            var dataId = data.Where(x => x.F_EnCode == "consumableTypes").SingleOrDefault();
            var itemdata = new ItemsDetailApp().GetList();
            return itemdata.FindAll(t => t.F_ItemId == dataId.F_Id);
        }

        public void SubmitFormP(ConsumableEntity consumableEntity)
        {
            var userInfo = OperatorProvider.Provider.GetCurrent();
            consumableEntity.Create();
            consumableEntity.P_RemNum = consumableEntity.P_AssetNumber;
            consumableEntity.P_AssetAmount = consumableEntity.P_AssetNumber * consumableEntity.P_AssetPrice;
            consumableEntity.F_CreatorUserId = userInfo.UserId;
            service.SubmitForm(consumableEntity, "");
        }
    }

    public class ParentClass
    {
        public string F_Id { get; set; }
        public string F_ParentId { get; set; }
    }
}
