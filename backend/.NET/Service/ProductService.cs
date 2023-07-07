using Domain.Entities;
using Domain.Repositories;
using EntitiesDto.Product;
using Mapster;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            _repositoryManger = repositoryManger;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            _repositoryManger.ProductRepository.AddProduct(product);

            await _repositoryManger.UnitOfWork.SaveChangeAsync();
            return product;


        }

        public async Task<List<Product>> GetAllProductAsync(CancellationToken cancellationToken = default)
        {
            List<Product> productList = await _repositoryManger.ProductRepository.GetAllProductAsync(cancellationToken);
            return productList;
        }

        public async Task<Product> GetByIdProduct(string id, CancellationToken cancellationToken = default)
        {
            Product product = await _repositoryManger.ProductRepository.GetByIdAsync(id, cancellationToken);
            return product;
        }

        public async Task<Product> UpdateByIdProduct(string id, Product product, CancellationToken cancellationToken = default)
        {
            var existingProduct = await _repositoryManger.ProductRepository.GetByIdAsync(id, cancellationToken);
            if (existingProduct == null)
            {
                throw new Exception("Product not found.");
            }
            else
            {
                existingProduct.Id= product.Id;
                existingProduct.BarCode= product.BarCode;
                existingProduct.CostPrice= product.CostPrice;
                existingProduct.RetailPrice= product.RetailPrice;
                existingProduct.Description= product.Description;
                existingProduct.Brand= product.Brand;
                existingProduct.DiscountRate= product.DiscountRate;
                existingProduct.ModifiedDate= product.ModifiedDate;
                existingProduct.Name= product.Name;
                existingProduct.Status= product.Status;
                existingProduct.CreatedDate= product.CreatedDate;              
                await _repositoryManger.UnitOfWork.SaveChangeAsync(cancellationToken);
                return existingProduct;
            }
        }
    }
}
