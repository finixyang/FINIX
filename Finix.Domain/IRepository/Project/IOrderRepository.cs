using Finix.Data;
using Finix.Domain.Entity.Project;

namespace Finix.Domain.IRepository.Project
{
    public interface IOrderRepository : IRepositoryBase<OrderEntity>
    {
        void DeleteForm(string keyValue);
        int SubmitForm(OrderEntity orderEntity, string keyValue);
        void CreateFlow(OrderEntity orderEntity);
    }
}
