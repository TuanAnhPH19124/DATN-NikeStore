using Domain.Entities;
using Domain.Repositories;
using EntitiesDto.Images;
using EntitiesDto.Product;
using EntitiesDto.Stock;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Ultilities;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
            _repositoryManger = repositoryManger;

        }

        public async Task<IEnumerable<Product>> SelectByCategoryOnCacheAsync(string categoryId)
        {
            var cacheData = _repositoryManger.CacheRepository.GetData<IEnumerable<Product>>(categoryId);
            if (cacheData != null && cacheData.Count() > 0)
            {
                return cacheData;
            }
            cacheData = await _repositoryManger.ProductRepository.GetByCategoryAsync(categoryId);
            var expiryTime = DateTimeOffset.Now.AddMinutes(15);
            _repositoryManger.CacheRepository.SetData<IEnumerable<Product>>(categoryId, cacheData, expiryTime);
            return cacheData;
        }

        public async Task<IEnumerable<Product>> SelectProductOnCacheAsync()
        {
            var cacheData = _repositoryManger.CacheRepository.GetData<IEnumerable<Product>>("homepage-product");
            if (cacheData != null && cacheData.Count() > 0)
            {
                return cacheData;
            }
            cacheData = await _repositoryManger.ProductRepository.GetProductAsync();
            var expiryTime = DateTimeOffset.Now.AddMinutes(15);
            _repositoryManger.CacheRepository.SetData<IEnumerable<Product>>("homepage-product", cacheData, expiryTime);
            return cacheData;

        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _repositoryManger.ProductRepository.AddProduct(product);

            #region tạo ảnh barcode
            var qrcode = new UploadBarCode();
            product.BarCode = qrcode.generateAndUploadQRCode(product.Id);
            #endregion

            await _repositoryManger.UnitOfWork.SaveChangeAsync();
            return product;
        }

        public async Task UpdateByIdProduct(string id, Product updatedProduct, CancellationToken cancellationToken = default)
        {
            var existingProduct = await _repositoryManger.ProductRepository.GetByIdAsync(id, cancellationToken);

            if (existingProduct == null)
            {
                throw new Exception("Sản phẩm này không tồn tại.");
            }

            // Cập nhật thông tin cơ bản của sản phẩm từ updatedProduct
   
            existingProduct.RetailPrice = updatedProduct.RetailPrice;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.DiscountRate = updatedProduct.DiscountRate;
            existingProduct.Status = updatedProduct.Status;
            existingProduct.SoleId = updatedProduct.SoleId;
            existingProduct.MaterialId = updatedProduct.MaterialId;
            existingProduct.CategoryProducts = updatedProduct.CategoryProducts;
            existingProduct.ProductImages = updatedProduct.ProductImages;
            existingProduct.Stocks = updatedProduct.Stocks;
            // ... Cập nhật thông tin khác của sản phẩm
            existingProduct.ModifiedDate = DateTime.Now;
            // Gọi hàm Update trong repository để cập nhật sản phẩm
            _repositoryManger.ProductRepository.UpdateProduct(existingProduct);
            await _repositoryManger.UnitOfWork.SaveChangeAsync();

        }

        // Trong ProductService.cs

        public async Task<List<ProductForFilterDto>> FilterProductsAsync(
      string sizeId, string colorId, string categoryId, int? materialId, int? soleId)
        {
            var products = await _repositoryManger.ProductRepository.FilterProductsAsync(
                sizeId, colorId, categoryId, materialId, soleId);

            var productDTOs = products.Select(product => new ProductForFilterDto
            {

                // Sao chép các thuộc tính khác từ product
                SoleId = product.SoleId,
                MaterialId = product.MaterialId,

                Stocks = product.Stocks.Select(stock => new StockDto
                {
                    SizeId = stock.SizeId,
                    ColorId = stock.ColorId
                    // Sao chép các thuộc tính khác từ stock
                }).ToList(),

                CategoryProducts = product.CategoryProducts.Select(categoryProduct => new CategoryProductDto
                {
                    CategoryId = categoryProduct.CategoryId
                    // Sao chép các thuộc tính khác từ categoryProduct
                }).ToList()
            }).ToList();

            return productDTOs;
        }

        public async Task<List<ProductDtoForGet>> GetAllProductAsync(CancellationToken cancellationToken)
        {
            var products = await _repositoryManger.ProductRepository.GetAllProductAsync();

            var productDTOs = products.Select(product => new ProductDtoForGet
            {
                BarCode = product.BarCode,
                RetailPrice = product.RetailPrice,
                Description = product.Description,
                Status = product.Status,              
                DiscountRate = product.DiscountRate,
                SoleId = product.SoleId,
                MaterialId = product.MaterialId,

                Stocks = product.Stocks.Select(stock => new StockDto
                {
                    SizeId = stock.SizeId,
                    ColorId = stock.ColorId,
                    UnitInStock = stock.UnitInStock,
                    ProductId = stock.ProductId
                }).ToList(),

                CategoryProducts = product.CategoryProducts.Select(categoryProduct => new CategoryProductDto
                {
                    ProductId = categoryProduct.ProductId,
                    CategoryId = categoryProduct.CategoryId
                    // Sao chép các thuộc tính khác từ categoryProduct
                }).ToList(),

                ProductImages = product.ProductImages.Select(image => new ProductImageDto
                {
                    Id = image.Id,
                    ImageUrl = image.ImageUrl,
                    SetAsDefault = image.SetAsDefault,
                    ProductId = image.ProductId,
                    ColorId = image.ColorId
                    // Sao chép các thuộc tính khác từ image
                }).ToList()
            }).ToList();

            return productDTOs;
        }

        Task<Product> IProductService.GetByIdProduct(string id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        
          

        public async Task<ProductDtoForGet> GetProductByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var product = await _repositoryManger.ProductRepository.GetByIdAsync(id, cancellationToken);

            if (product == null)
            {
                // Xử lý nếu sản phẩm không tồn tại
                return null;
            }

            var productDto = new ProductDtoForGet
            {
                BarCode = product.BarCode,
                RetailPrice = product.RetailPrice,
                Description = product.Description,
                Status = product.Status,
                DiscountRate = product.DiscountRate,
                SoleId = product.SoleId,
                MaterialId = product.MaterialId,

                Stocks = product.Stocks.Select(stock => new StockDto
                {
                    SizeId = stock.SizeId,
                    ColorId = stock.ColorId,
                    UnitInStock = stock.UnitInStock,
                    ProductId = stock.ProductId
                }).ToList(),

                CategoryProducts = product.CategoryProducts.Select(categoryProduct => new CategoryProductDto
                {
                    ProductId = categoryProduct.ProductId,
                    CategoryId = categoryProduct.CategoryId
                    // Sao chép các thuộc tính khác từ categoryProduct
                }).ToList(),

                ProductImages = product.ProductImages.Select(image => new ProductImageDto
                {
                    Id = image.Id,
                    ImageUrl = image.ImageUrl,
                    SetAsDefault = image.SetAsDefault,
                    ProductId = image.ProductId,
                    ColorId = image.ColorId
                    // Sao chép các thuộc tính khác từ image
                }).ToList()
            };

            return productDto;
        }
    }
    }







    


