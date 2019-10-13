using Finix.Data;
using Finix.Domain.Entity.Project;
using Finix.Domain.IRepository.Project;

namespace Finix.Repository.Project
{
    public class RecipientRepository : RepositoryBase<RecipientEntity>, IRecipientRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<RecipientEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public int SubmitForm(RecipientEntity recipientEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(recipientEntity);
                }
                else
                {
                    db.Insert(recipientEntity);
                }
                return db.Commit();
            }
        }
    }
}
