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

        public ServiceManager(IRepositoryManager repositoryManager)
        {
            _lazyAppUserService = new Lazy<IAppUserService>(() => new AppUserService(repositoryManager));
        }

        public IAppUserService AppUserService => _lazyAppUserService.Value;
    }
}
