using Finix.Data;
using Finix.Domain.Entity.Project;
using Finix.Domain.IRepository.Project;

namespace Finix.Repository.Project
{
    public class BenefitsRepository : RepositoryBase<BenefitsEntity>, IBenefitsRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<BenefitsEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public int SubmitForm(BenefitsEntity benefitsEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(benefitsEntity);
                }
                else
                {
                    db.Insert(benefitsEntity);
                }
                return db.Commit();
            }
        }
    }
}
