using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IMaterialRepository
    {
        Task<Material> GetByIdAsync(int id);
        Task<IEnumerable<Material>> GetAllAsync();
        Task<IEnumerable<Material>> FindAsync(Expression<Func<Material, bool>> predicate);
        Task AddAsync(Material material);
        Task UpdateAsync(Material material);
        Task RemoveAsync(Material material);
    }
}
