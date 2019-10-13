using Finix.Data;
using Finix.Domain.Entity.Project;
using Finix.Domain.IRepository.Project;

namespace Finix.Repository.Project
{
    public class ProductRepository : RepositoryBase<ProductEntity>, IProductRepository
    {
        public int DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<ProductEntity>(t => t.F_Id == keyValue);
                return db.Commit();
            }
        }
        public int SubmitForm(ProductEntity productEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(productEntity);
                }
                else
                {
                    db.Insert(productEntity);
                }
                return db.Commit();
            }
        }
    }
}