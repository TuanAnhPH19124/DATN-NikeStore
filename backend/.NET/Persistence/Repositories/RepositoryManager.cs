using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Persistence.Configurations;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
  public class RepositoryManager : IRepositoryManger
  {
    private readonly Lazy<IAppUserRepository> _lazyAppUserRepository;
    private readonly Lazy<IVoucherRepository> _lazyVoucherRepository;
    private readonly Lazy<ICategoryRepository> _lazyCategoryRepository;
    private readonly Lazy<IProductRepository> _lazyProductRepository;
    private readonly Lazy<ICacheRepository> _lazyCacheRepository;
    private readonly Lazy<IOrderRepository> _lazyOrderRepository;
    private readonly Lazy<IOrderItemsRepository> _lazyOrderItemsRepository;
    private readonly Lazy<IStockRepository> _lazyStockRepository;
    private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;

    public RepositoryManager(AppDbContext context, UserManager<AppUser> userManager, IConfiguration configuration, IConnectionMultiplexer redis)
    {
      _lazyAppUserRepository = new Lazy<IAppUserRepository>(() => new AppUserRepository(userManager, configuration));
      _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(context));
      _lazyVoucherRepository = new Lazy<IVoucherRepository>(() => new VoucherRepository(context));
      _lazyCategoryRepository = new Lazy<ICategoryRepository>(() => new CategoryRepository(context));
      _lazyProductRepository = new Lazy<IProductRepository>(() => new ProductRepository(context));
      _lazyCacheRepository = new Lazy<ICacheRepository>(() => new CacheRepository(redis));
      _lazyOrderRepository = new Lazy<IOrderRepository>(() => new OrderRepository(context));
      _lazyOrderItemsRepository = new Lazy<IOrderItemsRepository>(() => new OrderItemsRepository(context));
      _lazyStockRepository = new Lazy<IStockRepository>(() => new StockRepository(context));
    }

    public IAppUserRepository AppUserRepository => _lazyAppUserRepository.Value;

    public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;

    public IVoucherRepository VoucherRepository => _lazyVoucherRepository.Value;

    public ICategoryRepository CategoryRepository => _lazyCategoryRepository.Value;

    public IProductRepository ProductRepository => _lazyProductRepository.Value;

    public ICacheRepository CacheRepository => _lazyCacheRepository.Value;

    public IOrderRepository OrderRepository => _lazyOrderRepository.Value; 

    public IOrderItemsRepository OrderItemsRepository => _lazyOrderItemsRepository.Value;

    public IStockRepository StockRepository => _lazyStockRepository.Value;
  }
    public class RepositoryManager : IRepositoryManger
    {
        private readonly Lazy<IAppUserRepository> _lazyAppUserRepository;
        private readonly Lazy<IVoucherRepository> _lazyVoucherRepository;
        private readonly Lazy<IProductRepository> _lazyProductRepository;
        private readonly Lazy<IWishListsRepository> _lazyWishListsRepository;
        private readonly Lazy<INewsRepository> _lazyNewsRepository;
        private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;
        private readonly Lazy<ShoppingCartsRepository> _lazyShoppingCartsRepository;
        private readonly Lazy<EmployeeRepository> _lazyEmployeeRepository;

        public RepositoryManager(AppDbContext context, UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _lazyAppUserRepository = new Lazy<IAppUserRepository>(() => new AppUserRepository(userManager, configuration, context));
            _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(context));
            _lazyVoucherRepository = new Lazy<IVoucherRepository>(() =>new VoucherRepository(context));
            _lazyProductRepository = new Lazy<IProductRepository>(() => new ProductRepository(context));
            _lazyNewsRepository= new Lazy<INewsRepository>(() =>new NewsRepository(context));
            _lazyWishListsRepository = new Lazy<IWishListsRepository>(() =>new WishListsRepository(context));
            _lazyShoppingCartsRepository = new Lazy<ShoppingCartsRepository>(() =>new ShoppingCartsRepository(context));
            _lazyEmployeeRepository = new Lazy<EmployeeRepository>(() => new EmployeeRepository(context));
        }

        public IAppUserRepository AppUserRepository => _lazyAppUserRepository.Value;
        
        public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;

        public IVoucherRepository VoucherRepository => _lazyVoucherRepository.Value;
        public IProductRepository ProductRepository => _lazyProductRepository.Value;

        public INewsRepository NewsRepository => _lazyNewsRepository.Value;

        public IWishListsRepository WishListsRepository => _lazyWishListsRepository.Value;
        public IShoppingCartsRepository ShoppingCartsRepository => _lazyShoppingCartsRepository.Value;

        public IEmployeeRepository EmployeeRepository => _lazyEmployeeRepository.Value;
    }
}
