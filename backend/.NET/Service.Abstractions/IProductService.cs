using Domain.Entities;
using EntitiesDto.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> SelectByCategoryOnCacheAsync(string categoryId);
        Task<IEnumerable<Product>> SelectProductOnCacheAsync();
        Task<Product> CreateAsync(Product product);
        Task<List<Product>> GetAllProductAsync(CancellationToken cancellationToken = default);
        Task<Product> GetByIdProduct(string id, CancellationToken cancellationToken = default);
        Task<Product> UpdateByIdProduct(string id, Product product, CancellationToken cancellationToken = default);
        Task<List<Product>> FilterProductsAsync(
        string sizeId, string colorId, string categoryId, int? materialId, int? soleId);
    }
}
