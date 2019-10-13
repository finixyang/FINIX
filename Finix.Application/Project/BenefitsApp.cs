using Finix.Application.SystemManage;
using Finix.Code;
using Finix.Domain.Entity.Project;
using Finix.Domain.Entity.SystemManage;
using Finix.Domain.IRepository.Project;
using Finix.Repository.Project;
using System.Collections.Generic;
using System.Linq;

namespace Finix.Application.Project
{
    public class BenefitsApp
    {
        private IBenefitsRepository service = new BenefitsRepository();

        public List<BenefitsEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<BenefitsEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.P_AssetModel.Contains(keyword));
            }
            expression = expression.And(t => t.F_DeleteMark != true);
            return service.FindList(expression, pagination);
        }
        public BenefitsEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(BenefitsEntity benefitsEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                benefitsEntity.Modify(keyValue);
            }
            else
            {
                benefitsEntity.Create();
                benefitsEntity.P_RemNum = benefitsEntity.P_AssetNumber;
            }
            benefitsEntity.P_AssetAmount = benefitsEntity.P_AssetNumber * benefitsEntity.P_AssetPrice;
            service.SubmitForm(benefitsEntity, keyValue);
        }

        public void UpdateForm(BenefitsEntity benefitsEntity)
        {
            service.Update(benefitsEntity);
        }

        public List<ItemsDetailEntity> GetAssetsTypes()
        {
            var data = new ItemsApp().GetList();
            var dataId = data.Where(x => x.F_EnCode == "benefitsTypes").SingleOrDefault();
            var itemdata = new ItemsDetailApp().GetList();
            return itemdata.FindAll(t => t.F_ItemId == dataId.F_Id);
        }
    }
}
