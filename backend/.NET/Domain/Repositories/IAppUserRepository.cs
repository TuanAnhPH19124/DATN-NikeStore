using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IAppUserRepository
    {
        Task<IdentityResult> Insert(AppUser appUser, string password);
      
        Task<AppUser> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<AppUser> AuthticationUserWithGoogle(string email);
        Task<AppUser> AuthticationUserWithLogin(string email, string password);
        Task<AppUser> FindByEmailAsync(string email);
        string GenerateJwtToken(AppUser user);
        Task<bool> CheckPassword(AppUser user,string password);
        Task<string> GenerateEmailConfirmToken(AppUser user);
        Task<IdentityResult> ConfirmEmailAsync(AppUser user, string code);
    }
}
