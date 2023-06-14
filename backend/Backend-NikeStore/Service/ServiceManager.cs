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

        public ServiceManager(IRepositoryManger repositoryManger)
        {
            _lazyAppUserService = new Lazy<IAppUserService>(() => new AppUserService(repositoryManger));
            _lazyVoucherService = new Lazy<VoucherService>(() => new VoucherService(repositoryManger));
            _lazyProductService = new Lazy<ProductService>(() => new ProductService(repositoryManger));
        }

        public IAppUserService AppUserService => _lazyAppUserService.Value;
        public IVoucherService VoucherService => _lazyVoucherService.Value;
        public IProductService ProductService => _lazyProductService.Value;
    }
}
