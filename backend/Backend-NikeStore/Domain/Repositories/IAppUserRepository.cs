using Domain.Entities;
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
        void Insert(AppUser appUser);
        void Update(AppUser appUser);
        Task<AppUser> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
