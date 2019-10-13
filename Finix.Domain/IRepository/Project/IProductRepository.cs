using Finix.Data;
using Finix.Domain.Entity.Project;

namespace Finix.Domain.IRepository.Project
{
    public interface IProductRepository : IRepositoryBase<ProductEntity>
    {
        int DeleteForm(string keyValue);
        int SubmitForm(ProductEntity productEntity, string keyValue);
    }
}