using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ISoleRepository
    {
        Task<Sole> GetByIdAsync(int id);
        Task<IEnumerable<Sole>> GetAllAsync();
        Task<IEnumerable<Sole>> FindAsync(Expression<Func<Sole, bool>> predicate);
        Task AddAsync(Sole sole);
        Task UpdateAsync(Sole sole);
        Task RemoveAsync(Sole sole);
    }
}
