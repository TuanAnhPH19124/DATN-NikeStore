using Domain.Repositories;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAppUserService> _lazyAppUserService;
        private readonly Lazy<VoucherService> _lazyVoucherService;
        private readonly Lazy<ProductService> _lazyProductService;
        private readonly Lazy<NewsService> _lazyNewsService;
        private readonly Lazy<WishListsService> _lazyWishListsService;
        private readonly Lazy<ShoppingCartsService> _lazyShoppingCartsService;
        private readonly Lazy<ProductRateService> _lazyProductRateService;

        public ServiceManager(IRepositoryManger repositoryManger)
        {
            _lazyAppUserService = new Lazy<IAppUserService>(() => new AppUserService(repositoryManger));
            _lazyVoucherService = new Lazy<VoucherService>(() => new VoucherService(repositoryManger));
            _lazyProductService = new Lazy<ProductService>(() => new ProductService(repositoryManger));
            _lazyNewsService = new Lazy<NewsService>(() => new NewsService(repositoryManger));
            _lazyWishListsService  = new Lazy<WishListsService>(() => new WishListsService(repositoryManger));
            _lazyShoppingCartsService = new Lazy<ShoppingCartsService>(() => new ShoppingCartsService(repositoryManger));
            _lazyProductRateService=new Lazy<ProductRateService>(() => new ProductRateService(repositoryManger));
        }

        public IAppUserService AppUserService => _lazyAppUserService.Value;
        public IVoucherService VoucherService => _lazyVoucherService.Value;
        public IProductService ProductService => _lazyProductService.Value;

        public INewsService NewsService => _lazyNewsService.Value;

        public IWishListsService WishListsService => _lazyWishListsService.Value;
        public IShoppingCartsService ShoppingCartsService => _lazyShoppingCartsService.Value;
        public IProductRateService ProductRateService => _lazyProductRateService.Value;
    }
}
