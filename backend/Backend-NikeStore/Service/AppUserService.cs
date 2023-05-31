using Domain.Entities;
using Domain.Repositories;
using EntitiesDto;
using EntitiesDto.User;
using Mapster;
using Service.Abstractions;
using System;
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

        public async Task<AppUser> GetauthenticationByGoogle(string email, CancellationToken cancellationToken = default)
        {
            return await _repositoryManger.AppUserRepository.AuthticationUserWithGoogle(email);                      
        }

        public async Task<AppUser> GetauthenticationByLogin(AppUserForLogin appUser, CancellationToken cancellationToken = default)
        {
            return await _repositoryManger.AppUserRepository.AuthticationUserWithLogin(appUser.Email, appUser.Password);
        }

        public async Task<AppUser> GetByIdAppUser(Guid id, CancellationToken cancellationToken = default)
        {          
                AppUser appUser = await _repositoryManger.AppUserRepository.GetByIdAsync(id, cancellationToken);     
            return appUser;
        }
    
        public async Task<AppUser> UpdateByIdAppUser(Guid id, AppUser updatedUser, CancellationToken cancellationToken = default)
        {
            var existingUser = await _repositoryManger.AppUserRepository.GetByIdAsync(id, cancellationToken);
            if (existingUser == null)
            {             
                throw new Exception("AppUser not found.");
            }
            else
            {
                existingUser.FullName = updatedUser.FullName;
                existingUser.PhoneNumber = updatedUser.PhoneNumber;
                existingUser.Password = updatedUser.Password;
                existingUser.AvatarUrl = updatedUser.AvatarUrl;
                await _repositoryManger.UnitOfWork.SaveChangeAsync(cancellationToken);
                return existingUser;
            }                   
        }
    }
}
