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
    ICategoryRepository CategoryRepository { get; }
    IProductRepository ProductRepository { get; }
    ICacheRepository CacheRepository { get; }
    IOrderRepository OrderRepository { get; }
    IOrderItemsRepository OrderItemsRepository { get; }
        IWishListsRepository WishListsRepository { get; }
        INewsRepository NewsRepository { get; }
        IShoppingCartsRepository ShoppingCartsRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
    IStockRepository StockRepository { get; }
  }
}
