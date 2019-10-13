using Finix.Data;
using Finix.Domain.Entity.Project;
using Finix.Domain.IRepository.Project;

namespace Finix.Repository.Project
{
    public class ConsumableRepository : RepositoryBase<ConsumableEntity>, IConsumableRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<ConsumableEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public int SubmitForm(ConsumableEntity consumableEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(consumableEntity);
                }
                else
                {
                    db.Insert(consumableEntity);
                }
                return db.Commit();
            }
        }
    }
}
