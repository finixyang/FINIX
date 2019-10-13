using Finix.Data;
using Finix.Domain.Entity.Project;

namespace Finix.Domain.IRepository.Project
{
    public interface IOrderInspectionRepository : IRepositoryBase<OrderInspectionEntity>
    {
        int SubmitForm(OrderInspectionEntity orderInspectionEntity, string keyValue);
        void CreateFlow(OrderInspectionEntity orderInspectionEntity);
    }
}
