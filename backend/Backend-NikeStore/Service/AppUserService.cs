using Domain.Entities;
using Domain.Repositories;
using EntitiesDto;
using Mapster;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    internal sealed class AppUserService : IAppUserService
    {
        private readonly IRepositoryManger _repositoryManger;

        public AppUserService(IRepositoryManger repositoryManger)
        {
            _repositoryManger = repositoryManger;
        }

        public async Task<AppUser> CreateAsync(AppUserForCreateDto appUserForCreationDto, CancellationToken cancellationToken = default)
        {
            var user = appUserForCreationDto.Adapt<AppUser>();
            _repositoryManger.AppUserRepository.Insert(user);

            await _repositoryManger.UnitOfWork.SaveChangeAsync(cancellationToken);

            return user;
        }
    }
}
