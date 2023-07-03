using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface IServiceManager
    {
        IAppUserService AppUserService { get; }
        IVoucherService VoucherService { get; }
        IProductService ProductService { get; }
        INewsService NewsService { get; }
        IWishListsService WishListsService { get; }
        IShoppingCartsService ShoppingCartsService { get; }
        IEmployeeService employeeService { get; }

    }
}
