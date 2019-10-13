using Finix.Data;
using Finix.Domain.Entity.SystemManage;
using System.Collections.Generic;

namespace Finix.Domain.IRepository.SystemManage
{
    public interface IItemsDetailRepository : IRepositoryBase<ItemsDetailEntity>
    {
        List<ItemsDetailEntity> GetItemList(string enCode);
    }
}
