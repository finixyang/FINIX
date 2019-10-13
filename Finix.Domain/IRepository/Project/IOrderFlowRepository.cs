using Finix.Data;
using Finix.Domain.Entity.Project;

namespace Finix.Domain.IRepository.Project
{
    public interface IOrderFlowRepository : IRepositoryBase<OrderFlowEntity>
    {
        int DeleteForm(string keyValue);
        int SubmitForm(OrderFlowEntity orderFlowEntity, string keyValue);
    }
}
