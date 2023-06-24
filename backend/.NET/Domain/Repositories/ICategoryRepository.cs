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
        
    }
}
