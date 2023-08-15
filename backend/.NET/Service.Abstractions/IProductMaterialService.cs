using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface IProductMaterialService
    {
        Task<ProductMaterial> GetProductMaterialAsync(int materialId, string productId);
        Task<IEnumerable<ProductMaterial>> GetAllProductMaterialsAsync();
        Task<IEnumerable<ProductMaterial>> FindProductMaterialsAsync(Expression<Func<ProductMaterial, bool>> predicate);
        Task AddProductMaterialAsync(ProductMaterial productMaterial);
        Task UpdateProductMaterialAsync(ProductMaterial productMaterial);
        Task RemoveProductMaterialAsync(ProductMaterial productMaterial);
        Task<int?> GetMaterialIdForProductAsync(string productId);

    }
}
