using Finix.Data;
using Finix.Domain.Entity.SystemManage;
using System.Collections.Generic;

namespace Finix.Domain.IRepository.SystemManage
{
    public interface IModuleButtonRepository : IRepositoryBase<ModuleButtonEntity>
    {
        void SubmitCloneButton(List<ModuleButtonEntity> entitys);
    }
}
