using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IProductMaterialRepository
    {

        Task<ProductMaterial> GetAsync(int materialId, string productId);
        Task<IEnumerable<ProductMaterial>> GetAllAsync();
        Task<IEnumerable<ProductMaterial>> FindAsync(Expression<Func<ProductMaterial, bool>> predicate);
        Task AddAsync(ProductMaterial entity);
        Task UpdateAsync(ProductMaterial entity);
        Task RemoveAsync(ProductMaterial entity);
        Task<int?> GetMaterialIdForProductAsync(string productId);
    }
}
