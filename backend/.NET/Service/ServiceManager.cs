using Domain.Repositories;
using Nest;
using Service.Abstractions;
using System;

namespace Service
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAppUserService> _lazyAppUserService;
        private readonly Lazy<IVoucherService> _lazyVoucherService;
        private readonly Lazy<ICategoryService> _lazyCategoryService;
        private readonly Lazy<IProductService> _lazyProductService;
        private readonly Lazy<IOrderService> _lazyOrderService;
        private readonly Lazy<IOrderItemService> _lazyOrderItemService;
        private readonly Lazy<INewsService> _lazyNewsService;
        private readonly Lazy<IWishListsService> _lazyWishListsService;
        private readonly Lazy<IShoppingCartsService> _lazyShoppingCartsService;
        private readonly Lazy<IEmployeeService> _lazyEmployeeService;
        private readonly Lazy<ISizeService> _lazySizeService;
        private readonly Lazy<IColorService> _lazyColorService;
        private readonly Lazy<IProductRateService> _lazyProductRateService;


        public ServiceManager(IRepositoryManger repositoryManger)
        {
            _lazyAppUserService = new Lazy<IAppUserService>(() => new AppUserService(repositoryManger));
            _lazyVoucherService = new Lazy<IVoucherService>(() => new VoucherService(repositoryManger));
            _lazyCategoryService = new Lazy<ICategoryService>(() => new CategoryService(repositoryManger));
            _lazyProductService = new Lazy<IProductService>(() => new ProductService(repositoryManger));
            _lazyOrderService = new Lazy<IOrderService>(() => new OrderService(repositoryManger));
            _lazyOrderItemService = new Lazy<IOrderItemService>(() => new OrderItemService(repositoryManger));
            _lazyNewsService = new Lazy<INewsService>(() => new NewsService(repositoryManger));
            _lazyWishListsService = new Lazy<IWishListsService>(() => new WishListsService(repositoryManger));
            _lazyShoppingCartsService = new Lazy<IShoppingCartsService>(() => new ShoppingCartsService(repositoryManger));
            _lazyEmployeeService = new Lazy<IEmployeeService>(() => new EmployeeService(repositoryManger));
            _lazySizeService = new Lazy<ISizeService>(() => new SizeService(repositoryManger));
            _lazyColorService = new Lazy<IColorService>(() => new ColorService(repositoryManger));
            _lazyProductRateService = new Lazy<IProductRateService>(() => new ProductRateService(repositoryManger));
        }

        public IAppUserService AppUserService => _lazyAppUserService.Value;
        public IVoucherService VoucherService => _lazyVoucherService.Value;
        public ICategoryService CategoryService => _lazyCategoryService.Value;

        public IProductService ProductService => _lazyProductService.Value;

        public IOrderService OrderService => _lazyOrderService.Value;

        public IOrderItemService OrderItemService => _lazyOrderItemService.Value;

        public INewsService NewsService => _lazyNewsService.Value;

        public IWishListsService WishListsService => _lazyWishListsService.Value;

        public IShoppingCartsService ShoppingCartsService => _lazyShoppingCartsService.Value;

        public IEmployeeService employeeService => _lazyEmployeeService.Value;

        public ISizeService SizeService => _lazySizeService.Value;
        public IColorService ColorService => _lazyColorService.Value;

        public IProductRateService ProductRateService => _lazyProductRateService.Value;
    }
    
}
