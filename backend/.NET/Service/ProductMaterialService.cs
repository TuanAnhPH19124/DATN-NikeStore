using Domain.Entities;
using Domain.Repositories;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ProductMaterialService : IProductMaterialService
    {
        private readonly IRepositoryManger _repositoryManager;

        public ProductMaterialService(IRepositoryManger repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task AddProductMaterialAsync(ProductMaterial productMaterial)
        {
           await  _repositoryManager.ProductMaterialRepository.AddAsync(productMaterial);
           await _repositoryManager.UnitOfWork.SaveChangeAsync();
        }

        public async Task<IEnumerable<ProductMaterial>> FindProductMaterialsAsync(Expression<Func<ProductMaterial, bool>> predicate)
        {
            return await _repositoryManager.ProductMaterialRepository.FindAsync(predicate);

        }

        public async Task<IEnumerable<ProductMaterial>> GetAllProductMaterialsAsync()
        {
            return await _repositoryManager.ProductMaterialRepository.GetAllAsync();
        }

        public async Task<int?> GetMaterialIdForProductAsync(string productId)
        {
            return await _repositoryManager.ProductMaterialRepository.GetMaterialIdForProductAsync(productId);
        }

        public async Task<ProductMaterial> GetProductMaterialAsync(int materialId, string productId)
        {
            return await _repositoryManager.ProductMaterialRepository.GetAsync(materialId, productId);
        }

        public async Task RemoveProductMaterialAsync(ProductMaterial productMaterial)
        {
            await _repositoryManager.ProductMaterialRepository.RemoveAsync(productMaterial);
            await _repositoryManager.UnitOfWork.SaveChangeAsync();
        }

        public async Task UpdateProductMaterialAsync(ProductMaterial productMaterial)
        {
            await _repositoryManager.ProductMaterialRepository.UpdateAsync(productMaterial);
            await _repositoryManager.UnitOfWork.SaveChangeAsync();
        }
    }
}
