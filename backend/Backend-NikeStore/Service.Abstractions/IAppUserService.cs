using Domain.Entities;
using Domain.Models;
using EntitiesDto;
using EntitiesDto.User;
using Microsoft.AspNetCore.Identity;
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
        Task<AuthResult> CreateAsync(AppUserForCreateDto appUserForCreationDto);
        Task<AuthResult> Login(AppUserForLogin user);
        Task<AppUser> GetauthenticationByGoogle(string email, CancellationToken cancellationToken = default);
        Task<AppUser> GetauthenticationByLogin(AppUserForLogin appUser, CancellationToken cancellationToken = default);
<<<<<<< HEAD
        Task<AppUser> GetByIdAsync(string id);
        Task<IdentityResult> ConfirmEmailAsync(AppUser user, string code);
=======
        Task<AppUser> GetByIdAppUser(Guid id, CancellationToken cancellationToken = default);       
        Task<AppUser> UpdateByIdAppUser(Guid id, AppUser updatedUser, CancellationToken cancellationToken = default);
>>>>>>> main
    }
}
