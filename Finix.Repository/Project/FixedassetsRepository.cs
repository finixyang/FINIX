using Finix.Data;
using Finix.Domain.Entity.Project;
using Finix.Domain.IRepository.Project;

namespace Finix.Repository.Project
{
    public class FixedassetsRepository : RepositoryBase<FixedassetsEntity>, IFixedassetsRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<FixedassetsEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public int SubmitForm(FixedassetsEntity fixedassetsEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(fixedassetsEntity);
                }
                else
                {
                    db.Insert(fixedassetsEntity);
                }
                return db.Commit();
            }
        }
    }
}
