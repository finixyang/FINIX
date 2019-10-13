using Finix.Domain.Entity.Project;
using Finix.Domain.IRepository.Project;
using Finix.Repository.Project;

namespace Finix.Application.Project
{
    public class OrderInspectionApp
    {
        private IOrderInspectionRepository service = new OrderInspectionRepository();
        
        public OrderInspectionEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void SubmitForm(OrderInspectionEntity orderInspectionEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                orderInspectionEntity.Modify(keyValue);
            }
            else
            {
                orderInspectionEntity.Create();
            }
            service.SubmitForm(orderInspectionEntity, keyValue);
        }
    }
}
