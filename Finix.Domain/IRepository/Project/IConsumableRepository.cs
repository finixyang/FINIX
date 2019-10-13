using Finix.Data;
using Finix.Domain.Entity.Project;

namespace Finix.Domain.IRepository.Project
{
    public interface IConsumableRepository : IRepositoryBase<ConsumableEntity>
    {
        void DeleteForm(string keyValue);
        int SubmitForm(ConsumableEntity consumableEntity, string keyValue);
    }
}
