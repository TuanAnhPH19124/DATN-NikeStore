using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ICategoryProductRepository
    {
        Task<IEnumerable<CategoryProduct>> GetAllAsync();
        Task<CategoryProduct> GetByIdAsync(string productId, string categoryId);
        Task AddAsync(CategoryProduct categoryProduct);
        Task UpdateAsync(CategoryProduct categoryProduct);
        Task DeleteAsync(string productId, string categoryId);
    }
}
