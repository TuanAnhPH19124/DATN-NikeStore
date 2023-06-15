using Domain.Entities;
using Domain.Repositories;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    public class ProductService : IProductService
    {
        private readonly IRepositoryManger _repositoryManger;

        public ProductService(IRepositoryManger repositoryManger)
        {
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
