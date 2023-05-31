using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal sealed class AppUserRepository : IAppUserRepository
    {
        private readonly AppDbContext _context;

        public AppUserRepository(AppDbContext context)
        {
            _context = context;    
        }

        public async Task<AppUser> AuthticationUserWithGoogle(string email)
        {
            var user = await _context.AppUsers.FirstOrDefaultAsync(p => p.Email == email);
            return user;
        }

        public async Task<AppUser> AuthticationUserWithLogin(string email, string password)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(p => p.Email == email && p.Password == password);
        }

        public async Task<AppUser> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var appUser = await _context.AppUsers.FirstOrDefaultAsync(e => e.Id== id);
            return appUser;
        }
      
        public void Insert(AppUser appUser)
        {
            _context.AppUsers.Add(appUser);
        }

        public async void Update( AppUser appUser)
        {                    
            _context.AppUsers.Update(appUser);
            _context.SaveChanges();
        }
    }
}
