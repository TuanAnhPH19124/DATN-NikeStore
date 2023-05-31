using Domain.Entities;
using EntitiesDto;
using EntitiesDto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface IAppUserService
    {
        Task<AppUser> CreateAsync(AppUserForCreateDto appUserForCreationDto, CancellationToken cancellationToken = default);
        Task<AppUser> GetauthenticationByGoogle(string email, CancellationToken cancellationToken = default);
        Task<AppUser> GetauthenticationByLogin(AppUserForLogin appUser, CancellationToken cancellationToken = default);
        Task<AppUser> GetByIdAppUser(Guid id, CancellationToken cancellationToken = default);       
        Task<AppUser> UpdateByIdAppUser(Guid id, AppUser updatedUser, CancellationToken cancellationToken = default);
    }
}
