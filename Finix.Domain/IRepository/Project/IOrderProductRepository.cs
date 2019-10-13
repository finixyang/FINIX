using Finix.Data;
using Finix.Domain.Entity.Project;

namespace Finix.Domain.IRepository.Project
{
    public interface IOrderProductRepository : IRepositoryBase<OrderProductEntity>
    {
        void DeleteForm(string keyValue);
        int SubmitForm(OrderProductEntity orderProductEntity, string keyValue);
    }
}
