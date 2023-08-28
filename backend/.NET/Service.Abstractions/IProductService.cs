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
        Task<List<ProductDtoForGet>> GetAllProductAsync(CancellationToken cancellationToken = default);
        Task<Product> GetByIdProduct(string id, CancellationToken cancellationToken = default);
        Task UpdateByIdProduct(string id, Product product, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProductDtoForGet>> FilterProductsAsync(ProductFilterOptionAPI options);
        Task<ProductDtoForGet> GetProductByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<List<ProductDtoForGet>> GetAllProductImageAsync(CancellationToken cancellationToken = default);
    }
}
