using Finix.Data;
using Finix.Domain.Entity.Project;

namespace Finix.Domain.IRepository.Project
{
    public interface IOrderpaymentRepository : IRepositoryBase<OrderpaymentEntity>
    {
        int SubmitForm(OrderpaymentEntity orderpaymentEntity, string keyValue);

        void CreateOrderpayment(OrderpaymentEntity orderpaymentEntity);

        void DeleteForm(string keyValue);
    }
}
