using Finix.Data;
using Finix.Domain.Entity.Project;
using Finix.Domain.IRepository.Project;

namespace Finix.Repository.Project
{
    public class CustomerRepository : RepositoryBase<CustomerEntity>, ICustomerRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<CustomerEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public void SubmitForm(CustomerEntity customerEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(customerEntity);
                }
                else
                {
                    db.Insert(customerEntity);
                }
                db.Commit();
            }
        }
    }
}