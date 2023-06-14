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

        private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;

        public RepositoryManager(AppDbContext context, UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _lazyAppUserRepository = new Lazy<IAppUserRepository>(() => new AppUserRepository(userManager, configuration));
            _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(context));
            _lazyVoucherRepository = new Lazy<IVoucherRepository>(() =>new VoucherRepository(context));
            _lazyProductRepository = new Lazy<IProductRepository>(() => new ProductRepository(context));
        }

        public IAppUserRepository AppUserRepository => _lazyAppUserRepository.Value;
        
        public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;

        public IVoucherRepository VoucherRepository => _lazyVoucherRepository.Value;
        public IProductRepository ProductRepository => _lazyProductRepository.Value;
    }
}
