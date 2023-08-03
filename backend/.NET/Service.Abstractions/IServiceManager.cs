using Domain.Entities;
using Domain.Repositories;
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
        ICategoryService CategoryService { get; }
        IProductService ProductService { get; }
        IOrderService OrderService {get; }
        IOrderItemService OrderItemService { get; }
        INewsService NewsService { get; }
        IWishListsService WishListsService { get; }
        IShoppingCartsService ShoppingCartsService { get; }
        IEmployeeService employeeService { get; }
        ISizeService SizeService { get; }
        IColorService ColorService { get; }
        IProductRateService ProductRateService { get; }

    }
}
