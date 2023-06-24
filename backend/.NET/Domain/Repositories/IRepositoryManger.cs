using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IRepositoryManger
    {
        IAppUserRepository AppUserRepository { get; }
        IUnitOfWork UnitOfWork { get; }
        IVoucherRepository VoucherRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        ICacheRepository CacheRepository { get; }
    }
}
