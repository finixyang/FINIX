using Finix.Code;
using Finix.Domain.Entity.Project;
using Finix.Domain.IRepository.Project;
using Finix.Repository.Project;
using System.Collections.Generic;
using System;
using Finix.Domain.Entity.SystemManage;
using Finix.Application.SystemManage;
using System.Linq;

namespace Finix.Application.Project
{
    public class FixedassetsApp
    {
        private IFixedassetsRepository service = new FixedassetsRepository();

        public List<FixedassetsEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<FixedassetsEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.P_AssetModel.Contains(keyword));
            }
            expression = expression.And(t => t.F_DeleteMark != true);
            return service.FindList(expression, pagination);
        }
        public FixedassetsEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(FixedassetsEntity fixedassetsEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                fixedassetsEntity.Modify(keyValue);
            }
            else
            {
                fixedassetsEntity.Create();
                fixedassetsEntity.P_RemNum = fixedassetsEntity.P_AssetNumber;
            }
            fixedassetsEntity.P_AssetAmount = fixedassetsEntity.P_AssetNumber * fixedassetsEntity.P_AssetPrice;
            service.SubmitForm(fixedassetsEntity, keyValue);
        }

        public void UpdateForm(FixedassetsEntity fixedassetsEntity)
        {
            service.Update(fixedassetsEntity);
        }

        public List<ItemsDetailEntity> GetAssetsTypes()
        {
            var data = new ItemsApp().GetList();
            var dataId = data.Where(x => x.F_EnCode == "fixedAssetTypes").SingleOrDefault();
            var itemdata = new ItemsDetailApp().GetList();
            return itemdata.FindAll(t => t.F_ItemId == dataId.F_Id);
        }
    }
}
