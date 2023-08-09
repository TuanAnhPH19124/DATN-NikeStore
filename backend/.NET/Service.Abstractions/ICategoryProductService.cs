using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface ICategoryProductService
    {
        Task<IEnumerable<CategoryProduct>> GetAllCategoryProductsAsync();
        Task<CategoryProduct> GetCategoryProductByIdAsync(string productId, string categoryId);
        Task<CategoryProduct> CreateCategoryProductAsync(CategoryProduct categoryProduct);
        Task UpdateCategoryProductAsync(string productId, string categoryId, CategoryProduct categoryProduct);
        Task DeleteCategoryProductAsync(string productId, string categoryId);
    }
}
