using Domain.Entities;
using Domain.Repositories;
using EntitiesDto.Product;
using Mapster;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal sealed class ProductService : IProductService
    {
        private readonly IRepositoryManger _repositoryManger;

        public ProductService(IRepositoryManger repositoryManger)
        {
            _repositoryManger=repositoryManger;
        }

        public async Task<IEnumerable<ProductForThirdServiceDto>> SelectByCategoryOnCacheAsync(string categoryId)
        {
            var cacheData = _repositoryManger.CacheRepository.GetData<IEnumerable<ProductForThirdServiceDto>>(categoryId);
            if (cacheData != null && cacheData.Count() > 0)
            {
                return cacheData;
            }
            var dbData = await _repositoryManger.ProductRepository.GetByCategoryAsync(categoryId);
            cacheData = dbData.Adapt(cacheData);
            var expiryTime = DateTimeOffset.Now.AddMinutes(15);
            _repositoryManger.CacheRepository.SetData<IEnumerable<ProductForThirdServiceDto>>(categoryId, cacheData, expiryTime);
            return cacheData;
        }

        public async Task<IEnumerable<ProductForThirdServiceDto>> SelectProductOnCacheAsync()
        {
            var cacheData = _repositoryManger.CacheRepository.GetData<IEnumerable<ProductForThirdServiceDto>>("homepage-product");
            if (cacheData != null && cacheData.Count() > 0)
            {
                return cacheData;
            }
            var dbData = await _repositoryManger.ProductRepository.GetProductAsync();
            var expiryTime = DateTimeOffset.Now.AddMinutes(15);
            _repositoryManger.CacheRepository.SetData<IEnumerable<ProductForThirdServiceDto>>("homepage-product", cacheData, expiryTime);
            return cacheData;
        }
    }
}
