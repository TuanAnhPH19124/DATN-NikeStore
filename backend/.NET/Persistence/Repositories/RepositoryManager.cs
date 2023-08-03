using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;

namespace Persistence.Repositories
{
    public class RepositoryManager : IRepositoryManger
    {
        private readonly Lazy<IAppUserRepository> _lazyAppUserRepository;
        private readonly Lazy<IVoucherRepository> _lazyVoucherRepository;
        private readonly Lazy<ICategoryRepository> _lazyCategoryRepository;
        private readonly Lazy<IProductRepository> _lazyProductRepository;
        private readonly Lazy<IWishListsRepository> _lazyWishListsRepository;
        private readonly Lazy<ICacheRepository> _lazyCacheRepository;
        private readonly Lazy<IOrderRepository> _lazyOrderRepository;
        private readonly Lazy<IOrderItemsRepository> _lazyOrderItemsRepository;
        private readonly Lazy<IStockRepository> _lazyStockRepository;
        private readonly Lazy<INewsRepository> _lazyNewsRepository;
        private readonly Lazy<IShoppingCartsRepository> _lazyShoppingCartsRepository;
        private readonly Lazy<IEmployeeRepository> _lazyEmployeeRepository;
        private readonly Lazy<ISizeRepository> _lazySizeRepository;
        private readonly Lazy<IColorRepository> _lazyColorRepository;
        private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;
        private readonly Lazy<IProductRateRepository> _lazyProductRateRepository;




        public RepositoryManager(AppDbContext context, UserManager<AppUser> userManager, IConfiguration configuration, IConnectionMultiplexer redis)
        {
            _lazyAppUserRepository = new Lazy<IAppUserRepository>(() => new AppUserRepository(userManager, configuration, context));
            _lazyVoucherRepository = new Lazy<IVoucherRepository>(() => new VoucherRepository(context));
            _lazyCategoryRepository = new Lazy<ICategoryRepository>(() => new CategoryRepository(context));
            _lazyProductRepository = new Lazy<IProductRepository>(() => new ProductRepository(context));
            _lazyCacheRepository = new Lazy<ICacheRepository>(() => new CacheRepository(redis));
            _lazyOrderRepository = new Lazy<IOrderRepository>(() => new OrderRepository(context));
            _lazyOrderItemsRepository = new Lazy<IOrderItemsRepository>(() => new OrderItemsRepository(context));
            _lazyStockRepository = new Lazy<IStockRepository>(() => new StockRepository(context));
            _lazyWishListsRepository = new Lazy<IWishListsRepository>(() => new WishListsRepository(context));
            _lazyNewsRepository = new Lazy<INewsRepository>(() => new NewsRepository(context));
            _lazyShoppingCartsRepository = new Lazy<IShoppingCartsRepository>(() => new ShoppingCartsRepository(context));
            _lazyEmployeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(context));
            _lazySizeRepository = new Lazy<ISizeRepository>(() => new SizeRepository(context));
            _lazyColorRepository = new Lazy<IColorRepository>(() => new ColorRepository(context));
            _lazyProductRateRepository=new Lazy<IProductRateRepository>(() => new ProductRateRepository(context));
            _lazyUnitOfWork=new Lazy<IUnitOfWork>(() => new UnitOfWork(context));

        }

        public IAppUserRepository AppUserRepository => _lazyAppUserRepository.Value;

        public IVoucherRepository VoucherRepository => _lazyVoucherRepository.Value;

        public ICategoryRepository CategoryRepository => _lazyCategoryRepository.Value;

        public IProductRepository ProductRepository => _lazyProductRepository.Value;

        public ICacheRepository CacheRepository => _lazyCacheRepository.Value;

        public IOrderRepository OrderRepository => _lazyOrderRepository.Value;

        public IOrderItemsRepository OrderItemsRepository => _lazyOrderItemsRepository.Value;

        public IStockRepository StockRepository => _lazyStockRepository.Value;

        public IWishListsRepository WishListsRepository => _lazyWishListsRepository.Value;

        public INewsRepository NewsRepository => _lazyNewsRepository.Value;

        public IShoppingCartsRepository ShoppingCartsRepository => _lazyShoppingCartsRepository.Value;

        public IEmployeeRepository EmployeeRepository => _lazyEmployeeRepository.Value;

        public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;

        public ISizeRepository SizeRepository => _lazySizeRepository.Value;

        public IColorRepository ColorRepository => _lazyColorRepository.Value;
        public IProductRateRepository ProductRateRepository => _lazyProductRateRepository.Value;
    }
}
