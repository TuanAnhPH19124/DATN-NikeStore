using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence.Configurations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal sealed class AppUserRepository : IAppUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _appDbContext;

        public AppUserRepository(UserManager<AppUser> userManager, IConfiguration configuration, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _configuration = configuration;
            _appDbContext = appDbContext;
        }

        public async Task<AppUser> AuthticationUserWithGoogle(string email)
        {
            //var user = await _context.AppUsers.FirstOrDefaultAsync(p => p.Email == email);
            //return user;
            return await Task.FromResult(new AppUser());

        }

        public async Task<AppUser> AuthticationUserWithLogin(string email, string password)
        {
            //return await _context.AppUsers.FirstOrDefaultAsync(p => p.Email == email && p.Password == password);
            return await Task.FromResult(new AppUser());

        }

        public async Task<bool> CheckPassword(AppUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(AppUser user, string code)
        {
            return await _userManager.ConfirmEmailAsync(user, code);
        }

        public async Task<AppUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<string> GenerateEmailConfirmToken(AppUser user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public string GenerateJwtToken(AppUser user)
        {
            var jwtTokenHandle = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);

            var tokendescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
                }),

                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandle.CreateToken(tokendescriptor);
            var jwtToken = jwtTokenHandle.WriteToken(token);

            return jwtToken;
        }

        public async Task<AppUser> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IdentityResult> Insert(AppUser appUser, string password)
        {
            return await _userManager.CreateAsync(appUser, password);
            
        }

      
        public async Task<List<AppUser>> GetAllAppUserAsync(CancellationToken cancellationToken = default)
        {                    
            var userList = await _appDbContext.AppUsers.ToListAsync(cancellationToken);
            return userList;
        }

        public async Task<AppUser> GetByIdAppUserAsync(string id, CancellationToken cancellationToken = default)
        {
            var appUser = await _appDbContext.AppUsers.FirstOrDefaultAsync(e => e.Id == id);
            return appUser;
        }

        public async Task UpdateAppUser(AppUser appUser)
        {
            _appDbContext.AppUsers.Update(appUser);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateAppUserbyAdmin(AppUser appUser)
        {
            _appDbContext.AppUsers.Update(appUser);
            await _appDbContext.SaveChangesAsync();
        }

        public void AddAppUser(AppUser appUser)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword)
        {
         
                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                return result;
            
        }

        public async Task<string> GeneratePasswordResetTokenAsync(AppUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(AppUser user, string token, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }
    }
}
