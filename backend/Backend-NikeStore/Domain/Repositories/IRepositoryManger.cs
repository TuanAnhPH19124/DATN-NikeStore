using Persistence.Repositories;
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
        IProductRepository ProductRepository { get; }
        INewsRepository NewsRepository { get; }
        IWishListsRepository WishListsRepository{get;}
        IShoppingCartsRepository ShoppingCartsRepository { get; }
        IProductRateRepository ProductRateRepository { get; }
    }
}
