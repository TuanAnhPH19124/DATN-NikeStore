using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetByCategoryAsync(string categoryId);
        Task<IEnumerable<Product>> GetProductAsync();
    }
}
