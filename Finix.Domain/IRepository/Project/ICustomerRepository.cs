using Finix.Data;
using Finix.Domain.Entity.Project;

namespace Finix.Domain.IRepository.Project
{
    public interface ICustomerRepository : IRepositoryBase<CustomerEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(CustomerEntity customerEntity, string keyValue);
    }
}