using Domain.DTOs;
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
        Task<AppUser> GetByIdAsync(string id);
        Task<IdentityResult> ConfirmEmailAsync(AppUser user, string code);
        Task<IdentityResult> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword);
        Task<AppUser> UpdateByIdAppUser(string id, AppUser appUser, CancellationToken cancellationToken = default);
        Task<AppUser> UpdateByIdAppUserByAdmin(string id, AppUser appUser, CancellationToken cancellationToken = default);
        Task<AppUser> GetByIdAppUserAsync(string id, CancellationToken cancellationToken = default);
        Task<List<AppUser>> GetAllAppUserAsync(CancellationToken cancellationToken = default);
        Task<AuthResult> ForgotPassword(string email);
        Task<AppUserPhoneDto> GetUserByPhoneNumber(string phoneNumber);
    }
}
