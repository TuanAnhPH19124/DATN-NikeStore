using Domain.Entities;
using Domain.Repositories;
using EntitiesDto.Images;
using EntitiesDto.Product;
using EntitiesDto.Stock;
using Persistence.Ultilities;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<ProductDtoForGet>> FilterProductsAsync(ProductFilterOptionAPI options)
        {
            var query = _repositoryManger.ProductRepository.GetAllProductsQuery();
            if (options.Categories != null && options.Categories.Any())
            {
                var categoryIds = options.Categories.Select(p => p.Id).ToList();
                query = query.Where(p => p.CategoryProducts.Any(cp => categoryIds.Contains(cp.CategoryId)));
            }

            if (options.SortBy.HasValue)
            {
                switch (options.SortBy.Value)
                {
                    case Domain.Enums.SortBy.ASCENDING:
                        query = query.OrderBy(p => p.RetailPrice).ThenBy(p => p.DiscountRate);
                        break;
                    case Domain.Enums.SortBy.DESCENDING:
                        query = query.OrderByDescending(p => p.RetailPrice).ThenByDescending(p => p.DiscountRate);
                        break;
                    case Domain.Enums.SortBy.NEWEST:
                        query = query.OrderBy(p => p.CreatedDate);
                        break;
                    case Domain.Enums.SortBy.FEATURED:
                        break;
                }
            }

            var productLists = await _repositoryManger.ProductRepository.GetProductByFilterAndSort(query);

            var productDTOs = productLists.Select(product => new ProductDtoForGet
            {
                Id = product.Id,
                Name = product.Name,
                RetailPrice = product.RetailPrice,
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

        public async Task<List<ProductDtoForGet>> GetAllProductAsync(CancellationToken cancellationToken)
        {
            var products = await _repositoryManger.ProductRepository.GetAllProductAsync();

            var productDTOs = products.Select(product => new ProductDtoForGet
            {
                Id = product.Id,
                Name = product.Name,
                RetailPrice = product.RetailPrice,
                Description = product.Description,
                DiscountRate = product.DiscountRate,
                SoleId = product.SoleId,
                MaterialId = product.MaterialId,
                Status= product.Status,
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

        public async Task<List<ProductDtoForGet>> GetAllProductImageAsync(CancellationToken cancellationToken)
        {
            var products = await _repositoryManger.ProductRepository.GetAllProductImageAsync();

            var productDTOs = products.Select(product => new ProductDtoForGet
            {
                Id = product.Id,
                Name = product.Name,
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
                Id = product.Id,
                Name = product.Name,
                RetailPrice = product.RetailPrice,
                Description = product.Description,
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

            // Thêm các ảnh mới vào danh sách ProductImages
            var additionalImages = new List<ProductImageDto>();

            // Thêm các ảnh mới vào danh sách additionalImages, ví dụ:
            additionalImages.Add(new ProductImageDto
            {
                Id = product.Id, // Đặt Id cho ảnh mới
                ImageUrl = "URL của ảnh mới",
                SetAsDefault = false, // Đặt giá trị mặc định cho SetAsDefault
                ProductId = product.Id,
                ColorId = null // Đặt giá trị ColorId nếu có
            });

            // Thêm additionalImages vào danh sách ProductImages
            productDto.ProductImages.AddRange(additionalImages);

            return productDto;
        }

        public Task<Product> GetByIdProduct(string id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}











