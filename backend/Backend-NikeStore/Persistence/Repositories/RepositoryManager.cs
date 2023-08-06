using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Persistence.Configurations;
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
        private readonly Lazy<IProductRepository> _lazyProductRepository;
        private readonly Lazy<IWishListsRepository> _lazyWishListsRepository;
        private readonly Lazy<INewsRepository> _lazyNewsRepository;
        private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;
        private readonly Lazy<IShoppingCartsRepository> _lazyShoppingCartsRepository;
        private readonly Lazy<IProductRateRepository> _lazyProductRateRepository;

        public RepositoryManager(AppDbContext context, UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _lazyAppUserRepository = new Lazy<IAppUserRepository>(() => new AppUserRepository(userManager, configuration, context));
            _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(context));
            _lazyVoucherRepository = new Lazy<IVoucherRepository>(() =>new VoucherRepository(context));
            _lazyProductRepository = new Lazy<IProductRepository>(() => new ProductRepository(context));
            _lazyNewsRepository= new Lazy<INewsRepository>(() =>new NewsRepository(context));
            _lazyWishListsRepository = new Lazy<IWishListsRepository>(() =>new WishListsRepository(context));
            _lazyShoppingCartsRepository = new Lazy<IShoppingCartsRepository>(() =>new ShoppingCartsRepository(context));
            _lazyProductRateRepository=new Lazy<IProductRateRepository>(() =>new ProductRateRepository(context));
        }

        public IAppUserRepository AppUserRepository => _lazyAppUserRepository.Value;
        
        public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;

        public IVoucherRepository VoucherRepository => _lazyVoucherRepository.Value;
        public IProductRepository ProductRepository => _lazyProductRepository.Value;

        public INewsRepository NewsRepository => _lazyNewsRepository.Value;

        public IWishListsRepository WishListsRepository => _lazyWishListsRepository.Value;
        public IShoppingCartsRepository ShoppingCartsRepository => _lazyShoppingCartsRepository.Value;
        public IProductRateRepository ProductRateRepository => _lazyProductRateRepository.Value;
    }
}
