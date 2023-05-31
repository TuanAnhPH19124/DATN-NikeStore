using Domain.Repositories;
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
        private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;
        private readonly Lazy<IVoucherRepository> _lazyVoucherRepository;
        public RepositoryManager(AppDbContext context)
        {
            _lazyAppUserRepository = new Lazy<IAppUserRepository>(() => new AppUserRepository(context));
            _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(context));
            _lazyVoucherRepository = new Lazy<IVoucherRepository>(() =>new VoucherRepository(context));
        }

        public IAppUserRepository AppUserRepository => _lazyAppUserRepository.Value;
        
        public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;

        public IVoucherRepository VoucherRepository => _lazyVoucherRepository.Value;
    }
}
