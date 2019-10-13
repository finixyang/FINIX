using Finix.Data;
using Finix.Domain.Entity.Project;

namespace Finix.Domain.IRepository.Project
{
    public interface IFixedassetsRepository : IRepositoryBase<FixedassetsEntity>
    {
        void DeleteForm(string keyValue);
        int SubmitForm(FixedassetsEntity fixedassetsEntity, string keyValue);
    }
}
