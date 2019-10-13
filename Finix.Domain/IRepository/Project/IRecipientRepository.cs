using Finix.Data;
using Finix.Domain.Entity.Project;

namespace Finix.Domain.IRepository.Project
{
    public interface IRecipientRepository : IRepositoryBase<RecipientEntity>
    {
        int SubmitForm(RecipientEntity recipientEntity, string keyValue);
        void DeleteForm(string keyValue);
    }
}
