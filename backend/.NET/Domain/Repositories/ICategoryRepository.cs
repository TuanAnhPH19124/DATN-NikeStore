using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ICategoryRepository
    {
        Task<bool> FindByNameAsync(string name);

        Task<IEnumerable<object>> GetAllAsync();
        Task<object> GetByIdAsync(string id);
        Task AddAsync(Category category);
        Task UpdateAsync(string id, Category category);
    }
}
