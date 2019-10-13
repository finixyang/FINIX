using Finix.Code;
using Finix.Domain.Entity.Project;
using Finix.Domain.IRepository.Project;
using Finix.Repository.Project;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Finix.Application.Project
{
    public class ProductApp
    {
        private IProductRepository service = new ProductRepository();
        private IOrderProductRepository opService = new OrderProductRepository();

        public List<ProductEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<ProductEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.P_ProductName.Contains(keyword));
            }
            expression = expression.And(t => t.F_DeleteMark != true);
            return service.FindList(expression, pagination);
        }
        public List<ProductEntity> GetList()
        {
            return service.IQueryable().Where(t => t.F_DeleteMark != true).OrderBy(t => t.F_CreatorTime).ToList();
        }
        public ProductEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(ProductEntity productEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                productEntity.Modify(keyValue);
            }
            else
            {
                productEntity.Create();
                productEntity.P_InventoryQuantity = 0;
            }
            service.SubmitForm(productEntity, keyValue);
        }
        public void UpdateForm(ProductEntity productEntity)
        {
            service.Update(productEntity);
        }

        public List<ProductEntity> CheckProductInfo(string name, string no)
        {
            return service.IQueryable().Where(t => t.P_ProductName == name && t.P_DrawingNoOrModelNo == no).OrderBy(t => t.F_CreatorTime).ToList();
        }
    }
}

