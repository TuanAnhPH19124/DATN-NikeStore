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
    public class ProductRateService:IProductRateService
    {
        private readonly IRepositoryManger _repositoryManger;

        public ProductRateService(IRepositoryManger repositoryManger)
        {
            _repositoryManger = repositoryManger;
        }

        public async Task<ProductRate> AddProductRateAsync(ProductRate productRate)
        {
            await _repositoryManger.ProductRateRepository.AddProductRateAsync(productRate);
            await _repositoryManger.UnitOfWork.SaveChangeAsync();
            return productRate;
        }

        public async Task<ProductRate> GetProductRateAsync(string appUserId, string productId)
        {
          return await _repositoryManger.ProductRateRepository.GetProductRateAsync(appUserId, productId);
           
        }

        public async Task<List<ProductRate>> GetProductRatesByProductId(string productId)
        {
            return await _repositoryManger.ProductRateRepository.GetProductRatesByProductId(productId);
            
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _repositoryManger.ProductRateRepository.SaveChangesAsync();
        }

        public async Task UpdateProductRate(ProductRate productRate)
        {
           
           _repositoryManger.ProductRateRepository.UpdateProductRate(productRate);
            await  _repositoryManger.UnitOfWork.SaveChangeAsync();
        }
    }
}
