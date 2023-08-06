using Domain.Entities;
using Domain.Repositories;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CategoryProductService : ICategoryProductService
    {
        private readonly IRepositoryManger _repositoryManger;
        public CategoryProductService(IRepositoryManger repositoryManger)
        {
            _repositoryManger = repositoryManger;
        }

        public async Task<CategoryProduct> CreateCategoryProductAsync(CategoryProduct categoryProduct)
        {
             await _repositoryManger.CategoryProductRepository.AddAsync(categoryProduct);
            return categoryProduct;
        }

        public async Task DeleteCategoryProductAsync(string productId, string categoryId)
        {
            await _repositoryManger.CategoryProductRepository.DeleteAsync(productId, categoryId);
        }

        public async Task<IEnumerable<CategoryProduct>> GetAllCategoryProductsAsync()
        {
            return await _repositoryManger.CategoryProductRepository.GetAllAsync();
        }

        public async Task<CategoryProduct> GetCategoryProductByIdAsync(string productId, string categoryId)
        {
            return await _repositoryManger.CategoryProductRepository.GetByIdAsync(productId, categoryId);
        }

        public async Task UpdateCategoryProductAsync(string productId, string categoryId, CategoryProduct categoryProduct)
        {
            categoryProduct.ProductId = productId; // Set the ProductId and CategoryId
            categoryProduct.CategoryId = categoryId;

            await _repositoryManger.CategoryProductRepository.UpdateAsync(categoryProduct);
        }
    }
}
