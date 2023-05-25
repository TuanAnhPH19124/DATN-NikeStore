using Domain.Entities;
using DTOConvert;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using Domain.Repositories;

namespace Service
{
    internal sealed class AppUserService : IAppUserService
    {
        private readonly IRepositoryManager _repositoryManager;

        public AppUserService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<AppUser> CreateAsync(AppUserForCreationDTO userForCreationDto, CancellationToken cancellationToken = default)
        {
            var user = userForCreationDto.Adapt<AppUser>();

            _repositoryManager.AppUserRepository.Insert(user);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
            return user.Adapt<AppUser>();

        }
    }
}
