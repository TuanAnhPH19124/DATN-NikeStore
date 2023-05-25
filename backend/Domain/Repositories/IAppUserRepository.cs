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
     
        Task<IEnumerable<AppUser>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<AppUser> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);

        void Insert(AppUser user);

        void Update(AppUser user);
       
    }
}
