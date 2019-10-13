using Finix.Code;
using Finix.Domain.Entity.Project;
using Finix.Domain.IRepository.Project;
using Finix.Repository.Project;
using System.Collections.Generic;
using System.Linq;

namespace Finix.Application.Project
{
    public class CustomerApp
    {
        private ICustomerRepository service = new CustomerRepository();

        public List<CustomerEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<CustomerEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.P_CustomerName.Contains(keyword));
            }
            expression = expression.And(t => t.F_DeleteMark != true);
            return service.FindList(expression, pagination);
        }

        public List<CustomerEntity> GetList()
        {
            return service.IQueryable().Where(t=>t.F_DeleteMark != true).OrderBy(t => t.F_CreatorTime).ToList();
        }

        public CustomerEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(CustomerEntity customerEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                customerEntity.Modify(keyValue);
            }
            else
            {
                customerEntity.Create();
            }
            service.SubmitForm(customerEntity, keyValue);
        }
        public void UpdateForm(CustomerEntity customerEntity)
        {
            service.Update(customerEntity);
        }
    }
}
